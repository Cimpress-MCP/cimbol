// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A collection of methods to extent the <see cref="BinaryOpType"/> enum with.
    /// </summary>
    public static class BinaryOpTypeExtensions
    {
        /// <summary>
        /// Get the operator that corresponds to the given operation type.
        /// </summary>
        /// <param name="opType">The operation type.</param>
        /// <returns>The corresponding operator.</returns>
        public static string GetOperator(this BinaryOpType opType)
        {
            switch (opType)
            {
                case BinaryOpType.Add:
                    return "+";
                case BinaryOpType.And:
                    return "and";
                case BinaryOpType.Concatenate:
                    return "++";
                case BinaryOpType.Divide:
                    return "/";
                case BinaryOpType.Equal:
                    return "==";
                case BinaryOpType.GreaterThan:
                    return ">";
                case BinaryOpType.GreaterThanOrEqual:
                    return ">=";
                case BinaryOpType.LessThan:
                    return "<";
                case BinaryOpType.LessThanOrEqual:
                    return "<=";
                case BinaryOpType.Remainder:
                    return "%";
                case BinaryOpType.Multiply:
                    return "*";
                case BinaryOpType.NotEqual:
                    return "!=";
                case BinaryOpType.Or:
                    return "or";
                case BinaryOpType.Power:
                    return "^";
                case BinaryOpType.Subtract:
                    return "-";
                default:
                    throw new ArgumentOutOfRangeException(nameof(opType), opType, null);
            }
        }
    }
}