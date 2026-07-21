using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Controllers;
using UserManagement.Models;

namespace UserManagement_Tests
{
    public class UserManagementTest
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "UserDbTest")
            .Options;

        private AppDbContext _context;
        UserController _userController;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new AppDbContext(_dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();
            _userController = new UserController(_context);
        }
        [OneTimeTearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        private void SeedDatabase()
        {
            var users = new List<UserModel> {

                new UserModel { Id = Guid.NewGuid(), FirstName = "Johnny", LastName = "Dox", Email = "user100@example.com"},
                new UserModel { Id = Guid.NewGuid(), FirstName = "Josh", LastName = "Doc", Email = "user101@example.com"},
                new UserModel { Id = Guid.NewGuid(), FirstName = "Joe", LastName = "Dov", Email = "user102@example.com"},
                new UserModel { Id = Guid.NewGuid(), FirstName = "John", LastName = "Dob", Email = "user103@example.com"}
            };
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
        [Test, Order(1)]
        public void GetAllUsers_ShouldReturnAllUsers_Test()
        {
            var users = (List<UserModel>)((OkObjectResult)_userController.GetAllUsers()).Value;
            Assert.That(users.Count, Is.EqualTo(4));
            Assert.That(users[0].FirstName, Is.EqualTo("Johnny"));
        }
        [Test, Order(2)]
        public void GetUserById_ShouldReturnUser_Test()
        {
            var existingUser = _context.Users.First();
            var result = (OkObjectResult)_userController.Get(existingUser.Id);
            var user = (UserModel)result.Value;
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(existingUser.Id));
        }
        [Test,Order(3)]
        public void CreateUser_ShouldAddUser_Test()
        {
            var newUser = new UserModel
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "user105@example.com"
            };
            var result = (OkObjectResult)_userController.Create(newUser);
            var message = (string)result.Value;
            Assert.That(message, Is.EqualTo("Kullanýcý baþarýyla eklendi"));
            Assert.That(_context.Users.Count(), Is.EqualTo(5));

            var dbUser = _context.Users.FirstOrDefault(u => u.Email == "user105@example.com");
            Assert.That(dbUser, Is.Not.Null);
            Assert.That(dbUser.FirstName, Is.EqualTo("Jane"));
        }
        [Test, Order(4)]
        public void UpdateUser_ShouldModifyUser_Test()
        {
            var existingUser = _context.Users.First();
            var updatedUser = new UserModel
            {
                FirstName = "Jhin",
                LastName = "Kazama",
                Email = "user106@example.com"
            };
            var result = (OkObjectResult)_userController.Update(existingUser.Id, updatedUser);
            var message = (string)result.Value;
            Assert.That(message, Is.EqualTo("Kullanýcý baþarýyla güncellendi"));
            var dbUser = _context.Users.FirstOrDefault(u => u.Email == "user106@example.com");
            Assert.That(dbUser, Is.Not.Null);
            Assert.That(dbUser.FirstName, Is.EqualTo("Jhin"));
        }
        [Test, Order(5)]
        public void DeleteUser_ShouldRemoveUser_Test()
        {
            var existingUser = _context.Users.First();
            var result = (OkObjectResult)_userController.Delete(existingUser.Id);
            var message = (string)result.Value;
            Assert.That(message, Is.EqualTo("Kullanýcý baþarýyla silindi"));
            var dbUser = _context.Users.FirstOrDefault(u => u.Id == existingUser.Id);
            Assert.That(dbUser, Is.Null);
        }
    }
}