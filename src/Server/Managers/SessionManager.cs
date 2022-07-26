using System.Security.Principal;

namespace ScreenServer.Server.Managers
{
    public class SessionManager
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ScreenServerDatabaseContext _context;

        public SessionManager(ILogger logger, IConfiguration configuration, ScreenServerDatabaseContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        public async Task InvalidateOutdatedSessionsAsync()
        {
            _logger.ForContext("SourceContext", nameof(SessionManager))
                .Verbose("Deleting outdated sessions...");

            var outdatedSessions = _context.Sessions
                .Where(x => x.Expiration < DateTime.Now)
                .ToArray();
            
            _context.Sessions.RemoveRange(outdatedSessions);
            await _context.SaveChangesAsync();

            _logger.ForContext("SourceContext", nameof(SessionManager))
                .Verbose("Deleted {count} outdated sessions.", outdatedSessions.Length);
        }

        public async Task<SessionModel> CreateSessionAsync(AccountModel account, IPAddress remoteAddress)
        {
            _logger.ForContext("SourceContext", nameof(SessionManager))
                .Verbose("Creating session for account {account} for {address}...", account, remoteAddress);
            
            SessionModel model = new()
            {
                Account = account,
                Expiration = DateTime.Now.AddSeconds(_configuration.GetValue<long>("Session.SessionDuration")),
                ID = Guid.NewGuid(),
                LoginAddress = remoteAddress
            };

            _context.Sessions.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<SessionModel?> FindSessionAsync(Guid sessionId)
        {
            await InvalidateOutdatedSessionsAsync();

            return _context.Sessions
                .FirstOrDefault(x => x.ID == sessionId);
        }

        public async Task DeleteSessionAsync(SessionModel session)
        {
            _logger.ForContext("SourceContext", nameof(SessionManager))
                    .Verbose("Deleting session {session}...", session);

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSessionsAsync(AccountModel account)
        {
            _logger.ForContext("SourceContext", nameof(SessionManager))
                    .Verbose("Deleting sessions for account {account}...", account);

            var sessions = _context.Sessions
                .Where(x => x.Account == account)
                .ToArray();

            _context.Sessions.RemoveRange(sessions);
            await _context.SaveChangesAsync();
        }
    }
}
