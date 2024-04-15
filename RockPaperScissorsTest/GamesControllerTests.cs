using Microsoft.AspNetCore.Mvc;
using Moq;
using RockPaperScissorAPI.Controllers;
using RockPaperScissorAPI.Models.DTO;
using RockPaperScissorAPI.Models.Enums;
using RockPaperScissorAPI.Services;

namespace RockPaperScissorsTest;

public class GamesControllerTests
{
    private readonly GamesController _controller;
    private readonly Mock<IGameService> _mockService;

    public GamesControllerTests()
    {
        _mockService = new Mock<IGameService>();
        _controller = new GamesController(_mockService.Object);
    }

    [Fact]
    public async Task GetGame_ReturnsGameId_WhenGameExists()
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

    [Fact]
    public async Task CreateGame_ShouldCreateGame_WhenGivenValidInformation()
    {
        var request = new GameRequestDto { PlayerName = "Alice" };
        var gameId = Guid.NewGuid();
        var gameResponse = new GameResponseDto { Id = gameId };

        _mockService.Setup(x => x.CreateGame(It.IsAny<string>()))
                    .ReturnsAsync(gameId);

        var result = await _controller.CreateGame(request);

        var createdAtActionResult = Assert.IsType<CreatedResult>(result.Result);
        var gameDto = Assert.IsType<GameResponseDto>(createdAtActionResult.Value);
        Assert.Equal(gameId, gameDto.Id);
    }

    [Fact]
    public async Task CreateGame_ReturnsBadRequest_WhenGivenInvalidInformation()
    {
        GameRequestDto request = new GameRequestDto { PlayerName = "" };

        var result = await _controller.CreateGame(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Player name is required", badRequestResult.Value);
    }
}