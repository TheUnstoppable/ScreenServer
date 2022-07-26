namespace ScreenServer.Shared;

[Table("accounts")]
public class AccountModel
{
    public static readonly AccountModel Default = new();

    [Key]
    public Guid ID { get; set; } = Guid.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public bool LocalAccount { get; set; } = false;
}