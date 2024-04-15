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

    public async Task<Game?> CreateGame(Game game)
    {
        try
        {
            await Task.Run(() => _games.Add(game));
            return game;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while creating game: {ex.Message}");
            return null;
        }
    }

    public async Task<Game?> GetGame(Guid id)
    {
        return await Task.Run(() => _games.FirstOrDefault(g => g.Id == id));
    }

    public async Task<Game?> UpdateGame(Game game)
    {
        var existingGame = await GetGame(game.Id);
        if (existingGame != null)
        {
            existingGame.State = game.State;
            existingGame.Player1 = game.Player1;
            existingGame.Player2 = game.Player2;
            existingGame.Winner = game.Winner;

            return existingGame;
        }

        return null;
    }
}