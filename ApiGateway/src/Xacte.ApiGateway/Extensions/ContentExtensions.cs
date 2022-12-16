using System.IO.Compression;
using System.Text;

namespace Xacte.ApiGateway.Extensions
{
    internal static class ContentExtensions
    {
        public static string ReadBytes(byte[] content, string? contentEncoding)
        {
            return Encoding.UTF8.GetString(Decompress(content, contentEncoding));
        }

        private static byte[] Decompress(byte[] bytes, string? contentEncoding)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var outputStream = new MemoryStream();

            if (contentEncoding == "br")
            {
                using var decompressStream = new BrotliStream(memoryStream, CompressionMode.Decompress);
                decompressStream.CopyTo(outputStream);
            }
            else if (contentEncoding == "gzip")
            {
                using var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                decompressStream.CopyTo(outputStream);
            }
            return outputStream.ToArray();
        }
    }
}
