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
}
