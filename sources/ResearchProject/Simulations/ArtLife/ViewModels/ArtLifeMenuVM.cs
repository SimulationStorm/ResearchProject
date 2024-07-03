using EasyBindings;
using System.Collections.ObjectModel;

public partial class ArtLifeMenuVM : SimulationMenuVM
{
	#region Properties
	public ObservableCollection<Substance> DisplayedSubstances => _presentationModel.DisplayedSubstances;

    public DisplayMode DisplayMode
	{
		get => _presentationModel.DisplayMode;
		set => _presentationModel.DisplayMode = value;
	}
	#endregion

	private readonly ArtLifePresentationModel _presentationModel;

	public ArtLifeMenuVM
	(
		PanelStatesModel panelStatesModel,
		ArtLifeModel artLifeModel,
		ArtLifePresentationModel presentationModel
	)
	: base(panelStatesModel)
	{
		_presentationModel = presentationModel;
		TriggerBinder.OnPropertyChanged(this, presentationModel, o => o.DisplayMode, () => OnPropertyChanged(nameof(DisplayMode)));
	}
}
