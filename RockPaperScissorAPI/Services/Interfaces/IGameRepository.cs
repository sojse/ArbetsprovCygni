using RockPaperScissorAPI.Models.Domain;

namespace RockPaperScissorAPI.Services.Interfaces;

public interface IGameRepository
{
    Task CreateGame(Game game);
    Task<Game?> GetGame(Guid id);
    Task UpdateGame(Game game);
}
