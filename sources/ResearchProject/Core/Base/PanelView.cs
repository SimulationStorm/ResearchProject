using EasyBindings;
using EasyBindings.Interfaces;
using Eranot;
using Godot;

public abstract partial class PanelView : Control, IUnsubscribe
{
    #region Fields
    private IPanelViewModel _viewModel = null!;

    private bool _isMoving;
    #endregion

    protected void Setup(IPanelViewModel viewModel, bool isMovable = false, bool isResizable = false)
    {
        _viewModel = viewModel;

        SetupTitlePanelContainer(isMovable);
        SetupCloseButton();

        if (isResizable)
            AddChild(new Resizable());

        PropertyBinder.BindOneWay(this, this, t => t.Visible, _viewModel, s => s.IsShown);
    }

    #region Controls
    #region Title panel container
    private PanelContainer _titlePanelContainer = null!;

    private void SetupTitlePanelContainer(bool isMovable)
    {
        _titlePanelContainer = (PanelContainer)FindChild("TitlePanelContainer");

        if (isMovable)
            _titlePanelContainer.GuiInput += OnTitlePanelContainerGuiInput;
    }

    private void OnTitlePanelContainerGuiInput(InputEvent @event)
    {
        switch (@event)
        {
            case InputEventMouseButton mouseButtonEvent:
                HandleTitlePanelContainerMouseButtonEvent(mouseButtonEvent);
                break;

            case InputEventMouseMotion mouseMotionEvent when _isMoving:
                HandleTitlePanelContainerMouseMotionEvent(mouseMotionEvent);
                break;
        }
    }

    private void HandleTitlePanelContainerMouseButtonEvent(InputEventMouseButton mouseButtonEvent) =>
        _isMoving = mouseButtonEvent is { ButtonIndex: MouseButton.Left, Pressed: true };

    private void HandleTitlePanelContainerMouseMotionEvent(InputEventMouseMotion mouseMotionEvent)
    {
        if (_isMoving == false)
            return;

        var viewportRect = GetViewportRect();
        Vector2 viewportPos = viewportRect.Position,
                viewportSize = viewportRect.Size;

        var newPosition = Position + mouseMotionEvent.Relative;

        if (newPosition.X < viewportPos.X)
            newPosition.X = viewportPos.X;

        if (newPosition.Y < viewportPos.Y)
            newPosition.Y = viewportPos.Y;

        if (newPosition.X + Size.X > viewportSize.X)
            newPosition.X = viewportSize.X - Size.X;

        if (newPosition.Y + Size.Y > viewportSize.Y)
            newPosition.Y = viewportSize.Y - Size.Y;

        Position = newPosition;
    }
    #endregion

    #region Close button
    private BaseButton _closeButton = null!;

    private void SetupCloseButton()
    {
        var closeButton = (BaseButton)FindChild("CloseButton");
        closeButton.Pressed += () => _viewModel.IsShown = false;
    }
    #endregion
    #endregion

    public virtual void Unsubscribe() => PropertyBinder.UnbindFromTarget(this, this, t => t.Visible);
}