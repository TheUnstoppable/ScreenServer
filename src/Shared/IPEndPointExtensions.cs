namespace ScreenServer.Shared;

public static class IPEndPointExtensions
{
    public static readonly IPEndPoint Zero = new IPEndPoint(IPAddress.None, IPEndPoint.MinPort);
}
