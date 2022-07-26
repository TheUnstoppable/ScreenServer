namespace ScreenServer.Shared;

public class ScreenshotModel
{
    public static readonly ScreenshotModel Default = new();

    [Key]
    public Guid ID { get; set; } = Guid.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.MinValue;
    public GameVersion GameVersion { get; set; } = GameVersion.Zero;
    public ServerModel? Server { get; set; } = null;
    public IPEndPoint ServerAddress { get; set; } = IPEndPointExtensions.Zero;
}
