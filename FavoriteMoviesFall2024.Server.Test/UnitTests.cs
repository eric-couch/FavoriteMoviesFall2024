using FavoriteMoviesFall2024.Server.Controllers;
using FavoriteMoviesFall2024.Server.Services;
using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FavoriteMoviesFall2024.Server.Test;

public class Tests
{
    private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetMovies_ReturnsUserDto()
    {
        // Arrange
        UserDto userDto = new UserDto()
        {
            Id = "0d77e2f5-8af5-47d3-9975-5e394068a900",
            UserName = "eric.couch@example.net",
            FavoriteMovies = new List<Movie>()
            {
                new Movie()
                {
                    Id = 1,
                    imdbId = "tt0816692"
                },
                new Movie()
                {
                    Id = 2,
                    imdbId = "tt0109686"
                },
                new Movie()
                {
                    Id = 3,
                    imdbId = "tt0468569"
                }
            }
        };
        string? userName = "eric.couch@example.net";
        _userServiceMock.Setup(x => x.GetMovies(userName)).ReturnsAsync(userDto);

        UserController userController = new UserController(_userServiceMock.Object);
        // Act
        var response = await userController.GetMovies(userName);
        UserDto resUserDto = response.Data;

        // Assert
        Assert.That(resUserDto.UserName, Is.EqualTo(userName));
        Assert.That(resUserDto.FavoriteMovies.Count, Is.EqualTo(3));
        Assert.That(resUserDto.FavoriteMovies[0].imdbId, Is.EqualTo("tt0816692"));
    }
}