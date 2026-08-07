using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Mappers
{
    public static class WatchlistMapper
    {
        public static WatchlistDto ToDto(this WatchlistModel watchlist)
        {
            if (watchlist == null) return null!;
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

        // SENARYO 1: İzlenecekler listesine yepyeni bir film eklerken
        public static WatchlistModel ToUnwatchedModel(this WatchlistCreateDto dto, Guid userId, MovieModel movie)
        {
            if (dto == null) return null!;

            return new WatchlistModel
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MovieId = dto.MovieId,
                Movie = movie,
                AddedAt = DateTime.UtcNow,
                IsWatched = false,
                WatchedAt = null
            };
        }

        // SENARYO 2: Listede hiç olmayan bir filmi doğrudan "İzledim" diye eklerken (Upsert)
        public static WatchlistModel ToWatchedModel(this WatchlistCreateDto dto, Guid userId, MovieModel movie)
        {
            if (dto == null) return null!;

            var now = DateTime.UtcNow;
            return new WatchlistModel
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MovieId = dto.MovieId,
                Movie = movie,
                AddedAt = now,
                IsWatched = true,
                WatchedAt = now
            };
        }

        // SENARYO 3: Listede var olan bir filmi "İzlendi" olarak güncellerken
        public static void UpdateAsWatched(this WatchlistModel model, MovieModel movie)
        {
            if (model == null) return;

            model.IsWatched = true;
            model.WatchedAt = DateTime.UtcNow;
            model.Movie ??= movie;
        }
    }
}