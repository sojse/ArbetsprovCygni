using RockPaperScissorAPI.Models.Domain;
using RockPaperScissorAPI.Models.DTO;
using RockPaperScissorAPI.Models.Enums;
using RockPaperScissorAPI.Services.Interfaces;

namespace RockPaperScissorAPI.Services;

public class GameService : IGameService
{

    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<Guid> CreateGame(string playerName)
    {
        Guid gameId = Guid.NewGuid();

        var newGame = new Game
        {
            Id = gameId,
            Player1 = new Player { Name = playerName },
            State = GameState.WaitingForPlayerToJoin,
            LatestActiveTime = DateTime.UtcNow,
        };

        var game = await _gameRepository.CreateGame(newGame);

        if (game == null)
        {
            return Guid.Empty;
        }

        return game.Id;
    }

    public async Task<string?> GetGameById(Guid id)
    {
        var foundGame = await _gameRepository.GetGame(id);

        if (foundGame != null)
        {
            if (foundGame.State == GameState.Finished)
            {
                string winnerOrDraw = DetermineWinnerOrDraw(foundGame);
                return $"{foundGame.State}: {winnerOrDraw}";
            }
            else
            {
                return foundGame.State.ToString();
            }
        }
        else
        {
            return null;
        }
    }

    public async Task<JoinGameResult> JoinGame(Guid gameId, string playerName)
    {
        var game = await _gameRepository.GetGame(gameId);

        if (game == null)
        {
            return JoinGameResult.GameNotFound;
        }

        if (game.State == GameState.BothPlayersHaveJoined)
        {
            return JoinGameResult.GameAlreadyFull;
        }

        else if (playerName == game.Player1.Name)
        {
            return JoinGameResult.MustEnterUniqueName;
        }

        game.State = GameState.BothPlayersHaveJoined;
        game.Player2 = new Player { Name = playerName };
        game.LatestActiveTime = DateTime.UtcNow;

        await _gameRepository.UpdateGame(game);

        return JoinGameResult.Success;
    }

    public async Task<MoveResult> MakeMove(Guid gameId, MoveRequestDto request)
    {
        var game = await _gameRepository.GetGame(gameId);

        if (game == null)
        {
            return MoveResult.GameNotFound;
        }

        var player = FindPlayer(game, request.PlayerName);
        if (player == null)
        {
            return MoveResult.PlayerNotFound;
        }

        if (game.Player1?.Move != null && game.Player2?.Move != null)
        {
            game.State = GameState.Finished;
            DetermineWinner(game);
            return MoveResult.GameFinished;
        }
        else
        {
            game.State = GameState.WaitingForOtherPlayerToMove;
        }

        var validationResult = ValidateAndSetMove(player, request.Move);
        if (validationResult != MoveResult.Success)
        {
            return validationResult;
        }

        game.LatestActiveTime = DateTime.UtcNow;

        await _gameRepository.UpdateGame(game);

        return MoveResult.Success;
    }

    public async Task CleanupExpiredGames()
    {
        var currentTime = DateTime.UtcNow;
        var games = await _gameRepository.GetGames();

        var expiredGames = games.Where(game => (currentTime - game.LatestActiveTime).TotalHours >= 24).ToList();

        foreach (var game in expiredGames)
        {
            await _gameRepository.DeleteGame(game.Id);
        }
    }

    private string DetermineWinnerOrDraw(Game game)
    {
        if (game.Winner != null)
        {
            return $"{game.Winner.Name} is the winner!";
        }

        return "It's a draw!";
    }
    private Player? FindPlayer(Game game, string playerName)
    {
        if (game.Player1?.Name == playerName)
        {
            return game.Player1;
        }
        else if (game.Player2?.Name == playerName)
        {
            return game.Player2;
        }
        else
        {
            return null;
        }
    }

    private MoveResult ValidateAndSetMove(Player player, string move)
    {
        if (player.Move != null)
        {
            return MoveResult.PlayerAlreadyMoved;
        }

        bool isValidMove = Enum.TryParse<MoveType>(move, true, out MoveType moveType);
        if (isValidMove)
        {
            player.Move = moveType;
            return MoveResult.Success;
        }
        else
        {
            return MoveResult.InvalidMove;
        }
    }

    private void DetermineWinner(Game game)
    {
        var player1Move = game.Player1.Move;
        var player2Move = game.Player2.Move;

        if (player1Move == player2Move)
        {
            game.Winner = null;
        }
        else if ((player1Move == MoveType.Rock && player2Move == MoveType.Scissors) ||
                 (player1Move == MoveType.Scissors && player2Move == MoveType.Paper) ||
                 (player1Move == MoveType.Paper && player2Move == MoveType.Rock))
        {
            game.Winner = game.Player1;
        }
        else
        {
            game.Winner = game.Player2;
        }
    }
}
