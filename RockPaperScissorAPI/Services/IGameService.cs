using RockPaperScissorAPI.Models.Domain;

namespace RockPaperScissorAPI.Services;

public interface IGameService
{
    Guid CreateGame(string playerName);
    string? GetGameById(Guid id);
}
