using RockPaperScissorAPI.Models.Enums;

namespace RockPaperScissorAPI.Models.DTO;

public class MoveRequestDto
{
    public required string PlayerName { get; set; }
    public required string Move { get; set; }
}

