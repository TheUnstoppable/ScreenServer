namespace ScreenServer.Shared;

[Table("servers")]
public class ServerModel
{
    public static readonly ServerModel Default = new();

    [Key]
    public Guid ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}
