namespace Xacte.Common.Hosting.Api.Configurations
{
    /// <summary>
    /// Xacte supported compression providers.
    /// </summary>
    [Flags]
    public enum CompressionProviders
    {
        /// <summary>
        /// Brotli compression provider
        /// </summary>
        Brotli = 1,
        /// <summary>
        /// GZip compression provider
        /// </summary>
        Gzip = 2
    }
}
