using RockPaperScissorAPI.Models.Domain;
using RockPaperScissorAPI.Services.Interfaces;

namespace RockPaperScissorAPI.Repositories;

public class InMemoryGameRepository : IGameRepository
{
    private readonly Dictionary<Guid, Game> _games;

    public InMemoryGameRepository()
    {
        _games = new Dictionary<Guid, Game>();
    }

    public async Task<Game?> CreateGame(Game game)
    {
        try
        {
            await Task.Run(() => _games.Add(game.Id, game));
            return game;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while creating game: {ex.Message}");
            return null;
        }
    }

    public async Task DeleteGame(Guid id)
    {

        await Task.Run(() => _games.Remove(id));
    }

    public async Task<Game?> GetGame(Guid id)
    {
        Game? game = null;
        await Task.Run(() =>
        {
            _games.TryGetValue(id, out game);
        });
        return game;
    }

    public async Task<List<Game>> GetGames()
    {
        return await Task.Run(() => _games.Values.ToList());
    }

    public async Task<Game?> UpdateGame(Game game)
    {
        if (_games.ContainsKey(game.Id))
        {
           await Task.Run(() => _games[game.Id] = game);
            return game;
        }

        return null;
    }
}