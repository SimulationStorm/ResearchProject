using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

public partial class ArtLifePresentationModel : ObservableObject
{
	[ObservableProperty]
	private DisplayMode _displayMode;

	public ObservableCollection<Substance> DisplayedSubstances { get; }

    [ObservableProperty]
    private Cell? _selectedCell;

    public ArtLifePresentationModel()
	{
		DisplayMode = ArtLifeSettings.InitialCellDisplayMode;
		DisplayedSubstances = new ObservableCollection<Substance>(ArtLifeSettings.InitialDisplayedSubstances);
	}
}