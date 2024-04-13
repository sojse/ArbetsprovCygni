using RockPaperScissorAPI.Models.Domain;
using RockPaperScissorAPI.Models.Enums;
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
}