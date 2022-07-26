namespace ScreenServer.Server.Utils;

public static class StringExtensions
{
    public static string ToMD5Hash(this string text)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.Default.GetBytes(text);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
