using Microsoft.EntityFrameworkCore;
using UserManagement.DTOs;
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
    private Guid _testCategoryId;
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
        _testCategoryId = Guid.NewGuid();
        var category = new CategoryModel
        {
            Id = _testCategoryId,
            Name = "aksiyon"
        };
        var movies = new List<MovieModel>
        {
            new MovieModel {
                Id = Guid.NewGuid(),
                Title = "MadMax",
                Description = "çölde takılmaca",
                Duration = 148,
                ReleaseYear = 2010,
                AverageRating = 8.2f,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryId = _testCategoryId
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
                CategoryId = _testCategoryId
            },
                new MovieModel {
                Id = Guid.NewGuid(),
                Title = "Undisputed",
                Description = "yuri boyka",
                Duration = 135,
                ReleaseYear = 2000,
                AverageRating = 9.0f,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryId = _testCategoryId
            },
                  new MovieModel {
                Id = Guid.NewGuid(),
                Title = "Gladiator",
                Description = "Maximus Decimus Meridius",
                Duration = 157,
                ReleaseYear = 2014,
                AverageRating = 8.6f,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryId = _testCategoryId
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
        Assert.AreEqual("MadMax", movies.First().Title);
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
        var newMovieDto = new MovieDto
        {
            Title = "TheDarkKnight",
            Description = "Heath Ledger'a Saygı",
            Duration = 133,
            ReleaseYear = 2011,
            AverageRating = 8.4f,
            CategoryId = _testCategoryId
        };
        var result = _movieService.Create(newMovieDto);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Error, Is.Null);
        var dbMovie = _context.Movies.FirstOrDefault(m => m.Title == "TheDarkKnight");
        Assert.That(dbMovie, Is.Not.Null);
        Assert.That(dbMovie.Title, Is.EqualTo("TheDarkKnight"));
    }
    [Test, Order(4)]
    public void Update_ShouldModifyMovie_Test()
    {
        var existingMovie = _context.Movies.First();
        var updatedDto = new MovieDto
        {
            Title = "Maskeli Beşler",
            Description = "Irak",
            Duration = existingMovie.Duration,
            ReleaseYear = existingMovie.ReleaseYear,
            AverageRating = existingMovie.AverageRating,
            CategoryId = existingMovie.CategoryId
        };
        var result = _movieService.Update(existingMovie.Id, updatedDto);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Error, Is.Null);
        var dbMovie = _context.Movies.Find(existingMovie.Id);
        Assert.That(dbMovie, Is.Not.Null);
        Assert.That(dbMovie.Title, Is.EqualTo("Maskeli Beşler"));
        Assert.That(dbMovie.Description, Is.EqualTo("Irak"));
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
    public void GetMoviesByCategory_ShouldReturnMoviesByCategory_Test()
    {
        var categoryId = _context.Movies.First().CategoryId;
        var movies = _movieService.GetMoviesByCategory(categoryId);
        Assert.IsNotNull(movies);
        Assert.IsTrue(movies.All(m => m.CategoryId == categoryId));
    }
}
