namespace ScreenServer.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ScreenshotController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ScreenServerDatabaseContext _context;

    public ScreenshotController(ILogger _logger, ScreenServerDatabaseContext _context)
    {
        this._logger = _logger;
        this._context = _context;
    }

    [HttpPost("screenshot/{server}")]
    public async Task<IActionResult> PostScreenshotAsync(
        [FromForm(Name = "PlayerName")]string playerName, 
        [FromForm(Name = "Screenshot")]FormFile file, 
        [FromHeader(Name = "Referrer")]string serverUrl, 
        [FromHeader(Name = "User-Agent")]string gameVersion, 
        [FromRoute] string? server = null)
    {
        ScreenshotModel model = new()
        {
            ID = Guid.NewGuid(),
            PlayerName = playerName,
            ServerUrl = serverUrl,
            GameVersion = gameVersion,
            Server = server
        };
    }
}
