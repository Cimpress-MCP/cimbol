// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
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
        /// specialized to hold <see cref="ILocalValue"/> objects.
        /// </summary>
        public static ConstructorInfo DictionaryConstructorInfo { get; } =
            typeof(Dictionary<string, ILocalValue>).GetConstructor(new[] { typeof(IEqualityComparer<string>) });

        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for a <see cref="List{T}"/>.
        /// specialized to hold <see cref="CimbolRuntimeException"/> objects.
        /// </summary>
        public static ConstructorInfo ExceptionListConstructorInfo { get; } =
            typeof(List<CimbolRuntimeException>).GetConstructor(new[] { typeof(int) });

        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for a <see cref="Dictionary{TKey,TValue}"/>
        /// specialized to hold <see cref="ObjectValue"/> objects.
        /// </summary>
        public static ConstructorInfo ModuleDictionaryConstructorInfo { get; } =
            typeof(Dictionary<string, ObjectValue>).GetConstructor(new[] { typeof(IEqualityComparer<string>) });

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for adding items to a dictionary of local values.
        /// specialized to hold <see cref="ILocalValue"/> objects.
        /// </summary>
        public static MethodInfo DictionaryAddInfo { get; } =
            typeof(Dictionary<string, ILocalValue>).GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for adding items to a <see cref="Dictionary{TKey,TValue}"/>
        /// specialized to hold <see cref="ObjectValue"/> objects.
        /// </summary>
        public static MethodInfo ModuleDictionaryAddInfo { get; } =
            typeof(Dictionary<string, ObjectValue>).GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for awaiting multiple tasks at once.
        /// </summary>
        public static MethodInfo TaskWhenAllInfo { get; } =
            typeof(Task).GetMethod("WhenAll", new[] { typeof(Task<ILocalValue>[]) });
    }
}