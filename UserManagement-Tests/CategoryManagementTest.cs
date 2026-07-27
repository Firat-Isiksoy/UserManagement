using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement_Tests;

public class CategoryManagementTest
{
    private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
             .UseInMemoryDatabase(databaseName: "CategoryDbTest")
             .Options;

    private AppDbContext _context;
    CategoryService _categoryService;

    [OneTimeSetUp]
    public void Setup()
    {
        _context = new AppDbContext(_dbContextOptions);
        _context.Database.EnsureCreated();

        SeedDatabase();
        _categoryService = new CategoryService(_context);
    }
    [OneTimeTearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    private void SeedDatabase()
    {
        var categories = new List<CategoryModel>
{
    new CategoryModel
    {
        Id = Guid.NewGuid(),
        Name = "Aksiyon",
        Movies = new List<MovieModel>() 
    },
    new CategoryModel
    {
        Id = Guid.NewGuid(),
        Name = "Komedi",
        Movies = new List<MovieModel>()
    },
    new CategoryModel
    {
        Id = Guid.NewGuid(),
        Name = "Bilim Kurgu",
        Movies = new List<MovieModel>()
    },
    new CategoryModel
    {
        Id = Guid.NewGuid(),
        Name = "Dram",
        Movies = new List<MovieModel>()
    }
};
        _context.Categories.AddRange(categories);
        _context.SaveChanges();
    }

    [Test, Order(1)]
    public void GetAllCategories_ShouldReturnAllCategories_Test()
    {
        var categories = _categoryService.GetAll();
        Assert.That(categories.Count, Is.EqualTo(4));
        Assert.That(categories[0].Name, Is.EqualTo("Aksiyon"));
    }
    [Test, Order(2)]
    public void GetById_ShouldReturnCategory_Test()
    {
        var existingCategory = _context.Categories.First();
        var category = _categoryService.GetById(existingCategory.Id);

        Assert.That(category, Is.Not.Null);
        Assert.That(category.Id, Is.EqualTo(existingCategory.Id));
    }
    [Test, Order(3)]
    public void Create_ShouldAddCategory_Test()
    {
        var newCategory = new CategoryModel
        {
            Id = Guid.NewGuid(),
            Name = "Macera",
            Movies = new List<MovieModel>()
        };
        var result = _categoryService.Create(newCategory);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Error, Is.Empty);

        Assert.That(_context.Categories.Count(), Is.EqualTo(5));

        var dbCategory = _context.Categories.FirstOrDefault(c => c.Name == "Macera");
        Assert.That(dbCategory, Is.Not.Null);
    }
    [Test,Order(4)] 
    public void UpdateCategory_ShouldModifyCategory_Test()
    {
     
        var existingCategory = _context.Categories.First();

        existingCategory.Name = "Animasyon";

        var result = _categoryService.Update(existingCategory.Id, existingCategory);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Error, Is.EqualTo("Kategori başarıyla güncellendi"));

        
        var dbCategory = _context.Categories.FirstOrDefault(u => u.Id == existingCategory.Id);
        Assert.That(dbCategory, Is.Not.Null);
        Assert.That(dbCategory.Name, Is.EqualTo("Animasyon"));
    }
    [Test, Order(5)]
    public void DeleteCategory_ShouldRemoveCategory_Test()
    {
        var existingCategory = _context.Categories.First();
        var isDeleted = _categoryService.Delete(existingCategory.Id);

        Assert.That(isDeleted, Is.True);

        var dbCategory = _context.Users.FirstOrDefault(u => u.Id == existingCategory.Id);
        Assert.That(dbCategory, Is.Null);
    }
}

