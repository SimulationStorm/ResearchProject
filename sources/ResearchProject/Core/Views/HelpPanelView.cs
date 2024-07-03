public partial class HelpPanelView : PanelView, IView<HelpPanelVM>
{
	public void Setup(HelpPanelVM viewModel) => Setup(viewModel, true, true);
}