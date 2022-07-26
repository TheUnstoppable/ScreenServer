namespace ScreenServer.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly SessionManager _sessionManager;
    private readonly ScreenServerDatabaseContext _context;

    public AccountController(ILogger logger, SessionManager sessionManager, ScreenServerDatabaseContext context)
    {
        _logger = logger;
        _sessionManager = sessionManager;
        _context = context;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(string username, string password, [FromHeader(Name = "X-Forwarded-For")]string? forwardedAddress = null)
    {
        if (await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == username) is { } user && 
            user.PasswordHash == (user.PasswordSalt + password).ToMD5Hash())
        {
            var address = forwardedAddress == null ? HttpContext.Connection.RemoteIpAddress : (IPAddress.TryParse(forwardedAddress, out var _address) ? _address : null);

            if (user.LocalAccount && address != null && address.IsLocal())
            {
                return Ok(_sessionManager.CreateSessionAsync(user, address));
            }
            else
            {
                return Unauthorized(new
                {
                    message = "Specified account is a local account, and cannot be authorized for external network."
                });
            }
        }
        else
        {
            return NotFound(new
            {
                message = "Could not find an account with specified credentials."
            });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(Guid sessionToken, [FromHeader(Name = "X-Forwarded-For")] string? forwardedAddress = null)
    {
        if (await _sessionManager.FindSessionAsync(sessionToken) is { } session)
        {
            await _sessionManager.DeleteSessionAsync(session);
            return NoContent();
        }
        else
        {
            return NotFound(new
            {
                message = "Invalid session token."
            });
        }
    }
}