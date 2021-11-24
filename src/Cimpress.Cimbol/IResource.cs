// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

namespace Cimpress.Cimbol
{
    /// <summary>
    /// The interface for any referenceable object.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// The name of the resource.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The program that the resource belongs to.
        /// </summary>
        Program Program { get; }
    }
}