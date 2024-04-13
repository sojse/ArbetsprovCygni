using RockPaperScissorsAPI.Models.Enums;

namespace RockPaperScissorsAPI.Models.Domain;

public class Game
{
    public required Guid Id { get; set; }

    public required List<Player> Players { get; set; }

    public Player? Winner { get; set; }

    public required GameState State { get; set; }

    public GameResult? Result { get; set; }

}
