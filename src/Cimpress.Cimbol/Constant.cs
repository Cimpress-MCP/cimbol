using System;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// A constant value to be used within the context of a <see cref="Program"/>.
    /// </summary>
    public class Constant : IResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Constant"/> class.
        /// </summary>
        /// <param name="program">The program that the constant belongs to.</param>
        /// <param name="name">The name of the constant.</param>
        /// <param name="value">The value of the constant.</param>
        internal Constant(Program program, string name, ILocalValue value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            Program = program ?? throw new ArgumentNullException(nameof(program));

            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The name of the constant.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The program that the constant belongs to.
        /// </summary>
        public Program Program { get; }

        /// <summary>
        /// THe value of the constant.
        /// </summary>
        public ILocalValue Value { get; }
    }
}