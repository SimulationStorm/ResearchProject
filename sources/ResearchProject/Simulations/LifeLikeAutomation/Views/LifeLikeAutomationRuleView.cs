using System;
using System.Collections.Generic;
using EasyBindings;
using Godot;

public partial class LifeLikeAutomationRuleView : Control, IView<LifeLikeAutomationRuleVM>
{
    #region Fields
    private LifeLikeAutomationRuleVM _viewModel = null!;

    private bool _skipViewModelStateChangedNotification;
    #endregion

    public void Setup(LifeLikeAutomationRuleVM viewModel)
    {
        _viewModel = viewModel;

        SetupButtonsBoxes();
    }

    #region Buttons boxes
    [Export] public NodePath BornButtonsBoxPath { get; set; } = null!;
    private Container _bornButtonsBox = null!;

    [Export] public NodePath SurvivalButtonsBoxPath { get; set; } = null!;
    private Container _survivalButtonsBox = null!;

    private readonly IDictionary<ToggleButton, int>
        _neighborCountsByBornButton = new Dictionary<ToggleButton, int>(),
        _neighborCountsBySurvivalButton = new Dictionary<ToggleButton, int>();

    private void SetupButtonsBoxes()
    {
        _bornButtonsBox = GetNode<Container>(BornButtonsBoxPath);
        _survivalButtonsBox = GetNode<Container>(SurvivalButtonsBoxPath);

        GenerateButtons(_bornButtonsBox, _neighborCountsByBornButton, OnBornButtonIsToggledChanged);
        GenerateButtons(_survivalButtonsBox, _neighborCountsBySurvivalButton, OnSurvivalButtonIsToggledChanged);

        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.State, UpdateButtons);
    }

    private void GenerateButtons(Container buttonsBox, IDictionary<ToggleButton, int> neighborCountsByButton, Action<ToggleButton, bool> buttonIsToggledChangedHandler)
    {
        for (var neighborCount = 0; neighborCount < LifeLikeAutomationRule.RuleLength; neighborCount++)
        {
            var button = new ToggleButton
            {
                Text = $"{neighborCount}",
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                FocusMode = FocusModeEnum.None
            };

            buttonsBox.AddChild(button);
            neighborCountsByButton[button] = neighborCount;

            TriggerBinder.OnPropertyChanged(this, button, o => o.IsToggled, buttonIsToggledChangedHandler);
        }
    }

    private void OnBornButtonIsToggledChanged(ToggleButton button, bool isToggled)
    {
        _skipViewModelStateChangedNotification = true;
        var neighborCount = _neighborCountsByBornButton[button];
        _viewModel.SetBornWhen(neighborCount, isToggled);
        UpdateButtonSelfModulate(button);
    }

    private void OnSurvivalButtonIsToggledChanged(ToggleButton button, bool isToggled)
    {
        _skipViewModelStateChangedNotification = true;
        var neighborCount = _neighborCountsBySurvivalButton[button];
        _viewModel.SetSurvivalWhen(neighborCount, isToggled);
        UpdateButtonSelfModulate(button);
    }

    private void UpdateButtons()
    {
        if (_skipViewModelStateChangedNotification)
        {
            _skipViewModelStateChangedNotification = false;
            return;
        }

        foreach (var (button, neighborCount) in _neighborCountsByBornButton)
        {
            button.SetPressedNoSignal(_viewModel.IsBornWhen(neighborCount));
            UpdateButtonSelfModulate(button);
        }

        foreach (var (button, neighborCount) in _neighborCountsBySurvivalButton)
        {
            button.SetPressedNoSignal(_viewModel.IsSurvivalWhen(neighborCount));
            UpdateButtonSelfModulate(button);
        }
    }
    #endregion

    private static void UpdateButtonSelfModulate(BaseButton button) => button.ToggleSelfModulate(Colors.Green, Colors.White);

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}