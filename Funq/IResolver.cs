namespace Funq
{
    public interface IResolver
    {
        /// <summary>
        /// Resolve a dependency from the container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T TryResolve<T>();
    }
}