using RockPaperScissorAPI.Models.Domain;

namespace RockPaperScissorAPI.Services.Interfaces;

public interface IGameRepository
{
    void CreateGame(Game game);
    Game? GetGame(Guid id);
}
