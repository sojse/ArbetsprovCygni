using System.Text.Encodings.Web;

namespace RockPaperScissorAPI.Utils;

public class UserInputSanitizer
{
    public static string Sanitize(string input)
    {
        return HtmlEncoder.Default.Encode(input);
    }
}
