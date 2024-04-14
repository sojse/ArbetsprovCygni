using RockPaperScissorAPI.Models.Domain;
using RockPaperScissorAPI.Models.DTO;
using RockPaperScissorAPI.Models.Enums;

namespace RockPaperScissorAPI.Services;

public interface IGameService
{
    Guid CreateGame(string playerName);
    string? GetGameById(Guid id);
    JoinGameResult JoinGame(Guid gameId, string playerName);
    MoveResult MakeMove(Guid gameId, MoveRequestDto request);
}
