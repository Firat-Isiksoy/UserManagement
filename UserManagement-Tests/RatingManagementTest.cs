using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement_Tests;

public class RatingManagementTest
{
    private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "RatingDbTest")
            .Options;

    private AppDbContext _context;
    private RatingService _ratingService;

    private Guid _testMovieId = Guid.NewGuid();
    private Guid _testUserId1 = Guid.NewGuid();
    private Guid _testUserId2 = Guid.NewGuid();
    private Guid _testUserId3 = Guid.NewGuid();

    [OneTimeSetUp]
    public void Setup()
    {
        _context = new AppDbContext(_dbContextOptions);
        _context.Database.EnsureCreated();

        _ratingService = new RatingService(_context);
        SeedDatabase();
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void SeedDatabase()
    {
        var testMovie = new MovieModel
        {
            Id = _testMovieId,
            Title = "Test Movie",
            Description = "This is a test movie.",
            ReleaseYear = 2009,
            AverageRating = 0f, // float formatı
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CategoryId = Guid.NewGuid()
        };

        var ratings = new List<MovieRating>
        {
            new MovieRating { Id = Guid.NewGuid(), MovieId = _testMovieId, UserId = _testUserId1, Rating = 5f, Note = "Harika!" },
            new MovieRating { Id = Guid.NewGuid(), MovieId = _testMovieId, UserId = _testUserId2, Rating = 3f, Note = "Orta." },
            new MovieRating { Id = Guid.NewGuid(), MovieId = _testMovieId, UserId = _testUserId3, Rating = 1f, Note = "Kötü." }
        };

        _context.Movies.Add(testMovie);
        _context.MovieRatings.AddRange(ratings);
        _context.SaveChanges();

        _ratingService.RecalculateMovieAverageRating(_testMovieId);
        _context.SaveChanges();
    }

    [Test, Order(1)]
    public void GetAllRatings_ShouldReturnAllRatings_Test()
    {
        var filter = new RatingFilterDto
        {
            MovieId = _testMovieId,
            PageSize = 10,
            PageNumber = 1
        };

        var result = _ratingService.GetRatings(filter);

        Assert.AreEqual(3, result.TotalCount);
        Assert.AreEqual(3, result.Data.Count());
    }

    [Test, Order(2)]
    public void MovieAverage_ShouldBeCorrect_Initially_Test()
    {
        var movie = _context.Movies.FirstOrDefault(m => m.Id == _testMovieId);
        Assert.AreEqual(3.0f, movie.AverageRating);
    }

    [Test, Order(3)]
    public void AddRating_ShouldAddRatingAndUpdateAverage_Test()
    {
        var newRating = new MovieRatingDto
        {
            MovieId = _testMovieId,
            UserId = Guid.NewGuid(),
            Rating = 4f,
            Note = "İyi."
        };

        var result = _ratingService.Create(newRating);
        Assert.IsTrue(result.Success);

        var movie = _context.Movies.First(m => m.Id == _testMovieId);
        Assert.AreEqual(3.25f, movie.AverageRating);
    }

    [Test, Order(4)]
    public void UpdateRating_ShouldUpdateRatingAndAverage_Test()
    {
        var existingRating = _context.MovieRatings.First(r => r.UserId == _testUserId1);
        var updateDto = new MovieRatingDto
        {
            MovieId = _testMovieId,
            UserId = _testUserId1,
            Rating = 2f,  
            Note = "Fikrimi değiştirdim, puanım 2."
        };

        var result = _ratingService.Update(existingRating.Id, updateDto);

        Assert.IsTrue(result.Success);
        Assert.AreEqual("Rating updated successfully.", result.Error);
        Assert.AreEqual(2f, result.Data.Rating);

        var movie = _context.Movies.First(m => m.Id == _testMovieId);
        Assert.AreEqual(2.5f, movie.AverageRating);
    }

    [Test, Order(5)]
    public void DeleteRating_ShouldDeleteRatingAndRecalculateAverage_Test()
    {
        var existingRating = _context.MovieRatings.First(r => r.UserId == _testUserId1);
        var ratingIdToDelete = existingRating.Id;

        var isDeleted = _ratingService.Delete(ratingIdToDelete);

        Assert.IsTrue(isDeleted);
        var deletedRating = _context.MovieRatings.Find(ratingIdToDelete);
        Assert.IsNull(deletedRating);

        var movie = _context.Movies.First(m => m.Id == _testMovieId);
        Assert.AreEqual(2.67f, movie.AverageRating, 0.01f, "Ortalama hesaplaması tolerans dışı!");
    
    }
}