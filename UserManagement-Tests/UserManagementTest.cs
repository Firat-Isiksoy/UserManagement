using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement_Tests
{
    public class UserManagementTest
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "UserDbTest")
            .Options;

        private AppDbContext _context;
        UserService _userService;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new AppDbContext(_dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();
            _userService = new UserService(_context);
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
                new UserModel { Id = Guid.NewGuid(), FirstName = "Johnny", LastName = "Dox", Email = "user100@example.com", Password = "123", Role = "User" },
                new UserModel { Id = Guid.NewGuid(), FirstName = "Josh", LastName = "Doc", Email = "user101@example.com", Password = "123", Role = "User" },
                new UserModel { Id = Guid.NewGuid(), FirstName = "Joe", LastName = "Dov", Email = "user102@example.com", Password = "123", Role = "User" },
                new UserModel { Id = Guid.NewGuid(), FirstName = "John", LastName = "Dob", Email = "user103@example.com", Password = "123", Role = "User" }
            };
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
        [Test, Order(1)]
        public void GetAllUsers_ShouldReturnAllUsers_Test()
        {
            var users = _userService.GetAll().ToList();
            Assert.That(users.Count, Is.EqualTo(4));
            Assert.That(users[0].FirstName, Is.EqualTo("Johnny"));
        }
        [Test, Order(2)]
        public void GetUserById_ShouldReturnUser_Test()
        {
            var existingUser = _context.Users.First();
            var user = _userService.GetById(existingUser.Id);

            Assert.That(user, Is.Not.Null);
            Assert.That(user.Email, Is.EqualTo(existingUser.Email));
        }
        [Test, Order(3)]
        public void CreateUser_ShouldAddUser_Test()
        {
            var newUser = new UserCreateDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "user105@example.com",
                Password = "password123"
            };
            var result = _userService.Create(newUser);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Error, Is.Null);
            Assert.That(_context.Users.Count(), Is.EqualTo(5));

            var dbUser = _context.Users.FirstOrDefault(u => u.Email == "user105@example.com");
            Assert.That(dbUser, Is.Not.Null);
            Assert.That(dbUser.FirstName, Is.EqualTo("Jane"));
        }
        [Test, Order(4)]
        public void UpdateUser_ShouldModifyUser_Test()
        {
            var existingUser = _context.Users.First();
            var updatedUser = new UserCreateDto
            {
                FirstName = "Jhin",
                LastName = "Kazama",
                Email = "user106@example.com",
                Password = "newpassword123"
            };
            var result = _userService.Update(existingUser.Id, updatedUser);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Error, Is.Null);

            var dbUser = _context.Users.FirstOrDefault(u => u.Email == "user106@example.com");
            Assert.That(dbUser, Is.Not.Null);
            Assert.That(dbUser.FirstName, Is.EqualTo("Jhin"));
        }

        [Test, Order(5)]
        public void DeleteUser_ShouldRemoveUser_Test()
        {
            var existingUser = _context.Users.First();
            var isDeleted = _userService.Delete(existingUser.Id);

            Assert.That(isDeleted, Is.True);

            var dbUser = _context.Users.FirstOrDefault(u => u.Id == existingUser.Id);
            Assert.That(dbUser, Is.Null);
        }   
    }
}