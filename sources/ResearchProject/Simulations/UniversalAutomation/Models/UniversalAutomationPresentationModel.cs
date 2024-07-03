using System;
using System.Collections.Generic;
using Godot;

public class UniversalAutomationPresentationModel : AutomationPresentationModel<byte>
{
    #region Properties
    public IReadOnlyDictionary<byte, string> NamesByState { get; }

    public IReadOnlyDictionary<byte, Color> ColorsByState { get; }
    #endregion

    #region Fields
    private readonly IDictionary<byte, string> _namesByState = new Dictionary<byte, string>();

    private readonly IDictionary<byte, Color> _colorsByState = new Dictionary<byte, Color>();
    #endregion

    public UniversalAutomationPresentationModel(byte initialDrawingBrushCellState) : base(initialDrawingBrushCellState)
    {
        NamesByState = (IReadOnlyDictionary<byte, string>)_namesByState;
        ColorsByState = (IReadOnlyDictionary<byte, Color>)_colorsByState;

        // Set initial name and colors
        foreach (var state in UniversalAutomationSettings.InitialKind.States)
        {
            _namesByState[state.Number] = state.Name;
            _colorsByState[state.Number] = state.Color;
        }
    }

    public event Action<byte>? StateAdded,
                               StateNameChanged,
                               StateColorChanged,
                               StateRemoved;

    public void AddState(byte state, string name, Color color)
    {
        _namesByState[state] = name;
        _colorsByState[state] = color;
        StateAdded?.Invoke(state);
    }

    public void SetStateName(byte state, string name)
    {
        _namesByState[state] = name;
        StateNameChanged?.Invoke(state);
    }

    public void SetStateColor(byte state, Color color)
    {
        _colorsByState[state] = color;
        StateColorChanged?.Invoke(state);
    }

    public void RemoveState(byte state)
    {
        _namesByState.Remove(state);
        _colorsByState.Remove(state);
        StateRemoved?.Invoke(state);
    }
}