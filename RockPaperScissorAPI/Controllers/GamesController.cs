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
