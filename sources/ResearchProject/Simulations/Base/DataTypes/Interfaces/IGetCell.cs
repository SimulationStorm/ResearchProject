/// <summary>
/// Provides a method for getting a cell at the specified coordinates
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGetCell<T>
{
    /// <summary>
    /// Gets a cell at the specified coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    T GetCell(int x, int y);
}