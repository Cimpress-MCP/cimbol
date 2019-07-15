﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cimpress.Cimbol.Runtime.Types;

[assembly:InternalsVisibleTo("Cimpress.Cimbol.UnitTests")]

namespace Cimpress.Cimbol
{
    /// <summary>
    /// A complete Cimbol program intended to be compiled and executed.
    /// </summary>
    public sealed class Program
    {
        private readonly Dictionary<string, Argument> _arguments;

        private readonly Dictionary<string, Constant> _constants;

        private readonly Dictionary<string, Module> _modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        public Program()
        {
            _arguments = new Dictionary<string, Argument>(StringComparer.OrdinalIgnoreCase);

            _constants = new Dictionary<string, Constant>(StringComparer.OrdinalIgnoreCase);

            _modules = new Dictionary<string, Module>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Create an argument and add it to the program.
        /// </summary>
        /// <param name="argumentName">The name of the argument to create.</param>
        /// <returns>The created argument.</returns>
        public Argument AddArgument(string argumentName)
        {
            if (argumentName == null)
            {
                throw new ArgumentNullException(nameof(argumentName));
            }

            if (_arguments.ContainsKey(argumentName))
            {
                // Disallow duplicate resource names.
                throw new NotSupportedException();
            }

            var argument = new Argument(this, argumentName);

            _arguments[argumentName] = argument;

            return argument;
        }

        /// <summary>
        /// Create a constant and add it to the program.
        /// </summary>
        /// <param name="constantName">The name of the constant to create.</param>
        /// <param name="constantValue">The value of the constant to create.</param>
        /// <returns>The created constant.</returns>
        public Constant AddConstant(string constantName, ILocalValue constantValue)
        {
            if (constantName == null)
            {
                throw new ArgumentNullException(nameof(constantName));
            }

            if (_constants.ContainsKey(constantName))
            {
                // Disallow duplicate resource names.
                throw new NotSupportedException();
            }

            var constant = new Constant(this, constantName, constantValue);

            _constants[constantName] = constant;

            return constant;
        }

        /// <summary>
        /// Create a module and add it to the program.
        /// </summary>
        /// <param name="moduleName">The name of the module to create.</param>
        /// <returns>The created module.</returns>
        public Module AddModule(string moduleName)
        {
            if (moduleName == null)
            {
                throw new ArgumentNullException(nameof(moduleName));
            }

            if (_modules.ContainsKey(moduleName))
            {
                // Disallow duplicate resource names.
                throw new NotSupportedException();
            }

            var module = new Module(this, moduleName);

            _modules[moduleName] = module;

            return module;
        }

        /// <summary>
        /// Try and retrieve a argument from the program by name.
        /// </summary>
        /// <param name="argumentName">The argument name to look up by.</param>
        /// <param name="argument">The result of getting the argument out of the program.</param>
        /// <returns>True if the argument was retrieved, false otherwise.</returns>
        public bool TryGetArgument(string argumentName, out Argument argument)
        {
            return _arguments.TryGetValue(argumentName, out argument);
        }

        /// <summary>
        /// Try and retrieve a constant from the program by name.
        /// </summary>
        /// <param name="constantName">The constant name to look up by.</param>
        /// <param name="constant">The result of getting the constant out of the program.</param>
        /// <returns>True if the constant was retrieved, false otherwise.</returns>
        public bool TryGetConstant(string constantName, out Constant constant)
        {
            return _constants.TryGetValue(constantName, out constant);
        }

        /// <summary>
        /// Try and retrieve a module from the program by name.
        /// </summary>
        /// <param name="moduleName">The module name to look up by.</param>
        /// <param name="module">The result of getting the module out of the program.</param>
        /// <returns>True if the module was retrieved, false otherwise.</returns>
        public bool TryGetModule(string moduleName, out Module module)
        {
            return _modules.TryGetValue(moduleName, out module);
        }
    }
}