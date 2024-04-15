using Microsoft.AspNetCore.Mvc;
using Moq;
using RockPaperScissorAPI.Controllers;
using RockPaperScissorAPI.Services;

namespace RockPaperScissorsTest;

public class GamesControllerTests
{
    private GamesController _controller;
    private Mock<IGameService> _mockService;

    public GamesControllerTests()
    {
        _mockService = new Mock<IGameService>();
        _controller = new GamesController(_mockService.Object);
    }

    [Fact]
    public async Task GetGame_ReturnsGameGameId_WhenGameExists()
    {

        Guid gameId = Guid.NewGuid();
        string expectedGame = "Some game data";
        _mockService.Setup(x => x.GetGameById(gameId)).ReturnsAsync(expectedGame);

        var result = await _controller.GetGame(gameId);

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var gameResponse = okResult.Value as string;
        Assert.NotNull(gameResponse);
        Assert.Equal(expectedGame, gameResponse);
    }

    [Fact]
    public async Task GetGame_ReturnsNotFound_WhenGameDoesNotExist()
    {
        Guid gameId = Guid.NewGuid();
        string nullValue = null;
        _mockService.Setup(x => x.GetGameById(gameId)).ReturnsAsync(nullValue);

        var result = await _controller.GetGame(gameId);

        Assert.IsType<NotFoundResult>(result);
    }
}