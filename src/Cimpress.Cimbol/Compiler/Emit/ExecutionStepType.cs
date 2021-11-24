// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// The type of an execution step.
    /// </summary>
    public enum ExecutionStepType
    {
        /// <summary>
        /// Represents an asynchronous execution step.
        /// </summary>
        Asynchronous,

        /// <summary>
        /// Represents a synchronous execution step.
        /// </summary>
        Synchronous,
    }
}