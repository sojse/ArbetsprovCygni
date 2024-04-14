namespace RockPaperScissorAPI.Models.Enums;

public enum MoveResult
{
    Success,
    GameNotFound,
    PlayerNotFound,
    InvalidMove,
    PlayerAlreadyMoved,
    GameFinished,
    InvalidRequest
}
