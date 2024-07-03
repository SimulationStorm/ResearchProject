using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyBindings;
using Godot;

public partial class LifeLikeAutomationMenuVM : AutomationMenuVM<LifeLikeAutomationCellState>
{
    #region Properties
    #region View models
    public LifeLikeAutomationDrawingModeVM DrawingModeVM { get; }

    public LifeLikeAutomationFieldWrappingVM FieldWrappingVM { get; }

    public LifeLikeAutomationRuleVM RuleVM => _automationModel.RuleVm;
    #endregion

    public LifeLikeAutomationAlgorithm Algorithm
    {
        get => _automationModel.Algorithm;
        set => _automationModel.Algorithm = value;
    }

    public LifeLikeAutomationKind? Kind
    {
        get => _automationModel.Kind;
        set
        {
            if (_automationModel.Kind == value)
                return;
            
            _automationModel.Kind = value;
            ResetRuleCommand.NotifyCanExecuteChanged();
        }
    }

    public LifeLikeAutomationPattern? Pattern
    {
        get => _automationModel.Pattern;
        set
        {
            if (_automationModel.Pattern == value)
                return;

            _automationModel.Pattern = value;

            if (value is not null && DrawingModeVM.DrawingModeEnabled)
                DrawingModeVM.DrawingModeEnabled = false;

            UnselectPatternCommand.NotifyCanExecuteChanged();
        }
    }

    [ObservableProperty]
    private double _liveDensity = LifeLikeAutomationSettings.InitialLiveDensity;

    #region Presentation properties
    public Color DeadCellColor
    {
        get => _presentationModel.DeadCellColor;
        set => _presentationModel.DeadCellColor = value;
    }

    public Color AliveCellColor
    {
        get => _presentationModel.AliveCellColor;
        set => _presentationModel.AliveCellColor = value;
    }
    #endregion
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanResetRule))]
    private void ResetRule() => _automationModel.RuleVm.SetRule(LifeLikeAutomationRule.Empty);
    private bool CanResetRule() => !_automationModel.RuleVm.AsRule().Equals(LifeLikeAutomationRule.Empty);


    [RelayCommand(CanExecute = nameof(CanUnselectPattern))]
    private void UnselectPattern() => Pattern = null;
    private bool CanUnselectPattern() => Pattern is not null;


    [RelayCommand]
    private void Populate() => _automationModel.PopulateRandomly(LiveDensity);


    [RelayCommand]
    private void ChooseRandomCellColors()
    {
        Color newDeadCellColor = ColorExtensions.GenerateRandomColor(),
              newAliveCellColor = newDeadCellColor.Inverted();

        _presentationModel.DeadCellColor = newDeadCellColor;
        _presentationModel.AliveCellColor = newAliveCellColor;
    }
    #endregion

    #region Fields
    private readonly LifeLikeAutomationModel _automationModel;

    private readonly LifeLikeAutomationPresentationModel _presentationModel;
    #endregion

    public LifeLikeAutomationMenuVM
    (
        PanelStatesModel panelStatesModel,
        LifeLikeAutomationModel automationModel,
        LifeLikeAutomationPresentationModel presentationModel
    )
    : base(panelStatesModel)
    {
        DrawingModeVM = new LifeLikeAutomationDrawingModeVM(presentationModel);
        TriggerBinder.OnPropertyChanged(this, DrawingModeVM, o => o.DrawingModeEnabled, drawingModeEnabled =>
        {
            if (drawingModeEnabled && Pattern is not null)
                Pattern = null;
        });

        FieldWrappingVM = new LifeLikeAutomationFieldWrappingVM(automationModel);

        _automationModel = automationModel;
        TriggerBinder.OnPropertyChanged(this, RuleVM, o => o.State, ResetRuleCommand.NotifyCanExecuteChanged);
        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.Kind, () => OnPropertyChanged(nameof(Kind)));
        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.Pattern, () => OnPropertyChanged(nameof(Pattern)));

        _presentationModel = presentationModel;
        TriggerBinder.OnPropertyChanged(this, _presentationModel, o => o.DeadCellColor, () => OnPropertyChanged(nameof(DeadCellColor)));
        TriggerBinder.OnPropertyChanged(this, _presentationModel, o => o.AliveCellColor, () => OnPropertyChanged(nameof(AliveCellColor)));
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        DrawingModeVM.Unsubscribe();
        FieldWrappingVM.Unsubscribe();
        TriggerBinder.Unbind(this);
    }
}