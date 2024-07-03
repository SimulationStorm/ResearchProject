/// <summary>
/// Provides a method that processes item in a certain way
/// </summary>
public interface IProcessor<T>
{
    /// <summary>
    /// Performs processing of an item
    /// </summary>
    /// <param name="item"></param>
    void Process(T item);
}