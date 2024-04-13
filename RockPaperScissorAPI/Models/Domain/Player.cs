using RockPaperScissorsAPI.Models.Enums;

namespace RockPaperScissorsAPI.Models.Domain;

public class Player
{
    public required string Name { get; set; }
    public MoveType? Move { get; set; }
}
