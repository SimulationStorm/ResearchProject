using System;
using EasyBindings;
using Godot;

public partial class SimulationFieldCameraView : Camera2D, IView<SimulationFieldCameraVM>
{
    #region Fields
    private SimulationFieldCameraVM _viewModel = null!;

    private Vector2 _globalMousePosDelta;

    private bool _skipViewScaleChangedNotification;
    #endregion

    #region Setting up
    public void Setup(SimulationFieldCameraVM viewModel)
    {
        SetupViewModel(viewModel);
        SetupCamera();
    }

    private void SetupViewModel(SimulationFieldCameraVM viewModel)
    {
        _viewModel = viewModel;

        if (_viewModel.ViewScale is not 1)
            ZoomToDefaultPosition(_viewModel.ViewScale);

        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.ViewScale, viewScale =>
        {
            if (_skipViewScaleChangedNotification)
            {
                _skipViewScaleChangedNotification = false;
                ZoomToMousePosition(viewScale);
            }
            else
            {
                ZoomToDefaultPosition(viewScale);
            }
        });
    }

    private void SetupCamera()
    {
        PositionSmoothingEnabled = FieldCameraSettings.PositionSmoothingEnabled;
        PositionSmoothingSpeed = (float)FieldCameraSettings.PositionSmoothingSpeed;
        LimitSmoothed = FieldCameraSettings.LimitSmoothed;

        AnchorMode = AnchorModeEnum.DragCenter;
        Position = _viewModel.ScreenSize / 2;

        SetupSideLimits();
    }

    private void SetupSideLimits()
    {
        var inversedMinViewScale = new Vector2
        (
            1 / (float)FieldCameraSettings.MinViewScale,
            1 / (float)FieldCameraSettings.MinViewScale
        );

        Vector2 leftTopLimit = -(_viewModel.ScreenSize * (inversedMinViewScale - Vector2.One)),
                rightBottomLimit = _viewModel.ScreenSize * inversedMinViewScale;

        LimitLeft = (int)leftTopLimit.X;
        LimitTop = (int)leftTopLimit.Y;
        LimitRight = (int)rightBottomLimit.X;
        LimitBottom = (int)rightBottomLimit.Y;
    }
    #endregion

    #region Controller
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            _globalMousePosDelta = mouseMotionEvent.Relative;

            if (mouseMotionEvent.CtrlPressed)
                Move(-_globalMousePosDelta);

            return;
        }

        bool isWheelUp = Input.IsMouseButtonPressed(MouseButton.WheelUp),
             isWheelDown = Input.IsMouseButtonPressed(MouseButton.WheelDown),
             isAltPressed = Input.IsKeyLabelPressed(Key.Alt);
        if (!(isWheelUp || isWheelDown) || !isAltPressed)
            return;

        var viewScaleSign = isWheelUp ? 1 : -1;
        UpdateViewScale(viewScaleSign * FieldCameraSettings.ZoomStep);
    }
    #endregion

    #region Private methods 
    private void Move(Vector2 offset) => Translate(offset * (float)FieldCameraSettings.MoveStep);

    private void UpdateViewScale(double delta)
    {
        _skipViewScaleChangedNotification = true;
        _viewModel.ViewScale = GetClampedViewScale(_viewModel.ViewScale + delta);
    }

    private void ZoomToDefaultPosition(double zoom)
    {
        Zoom = GetClampedZoom(zoom);
        Position = _viewModel.ScreenSize / 2;
    }

    private void ZoomToMousePosition(double zoom)
    {
        var prevMouseLocalPos = GetLocalMousePosition();
        Zoom = GetClampedZoom(zoom);
        var currMouseLocalPos = GetLocalMousePosition();

        Translate(prevMouseLocalPos - currMouseLocalPos);
    }

    private static double GetClampedViewScale(double viewScale) =>
        Math.Clamp(viewScale, FieldCameraSettings.MinViewScale, FieldCameraSettings.MaxViewScale);

    private static Vector2 GetClampedZoom(double zoom)
    {
        zoom = Math.Clamp(zoom, FieldCameraSettings.MinViewScale, FieldCameraSettings.MaxViewScale);
        return new((float)zoom, (float)zoom);
    }
    #endregion

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}