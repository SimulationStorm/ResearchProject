using Godot;

public static class ButtonExtensions
{
    public static void ToggleSelfModulate(this BaseButton baseButton, Color whenPressed, Color whenUnpressed) =>
        baseButton.ToggleSelfModulate(baseButton.ButtonPressed, whenPressed, whenUnpressed);
}