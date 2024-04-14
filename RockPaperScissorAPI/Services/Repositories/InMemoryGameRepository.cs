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

    public async Task CreateGame(Game game)
    {
        await Task.Run(() => _games.Add(game));
    }

    public async Task<Game?> GetGame(Guid id)
    {
        return await Task.Run(() => _games.FirstOrDefault(g => g.Id == id));
    }

    public async Task UpdateGame(Game game)
    {
        var existingGame = await GetGame(game.Id);
        if (existingGame != null)
        {
            existingGame.State = game.State;
            existingGame.Player1 = game.Player1;
            existingGame.Player2 = game.Player2;
            existingGame.Winner = game.Winner;
        }
    }
}