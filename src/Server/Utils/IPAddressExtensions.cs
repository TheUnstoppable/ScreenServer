namespace ScreenServer.Server;

public static class IPAddressExtensions
{
    public static bool IsLocal(this IPAddress address)
    {
        if (address is null)
            return false;

        if (address == IPAddress.Loopback || address == IPAddress.IPv6Loopback)
            return true;

        IPHostEntry entries = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var entry in entries.AddressList)
        {
            if (entry == address)
            {
                return true;
            }
        }
        return false;
    }
}
