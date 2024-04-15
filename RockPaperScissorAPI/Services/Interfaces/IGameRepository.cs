using RockPaperScissorAPI.Models.Domain;

namespace RockPaperScissorAPI.Services.Interfaces;

public interface IGameRepository
{
    Task<Game?> CreateGame(Game game);
    Task<Game?> GetGame(Guid id);
    Task<Game?> UpdateGame(Game game);
}
