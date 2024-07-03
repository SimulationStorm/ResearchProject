using System.Text.Json;
using Godot;

public static class DebugHelpers
{
    public static void Print(dynamic obj) => GD.Print(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));

    public static void Print(params dynamic[] objects)
    {
        foreach (var obj in objects)
            Print(obj);
    }
}