using EasyBindings.Interfaces;

public interface IView<TViewModel> : IUnsubscribe
{
    void Setup(TViewModel viewModel);
}