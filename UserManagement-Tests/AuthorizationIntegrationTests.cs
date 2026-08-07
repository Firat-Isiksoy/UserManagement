using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement_Tests
{
    [TestFixture]
    public class AuthorizationIntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _anonClient;

        [OneTimeSetUp]
        public void Setup()
        {
            // 1. Program.cs'in daha en başında okuyabilmesi için ortam değişkenlerini (Environment Variables) atıyoruz.
            // JSON'daki iki nokta (:) yerine ortam değişkenlerinde çift alt çizgi (__) kullanılır.
            Environment.SetEnvironmentVariable("JwtSettings__SecretKey", "TestOrtamiIcinGecerliUzunluktaBirSecretKey123!@#");
            Environment.SetEnvironmentVariable("JwtSettings__Issuer", "UserManagementAPI");
            Environment.SetEnvironmentVariable("JwtSettings__Audience", "UserManagementClient");

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // DİKKAT: Eski yazdığımız ConfigureAppConfiguration bloğunu tamamen sildik!
                    // Artık sistem her şeyi yukarıdaki Environment değerlerinden çekecek.

                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                        if (descriptor != null)
                            services.Remove(descriptor);

                        services.AddDbContext<AppDbContext>(options =>
                            options.UseInMemoryDatabase("AuthorizationIntegrationTestsDb"));

                        var sp = services.BuildServiceProvider();
                        using var scope = sp.CreateScope();

                        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        db.Database.EnsureCreated();

                        if (!db.Users.Any(u => u.Email == "admin@integration.test"))
                        {
                            db.Users.AddRange(
                                new UserModel
                                {
                                    Id = Guid.NewGuid(),
                                    FirstName = "Admin",
                                    LastName = "Test",
                                    Email = "admin@integration.test",
                                    Password = BCrypt.Net.BCrypt.HashPassword("AdminPass123"),
                                    Role = "Admin"
                                },
                                new UserModel
                                {
                                    Id = Guid.NewGuid(),
                                    FirstName = "Standard",
                                    LastName = "Test",
                                    Email = "user@integration.test",
                                    Password = BCrypt.Net.BCrypt.HashPassword("UserPass123"),
                                    Role = "User"
                                }
                            );

                            db.SaveChanges();
                        }
                    });
                });

            _anonClient = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _anonClient?.Dispose();
            _factory?.Dispose();
        }
        private async Task<string> GetTokenAsync(string email, string password)
        {
            var response = await _anonClient.PostAsJsonAsync("/api/auth/login", new LoginDto
            {
                Email = email,
                Password = password
            });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ResponseModel<string>>();

            return result!.Data!;
        }
        private HttpClient CreateClientWithToken(string? token)
        {
            var client = _factory.CreateClient();

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }
        [Test]
        public async Task GetUsers_WithoutToken_Returns401()
        {
            var client = CreateClientWithToken(null);

            var response = await client.GetAsync("/api/user");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task DeleteUser_WithoutToken_Returns401()
        {
            var client = CreateClientWithToken(null);

            var response = await client.DeleteAsync($"/api/user/{Guid.NewGuid()}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task DeleteUser_WithInvalidToken_Returns401()
        {
            var client = CreateClientWithToken("bu-gecerli-olmayan-bir-token");

            var response = await client.DeleteAsync($"/api/user/{Guid.NewGuid()}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task DeleteUser_WithNonAdminToken_Returns403()
        {
            var userToken = await GetTokenAsync("user@integration.test", "UserPass123");
            var client = CreateClientWithToken(userToken);

            var response = await client.DeleteAsync($"/api/user/{Guid.NewGuid()}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
        [Test]
        public async Task CreateUser_WithNonAdminToken_Returns403()
        {
            var userToken = await GetTokenAsync("user@integration.test", "UserPass123");
            var client = CreateClientWithToken(userToken);

            var response = await client.PostAsJsonAsync("/api/user", new UserCreateDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "shouldnotbecreated@test.com",
                Password = "irrelevant"
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
        [Test]
        public async Task GetUsers_WithValidNonAdminToken_Returns200()
        {
            var userToken = await GetTokenAsync("user@integration.test", "UserPass123");
            var client = CreateClientWithToken(userToken);

            var response = await client.GetAsync("/api/user");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public async Task CreateUser_WithAdminToken_Returns200()
        {
            var adminToken = await GetTokenAsync("admin@integration.test", "AdminPass123");
            var client = CreateClientWithToken(adminToken);

            var response = await client.PostAsJsonAsync("/api/user", new UserCreateDto
            {
                FirstName = "Yeni",
                LastName = "Kullanici",
                Email = $"yeni_{Guid.NewGuid()}@test.com",
                Password = "GecerliSifre123"
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public async Task DeleteUser_WithAdminToken_Returns200()
        {
            var adminToken = await GetTokenAsync("admin@integration.test", "AdminPass123");
            var client = CreateClientWithToken(adminToken);
            var createResponse = await client.PostAsJsonAsync("/api/user", new UserCreateDto
            {
                FirstName = "Silinecek",
                LastName = "Kullanici",
                Email = $"silinecek_{Guid.NewGuid()}@test.com",
                Password = "GecerliSifre123"
            });

            createResponse.EnsureSuccessStatusCode();

            var created = await createResponse
                .Content
                .ReadFromJsonAsync<ResponseModel<UserDetailsDto>>();

            var response = await client.DeleteAsync($"/api/user/{created!.Data!.Id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}