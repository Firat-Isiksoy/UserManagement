using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagement.DTOs;
using UserManagement.Mappers;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class MovieService : IMovieService
    {

        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }
        public ResponseModel<MovieDto> Create(MovieDto request)
        {
            var newMovie = request.ToModel();
            newMovie.Id = Guid.NewGuid();
            newMovie.CreatedAt = DateTime.UtcNow;

            _context.Movies.Add(newMovie);
            _context.SaveChanges();

            return new ResponseModel<MovieDto>
            {
                Success = true,
                Error = null,
                Data = newMovie.ToDto()
            };
        }
        public PagedResponse<MovieDto> GetAll(MovieFilterDto filter)
        {
            filter ??= new MovieFilterDto();
            var query = _context.Movies.AsQueryable();
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(m => m.CategoryId == filter.CategoryId.Value);
            }
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "title" => query.OrderBy(m => m.Title),
                    "duration" => query.OrderBy(m => m.Duration),
                    "averagerating" => query.OrderBy(m => m.AverageRating),
                    "releaseyear" => query.OrderBy(m => m.ReleaseYear),
                    _ => query.OrderBy(m => m.Id)
                };
            }
            else
            {
                query = query.OrderBy(m => m.Id);
            }
            var movies = query
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(m => m.ToDto())
                .ToList();

            return new PagedResponse<MovieDto>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = filter.PageIndex,
                PageSize = filter.PageSize,
                Data = movies
            };
        }
        public MovieDto? GetById(Guid id) => _context.Movies.Find(id)?.ToDto();
        public ResponseModel<MovieDto> Update(Guid Id, MovieDto movie)
        {
            var existingMovie = _context.Movies.Find(Id);
            if (existingMovie is null)
            {
                return new ResponseModel<MovieDto>
                {
                    Success = false,
                    Error = "Aranan film bulunamadı",
                    Data = null
                };
            }
            ;
            movie.UpdateModel(existingMovie);
            existingMovie.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return new ResponseModel<MovieDto>
            {
                Success = true,
                Error = null,
                Data = existingMovie.ToDto()
            };
        }
        public bool Delete(Guid id)
        {
            var movie = _context.Movies.Find(id);
            if (movie is null) return false;
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return true;
        }
        public ResponseModel<MovieDetailsDto> GetMovieWithInfo(Guid id, PaginationFilter filter)
        {
            try
            {
                var movie = _context.Movies
                    .Include(m => m.Category)
                    .FirstOrDefault(m => m.Id == id);

                if (movie == null)
                    return new ResponseModel<MovieDetailsDto> { Success = false, Error = "Film bulunamadı." };

                var pagedRatings = _context.MovieRatings
                        .Where(r => r.MovieId == id)
                        .Skip((filter.PageNumber - 1) * filter.PageSize)
                        .Take(filter.PageSize)
                        .Select(r => new RatingDetailsDto
                        {
                            UserId = r.UserId,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName,
                            Rating = r.Rating,
                            Note = r.Note
                        })
                        .ToList();

                var totalCount = _context.MovieRatings.Count(r => r.MovieId == id);
                var movieDetails = movie.ToDetailsDto(pagedRatings, totalCount, filter.PageNumber, filter.PageSize);
                return new ResponseModel<MovieDetailsDto>
                {
                    Success = true,
                    Data = movieDetails
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<MovieDetailsDto>
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }
    }
}
