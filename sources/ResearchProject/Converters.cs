using Godot;

public static class Converters
{
    public static string IntToString(int number) => number.ToStringWithDelimiter(3, ' ');

    public static string Vector2ToString(Vector2 vector) => $"{vector.X} x {vector.Y}";

    public static string Vector2IToString(Vector2I vector) => $"{vector.X.ToStringWithDelimiter(3, ' ')} x {vector.Y.ToStringWithDelimiter(3, ' ')}";

    public static string PercentToString(double percent) => $"{((int)(percent * 100)).ToStringWithDelimiter(3, ' ')} %";

    public static Color PercentToColor(double percent) => percent < 0.5
        ? Colors.Green.Lerp(Colors.Yellow, (float)percent * 2)
        : Colors.Yellow.Lerp(Colors.Red, (float)((percent - 0.5) * 2));
}