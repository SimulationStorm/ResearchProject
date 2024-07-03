using EasyBindings;
using Godot;

public partial class CellInfoPanelView : PanelView
{
	private CellInfoPanelVM _viewModel = null!;

	public void Setup(CellInfoPanelVM viewModel)
	{
		base.Setup(viewModel);
		_viewModel = viewModel;

		SetupControls();
	}

	#region Controls
	private void SetupControls()
	{
		SetupTemperatureLabel();
		SetupAcidityLabel();
		SetupSubstanceIndicators();
	}

	#region Temperature label 
	[Export] public NodePath TemperatureLabelPath { get; set; } = null!;
	private Label _temperatureLabel = null!;

	private void SetupTemperatureLabel()
	{
		_temperatureLabel = GetNode<Label>(TemperatureLabelPath);

		TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.State, () => UpdateTemperatureLabel(_viewModel.Temperature));
		UpdateTemperatureLabel(_viewModel.Temperature);
	}

	private void UpdateTemperatureLabel(double temperature) => _temperatureLabel.Text = $"{(temperature > 0 ? "+" : "")}{temperature:0.00} °";
	#endregion

	#region Acidity label 
	[Export] public NodePath AcidityLabelPath { get; set; } = null!;
	private Label _acidityLabel = null!;

	private void SetupAcidityLabel()
	{
		_acidityLabel = GetNode<Label>(AcidityLabelPath);

		TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.State, () => UpdateAcidityLabel(_viewModel.Acidity));
		UpdateAcidityLabel(_viewModel.Acidity);
	}

    private void UpdateAcidityLabel(double acidity) => _acidityLabel.Text = $"{acidity:0.0} pH";
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
		label.Text = $"{_viewModel.Substances[substance].ToStringWithDelimiter(3, ' ')} ед.";
	#endregion
	#endregion

	public override void Unsubscribe()
	{
		base.Unsubscribe();

		TriggerBinder.Unbind(this);
		PropertyBinder.Unbind(this);
	}
}
