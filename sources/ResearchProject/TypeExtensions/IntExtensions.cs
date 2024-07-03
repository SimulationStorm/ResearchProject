public static class IntExtensions
{
    /// <summary>
    /// Makes number representation divided by delimiter every N digits
    /// Example of using: var num = 1100500; var numStr = num.ToStringWithDelimiter(3, ' '); // numStr = " 1 100 500"
    /// </summary>
    /// <param name="num"></param>
    /// <param name="digits"></param>
    /// <param name="delimiter"></param>
    /// <returns>Number divided by delimiter every N digits</returns>
    public static string ToStringWithDelimiter(this int num, int digits, char delimiter)
    {
        var numStr = num.ToString();

        // Base case
        if (numStr.Length <= 3)
            return numStr;

        // Recursive case
        string left = (num / 1000).ToStringWithDelimiter(digits, delimiter),
               right = numStr[^3..]; // numStr.Substring(numStr.Length - 3)

        return $"{left}{delimiter}{right}";
    }
}