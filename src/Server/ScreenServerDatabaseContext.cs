public class ScreenServerDatabaseContext : DbContext
{
    private IConfiguration _configuration;

    public ScreenServerDatabaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (_configuration.GetSection("Database") is IConfigurationSection section)
            {
                string type = section.GetValue<string>("Provider");
                if (type.Equals("SQLite", StringComparison.OrdinalIgnoreCase))
                {
                    optionsBuilder.UseSqlite(section.GetValue<string>("ConnectionString"));
                }
                else if (type.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
                {
                    string address = section.GetValue<string>("Address");
                    string port = section.GetValue<string>("Port");
                    string user = section.GetValue<string>("User");
                    string pass = section.GetValue<string>("Password");
                    string db = section.GetValue<string>("Database");
                    var connString = $"Server={address};Port={port};Uid={user};Pwd={pass};Database={db}";
                    optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
                }
                else
                {
                    string pass = section.GetValue<string>("Password");
                    if (pass is null)
                    {
                        optionsBuilder.UseSqlite("Data Source=ScreenServer.db;Version=3");
                    }
                    else
                    {
                        optionsBuilder.UseSqlite($"Data Source=ScreenServer.db;Version=3;Password={pass}");
                    }
                }
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionModel>()
            .Property(p => p.Account)
            .HasConversion(a => a.ID, b => Accounts.Find(b));


        modelBuilder.Entity<ScreenshotModel>()
           .Property(p => p.GameVersion)
           .HasConversion(a => a.ToString(), b => GameVersion.FromVersionText(b));

        modelBuilder.Entity<ScreenshotModel>()
           .Property(p => p.Server)
           .HasConversion(a => a == null ? Guid.Empty : a.ID, b => b == Guid.Empty ? null : Servers.Find(b));

        modelBuilder.Entity<ScreenshotModel>()
            .Property(p => p.ServerAddress)
            .HasConversion(a => a.ToString(), b => IPEndPoint.Parse(b));
    }

    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<SessionModel> Sessions { get; set; }
    public DbSet<ServerModel> Servers { get; set; }
    public DbSet<ScreenshotModel> Screenshots { get; set; }
}