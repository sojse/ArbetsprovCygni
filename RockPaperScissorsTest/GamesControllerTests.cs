using Microsoft.AspNetCore.Mvc;
using Moq;
using RockPaperScissorAPI.Controllers;
using RockPaperScissorAPI.Services;

namespace RockPaperScissorsTest;

public class GamesControllerTests
{
    private GamesController _controller;
    private Mock<IGameService> _mockService;

    public GamesControllerTests()
    {
        _mockService = new Mock<IGameService>();
        _controller = new GamesController(_mockService.Object);
    }
}