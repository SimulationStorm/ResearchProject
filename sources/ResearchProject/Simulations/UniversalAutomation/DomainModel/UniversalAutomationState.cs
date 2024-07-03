using System;
using Godot;

public class UniversalAutomationState : IEquatable<UniversalAutomationState>
{
    #region Properties
    public byte Number { get; }

    public string Name { get; }

    public Color Color { get; }
    #endregion

    public UniversalAutomationState(byte number, string name, Color color)
    {
        Number = number;
        Name = name;
        Color = color;
    }

    public bool Equals(UniversalAutomationState? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UniversalAutomationState)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Number);
}