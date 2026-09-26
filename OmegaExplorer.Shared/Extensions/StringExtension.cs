#region

using System.Security.Cryptography;
using System.Text;

#endregion

namespace OmegaExplorer.Server.Extensions;

public static class StringExtension
{
    public static string RemoveFirstChars(this string source, int count = 1)
    {
        return source.Remove(0, count);
    }

    public static string TransformPathSingleDashToDouble(this string source)
    {
        return source.Replace(oldValue: "\\", newValue: "\\\\");
    }

    public static string CreateSHA256(this string source)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(s: source);

        // Calcul du checksum SHA-256
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(buffer: inputBytes);

        // Conversion du tableau de bytes en une représentation hexadécimale
        StringBuilder builder = new StringBuilder();
        foreach (byte t in hashBytes)
        {
            builder.Append(value: t.ToString(format: "x2"));
        }

        string checksum = builder.ToString();

        return checksum;
    }

    public static string GetRelativeOtherString(this string source, string path)
    {
        string res = source.Replace(oldValue: path, newValue: string.Empty);
        return res;
    }
}