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
            Players = new List<Player> { new Player { Name = playerName } }, // Add initial player
            State = GameState.WaitingForPlayer
        };
        _gameRepository.CreateGame(newGame);

        return gameId;
    }
}
