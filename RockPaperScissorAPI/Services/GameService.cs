using RockPaperScissorAPI.Models.Domain;
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

    public Guid CreateGame(string playerName)
    {
        Guid gameId = Guid.NewGuid();

        var newGame = new Game
        {
            Id = gameId,
            Player1 =  new Player { Name = playerName },
            State = GameState.WaitingForPlayerToJoin
        };
        _gameRepository.CreateGame(newGame);

        return gameId;
    }

    public string? GetGameById(Guid id)
    {

        var foundGame = _gameRepository.GetGame(id);

        if (foundGame != null)
        {
            if (foundGame.State == GameState.Finished)
            {
                string winnerOrDraw = DetermineWinnerOrDraw(foundGame);

                return $"{foundGame.State}: {winnerOrDraw}";
            }
            else
            {
                // Game is not finished, return only the game state
                return foundGame.State.ToString();
            }
        }
        else
        {
            return null;
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
}
