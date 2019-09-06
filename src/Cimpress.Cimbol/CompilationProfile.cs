namespace Cimpress.Cimbol
{
    /// <summary>
    /// Describes the level of detail that should be reported with when returning errors.
    /// </summary>
    public enum CompilationProfile
    {
        /// <summary>
        /// Return the first exception that is thrown with no additional context.
        /// </summary>
        Minimal = 0,

        /// <summary>
        /// Return the first exception that is thrown with the context of the exception.
        /// </summary>
        Trace = 1,

        /// <summary>
        /// Return all exceptions that are thrown with the context of the exceptions.
        /// </summary>
        Verbose = 2,
    }
}