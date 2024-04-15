using Moq;
using RockPaperScissorAPI.Models.Domain;
using RockPaperScissorAPI.Models.Enums;
using RockPaperScissorAPI.Services;
using RockPaperScissorAPI.Services.Interfaces;

namespace RockPaperScissorsTest;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _mockRepository;
    private readonly IGameService _gameService;

    public GameServiceTests()
    {
        _mockRepository = new Mock<IGameRepository>();
        _gameService = new GameService(_mockRepository.Object);
    }

}

