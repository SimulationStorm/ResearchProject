using Godot;

public static class NodeExtensions
{
    public static void RemoveChildren(this Node node)
    {
        var children = node.GetChildren();
        foreach (var child in children)
            node.RemoveChild(child);
    }
}