using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Runtime.Functions
{
    /// <summary>
    /// A collection of functions that work with built-in C# functionality.
    /// </summary>
    internal static class StandardFunctions
    {
        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for a <see cref="Dictionary{TKey,TValue}"/>.
        /// </summary>
        public static ConstructorInfo DictionaryConstructorInfo { get; } =
            typeof(Dictionary<string, ILocalValue>).GetConstructor(new[] { typeof(IEqualityComparer<string>) });

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for adding items to a dictionary of local values.
        /// </summary>
        public static MethodInfo DictionaryAddInfo { get; } =
            typeof(Dictionary<string, ILocalValue>).GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for awaiting multiple tasks at once.
        /// </summary>
        public static MethodInfo TaskWhenAllInfo { get; } =
            typeof(Task).GetMethod("WhenAll", new[] { typeof(Task<ILocalValue>[]) });
    }
}