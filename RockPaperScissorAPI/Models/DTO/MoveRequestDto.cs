using RockPaperScissorAPI.Utils;

namespace RockPaperScissorAPI.Models.DTO;

public class MoveRequestDto
{
    private string _playerName;
    public string PlayerName
    {
        get => _playerName;
        set => _playerName = UserInputSanitizer.Sanitize(value);
    }
    public string Move { get; set; }
}

