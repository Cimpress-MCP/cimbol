using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// Stores the results of an evaluation.
    /// </summary>
    public class EvaluationResult
    {
        private readonly ImmutableArray<CimbolRuntimeException> _errors;

        private readonly ImmutableDictionary<string, ObjectValue> _modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationResult"/> class.
        /// </summary>
        /// <param name="modules">The list of module exports resulting from evaluating the program.</param>
        /// <param name="errors">The list of errors encountered while evaluating the program.</param>
        public EvaluationResult(Dictionary<string, ObjectValue> modules, List<CimbolRuntimeException> errors)
        {
            _errors = errors.ToImmutableArray();

            _modules = modules.ToImmutableDictionary(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// The list of errors encountered while evaluating the program.
        /// </summary>
        public IReadOnlyCollection<CimbolRuntimeException> Errors => _errors;

        /// <summary>
        /// The list of module exports resulting from evaluating the program.
        /// </summary>
        public IReadOnlyDictionary<string, ObjectValue> Modules => _modules;
    }
}