// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A collection of methods to extent the <see cref="UnaryOpType"/> enum with.
    /// </summary>
    public static class UnaryOpTypeExtensions
    {
        /// <summary>
        /// Get the operator that corresponds to the given operation type.
        /// </summary>
        /// <param name="opType">The operation type.</param>
        /// <returns>The corresponding operator.</returns>
        public static string GetOperator(this UnaryOpType opType)
        {
            switch (opType)
            {
                case UnaryOpType.Await:
                    return "await";

                case UnaryOpType.Negate:
                    return "-";

                case UnaryOpType.Not:
                    return "not";

                default:
                    throw new ArgumentOutOfRangeException(nameof(opType), opType, null);
            }
        }
    }
}