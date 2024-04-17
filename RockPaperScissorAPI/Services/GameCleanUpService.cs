namespace RockPaperScissorAPI.Services.Interfaces;

public class GameCleanUpService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(1);

    public GameCleanUpService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory; 
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_cleanupInterval);
        while (
        !stoppingToken.IsCancellationRequested &&
        await timer.WaitForNextTickAsync(stoppingToken))
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var gameService = scope.ServiceProvider.GetService<IGameService>();
                await gameService.CleanupExpiredGames();
            }
        }
        
    }
}
