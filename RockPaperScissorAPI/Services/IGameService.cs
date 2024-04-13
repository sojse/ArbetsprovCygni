namespace RockPaperScissorAPI.Services;

public interface IGameService
{
    Guid CreateGame(string playerName);
}
