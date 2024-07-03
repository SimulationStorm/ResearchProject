using EasyBindings;
using Godot;
using System;

public partial class WorldEnvPanelView : PanelView
{
	private WorldEnvPanelVM _viewModel = null!;

	public void Setup(WorldEnvPanelVM viewModel)
	{
		base.Setup(viewModel);
		_viewModel = viewModel;

		SetupControls();
	}

	#region Controls
	private void SetupControls()
	{
		SetupYearNumberLabel();
		SetupDayNumberLabel();

		SetupYearSeasonIndicators();
		SetupTimeOfDayIndicators();

		SetupYearSeasonProgressBar();
		SetupTimeOfDayProgressBar();
	}

	#region Counters
	#region Year number label 
	[Export] public NodePath YearNumberLabelPath { get; set; } = null!;
	private Label _yearNumberLabel = null!;

	private void SetupYearNumberLabel()
	{
		_yearNumberLabel = GetNode<Label>(YearNumberLabelPath);

		PropertyBinder.BindOneWay(this, _yearNumberLabel, t => t.Text, _viewModel, s => s.YearNumber, yearNumber =>
			yearNumber.ToStringWithDelimiter(3, ' '));
	}
	#endregion

	#region Day number label 
	[Export] public NodePath DayNumberLabelpath { get; set; } = null!;
	private Label _dayNumberLabel = null!;

	private void SetupDayNumberLabel()
	{
		_dayNumberLabel = GetNode<Label>(DayNumberLabelpath);

		PropertyBinder.BindOneWay(this, _dayNumberLabel, t => t.Text, _viewModel, s => s.DayNumber, dayNumber =>
			dayNumber.ToStringWithDelimiter(3, ' '));
	}
	#endregion
	#endregion

	#region Indicators
	[Export] public StyleBox IndicatorStyleBox { get; set; } = null!;

	#region YearSeasonIndicators
	[Export] public NodePath YearSeasonIndicatorsContainerPath { get; set; } = null!;
	private Container _yearSeasonIndicatorsContainer = null!;

	private void SetupYearSeasonIndicators()
	{
		_yearSeasonIndicatorsContainer = GetNode<Container>(YearSeasonIndicatorsContainerPath);

		var yearSeasons = new[]
		{
			YearSeason.Spring, YearSeason.Winter,
			YearSeason.Summer, YearSeason.Autumn
		};
		foreach (var yearSeason in yearSeasons)
		{
			var indicator = CreateIndicator(yearSeason.IconPath(), yearSeason.Name(), yearSeason.Color() with { A = 0.5f }, out var colorToggler);  ;
			_yearSeasonIndicatorsContainer.AddChild(indicator);

			TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.YearSeason, newYearSeason =>
				colorToggler(yearSeason == newYearSeason));

			colorToggler(yearSeason == _viewModel.YearSeason);
		}
	}
	#endregion

	#region TimeOfDayIndicators
	[Export] public NodePath TimeOfDayIndicatorsContainerPath { get; set; } = null!;
	private Container _timeOfDayIndicatorsContainer = null!;

	private void SetupTimeOfDayIndicators()
	{
		_timeOfDayIndicatorsContainer = GetNode<Container>(TimeOfDayIndicatorsContainerPath);

		var timesOfDay = new[]
		{
			TimeOfDay.Morning, TimeOfDay.Night,
			TimeOfDay.Day, TimeOfDay.Evening
		};
		foreach (var timeOfDay in timesOfDay)
		{
			var indicator = CreateIndicator(timeOfDay.IconPath(), timeOfDay.Name(), timeOfDay.Color(), out var colorToggler);
			_timeOfDayIndicatorsContainer.AddChild(indicator);

			TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.TimeOfDay, newTimeOfDay =>
				colorToggler(timeOfDay == newTimeOfDay));

			colorToggler(timeOfDay == _viewModel.TimeOfDay);
		}
	}
	#endregion

	private static PanelContainer CreateIndicator(string iconPath, string name, Color color, out Action<bool> colorToggler)
	{
		var styleBox = new StyleBoxFlat()
		{
			CornerDetail = 5,
			CornerRadiusTopLeft = 3,
			CornerRadiusTopRight = 3,
			CornerRadiusBottomLeft = 3,
			CornerRadiusBottomRight = 3,
			ContentMarginLeft = 5,
			ContentMarginTop = 5,
			ContentMarginRight = 5,
			ContentMarginBottom = 5
		};

		var panelContainer = new PanelContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		panelContainer.AddThemeStyleboxOverride("panel", styleBox);

		var hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 5);

		var textureRect = new TextureRect
		{
			Texture = ResourceLoader.Load<CompressedTexture2D>(iconPath),
			ExpandMode = TextureRect.ExpandModeEnum.FitWidth
		};
		var label = new Label
		{
			Text = name,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};
		hBoxContainer.AddChild(textureRect);
		hBoxContainer.AddChild(label);

		panelContainer.AddChild(hBoxContainer);

		colorToggler = toggle => styleBox.BgColor = toggle ? color : default;

		return panelContainer;
	}
	#endregion

	#region Progress bars
	#region Year season progress bar
	[Export] public NodePath YearSeasonProgressBarPath { get; set; } = null!;
	private ProgressBar _yearSeasonProgressBar = null!;

	private void SetupYearSeasonProgressBar()
	{
		_yearSeasonProgressBar = GetNode<ProgressBar>(YearSeasonProgressBarPath);

		PropertyBinder.BindOneWay(this, _yearSeasonProgressBar, t => t.Value, _viewModel, s => s.YearSeasonProgress,
			yearSeasonProgress => _yearSeasonProgressBar.Value = yearSeasonProgress);

		TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.YearSeason, yearSeason =>
			_yearSeasonProgressBar.MaxValue = _viewModel.YearSeason.DurationInDays());
	}
	#endregion

	#region Time of day progress bar 
	[Export] public NodePath TimeOfDayProgressBarPath { get; set; } = null!;
	private ProgressBar _timeOfDayProgressBar = null!;

	private void SetupTimeOfDayProgressBar()
	{
		_timeOfDayProgressBar = GetNode<ProgressBar>(TimeOfDayProgressBarPath);

		PropertyBinder.BindOneWay(this, _timeOfDayProgressBar, t => t.Value, _viewModel, s => s.TimeOfDayProgress,
			timeOfDayProgress => _timeOfDayProgressBar.Value = timeOfDayProgress);

		TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.TimeOfDay, timeOfDay =>
			_timeOfDayProgressBar.MaxValue = _viewModel.TimeOfDay.DurationInTicks());
	}
	#endregion
	#endregion
	#endregion

	public override void Unsubscribe()
	{
		base.Unsubscribe();

		PropertyBinder.Unbind(this);
		TriggerBinder.Unbind(this);
	}
}
