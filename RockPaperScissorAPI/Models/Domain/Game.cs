using RockPaperScissorAPI.Models.Enums;

namespace RockPaperScissorAPI.Models.Domain;

public class Game
{
    public required Guid Id { get; set; }

    public required Player Player1 { get; set; }

    public Player? Player2 { get; set; }

    public Player? Winner { get; set; }

    public required GameState State { get; set; }

    public GameResult? Result { get; set; }

}
