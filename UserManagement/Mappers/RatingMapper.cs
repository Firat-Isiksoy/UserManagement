using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Mappers
{
    public static class RatingMapper
    {
        public static MovieRatingDto ToDto(this MovieRating rating)
        {
            if (rating == null) return null;
            return new MovieRatingDto
            {
                MovieId = rating.MovieId,
                UserId = rating.UserId,
                Rating = rating.Rating,
                Note = rating.Note
            };
        }
        public static MovieRating ToModel(this MovieRatingDto ratingDto)
        {
            if (ratingDto == null) return null;
            return new MovieRating
            {
                MovieId = ratingDto.MovieId,
                UserId = ratingDto.UserId,
                Rating = ratingDto.Rating,
                Note = ratingDto.Note
            };
        }
        public static MovieRating UpdateModel (this MovieRating existingRating, MovieRatingDto ratingDto)
        {
            if (existingRating == null || ratingDto == null) return existingRating;
            existingRating.Rating = ratingDto.Rating;
            existingRating.Note = ratingDto.Note;
            return existingRating;
        }
    }
}