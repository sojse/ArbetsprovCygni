using Microsoft.AspNetCore.Mvc;
using RockPaperScissorAPI.Models.DTO;
using RockPaperScissorAPI.Services;

namespace RockPaperScissorAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class GamesController : Controller
{

    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Gets one game that is matching the id
    /// </summary>
    /// <returns>The state of the game</returns>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetGame(Guid id)
    {
        // behövs felhantering för ifall GUID inte är giltig med en 400 bad request?

        var game = _gameService.GetGameById(id);

        if (game == null)
        {
            return NotFound(); // Game not found
        }

        return Ok(game);
    }

    /// <summary>
    /// Create a game and adding player one into the game
    /// </summary>
    /// <returns>The ID of the created game</returns>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<GameResponseDto> CreateGame(GameRequestDto request)
    {

        try
        {
            var newGame = _gameService.CreateGame(request.Player);

            var gameResponse = new GameResponseDto
            {
                Id = newGame,
            };

            return Created("Game was created!", gameResponse);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



}
