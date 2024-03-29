﻿// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The root node for a program.
    /// </summary>
    public class ProgramNode : ISyntaxNode
    {
        private readonly ImmutableDictionary<string, ArgumentNode> _argumentTable;

        private readonly ImmutableDictionary<string, ConstantNode> _constantTable;

        private readonly ImmutableDictionary<string, ModuleNode> _moduleTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramNode"/> class.
        /// </summary>
        /// <param name="arguments">The list of arguments in the program.</param>
        /// <param name="constants">The list of constants in the program.</param>
        /// <param name="modules">The list of modules in the program.</param>
        public ProgramNode(
            IEnumerable<ArgumentNode> arguments,
            IEnumerable<ConstantNode> constants,
            IEnumerable<ModuleNode> modules)
        {
            Arguments = arguments?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(arguments));

            _argumentTable = Arguments.ToImmutableDictionary(
                argument => argument.Name,
                argument => argument,
                StringComparer.OrdinalIgnoreCase);

            Constants = constants?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(constants));

            _constantTable = Constants.ToImmutableDictionary(
                constant => constant.Name,
                constant => constant,
                StringComparer.OrdinalIgnoreCase);

            Modules = modules?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(modules));

            _moduleTable = Modules.ToImmutableDictionary(
                module => module.Name,
                module => module,
                StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The list of arguments in the program.
        /// </summary>
        public IEnumerable<ArgumentNode> Arguments { get; }

        /// <summary>
        /// The list of constants in the program.
        /// </summary>
        public IEnumerable<ConstantNode> Constants { get; }

        /// <summary>
        /// The list of modules in the program.
        /// </summary>
        public IEnumerable<ModuleNode> Modules { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            foreach (var argument in _argumentTable.Values)
            {
                yield return argument;
            }

            foreach (var constant in _constantTable.Values)
            {
                yield return constant;
            }

            foreach (var module in _moduleTable.Values)
            {
                yield return module;
            }
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            foreach (var module in _moduleTable.Values.Reverse())
            {
                yield return module;
            }

            foreach (var constant in _constantTable.Values.Reverse())
            {
                yield return constant;
            }

            foreach (var argument in _argumentTable.Values.Reverse())
            {
                yield return argument;
            }
        }

        /// <summary>
        /// Retrieve an argument by name from the program.
        /// </summary>
        /// <param name="argumentName">The name of the argument to retrieve.</param>
        /// <returns>The retrieved argument.</returns>
        public ArgumentNode GetArgument(string argumentName)
        {
            if (_argumentTable.TryGetValue(argumentName, out var argument))
            {
                return argument;
            }

            throw new KeyNotFoundException("Argument not found in the program.");
        }

        /// <summary>
        /// Retrieve a constant by name from the program.
        /// </summary>
        /// <param name="constantName">The name of the constant to retrieve.</param>
        /// <returns>The retrieved constant.</returns>
        public ConstantNode GetConstant(string constantName)
        {
            if (_constantTable.TryGetValue(constantName, out var constant))
            {
                return constant;
            }

            throw new KeyNotFoundException("Constant not found in the program.");
        }

        /// <summary>
        /// Retrieve a module by name from the program.
        /// </summary>
        /// <param name="moduleName">The name of the module to retrieve.</param>
        /// <returns>The retrieved module.</returns>
        public ModuleNode GetModule(string moduleName)
        {
            if (_moduleTable.TryGetValue(moduleName, out var module))
            {
                return module;
            }
            
            throw new KeyNotFoundException("Module not found in the program.");
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(ProgramNode)}}}";
        }

        /// <summary>
        /// Try and retrieve an argument by name from the program.
        /// </summary>
        /// <param name="argumentName">The name of the argument to retrieve.</param>
        /// <param name="argument">The retrieved argument.</param>
        /// <returns>Whether or not the argument was retrieved.</returns>
        public bool TryGetArgument(string argumentName, out ArgumentNode argument)
        {
            return _argumentTable.TryGetValue(argumentName, out argument);
        }

        /// <summary>
        /// Try and retrieve a constant by name from the program.
        /// </summary>
        /// <param name="constantName">The name of the constant to retrieve.</param>
        /// <param name="constant">The retrieved constant.</param>
        /// <returns>Whether or not the argument was retrieved.</returns>
        public bool TryGetConstant(string constantName, out ConstantNode constant)
        {
            return _constantTable.TryGetValue(constantName, out constant);
        }

        /// <summary>
        /// Try and retrieve a module by name from the program.
        /// </summary>
        /// <param name="moduleName">The name of the module to retrieve.</param>
        /// <param name="module">The retrieved module.</param>
        /// <returns>Whether or not the module was retrieved.</returns>
        public bool TryGetModule(string moduleName, out ModuleNode module)
        {
            return _moduleTable.TryGetValue(moduleName, out module);
        }
    }
}
