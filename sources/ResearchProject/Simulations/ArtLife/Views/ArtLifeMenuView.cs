using EasyBindings;
using Godot;
using System.Linq;

public partial class ArtLifeMenuView : SimulationMenuView
{
	private ArtLifeMenuVM _viewModel = null!;

	public void Setup(ArtLifeMenuVM viewModel)
	{
		base.Setup(viewModel);
		_viewModel = viewModel;

		SetupControls();
	}

	#region Controls
	private void SetupControls()
	{
		SetupDisplayModeMenu();
	}

	// TODO: Refactor
	#region Display mode menu
	private void SetupDisplayModeMenu()
	{
		SetupDisplayModeButtons();
		SetupSubstanceToggleButtons();

		void onDisplayModeChanged(DisplayMode displayMode)
		{
			_substancesCheckButton.SetPressedNoSignal(false);
			_substancesCheckButton.IsEnabled = true;

			_temperatureCheckButton.SetPressedNoSignal(false);
			_temperatureCheckButton.IsEnabled = true;

			_acidityCheckButton.SetPressedNoSignal(false);
			_acidityCheckButton.IsEnabled = true;

			_creaturesCheckButton.SetPressedNoSignal(false);
			_creaturesCheckButton.IsEnabled = true;

			_substancesAndCreaturesCheckButton.SetPressedNoSignal(false);
			_substancesAndCreaturesCheckButton.IsEnabled = true;

			_temperatureAndCreaturesCheckButton.SetPressedNoSignal(false);
			_temperatureAndCreaturesCheckButton.IsEnabled = true;

			if (displayMode == DisplayMode.Substances)
			{
				_substancesCheckButton.SetPressedNoSignal(true);
				_substancesCheckButton.IsEnabled = false;
			}
			else if (displayMode == DisplayMode.Temperature)
			{
				_temperatureCheckButton.SetPressedNoSignal(true);
				_temperatureCheckButton.IsEnabled = false;

			}
			else if (displayMode == DisplayMode.Acidity)
			{
				_acidityCheckButton.SetPressedNoSignal(true);
				_acidityCheckButton.IsEnabled = false;
			}
			else if (displayMode == DisplayMode.Creatures)
			{
				_creaturesCheckButton.SetPressedNoSignal(true);
				_creaturesCheckButton.IsEnabled = false;

			}
			else if (displayMode == DisplayMode.SubstancesAndCreatures)
			{
				_substancesAndCreaturesCheckButton.SetPressedNoSignal(true);
				_substancesAndCreaturesCheckButton.IsEnabled = false;

			}
			else if (displayMode == DisplayMode.TemperatureAndCreatures)
			{
				_temperatureAndCreaturesCheckButton.SetPressedNoSignal(true);
				_temperatureAndCreaturesCheckButton.IsEnabled = false;
			}

			_substanceButtonGroup.IsEnabled = displayMode == DisplayMode.Substances || displayMode == DisplayMode.SubstancesAndCreatures;
			UpdateSubstanceButtonGroup();
		}

		TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.DisplayMode, onDisplayModeChanged);
		onDisplayModeChanged(_viewModel.DisplayMode);
	}

	#region Display mode buttons 
	private void SetupDisplayModeButtons()
	{
		SetupSubstancesCheckButton();
		SetupTemperatureCheckButton();
		SetupAcidityCheckButton();
		SetupCreaturesCheckButton();
		SetupSubstancesAndCreaturesCheckButton();
		SetupTemperatureAndCreaturesCheckButton();
	}

	#region Substances check button 
	[Export] public NodePath SubstancesCheckButtonPath { get; set; } = null!;
	private RichCheckButton _substancesCheckButton = null!;

	private void SetupSubstancesCheckButton()
	{
		_substancesCheckButton = GetNode<RichCheckButton>(SubstancesCheckButtonPath);

		if (_viewModel.DisplayMode == DisplayMode.Substances)
			_substancesCheckButton.IsChecked = true;

		TriggerBinder.OnPropertyChanged(this, _substancesCheckButton, o => o.IsChecked, _ => _viewModel.DisplayMode = DisplayMode.Substances);
	}
	#endregion

	#region Temperature check button 
	[Export] public NodePath TemperatureCheckButtonPath { get; set; } = null!;
	private RichCheckButton _temperatureCheckButton = null!;

	private void SetupTemperatureCheckButton()
	{
		_temperatureCheckButton = GetNode<RichCheckButton>(TemperatureCheckButtonPath);

		if (_viewModel.DisplayMode == DisplayMode.Temperature)
			_temperatureCheckButton.IsChecked = true;

		TriggerBinder.OnPropertyChanged(this, _temperatureCheckButton, o => o.IsChecked, isChecked => _viewModel.DisplayMode = DisplayMode.Temperature);
	}
	#endregion

	#region Acidity check button 
	[Export] public NodePath AcidityCheckButtonPath { get; set; } = null!;
	private RichCheckButton _acidityCheckButton = null!;

	private void SetupAcidityCheckButton()
	{
		_acidityCheckButton = GetNode<RichCheckButton>(AcidityCheckButtonPath);

		if (_viewModel.DisplayMode == DisplayMode.Acidity)
			_acidityCheckButton.IsChecked = true;

		TriggerBinder.OnPropertyChanged(this, _acidityCheckButton, o => o.IsChecked, isChecked => _viewModel.DisplayMode = DisplayMode.Acidity);
	}
	#endregion

	#region Creatures check button
	[Export] public NodePath CreaturesCheckButtonPath { get; set; } = null!;
	private RichCheckButton _creaturesCheckButton = null!;

	private void SetupCreaturesCheckButton()
	{
		_creaturesCheckButton = GetNode<RichCheckButton>(CreaturesCheckButtonPath);

		if (_viewModel.DisplayMode == DisplayMode.Creatures)
			_creaturesCheckButton.IsChecked = true;

		TriggerBinder.OnPropertyChanged(this, _creaturesCheckButton, o => o.IsChecked, isChecked => _viewModel.DisplayMode = DisplayMode.Creatures);
	}
	#endregion

	#region Substances and creatures check button
	[Export] public NodePath SubstancesAndCreaturesCheckButtonPath { get; set; } = null!;
	private RichCheckButton _substancesAndCreaturesCheckButton = null!;

	private void SetupSubstancesAndCreaturesCheckButton()
	{
		_substancesAndCreaturesCheckButton = GetNode<RichCheckButton>(SubstancesAndCreaturesCheckButtonPath);

		if (_viewModel.DisplayMode == DisplayMode.SubstancesAndCreatures)
			_substancesAndCreaturesCheckButton.IsChecked = true;

		TriggerBinder.OnPropertyChanged(this, _substancesAndCreaturesCheckButton, o => o.IsChecked, isChecked =>
			_viewModel.DisplayMode = DisplayMode.SubstancesAndCreatures);
	}
	#endregion

	#region Temperature and creatures check button
	[Export] public NodePath TemperatureAndCreaturesCheckButtonPath { get; set; } = null!;
	private RichCheckButton _temperatureAndCreaturesCheckButton = null!;

	private void SetupTemperatureAndCreaturesCheckButton()
	{
		_temperatureAndCreaturesCheckButton = GetNode<RichCheckButton>(TemperatureAndCreaturesCheckButtonPath);

		if (_viewModel.DisplayMode == DisplayMode.TemperatureAndCreatures)
			_temperatureAndCreaturesCheckButton.IsChecked = true;

		TriggerBinder.OnPropertyChanged(this, _temperatureAndCreaturesCheckButton, o => o.IsChecked, isChecked =>
			_viewModel.DisplayMode = DisplayMode.TemperatureAndCreatures);
	}
	#endregion
	#endregion

	#region Substance buttons 
	[Export] public NodePath SubstanceToggleButtonsBoxPath { get; set; } = null!;
	private Container _substanceToggleButtonsBox = null!;
	private ToggleButtonGroup _substanceButtonGroup = null!;

	private void SetupSubstanceToggleButtons()
	{
		_substanceToggleButtonsBox = GetNode<Container>(SubstanceToggleButtonsBoxPath);

		_substanceButtonGroup = new ToggleButtonGroup();

		foreach (var substance in SubstanceExtensions.AllSubstances)
		{
			var substanceButton = new ToggleButton
			{
				Text = substance.Name(),
				Alignment = HorizontalAlignment.Left,
				Icon = ResourceLoader.Load<CompressedTexture2D>(substance.IconPath()),
				IconAlignment = HorizontalAlignment.Right,
				ExpandIcon = true,
			};
			_substanceButtonGroup.Add(substanceButton);
			_substanceToggleButtonsBox.AddChild(substanceButton);

			PropertyBinder.BindOneWay(this, substanceButton, t => t.SelfModulate, substanceButton, s => s.IsToggled,
				isToggled => isToggled ? Colors.Green : Colors.White);

			if (_viewModel.DisplayedSubstances.Any(ds => ds == substance))
				substanceButton.IsToggled = true;

			TriggerBinder.OnPropertyChanged(this, substanceButton, o => o.IsToggled, isToggled =>
			{
				if (isToggled)
					_viewModel.DisplayedSubstances.Add(substance);
				else
					_viewModel.DisplayedSubstances.Remove(substance);
			});
		}

		TriggerBinder.OnPropertyChanged(this, _substanceButtonGroup, o => o.ToggledButtons, UpdateSubstanceButtonGroup);
		UpdateSubstanceButtonGroup();
	}

	private void UpdateSubstanceButtonGroup()
	{
		var toggledButtons = _substanceButtonGroup.ToggledButtons;
		if (toggledButtons.Count() == 1)
		{
			var toggledButton = toggledButtons.First();
			toggledButton.IsEnabled = false;
		}
		else
			_substanceButtonGroup.IsEnabled = true;
	}
	#endregion
	#endregion
	#endregion

	public override void Unsubscribe()
	{
		base.Unsubscribe();
		TriggerBinder.Unbind(this);
		PropertyBinder.Unbind(this);
	}
}
