namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// The interface for all value containers.
    /// </summary>
    public interface ILocalValue
    {
        /// <summary>
        /// Access a property of the <see cref="ILocalValue"/>.
        /// </summary>
        /// <param name="key">The key to access on the value.</param>
        /// <returns>The result of accessing the specified key on the local value.</returns>
        ILocalValue Access(string key);

        /// <summary>
        /// Cast the <see cref="ILocalValue"/> to a <see cref="BooleanValue"/>.
        /// </summary>
        /// <returns>The result of casting the local value to a boolean.</returns>
        BooleanValue CastBoolean();

        /// <summary>
        /// Cast the <see cref="ILocalValue"/> to a <see cref="NumberValue"/>.
        /// </summary>
        /// <returns>The result of casting the local value to a number.</returns>
        NumberValue CastNumber();

        /// <summary>
        /// Cast the <see cref="ILocalValue"/> to a <see cref="StringValue"/>.
        /// </summary>
        /// <returns>The result of casting the local value to a string.</returns>
        StringValue CastString();

        /// <summary>
        /// Check if this <see cref="ILocalValue"/> is equal to another <see cref="ILocalValue"/>.
        /// </summary>
        /// <param name="other">The value to compare with.</param>
        /// <returns>The result of checking if this local value is equal to another local value.</returns>
        bool EqualTo(ILocalValue other);

        /// <summary>
        /// Invoke the <see cref="ILocalValue"/> with a list of arguments.
        /// </summary>
        /// <param name="arguments">The list of arguments to use in the invocation.</param>
        /// <returns>The result of invoking the local value with the provided arguments.</returns>
        ILocalValue Invoke(params ILocalValue[] arguments);
    }
}