using Microsoft.EntityFrameworkCore;
using UserManagement.DTOs;
using UserManagement.Mappers;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly AppDbContext _context;

        public WatchlistService(AppDbContext context)
        {
            _context = context;
        }

        public ResponseModel<WatchlistDto> AddToWatchlist(Guid userId, WatchlistCreateDto request)
        {
            var movie = _context.Movies.Find(request.MovieId);
            if (movie == null)
            {
                return new ResponseModel<WatchlistDto>
                {
                    Success = false,
                    Error = "Film bulunamadı.",
                    Data = null
                };
            }

            var existing = _context.Watchlists
                .FirstOrDefault(w => w.UserId == userId && w.MovieId == request.MovieId);

            if (existing != null)
            {
                return new ResponseModel<WatchlistDto>
                {
                    Success = false,
                    Error = "Bu film zaten izleme listenizde.",
                    Data = null
                };
            }

            // MAPPER DEVREDE: Bütün atama işlemi tek satıra indi!
            var entry = request.ToUnwatchedModel(userId, movie);

            _context.Watchlists.Add(entry);
            _context.SaveChanges();

            return new ResponseModel<WatchlistDto>
            {
                Success = true,
                Error = null,
                Data = entry.ToDto()
            };
        }

        public ResponseModel<WatchlistDto> MarkAsWatched(Guid userId, WatchlistCreateDto request)
        {
            var movie = _context.Movies.Find(request.MovieId);
            if (movie == null)
            {
                return new ResponseModel<WatchlistDto>
                {
                    Success = false,
                    Error = "Film bulunamadı.",
                    Data = null
                };
            }

            var entry = _context.Watchlists
                .FirstOrDefault(w => w.UserId == userId && w.MovieId == request.MovieId);

            if (entry == null)
            {
                // MAPPER DEVREDE: Yeni kayıt + İzlendi (Upsert)
                entry = request.ToWatchedModel(userId, movie);
                _context.Watchlists.Add(entry);
            }
            else
            {
                // MAPPER DEVREDE: Var olan kaydı İzlendi olarak güncelle
                entry.UpdateAsWatched(movie);
            }

            _context.SaveChanges();

            return new ResponseModel<WatchlistDto>
            {
                Success = true,
                Error = null,
                Data = entry.ToDto()
            };
        }

        public ResponseModel<List<WatchlistDto>> GetWatchlist(Guid userId, bool? watchedFilter = null)
        {
            var query = _context.Watchlists
                .Include(w => w.Movie)
                .Where(w => w.UserId == userId);

            if (watchedFilter.HasValue)
            {
                query = query.Where(w => w.IsWatched == watchedFilter.Value);
            }

            var items = query
                .OrderByDescending(w => w.AddedAt)
                .ToList()
                .Select(w => w.ToDto()!)
                .ToList();

            return new ResponseModel<List<WatchlistDto>>
            {
                Success = true,
                Error = null,
                Data = items
            };
        }
        public bool RemoveFromWatchlist(Guid userId, Guid movieId)
        {
            var entry = _context.Watchlists
                .FirstOrDefault(w => w.UserId == userId && w.MovieId == movieId);

            if (entry == null)
            {
                return false;
            }

            _context.Watchlists.Remove(entry);
            _context.SaveChanges();
            return true;
        }
    }
}