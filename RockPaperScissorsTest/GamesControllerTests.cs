using Microsoft.AspNetCore.Mvc;
using Moq;
using RockPaperScissorAPI.Controllers;
using RockPaperScissorAPI.Models.DTO;
using RockPaperScissorAPI.Models.Enums;
using RockPaperScissorAPI.Services.Interfaces;

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

    [Fact]
    public async Task JoinGame_ShouldAddPlayerToGame_WhenJoinedSuccesfully()
    {
        var request = new GameRequestDto { PlayerName = "Alice" };
        var gameId = Guid.NewGuid();

        _mockService.Setup(x => x.JoinGame(gameId, request.PlayerName))
                    .ReturnsAsync(JoinGameResult.Success);

        var result = await _controller.JoinGame(gameId, request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Player joined successfully.", okResult.Value);

    }

    [Fact]
    public async Task JoinGame_ReturnsGameNotFound_WhenGameDoesNotExist()
    {
        Guid gameId = Guid.NewGuid();
        var request = new GameRequestDto { PlayerName = "Alice" };

        _mockService.Setup(x => x.JoinGame(gameId, request.PlayerName))
                    .ReturnsAsync(JoinGameResult.GameNotFound);

        var result = await _controller.JoinGame(gameId, request);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task JoinGame_ReturnsBadRequest_WhenBothPLayersEnterSameName()
    {
        Guid gameId = Guid.NewGuid();
        var request = new GameRequestDto { PlayerName = "Alice" };

        _mockService.Setup(x => x.JoinGame(gameId, request.PlayerName))
                    .ReturnsAsync(JoinGameResult.MustEnterUniqueName);

        var result = await _controller.JoinGame(gameId, request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Must enter unique name.", badRequestResult.Value);
    }

    [Fact]
    public async Task JoinGame_ReturnsBadRequest_WhenGameIsFull()
    {
        Guid gameId = Guid.NewGuid();
        var request = new GameRequestDto { PlayerName = "Alice" };

        _mockService.Setup(x => x.JoinGame(gameId, request.PlayerName))
                    .ReturnsAsync(JoinGameResult.GameAlreadyFull);

        var result = await _controller.JoinGame(gameId, request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("The game is already full.", badRequestResult.Value);
    }

    [Fact]
    public async Task MakeMove_ReturnsValidRequest_WhenMoveWasSuccessfull()
    {
        Guid gameId = Guid.NewGuid();
        var request = new MoveRequestDto { PlayerName = "Alice", Move = "Rock" };

        _mockService.Setup(x => x.MakeMove(gameId, request))
                    .ReturnsAsync(MoveResult.Success);

        var result = await _controller.MakeMove(gameId, request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Move successful.", okResult.Value);
    }

    [Fact]
    public async Task MakeMove_ReturnsBadRequest_WhenPlayerNotFound()
    {
        Guid gameId = Guid.NewGuid();
        var request = new MoveRequestDto { PlayerName = "Alice", Move = "Rock" };

        _mockService.Setup(x => x.MakeMove(gameId, request))
                    .ReturnsAsync(MoveResult.PlayerNotFound);

        var result = await _controller.MakeMove(gameId, request);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    

    [Fact]
    public async Task MakeMove_ReturnsBadRequest_WhenInvalidMove()
    {
        Guid gameId = Guid.NewGuid();
        var request = new MoveRequestDto { PlayerName = "Alice", Move = "RRock" };

        _mockService.Setup(x => x.MakeMove(gameId, request))
                    .ReturnsAsync(MoveResult.InvalidMove);

        var result = await _controller.MakeMove(gameId, request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task MakeMove_ReturnsBadRequest_WhenPlayerAlreadyMoved()
    {
        Guid gameId = Guid.NewGuid();
        var request = new MoveRequestDto { PlayerName = "Alice", Move = "Rock" };

        _mockService.Setup(x => x.MakeMove(gameId, request))
                    .ReturnsAsync(MoveResult.PlayerAlreadyMoved);

        var result = await _controller.MakeMove(gameId, request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task MakeMove_ReturnsBadRequest_WhenGameAlreadyFinished()
    {
        Guid gameId = Guid.NewGuid();
        var request = new MoveRequestDto { PlayerName = "Alice", Move = "Rock" };

        _mockService.Setup(x => x.MakeMove(gameId, request))
                    .ReturnsAsync(MoveResult.GameFinished);

        var result = await _controller.MakeMove(gameId, request);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}