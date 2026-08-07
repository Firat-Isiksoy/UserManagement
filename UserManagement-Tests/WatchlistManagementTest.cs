using Microsoft.EntityFrameworkCore;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement_Tests
{
    public class WatchlistManagementTest
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "WatchlistDbTest")
            .Options;

        private AppDbContext _context;
        private WatchlistService _watchlistService;

        private Guid _testUserId = Guid.NewGuid();
        private Guid _movie1Id = Guid.NewGuid();
        private Guid _movie2Id = Guid.NewGuid();
        private Guid _movie3Id = Guid.NewGuid(); 

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new AppDbContext(_dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();
            _watchlistService = new WatchlistService(_context);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            var user = new UserModel
            {
                Id = _testUserId,
                FirstName = "Cinephile",
                LastName = "User",
                Email = "cinephile@test.com",
                Password = "123",
                Role = "User"
            };
            _context.Users.Add(user);

            var movies = new List<MovieModel>
            {
                new MovieModel { Id = _movie1Id, Title = "Matrix" },
                new MovieModel { Id = _movie2Id, Title = "Inception" },
                new MovieModel { Id = _movie3Id, Title = "Interstellar" }
            };
            _context.Movies.AddRange(movies);

            var watchlists = new List<WatchlistModel>
            {
                new WatchlistModel
                {
                    Id = Guid.NewGuid(),
                    UserId = _testUserId,
                    MovieId = _movie1Id,
                    IsWatched = false,
                    AddedAt = DateTime.UtcNow
                },
                new WatchlistModel
                {
                    Id = Guid.NewGuid(),
                    UserId = _testUserId,
                    MovieId = _movie2Id,
                    IsWatched = true,
                    AddedAt = DateTime.UtcNow,
                    WatchedAt = DateTime.UtcNow
                }
            };
            _context.Watchlists.AddRange(watchlists);

            _context.SaveChanges();
        }

        [Test, Order(1)]
        public void GetWatchlist_ShouldReturnAllItems_Test()
        {
            var result = _watchlistService.GetWatchlist(_testUserId);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(2));
        }

        [Test, Order(2)]
        public void GetWatchlist_WithWatchedFilter_ShouldReturnOnlyWatched_Test()
        {
            var result = _watchlistService.GetWatchlist(_testUserId, watchedFilter: true);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].MovieId, Is.EqualTo(_movie2Id));
        }

        [Test, Order(3)]
        public void AddToWatchlist_ShouldAddNewMovie_Test()
        {
            var request = new WatchlistCreateDto { MovieId = _movie3Id };

            var result = _watchlistService.AddToWatchlist(_testUserId, request);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data.MovieId, Is.EqualTo(_movie3Id));
            Assert.That(result.Data.IsWatched, Is.False);

            var dbCheck = _context.Watchlists.FirstOrDefault(w => w.MovieId == _movie3Id);
            Assert.That(dbCheck, Is.Not.Null);
        }

        [Test, Order(4)]
        public void AddToWatchlist_WhenAlreadyExists_ShouldFail_Test()
        {
            var request = new WatchlistCreateDto { MovieId = _movie1Id };

            var result = _watchlistService.AddToWatchlist(_testUserId, request);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Bu film zaten izleme listenizde."));
        }

        [Test, Order(5)]
        public void MarkAsWatched_ExistingUnwatchedMovie_ShouldUpdate_Test()
        {
            var request = new WatchlistCreateDto { MovieId = _movie1Id };

            var result = _watchlistService.MarkAsWatched(_testUserId, request);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data.IsWatched, Is.True);
            Assert.That(result.Data.WatchedAt, Is.Not.Null);
        }

        [Test, Order(6)]
        public void MarkAsWatched_NewMovie_ShouldUpsert_Test()
        {
            var newMovieId = Guid.NewGuid();
            _context.Movies.Add(new MovieModel { Id = newMovieId, Title = "Dune" });
            _context.SaveChanges();

            var request = new WatchlistCreateDto { MovieId = newMovieId };

            var result = _watchlistService.MarkAsWatched(_testUserId, request);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data.IsWatched, Is.True);

            var dbCheck = _context.Watchlists.FirstOrDefault(w => w.MovieId == newMovieId);
            Assert.That(dbCheck, Is.Not.Null);
        }

        [Test, Order(7)]
        public void RemoveFromWatchlist_ShouldDeleteEntry_Test()
        {
            var isRemoved = _watchlistService.RemoveFromWatchlist(_testUserId, _movie1Id);

            Assert.That(isRemoved, Is.True);

            var dbCheck = _context.Watchlists.FirstOrDefault(w => w.MovieId == _movie1Id && w.UserId == _testUserId);
            Assert.That(dbCheck, Is.Null);
        }
    }
}