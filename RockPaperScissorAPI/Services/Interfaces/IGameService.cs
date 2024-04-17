using RockPaperScissorAPI.Models.DTO;
using RockPaperScissorAPI.Models.Enums;

namespace RockPaperScissorAPI.Services;

public interface IGameService
{
    Task<Guid> CreateGame(string playerName);
    Task<string?> GetGameById(Guid id);
    Task<JoinGameResult> JoinGame(Guid gameId, string playerName);
    Task<MoveResult> MakeMove(Guid gameId, MoveRequestDto request);
    Task CleanupExpiredGames();
}
