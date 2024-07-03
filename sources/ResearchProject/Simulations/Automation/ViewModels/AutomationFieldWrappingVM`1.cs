using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public abstract class AutomationFieldWrappingVM<TCellState> : ObservableObject, IUnsubscribe
{
    public AutomationFieldWrapping FieldWrapping
    {
        get => _automationModel.FieldWrapping;
        set => _automationModel.FieldWrapping = value;
    }

    private readonly AutomationModel<TCellState> _automationModel;

    protected AutomationFieldWrappingVM(AutomationModel<TCellState> automationModel)
    {
        _automationModel = automationModel;
        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.FieldWrapping, () => OnPropertyChanged(nameof(FieldWrapping)));
    }

    public virtual void Unsubscribe() => TriggerBinder.Unbind(this);
}