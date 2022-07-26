namespace ScreenServer.Shared;

public class GameVersion
{
    public static readonly GameVersion Zero = new();

    private GameVersion()
    {
        
    }

    public GameVersion(double v, uint r)
    {
        Version = v;
        Revision = r;
    }

    public double Version { get; set; } = 0.0;
    public uint Revision { get; set; } = 0;

    public static GameVersion? FromVersionText(string versionText)
    {
        var tokens = versionText.Split('/');
        if (double.TryParse(tokens[0], out var v) && uint.TryParse(tokens[1], out var r))
        {
            return new GameVersion(v, r);
        }
        else
        {
            return null;
        }
    }


    public override string ToString()
    {
        return $"{Version}/{Revision}";
    }
}
