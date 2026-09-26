using System.IO;

namespace OmegaExplorer.Server.Extensions;

public static class StreamExtension
{
    public static byte[] UseStreamDotReadMethod(this Stream stream)
    {
        byte[] bytes;
        List<byte> totalStream = new();
        byte[] buffer = new byte[32];
        int read;
        while ((read = stream.Read(buffer: buffer, offset: 0, count: buffer.Length)) > 0)
        {
            totalStream.AddRange(collection: buffer.Take(count: read));
        }

        bytes = totalStream.ToArray();
        return bytes;
    }
}