using UserManagement.DTOs;
using UserManagement.Models;

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
    }
}
