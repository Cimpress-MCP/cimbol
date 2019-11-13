using System.Reflection;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Runtime.Functions
{
    /// <summary>
    /// A collection of functions for use when a Cimbol program encounters an error.
    /// </summary>
    internal static class ErrorFunctions
    {
        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the argument count error.
        /// </summary>
        internal static MethodInfo ArgumentCountErrorInfo { get; } = typeof(CimbolRuntimeException).GetMethod(
            "ArgumentCountError",
            BindingFlags.NonPublic | BindingFlags.Static);
    }
}