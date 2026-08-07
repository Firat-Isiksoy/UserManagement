using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Mappers
{
    public static class WatchlistMapper
    {
        public static WatchlistDto ToDto(this WatchlistModel watchlist)
        {
            if (watchlist == null) return null;
            return new WatchlistDto
            {
                Id = watchlist.Id,
                MovieId = watchlist.MovieId,
                MovieTitle = watchlist.Movie?.Title ?? string.Empty,
                IsWatched = watchlist.IsWatched,
                AddedAt = watchlist.AddedAt,
                WatchedAt = watchlist.WatchedAt
            };
        }
        public static WatchlistModel ToModel(this WatchlistCreateDto dto, Guid userId)
        {
            if (dto == null) return null;

            return new WatchlistModel
            {
                UserId = userId,
                MovieId = dto.MovieId
            };
        }
    }
}
