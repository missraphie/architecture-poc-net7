using System.IO.Compression;
using System.Text;

namespace Xacte.ApiGateway.Extensions
{
    internal static class ContentExtensions
    {
        private const string BrotliTypeCode = "br";
        private const string GZipTypeCode = "gzip";
        private const string DeflateTypeCode = "deflate";

        private enum ContentEncoding
        {
            None = 0,
            Brotli = 1,
            GZip = 2,
            Deflate = 3
        }

        public static string ReadBytes(byte[] content, ICollection<string> contentEncodings)
        {
            return Encoding.UTF8.GetString(Decompress(content, contentEncodings));
        }

        private static byte[] Decompress(byte[] bytes, ICollection<string> contentEncodings)
        {
            if (!contentEncodings.Any())
            {
                return bytes;
            }

            var contentEncoding = FindContentEncoding(contentEncodings);

            using var memoryStream = new MemoryStream(bytes);
            using var outputStream = new MemoryStream();

            if (contentEncoding == ContentEncoding.Brotli)
            {
                using var decompressStream = new BrotliStream(memoryStream, CompressionMode.Decompress);
                decompressStream.CopyTo(outputStream);
            }
            if (contentEncoding == ContentEncoding.GZip)
            {
                using var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                decompressStream.CopyTo(outputStream);
            }
            if (contentEncoding == ContentEncoding.Deflate)
            {
                using var decompressStream = new DeflateStream(memoryStream, CompressionMode.Decompress);
                decompressStream.CopyTo(outputStream);
            }
            return outputStream.ToArray();
        }

        private static ContentEncoding FindContentEncoding(ICollection<string> contentEncodings)
        {
            var encoding = contentEncodings.First();
            var contentEncoding = encoding.ToLowerInvariant() switch
            {
                BrotliTypeCode => ContentEncoding.Brotli,
                DeflateTypeCode => ContentEncoding.Deflate,
                GZipTypeCode => ContentEncoding.GZip,
                _ => throw new NotSupportedException($"Decompression method not supported [{encoding}]"),
            };
            return contentEncoding;
        }
    }
}
