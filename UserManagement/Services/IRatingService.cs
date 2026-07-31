using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IRatingService
    {
        ResponseModel<MovieRatingDto> Create(MovieRatingDto ratingDto);
        PagedResponse<MovieRatingDto> GetRatings(RatingFilterDto filter);
        ResponseModel<MovieRatingDto> Update(Guid id,MovieRatingDto ratingDto);
        bool Delete(Guid id);
    }
}
