// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Runtime.Serialization;

namespace Cimpress.Cimbol.Exceptions
{
    /// <summary>
    /// An exception created when a Cimbol program has to skip the evaluation of a formula.
    /// </summary>
    [Serializable]
    public class CimbolRuntimeSkipException : CimbolRuntimeException
    {
        private const string DefaultMessage = "The evaluation of a formula was skipped.";

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeSkipException"/> class.
        /// </summary>
        public CimbolRuntimeSkipException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeSkipException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CimbolRuntimeSkipException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeSkipException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public CimbolRuntimeSkipException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeSkipException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CimbolRuntimeSkipException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}