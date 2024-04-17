using Moq;
using RockPaperScissorAPI.Models.Domain;
using RockPaperScissorAPI.Models.DTO;
using RockPaperScissorAPI.Models.Enums;
using RockPaperScissorAPI.Services;
using RockPaperScissorAPI.Services.Interfaces;

namespace RockPaperScissorsTest;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _mockRepository;
    private readonly IGameService _gameService;

    public GameServiceTests()
    {
        _mockRepository = new Mock<IGameRepository>();
        _gameService = new GameService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateGame_ReturnsValidGuid_WhenGameCreated()
    {
        string playerName = "Alice";
        var gameId = Guid.NewGuid();
        var newGame = new Game { 
            Id = gameId, 
            Player1 = new Player { Name = playerName }, 
            State = GameState.WaitingForPlayerToJoin 
        };

        _mockRepository.Setup(repo => repo.CreateGame(It.IsAny<Game>()))
            .ReturnsAsync(newGame);

        var result = await _gameService.CreateGame(playerName);

        Assert.Equal(gameId, result);
    }

    [Fact]
    public async Task CreateGame_ReturnsEmptyGuid_WhenGameCreationFails()
    {
        string playerName = "Alice";

        _mockRepository.Setup(repo => repo.CreateGame(It.IsAny<Game>()))
            .ReturnsAsync((Game)null); 

        var result = await _gameService.CreateGame(playerName);

        Assert.Equal(Guid.Empty, result);
    }

    [Fact]
    public async Task GetGameById_ReturnsGameState_IfGameExists()
    {
        Guid gameId = Guid.NewGuid();
        string playerName = "Alice";
        var gameStatus = GameState.WaitingForPlayerToJoin;
        var game = new Game
        {
            Id = Guid.NewGuid(),
            State = gameStatus,
            Player1 = new Player { Name = playerName }
        };

        _mockRepository.Setup(repo => repo.GetGame(It.IsAny<Guid>()))
            .ReturnsAsync(game);

        string result = await _gameService.GetGameById(gameId);

        Assert.Equal(gameStatus.ToString(), result);
    }

    [Fact]
    public async Task GetGameById_ReturnsWinnerOrDraw_WhenGameExistsAndFinished()
    {
        var gameId = Guid.NewGuid();
        var gameState = GameState.Finished;
        string playerName = "Alice";
        var game = new Game 
        { 
            Id = gameId, 
            State = gameState, 
            Player1 = new Player { Name = playerName }, 
            Winner = new Player { Name = playerName } 
        };

        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.GetGameById(gameId);

        Assert.Contains(gameState.ToString(), result);
    }

    [Fact]
    public async Task GetGameById_ReturnsNull_WhenGameDoesNotExist()
    {
        var gameId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync((Game)null);

        var result = await _gameService.GetGameById(gameId);

        Assert.Null(result);
    }

    [Fact]
    public async Task JoinGame_ReturnsSuccess_WhenGameFoundAndNotFull()
    {
        var gameId = Guid.NewGuid();
        var player1 = "Alice";
        var game = new Game
        {
            Id = gameId,
            State = GameState.WaitingForPlayerToJoin,
            Player1 = new Player { Name = player1 },
        };
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.JoinGame(gameId, "Jan");

        Assert.Equal(JoinGameResult.Success, result);
    }

    [Fact]
    public async Task JoinGame_ReturnsGameAlreadyFull_WhenGameFoundAndAlreadyFull()
    {
        var gameId = Guid.NewGuid();
        var player1 = "Alice";
        var player2 = "Jan";
        var game = new Game
        {
            Id = gameId,
            State = GameState.BothPlayersHaveJoined,
            Player1 = new Player { Name = player1 },
            Player2 = new Player { Name = player2 },
        };
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.JoinGame(gameId, "Sven");

        Assert.Equal(JoinGameResult.GameAlreadyFull, result);
    }

    [Fact]
    public async Task JoinGame_ReturnsMustEnterUniqueName_WhenBothPlayersEnterSameName()
    {
        var gameId = Guid.NewGuid();
        var playerName = "Alice";
        var game = new Game
        {
            Id = gameId,
            State = GameState.WaitingForPlayerToJoin,
            Player1 = new Player { Name = playerName }
        };
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.JoinGame(gameId, playerName);

        Assert.Equal(JoinGameResult.MustEnterUniqueName, result);
    }

    [Fact]
    public async Task JoinGame_ReturnsGameNotFound_WhenGameNotFound()
    {
        var gameId = Guid.NewGuid();
        var player1 = "Alice";
        var game = new Game
        {
            Id = gameId,
            State = GameState.BothPlayersHaveJoined,
            Player1 = new Player { Name = player1 },
        };
        _mockRepository.Setup(repo => repo.UpdateGame(game)).ReturnsAsync((Game)null);

        var result = await _gameService.JoinGame(gameId, "Jan");

        Assert.Equal(JoinGameResult.GameNotFound, result);
    }

    [Fact]
    public async Task MakeMove_ReturnsSuccess_IfMoveIsValid()
    {
        var gameId = Guid.NewGuid();
        var player1 = "Sven";
        var game = new Game { Id = gameId, Player1 = new Player { Name = player1 }, State = GameState.WaitingForPlayerToJoin };
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.MakeMove(gameId, new MoveRequestDto { PlayerName = player1, Move = "Rock" });

        Assert.Equal(MoveResult.Success, result);
    }

    [Fact]
    public async Task MakeMove_ReturnsGameNotFound_WhenGameNotFound()
    {
        var gameId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync((Game)null);

        var result = await _gameService.MakeMove(gameId, new MoveRequestDto());

        Assert.Equal(MoveResult.GameNotFound, result);
    }

    [Fact]
    public async Task MakeMove_ReturnsPlayerNotFound_WhenPlayerNotFound()
    {
        var gameId = Guid.NewGuid();
        var game = new Game { Id = gameId, Player1 = new Player { Name = "Sven" }, State = GameState.BothPlayersHaveJoined };
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.MakeMove(gameId, new MoveRequestDto { PlayerName = "NonExistingPlayer", Move = "Rock" });

        Assert.Equal(MoveResult.PlayerNotFound, result);
    }

    [Fact]
    public async Task MakeMove_ReturnsInvalidMove_IfMoveIsInvalid()
    {
        var gameId = Guid.NewGuid();
        var player1 = "Sven";
        var game = new Game { Id = gameId, Player1 = new Player { Name = player1 }, State = GameState.WaitingForPlayerToJoin };
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.MakeMove(gameId, new MoveRequestDto { PlayerName = player1, Move = "refds" });

        Assert.Equal(MoveResult.InvalidMove, result);
    }

    [Fact]
    public async Task MakeMove_ReturnsPlayerAlreadyMoved_IfPlayerAlreadyMoved()
    {
        var gameId = Guid.NewGuid();
        var player1 = "Sven";
        var game = new Game { Id = gameId, Player1 = new Player { Name = player1, Move = MoveType.Scissors }, State = GameState.BothPlayersHaveJoined };
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.MakeMove(gameId, new MoveRequestDto { PlayerName = player1, Move = "Rock" });

        Assert.Equal(MoveResult.PlayerAlreadyMoved, result);
    }

    [Fact]
    public async Task MakeMove_ReturnsGameAlreadyFinished_IfBothPlayersHaveMoved()
    {
        var gameId = Guid.NewGuid();
        var player1 = "Sven";
        var game = new Game { Id = gameId, Player1 = new Player { Name = player1, Move = MoveType.Scissors }, Player2 = new Player { Name = "Jan", Move = MoveType.Scissors }, State = GameState.BothPlayersHaveJoined };
        _mockRepository.Setup(repo => repo.GetGame(gameId)).ReturnsAsync(game);

        var result = await _gameService.MakeMove(gameId, new MoveRequestDto { PlayerName = player1, Move = "Rock" });

        Assert.Equal(MoveResult.GameFinished, result);
    }

    [Fact]
    public async Task CleanUpGames_RemovesGames_IfGameIsExpired()
    {
        var currentTime = DateTime.UtcNow;
        var expiredGame = new Game { Id = Guid.NewGuid(), LatestActiveTime = currentTime.AddHours(-25),
            Player1 = new Player { Name = "Sven" },
            State = GameState.WaitingForPlayerToJoin
        };
        var activeGame = new Game { Id = Guid.NewGuid(), LatestActiveTime = currentTime.AddHours(-12),
            Player1 = new Player { Name = "Jan" },
            State = GameState.WaitingForPlayerToJoin
        };

        var games = new List<Game> { expiredGame, activeGame };
        _mockRepository.Setup(repo => repo.GetGames()).ReturnsAsync(games);

        await _gameService.CleanupExpiredGames();

        _mockRepository.Verify(repo => repo.DeleteGame(expiredGame.Id), Times.Once);
        _mockRepository.Verify(repo => repo.DeleteGame(activeGame.Id), Times.Never);
    }
}

