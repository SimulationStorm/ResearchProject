using EasyBindings;
using Godot;
using System;
using System.Collections.Generic;

public partial class TopPanelView : Control, IView<TopPanelVM>
{
    private TopPanelVM _viewModel = null!;

    public void Setup(TopPanelVM viewModel)
    {
        _viewModel = viewModel;

        SetupControls();
    }

    #region Controls
    private void SetupControls()
    {
        SetupMenuButtons();
        SetupSimulationProcessControlButtons();
        SetupSimulationModeOptionButton();
    }

    #region Menu buttons
    private void SetupMenuButtons()
    {
        SetupMainMenuButton();
        SetupViewMenuButton();
    }

    #region Main menu
    [Export] public NodePath MainMenuButtonPath { get; set; } = null!;
    private RichMenuButton _mainMenuButton = null!;

    public void SetupMainMenuButton()
    {
        var itemAndActions = new Dictionary<MenuItem, Action>()
        {
            {
                new("KEY_QUIT", actionName: AppSettings.QuitAppActionName),
                () => _viewModel.QuitApplicationCommand.Execute(null)
            },
        };

        _mainMenuButton = GetNode<RichMenuButton>(MainMenuButtonPath);
        _mainMenuButton.Items = itemAndActions.Keys;

        _mainMenuButton.ItemPressed += item => itemAndActions[item].Invoke();
    }
    #endregion

    #region View menu
    [Export] public NodePath ViewMenuButtonPath { get; set; } = null!;
    private RichMenuButton _viewMenuButton = null!;

    public void SetupViewMenuButton()
    {
        // Todo: it may be refactored
        var itemAndActions = new Dictionary<MenuItem, Action>
        {
            {
                new("KEY_HELP_INFO", MenuItemType.CheckBox, AppSettings.ToggleHelpPanelActionName),
                () => _viewModel.HelpPanelShown = !_viewModel.HelpPanelShown
            },
            {
                new("KEY_SETTINGS", MenuItemType.CheckBox, AppSettings.ToggleSettingsPanelActionName),
                () => _viewModel.SettingsPanelShown = !_viewModel.SettingsPanelShown
            },
            {
                new("KEY_BASIC_INFO", MenuItemType.CheckBox, AppSettings.ToggleBasicInfoPanelActionName),
                () => _viewModel.BasicInfoPanelShown = !_viewModel.BasicInfoPanelShown
            },
            {
                new("KEY_CONTROL_PANEL", MenuItemType.CheckBox, AppSettings.ToggleControlPanelActionName),
                () => _viewModel.ControlPanelShown = !_viewModel.ControlPanelShown
            },
            {
                new("KEY_SIMULATION_MENU", MenuItemType.CheckBox, AppSettings.ToggleSimulationMenuActionName),
                () => _viewModel.SimulationMenuShown = !_viewModel.SimulationMenuShown
            },
            {
                new("KEY_SIMULATION_STATS", MenuItemType.CheckBox, AppSettings.ToggleSimulationStatsPanelActionName),
                () => _viewModel.SimulationStatsPanelShown = !_viewModel.SimulationStatsPanelShown
            },
        };

        _viewMenuButton = GetNode<RichMenuButton>(ViewMenuButtonPath);
        _viewMenuButton.Items = itemAndActions.Keys;
        _viewMenuButton.ItemChecked += item => itemAndActions[item].Invoke();

        _viewMenuButton.SetItemCheckedNoSignal(0, _viewModel.HelpPanelShown);
        _viewMenuButton.SetItemCheckedNoSignal(1, _viewModel.SettingsPanelShown);
        _viewMenuButton.SetItemCheckedNoSignal(2, _viewModel.BasicInfoPanelShown);
        _viewMenuButton.SetItemCheckedNoSignal(3, _viewModel.ControlPanelShown);
        _viewMenuButton.SetItemCheckedNoSignal(4, _viewModel.SimulationMenuShown);
        _viewMenuButton.SetItemCheckedNoSignal(5, _viewModel.SimulationStatsPanelShown);
    }
    #endregion
    #endregion

    #region Simulation state control buttons
    private void SetupSimulationProcessControlButtons()
    {
        SetupStartButton();
        SetupStopButton();
        SetupResetButton();
    }

    #region Start button
    [Export] public NodePath StartButtonPath { get; set; } = null!;
    private RichButton _startButton = null!;

    private void SetupStartButton()
    {
        _startButton = GetNode<RichButton>(StartButtonPath);
        CommandBinder.Bind(this, _startButton, _viewModel.SetSimulationRunningStateCommand, () => SimulationRunningState.Running);
    }
    #endregion

    #region Stop button
    [Export] public NodePath StopButtonPath { get; set; } = null!;
    private RichButton _stopButton = null!;

    private void SetupStopButton()
    {
        _stopButton = GetNode<RichButton>(StopButtonPath);
        CommandBinder.Bind(this, _stopButton, _viewModel.SetSimulationRunningStateCommand, () => SimulationRunningState.Stopped);
}
    #endregion

    #region Reset button
    [Export] public NodePath ResetButtonPath { get; set; } = null!;
    private RichButton _resetButton = null!;

    private void SetupResetButton()
    {
        _resetButton = GetNode<RichButton>(ResetButtonPath);
        CommandBinder.Bind(this, _resetButton, _viewModel.ResetSimulationCommand);
    }
    #endregion
    #endregion

    #region Simulation mode option button
    [Export] public NodePath SimulationModeOptionButtonPath { get; set; } = null!;
    private OptionButton _simulationModeButton = null!;

    private const int LifeLikeAutomationOptionIndex = 0,
                      UniversalAutomationOptionIndex = 1,
                      ArtLifeOptionIndex = 2;

    private void SetupSimulationModeOptionButton()
    {
        _simulationModeButton = GetNode<OptionButton>(SimulationModeOptionButtonPath);

        _simulationModeButton.Selected = _viewModel.SimulationMode switch
        {
            SimulationMode.LifeLikeAutomation => LifeLikeAutomationOptionIndex,
            SimulationMode.UniversalAutomation => UniversalAutomationOptionIndex,
            SimulationMode.ArtLife => ArtLifeOptionIndex
        };

        _simulationModeButton.ItemSelected += itemIndex =>
        {
            var simulationMode = itemIndex switch
            {
                LifeLikeAutomationOptionIndex => SimulationMode.LifeLikeAutomation,
                UniversalAutomationOptionIndex => SimulationMode.UniversalAutomation,
                ArtLifeOptionIndex => SimulationMode.ArtLife
            };

            _viewModel.SwitchSimulationModeCommand.Execute(simulationMode);
        };
    }
    #endregion
    #endregion

    public void Unsubscribe()
    {
        PropertyBinder.Unbind(this);
        TriggerBinder.Unbind(this);
        CommandBinder.Unbind(this);
    }
}