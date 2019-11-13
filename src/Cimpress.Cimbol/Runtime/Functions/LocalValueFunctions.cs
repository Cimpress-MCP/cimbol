using System;
using System.Collections.Generic;
using System.Reflection;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Runtime.Functions
{
    /// <summary>
    /// A collection of functions that work with local values.
    /// </summary>
    internal static class LocalValueFunctions
    {
        /// <summary>
        /// The constructor info for the constructor for the <see cref="EvaluationResult"/> class.
        /// </summary>
        internal static ConstructorInfo EvaluationResultConstructorInfo { get; } = typeof(EvaluationResult)
            .GetConstructor(new[] { typeof(Dictionary<string, ObjectValue>), typeof(List<CimbolRuntimeException>) });

        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for a <see cref="ListValue"/>.
        /// </summary>
        internal static ConstructorInfo ListValueConstructorInfo { get; } =
            typeof(ListValue).GetConstructor(new[] { typeof(ILocalValue[]) });

        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for an <see cref="ObjectValue"/>.
        /// </summary>
        internal static ConstructorInfo ObjectValueConstructorInfo { get; } =
            typeof(ObjectValue).GetConstructor(new[] { typeof(IDictionary<string, ILocalValue>) });

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for access a member of an object value.
        /// </summary>
        internal static MethodInfo AccessInfo { get; } =
            typeof(ILocalValue).GetMethod("Access", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for casting a value to a boolean.
        /// </summary>
        internal static MethodInfo CastBooleanInfo { get; } =
            typeof(ILocalValue).GetMethod("CastBoolean", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for casting a value to a number.
        /// </summary>
        internal static MethodInfo CastNumberInfo { get; } =
            typeof(ILocalValue).GetMethod("CastNumber", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for casting a value to a string.
        /// </summary>
        internal static MethodInfo CastStringInfo { get; } =
            typeof(ILocalValue).GetMethod("CastString", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for invoking a function value.
        /// </summary>
        internal static MethodInfo InvokeInfo { get; } =
            typeof(ILocalValue).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for assigning a value to an object value.
        /// </summary>
        internal static MethodInfo ObjectAssignInfo { get; } =
            typeof(ObjectValue).GetMethod("Assign", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// The cached <see cref="PropertyInfo"/> for the <see cref="Array.Length"/> property for <see cref="ILocalValue"/> arrays.
        /// </summary>
        internal static PropertyInfo ArrayLengthInfo { get; } = typeof(ILocalValue[]).GetProperty("Length");
    }
}