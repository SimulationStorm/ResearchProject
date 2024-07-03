using Godot;
using System.Collections.Generic;

public class LifeLikeAutomationPresentationModel : AutomationPresentationModel<LifeLikeAutomationCellState>
{
    #region Properties
    public IReadOnlyDictionary<LifeLikeAutomationCellState, Color> CellColorsByState { get; }

    private Color _deadCellColor;
    public Color DeadCellColor
    {
        get => _deadCellColor;
        set
        {
            if (value == _deadCellColor)
                return;
            
            _deadCellColor = value;
            
            _cellColorsByState[LifeLikeAutomationCellState.Dead] = value;
            
            OnPropertyChanged();
            OnPropertyChanged(nameof(CellColorsByState));
        }
    }

    private Color _aliveCellColor;
    public Color AliveCellColor
    {
        get => _aliveCellColor;
        set
        {
            if (value == _aliveCellColor)
                return;

            _aliveCellColor = value;

            _cellColorsByState[LifeLikeAutomationCellState.Alive] = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(CellColorsByState));
        }
    }
    #endregion

    private readonly IDictionary<LifeLikeAutomationCellState, Color> _cellColorsByState;

    public LifeLikeAutomationPresentationModel() : base(LifeLikeAutomationSettings.InitialBrushCellState)
    {
        _cellColorsByState = new Dictionary<LifeLikeAutomationCellState, Color>();
        CellColorsByState = (IReadOnlyDictionary<LifeLikeAutomationCellState, Color>)_cellColorsByState;

        DeadCellColor = LifeLikeAutomationSettings.InitialDeadCellColor;
        AliveCellColor = LifeLikeAutomationSettings.InitialAliveCellColor;
    }
}