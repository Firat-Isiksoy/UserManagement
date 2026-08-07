using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IWatchlistService
    {
        ResponseModel<WatchlistDto> MarkAsWatched(Guid userId, WatchlistCreateDto request);
        ResponseModel<WatchlistDto> AddToWatchlist(Guid userId, WatchlistCreateDto request);
        ResponseModel<List<WatchlistDto>> GetWatchlist(Guid userId, bool? watchedFilter = null);
        bool RemoveFromWatchlist(Guid userId, Guid movieId);
    }
}