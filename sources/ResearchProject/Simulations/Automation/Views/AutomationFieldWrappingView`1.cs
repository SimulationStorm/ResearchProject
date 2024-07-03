using EasyBindings;
using EasyBindings.Interfaces;
using Godot;

public abstract partial class AutomationFieldWrappingView<TCellState> : VBoxContainer, IUnsubscribe
{
    private AutomationFieldWrappingVM<TCellState> _viewModel = null!;

    protected void Setup(AutomationFieldWrappingVM<TCellState> viewModel)
    {
        _viewModel = viewModel;
        SetupControls();
    }

    #region Controls
    private void SetupControls()
    {
        SetupHorizontalFieldWrappingCheckBox();
        SetupVerticalFieldWrappingCheckBox();
    }

    #region Horizontal field wrapping check box
    [Export] public NodePath HorizontalFieldWrappingCheckBoxPath { get; set; } = null!;
    private RichCheckBox _horizontalFieldWrappingCheckBox = null!;

    private void SetupHorizontalFieldWrappingCheckBox()
    {
        _horizontalFieldWrappingCheckBox = GetNode<RichCheckBox>(HorizontalFieldWrappingCheckBoxPath);
        SetupFieldWrappingCheckBox(_horizontalFieldWrappingCheckBox,
            AutomationFieldWrapping.Horizontal, AutomationFieldWrapping.Vertical);
    }
    #endregion

    #region Vertical field wrapping check box
    [Export] public NodePath VerticalFieldWrappingCheckBoxPath { get; set; } = null!;
    private RichCheckBox _verticalFieldWrappingCheckBox = null!;

    private void SetupVerticalFieldWrappingCheckBox()
    {
        _verticalFieldWrappingCheckBox = GetNode<RichCheckBox>(VerticalFieldWrappingCheckBoxPath);
        SetupFieldWrappingCheckBox(_verticalFieldWrappingCheckBox,
            AutomationFieldWrapping.Vertical, AutomationFieldWrapping.Horizontal);
    }
    #endregion

    private void SetupFieldWrappingCheckBox
    (
        RichCheckBox checkBox,
        AutomationFieldWrapping wrapping,
        AutomationFieldWrapping oppositeWrapping)
    {
        checkBox.IsChecked = _viewModel.FieldWrapping == wrapping;

        TriggerBinder.OnPropertyChanged(this, checkBox, o => o.IsChecked, isChecked =>
        {
            AutomationFieldWrapping
                oldFieldWrapping = _viewModel.FieldWrapping,
                newFieldWrapping;

            if (isChecked)
                newFieldWrapping = oldFieldWrapping is AutomationFieldWrapping.NoWrap
                    ? wrapping
                    : AutomationFieldWrapping.Both;
            else
                newFieldWrapping = oldFieldWrapping == wrapping
                    ? AutomationFieldWrapping.NoWrap
                    : oppositeWrapping;

            _viewModel.FieldWrapping = newFieldWrapping;
        });
    }
    #endregion

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}