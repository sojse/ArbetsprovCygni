using RockPaperScissorAPI.Utils;

namespace RockPaperScissorAPI.Models.DTO;

public class GameRequestDto
{
    private string _playerName;
    public string PlayerName
    {
        get => _playerName;
        set => _playerName = UserInputSanitizer.Sanitize(value);
    }

}
