namespace ScreenServer.Shared;

public class SessionModel
{
    public static readonly SessionModel Default = new();

    [Key]
    public Guid ID { get; set; } = Guid.Empty;
    public AccountModel Account { get; set; } = AccountModel.Default;
    public DateTime Expiration { get; set; } = DateTime.MinValue;
    public IPAddress LoginAddress { get; set; } = IPAddress.None;
}
