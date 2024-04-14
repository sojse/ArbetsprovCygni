using RockPaperScissorAPI.Models.Domain;
using RockPaperScissorAPI.Services.Interfaces;

namespace RockPaperScissorAPI.Services.Repositories;

public class InMemoryGameRepository : IGameRepository
{
    private readonly List<Game> _games;

    public InMemoryGameRepository()
    {
        _games = new List<Game>();
    }

    public void CreateGame(Game game)
    {
        _games.Add(game);
    }

    public Game? GetGame(Guid id)
    {
        return _games.FirstOrDefault(g => g.Id == id);
    }

    public void UpdateGame(Game game)
    {
        var existingGame = _games.FirstOrDefault(g => g.Id == game.Id);
        if (existingGame != null)
        {
            existingGame.State = game.State;
            existingGame.Player1 = game.Player1;
            existingGame.Player2 = game.Player2;
            existingGame.Winner = game.Winner;
        }
    }
}