namespace Xacte.Common.Exceptions.Enums
{
    public enum SeverityKind
    {
        /// <summary>
        /// Information
        /// </summary>
        Information,

        /// <summary>
        /// Warning
        /// </summary>
        Warning,

        /// <summary>
        /// Error
        /// </summary>
        Error,

        /// <summary>
        /// A critical error cannot be recovered, don't try it again unless you change the parameters
        /// </summary>
        Critical,

        /// <summary>
        /// A transient error, please wait and retry
        /// </summary>
        Retryable,

        /// <summary>
        /// This is the end, must suicide the current process, there won't be any recovery.
        /// </summary>
        /// <remarks>For instance, out-of-diskspace error</remarks>
        Lethal
    }

}
