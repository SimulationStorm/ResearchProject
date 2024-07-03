using EasyBindings;
using Godot;

public partial class ArtLifeStatsPanelView : SimulationStatsPanelView
{
	private ArtLifeStatsPanelVM _viewModel = null!;

	public void Setup(ArtLifeStatsPanelVM viewModel)
	{
		base.Setup(viewModel);
		_viewModel = viewModel;

		SetupControls();
	}

	#region Controls
	private void SetupControls()
	{
		SetupAverageTemperatureLabel();
		SetupAverageAcidityLabel();
		SetupSubstanceIndicators();
	}

	#region Average temperature label
	[Export] public NodePath AverageTemperatureLabelPath { get; set; } = null!;
	private Label _averageTemperatureLabel = null!;

	private void SetupAverageTemperatureLabel()
	{
		_averageTemperatureLabel = GetNode<Label>(AverageTemperatureLabelPath);

		TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.State, _ => UpdateAverageTemperatureLabel(_viewModel.AverageTemperature));

		UpdateAverageTemperatureLabel(_viewModel.AverageTemperature);
	}

	private void UpdateAverageTemperatureLabel(double temperature) => _averageTemperatureLabel.Text = $"{(temperature > 0 ? "+" : "")}{temperature:0.00} °";
	#endregion

	#region Average acidity label
	[Export] public NodePath AverageAcidityLabelPath { get; set; } = null!;
	private Label _averageAcidityLabel = null!;

	private void SetupAverageAcidityLabel()
	{
		_averageAcidityLabel = GetNode<Label>(AverageAcidityLabelPath);

		TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.State, _ => UpdateAverageAcidityLabel(_viewModel.AverageAcidity));

		UpdateAverageAcidityLabel(_viewModel.AverageAcidity);
	}

	private void UpdateAverageAcidityLabel(double acidity) => _averageAcidityLabel.Text = $"{acidity:0.0} pH";
	#endregion

	#region Substance indicators
	[Export] public NodePath IndicatorsContainerPath { get; set; } = null!;
	private Container _indicatorsContainer = null!;

	private void SetupSubstanceIndicators()
	{
		_indicatorsContainer = GetNode<Container>(IndicatorsContainerPath);

		foreach (var substance in SubstanceExtensions.AllSubstances)
		{
			var rowContainer = new HBoxContainer
			{
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			rowContainer.AddThemeConstantOverride("separation", 10);

			var leftContainer = new HBoxContainer
			{
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				SizeFlagsVertical = SizeFlags.ExpandFill
			};
			leftContainer.AddThemeConstantOverride("separation", 10);

			var textureRect = new TextureRect
			{
				Texture = ResourceLoader.Load<CompressedTexture2D>(substance.IconPath()),
				ExpandMode = TextureRect.ExpandModeEnum.FitWidth
			};
			var substanceNameLabel = new Label
			{
				Text = substance.Name()
			};
			leftContainer.AddChild(textureRect);
			leftContainer.AddChild(substanceNameLabel);

			var substanceAmountLabel = new Label
			{
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				SizeFlagsVertical = SizeFlags.ExpandFill,
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			substanceAmountLabel.AddThemeColorOverride("font_color", Colors.Green);

			rowContainer.AddChild(leftContainer);
			rowContainer.AddChild(substanceAmountLabel);

			_indicatorsContainer.AddChild(rowContainer);

			TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.State, _ => UpdateSubstanceAmountLabel(substanceAmountLabel, substance));

			UpdateSubstanceAmountLabel(substanceAmountLabel, substance);
		}
	}

	private void UpdateSubstanceAmountLabel(Label label, Substance substance) =>
		label.Text = $"{_viewModel.TotalSubstances[substance].ToStringWithDelimiter(3, ' ')} ед.";
	#endregion
	#endregion

	public override void Unsubscribe()	
	{
		base.Unsubscribe();

		TriggerBinder.Unbind(this);
		PropertyBinder.Unbind(this);
	}
}
