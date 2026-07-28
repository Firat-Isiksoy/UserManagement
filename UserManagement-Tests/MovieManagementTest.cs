using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement_Tests;

public class MovieManagementTest
{
    private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
             .UseInMemoryDatabase(databaseName: "MovieDbTest")
             .Options;
    private AppDbContext _context;
    MovieService _movieService;
    [OneTimeSetUp]
    public void Setup()
    {
        _context = new AppDbContext(_dbContextOptions);
        _context.Database.EnsureCreated();

        SeedDatabase();
        _movieService = new MovieService(_context);
    }
    [OneTimeTearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    public void SeedDatabase()
    {
        var movies = new List<MovieModel>
        {
            new MovieModel {
                Id = Guid.NewGuid(),
                Title = "Star Wars",
                Description = "DarthVader",
                Duration = 148,
                ReleaseYear = 2010,
                AverageRating = 8.2f,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryId = Guid.NewGuid()
            },
              new MovieModel {
                Id = Guid.NewGuid(),
                Title = "BadBoys",
                Description = "Polis",
                Duration = 138,
                ReleaseYear = 2002,
                AverageRating = 8.8f,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryId = Guid.NewGuid()
            },
                new MovieModel {
                Id = Guid.NewGuid(),
                Title = "Seven",
                Description = "",
                Duration = 135,
                ReleaseYear = 2000,
                AverageRating = 9.0f,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryId = Guid.NewGuid()
            },
                  new MovieModel {
                Id = Guid.NewGuid(),
                Title = "Hobbit",
                Description = "Gollum",
                Duration = 157,
                ReleaseYear = 2014,
                AverageRating = 8.6f,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryId = Guid.NewGuid()
            },
        };
        _context.Movies.AddRange(movies);
        _context.SaveChanges();
    }
    [Test, Order(1)]
    public void GetAllMovies_ShouldReturnAllMovies_Test()
    {
       
        var movies = _movieService.GetAll();
        Assert.AreEqual(4, movies.Count());
        Assert.AreEqual("Star Wars", movies.First().Title);
    }
    [Test, Order(2)]
    public void GetById_ShouldReturnMovie_Test()
    {
        var existingMovie = _context.Movies.First();
        var movie = _movieService.GetById(existingMovie.Id);
        Assert.IsNotNull(movie);
        Assert.AreEqual(existingMovie.Id, movie.Id);
    }
    [Test, Order(3)]
    public void Create_ShouldAddMovie_Test()
    {
        var createdMovie = new MovieModel
        {
            Id = Guid.NewGuid(),
            Title = "GhostRider",
            Description = "",
            Duration = 133,
            ReleaseYear = 2011,
            AverageRating = 8.4f,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CategoryId = Guid.NewGuid()
        };
        _movieService.Create(createdMovie);
        var dbMovie = _context.Movies.Find(createdMovie.Id);
        Assert.IsNotNull(dbMovie);
        Assert.AreEqual("GhostRider", dbMovie.Title);
    }
    [Test, Order(4)]
    public void Update_ShouldModifyMovie_Test()
    {
        var existingMovie = _context.Movies.First();
        existingMovie.Title = "Maskeli Beşler";
        existingMovie.Description = "Irak";
        _movieService.Update(existingMovie.Id,existingMovie);
        var dbMovie = _context.Movies.Find(existingMovie.Id);
        Assert.IsNotNull(dbMovie);
        Assert.AreEqual("Maskeli Beşler", dbMovie.Title);
        Assert.AreEqual("Irak", dbMovie.Description);
    }
    [Test, Order(5)]
    public void Delete_ShouldRemoveMovie_Test()
    {
        var existingMovie = _context.Movies.First();
        _movieService.Delete(existingMovie.Id);
        var dbMovie = _context.Movies.Find(existingMovie.Id);
        Assert.IsNull(dbMovie);
    }
    [Test, Order(6)]
    public void GetByCategoryId_ShouldReturnMoviesByCategory_Test()
    {
        var categoryId = _context.Movies.First().CategoryId;
        var movies = _movieService.GetByCategoryId(categoryId);
        Assert.IsNotNull(movies);
        Assert.IsTrue(movies.All(m => m.CategoryId == categoryId));
    }
}
