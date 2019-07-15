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