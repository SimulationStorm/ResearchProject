using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings.Interfaces;
using Godot;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;

public partial class UniversalAutomationNeighborhoodVM : ObservableObject, INotifyStateChanged
{
    #region Properties
    public object? State { get; }

    private int _radius;
    public int Radius
    {
        get => _radius;
        set
        {
            _radius = value;

            if (_template is not null)
                ApplyTemplate(_template);
            else
                UpdateSelectedPositions();

            OnPropertyChanged();
            OnPropertyChanged(nameof(State));
            NotifyCommandsCanExecuteChanged();
        }
    }

    private UniversalAutomationNeighborhoodTemplate? _template;
    public UniversalAutomationNeighborhoodTemplate? Template
    {
        get => _template;
        set
        {
            _template = value;

            if (_template is not null)
                ApplyTemplate(_template);

            OnPropertyChanged();
            OnPropertyChanged(nameof(State));
            NotifyCommandsCanExecuteChanged();
        }
    }

    public IReadOnlySet<Vector2I> SelectedPositions => (IReadOnlySet<Vector2I>)_selectedPositions;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSelectAllPositions))]
    private void SelectAllPositions()
    {
        var allowedPositions = UniversalAutomationNeighborhood.GetAllowedPositions(Radius);
        SetPositions(allowedPositions, true);
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanSelectAllPositions()
    {
        var allowedPositions = UniversalAutomationNeighborhood.GetAllowedPositions(Radius);
        return allowedPositions.Any(position => !IsPositionSelected(position));
    }


    [RelayCommand(CanExecute = nameof(CanResetPositions))]
    private void ResetPositions()
    {
        var allowedPositions = UniversalAutomationNeighborhood.GetAllowedPositions(Radius);
        SetPositions(allowedPositions, false);
        NotifyCommandsCanExecuteChanged();
    }
    private bool CanResetPositions()
    {
        var allowedPositions = UniversalAutomationNeighborhood.GetAllowedPositions(Radius);
        return allowedPositions.Any(IsPositionSelected);
    }
    #endregion

    private ISet<Vector2I> _selectedPositions;

    public UniversalAutomationNeighborhoodVM(UniversalAutomationNeighborhood? neighborhood = null)
    {
        if (neighborhood is not null)
        {
            _radius = neighborhood.Radius;
            _selectedPositions = new HashSet<Vector2I>(neighborhood.SelectedPositions);
            _template = neighborhood.Template;
        }
        else
        {
            _radius = UniversalAutomationSettings.MinNeighborhoodRadius;
            _selectedPositions = new HashSet<Vector2I>();
        }
    }

    #region Public methods
    public void SetPosition(Vector2I position, bool isSelected)
    {
        UniversalAutomationNeighborhood.ValidatePosition(Radius, position);

        if (isSelected)
            _selectedPositions.Add(position);
        else
            _selectedPositions.Remove(position);

        Template = null;

        NotifyCommandsCanExecuteChanged();
    }

    public bool IsPositionSelected(Vector2I position)
    {
        UniversalAutomationNeighborhood.ValidatePosition(Radius, position);
        return _selectedPositions.Contains(position);
    }

    public UniversalAutomationNeighborhood AsNeighborhood() => new UniversalAutomationNeighborhoodBuilder()
        .HasRadius(Radius)
        .HasSelectedPositions(_selectedPositions)
        .Build();
    #endregion

    #region Private methods
    private void ApplyTemplate(UniversalAutomationNeighborhoodTemplate template) =>
        _selectedPositions = UniversalAutomationNeighborhood.GetAllowedPositions(Radius, template);

    private void UpdateSelectedPositions()
    {
        var newSelectedPositions = new HashSet<Vector2I>();
        UniversalAutomationNeighborhood.IterateOverAllowedPositions(Radius, position =>
        {
            if (_selectedPositions.Contains(position))
                newSelectedPositions.Add(position);
        });
        _selectedPositions = newSelectedPositions;
    }

    private void SetPositions(ISet<Vector2I> positions, bool isSelected)
    {
        UniversalAutomationNeighborhood.ValidatePositions(Radius, positions);

        foreach (var position in positions)
        {
            if (isSelected)
                _selectedPositions.Add(position);
            else
                _selectedPositions.Remove(position);
        }

        Template = null;

        NotifyCommandsCanExecuteChanged();
    }

    private void NotifyCommandsCanExecuteChanged()
    {
        SelectAllPositionsCommand.NotifyCanExecuteChanged();
        ResetPositionsCommand.NotifyCanExecuteChanged();
    }
    #endregion
}