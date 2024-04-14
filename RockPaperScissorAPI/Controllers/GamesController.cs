using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RockPaperScissorAPI.Models.DTO;
using RockPaperScissorAPI.Models.Enums;
using RockPaperScissorAPI.Services;

namespace RockPaperScissorAPI.Controllers;

[Route("[controller]")]
[ApiController]
[EnableRateLimiting("fixed")]
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
    public async Task<IActionResult> GetGame(Guid id)
    {
        var game = await _gameService.GetGameById(id);
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
    public async Task<ActionResult<GameResponseDto>> CreateGame(GameRequestDto request)
    {
        try
        {
            var newGame = await _gameService.CreateGame(request.PlayerName);
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

    /// <summary>
    /// Lets palyer 2 join the game and updates the game state
    /// </summary>
    /// <returns>A message about the join state</returns>
    [HttpPut("{id}/join")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> JoinGame(Guid id, GameRequestDto request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.PlayerName))
        {
            return BadRequest("Player name is required.");
        }

        var result = await _gameService.JoinGame(id, request.PlayerName);

        switch (result)
        {
            case JoinGameResult.Success:
                return Ok("Player joined successfully.");
            case JoinGameResult.GameNotFound:
                return NotFound();
            case JoinGameResult.GameAlreadyFull:
                return BadRequest("The game is already full.");
            default:
                return StatusCode(500, "An error occurred."); // Handle unexpected result
        }
    }

    /// <summary>
    /// Make a move in the game
    /// </summary>
    /// <returns>A message about the move state</returns>
    [HttpPut("{id}/move")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MakeMove(Guid id, MoveRequestDto request)
    {
        var result = await _gameService.MakeMove(id, request);

        switch (result)
        {
            case MoveResult.Success:
                return Ok("Move successful.");
            case MoveResult.GameNotFound:
                return NotFound();
            case MoveResult.PlayerNotFound:
                return BadRequest("Player not found in the game.");
            case MoveResult.InvalidMove:
                return BadRequest("Invalid move.");
            case MoveResult.PlayerAlreadyMoved:
                return BadRequest("Player have already made their move.");
            case MoveResult.GameFinished:
                return BadRequest("The game has already finished.");
            default:
                return StatusCode(500, "An error occurred.");
        }
    }
}
