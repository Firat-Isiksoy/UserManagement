using UserManagement.DTOs;
using UserManagement.Models;
using System.Linq;

namespace UserManagement.Mappers
{
    public static class MovieMapper
    {
        public static MovieDto ToDto(this MovieModel movie)
        {
            if (movie == null) return null;
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Duration = movie.Duration,
                ReleaseYear = movie.ReleaseYear,
                AverageRating = movie.AverageRating,
                CategoryId = movie.CategoryId
            };
        }
        public static MovieModel ToModel(this MovieDto dto)
        {
            if (dto == null) return null;
            return new MovieModel
            {
                Title = dto.Title,
                Description = dto.Description,
                Duration = dto.Duration,
                ReleaseYear = dto.ReleaseYear,
                AverageRating = dto.AverageRating,
                CategoryId = dto.CategoryId
            };
        }
        public static void UpdateModel(this MovieDto dto, MovieModel existingMovie)
        {
            if (dto == null || existingMovie == null) return;
            existingMovie.Title = dto.Title.Trim();
            existingMovie.Description = dto.Description?.Trim();
            existingMovie.Duration = dto.Duration;
            existingMovie.ReleaseYear = dto.ReleaseYear;
            existingMovie.AverageRating = dto.AverageRating;
            existingMovie.CategoryId = dto.CategoryId;
        }
        public static MovieDetailsDto ToDetailsDto(
             this MovieModel movie,
             IEnumerable<MovieRating> pagedRatings,
             int totalCount,
             int currentPage,
             int pageSize)
        {
            if (movie == null) return null;

            return new MovieDetailsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Duration = movie.Duration,
                ReleaseYear = movie.ReleaseYear ?? 0,
                AverageRating = movie.AverageRating,
                CategoryId = movie.CategoryId,
                Ratings = new PagedResponse<RatingDetailsDto>
                {
                    TotalCount = totalCount,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    Data = pagedRatings != null
                                ? pagedRatings.Select(r => r.ToDetailDto()).ToList()
                                : new List<RatingDetailsDto>()
                }
            };
        }
    }
}
