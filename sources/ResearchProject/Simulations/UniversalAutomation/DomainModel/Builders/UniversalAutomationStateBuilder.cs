using Godot;

public class UniversalAutomationStateBuilder
{
    private byte _number;

    private string _name = string.Empty;

    private Color _color;

    public UniversalAutomationStateBuilder HasNumber(byte number)
    {
        _number = number;
        return this;
    }

    public UniversalAutomationStateBuilder HasName(string name)
    {
        _name = name;
        return this;
    }

    public UniversalAutomationStateBuilder HasColor(Color color)
    {
        _color = color;
        return this;
    }

    public UniversalAutomationState Build() => new(_number, _name, _color);
}