using System;
using System.Linq.Expressions; // YENİ: Expression Selector için gerekli
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
        public static MovieRating UpdateModel(this MovieRating existingRating, MovieRatingDto ratingDto)
        {
            if (existingRating == null || ratingDto == null) return existingRating;
            existingRating.Rating = ratingDto.Rating;
            existingRating.Note = ratingDto.Note;
            return existingRating;
        }
        public static RatingDetailsDto ToDetailDto(this MovieRating rating)
        {
            if (rating == null) return null;

            return new RatingDetailsDto
            {
                UserId = rating.UserId,
                FirstName = rating.User?.FirstName ?? "Bilinmeyen",
                LastName = rating.User?.LastName ?? "Kullanıcı",
                Rating = rating.Rating,
                Note = rating.Note
            };
        }
        public static Expression<Func<MovieRating, RatingDetailsDto>> ToDetailDtoSelector => rating => new RatingDetailsDto
        {
            UserId = rating.UserId,
            FirstName = rating.User != null ? rating.User.FirstName : "Bilinmeyen",
            LastName = rating.User != null ? rating.User.LastName : "Kullanıcı",
            Rating = rating.Rating,
            Note = rating.Note
        };
    }
}