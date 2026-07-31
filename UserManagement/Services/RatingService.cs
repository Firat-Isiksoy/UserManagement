using Microsoft.IdentityModel.Tokens;
using UserManagement.DTOs;
using UserManagement.Mappers;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class RatingService : IRatingService
    {
        private readonly AppDbContext _context;
        public RatingService(AppDbContext context)
        {
            _context = context;
        }
        public void RecalculateMovieAverageRating(Guid movieId)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == movieId);
            if (movie == null) return;

            var average = _context.MovieRatings
                .Where(r => r.MovieId == movieId)
                .Average(r => (double?)r.Rating) ?? 0.0;
            movie.AverageRating = (float)average;
        }
        public PagedResponse<MovieRatingDto> GetRatings(RatingFilterDto filter)
        {
            var query = _context.MovieRatings.AsQueryable();

            if (filter.MovieId.HasValue)
            {
                query = query.Where(r => r.MovieId == filter.MovieId.Value);
            }

            if (filter.UserId.HasValue)
            {
                query = query.Where(r => r.UserId == filter.UserId.Value);
            }

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "rating" => query.OrderByDescending(r => r.Rating),
                    "date" => query.OrderByDescending(r => r.CreatedAt),
                    _ => query.OrderBy(r => r.Id)
                };
            }
            else
            {
                query = query.OrderBy(r => r.Id);
            }

            var data = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(r => r.ToDto())
                .ToList();

            return new PagedResponse<MovieRatingDto>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = filter.PageNumber,
                PageSize = filter.PageSize,
                Data = data
            };
        }
        public ResponseModel<MovieRatingDto> Create(MovieRatingDto ratingDto)
        {
            bool hasAlreadyRated = _context.MovieRatings
                .Any(r => r.UserId == ratingDto.UserId && r.MovieId == ratingDto.MovieId);

            if (hasAlreadyRated)
            {
                return new ResponseModel<MovieRatingDto>
                {
                    Success = false,
                    Error = "Bir kullanıcı bir filme yalnızca bir kez oy verebilir",
                    Data = null
                };
            }
            var newRating = ratingDto.ToModel();
            newRating.Id = Guid.NewGuid();
            newRating.CreatedAt = DateTime.UtcNow;

            _context.MovieRatings.Add(newRating);
            _context.SaveChanges();

            RecalculateMovieAverageRating(newRating.MovieId);

            _context.SaveChanges();
            return new ResponseModel<MovieRatingDto>
            {
                Success = true,
                Error = null,
                Data = newRating.ToDto()
            };
        }
        public ResponseModel<MovieRatingDto> Update(Guid id,MovieRatingDto ratingDto)
        {
            var existingRating = _context.MovieRatings.Find(id);
            if (existingRating == null)
            {
                return new ResponseModel<MovieRatingDto>
                {
                    Success = false,
                    Error = "Rating not found."
                };
            }
            existingRating.UpdateModel(ratingDto);
            _context.SaveChanges();
            RecalculateMovieAverageRating(existingRating.MovieId);
            _context.SaveChanges();
            return new ResponseModel<MovieRatingDto>
            {
                Success = true,
                Error = "Rating updated successfully.",
                Data = existingRating.ToDto()
            };
        }
        public bool Delete(Guid id)
        {
            var existingRating = _context.MovieRatings.Find(id);
            if (existingRating == null)
            {
                return false;
            }
            _context.MovieRatings.Remove(existingRating);
            _context.SaveChanges();
            RecalculateMovieAverageRating(existingRating.MovieId);
            _context.SaveChanges();
            return true;
        }
    }
}
