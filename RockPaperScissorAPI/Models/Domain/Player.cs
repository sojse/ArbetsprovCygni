using RockPaperScissorAPI.Models.Enums;

namespace RockPaperScissorAPI.Models.Domain;

public class Player
{
    public required string Name { get; set; }
    public MoveType? Move { get; set; }
}
