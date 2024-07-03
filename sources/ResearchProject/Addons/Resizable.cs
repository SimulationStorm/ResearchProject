using System;
using Godot;

// TODO:
namespace Eranot;

[Flags]
public enum Handle
{
    Top = 1 << 0,
    Bottom = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3,
    TopLeft = 1 << 4,
    TopRight = 1 << 5,
    BottomLeft = 1 << 6,
    BottomRight = 1 << 7
}

public enum Mode
{
    Size,
    MinimumSize
}

[Tool]
public partial class Resizable : Control
{
    #region Exportable properties
    /// <summary>
    /// Whether the resizing behaviour is active or not
    /// </summary>
    [Export] public bool Active { get; set; } = true;

    /// <summary>
    /// Resize mode. If set to Size, the parent node's size will be changed and position may also change.
    /// If set to MinimumSize, the parent node's minimum size will be changed. Position will not be changed.
    /// </summary>
    [Export] public Mode Mode { get; set; } = Mode.Size;

    /// <summary>
    /// Thickness of the line where the mouse has to be to be able to start the resizing
    /// </summary>
    [Export] public int BorderWidth { get; set; } = 6;

    /// <summary>
    /// Minimum size that the parent node will be
    /// </summary>
    [Export] public Vector2 MinSize { get; set; } = new(0, 0);

    /// <summary>
    /// Maximum size that the parent node will be
    /// </summary>
    [Export] public Vector2 MaxSize { get; set; } = new(0, 0);

    /// <summary>
    /// Keeps the parent from being resized beyond the viewport.
    /// </summary>
    [Export] public bool ClampToViewport { get; set; } = false;

    /// <summary>
    /// Whether each of the handles are active or not
    /// @export_flags("TOP", "BOTTOM", "LEFT", "RIGHT", "TOP_LEFT", "TOP_RIGHT", "BOTTOM_LEFT", "BOTTOM_RIGHT")
    /// </summary>
    [Export] public Handle ActiveHandles { get; set; } =
        Handle.Top | Handle.Bottom | Handle.Left | Handle.Right |
        Handle.TopLeft | Handle.TopRight | Handle.BottomLeft | Handle.BottomRight;
    #endregion

    #region Fields
    private Vector2? _initialResizePosition,
                     _initialParentSize,
                     _initialParentPosition;

    private Handle? _handleBeingResized;

    private Control _parent = null!;
    #endregion

    #region Node event handlers
    public override void _Ready() => _parent = GetParent<Control>();

    public override void _EnterTree() => MouseFilter = MouseFilterEnum.Ignore;

    public override void _Input(InputEvent @event)
    {
        if (!Active)
            return;

        switch (@event)
        {
            case InputEventMouseMotion:
                OnMouseMove();
                break;

            case InputEventMouseButton mouseButtonEvent:
                OnMouseClick(mouseButtonEvent);
                break;
        }
    }

    private void OnMouseMove()
    {
        var handle = _handleBeingResized ?? GetHoveredHandle();

        if (handle is not null && !ActiveHandles.HasFlag(handle.Value))
            return;

        switch (handle)
        {
            case Handle.TopLeft or Handle.BottomRight:
                DisplayServer.CursorSetShape(DisplayServer.CursorShape.Fdiagsize);
                GetViewport().SetInputAsHandled();
                break;

            case Handle.BottomLeft or Handle.TopRight:
                DisplayServer.CursorSetShape(DisplayServer.CursorShape.Bdiagsize);
                GetViewport().SetInputAsHandled();
                break;

            case Handle.Top or Handle.Bottom:
                DisplayServer.CursorSetShape(DisplayServer.CursorShape.Vsize);
                GetViewport().SetInputAsHandled();
                break;

            case Handle.Left or Handle.Right:
                DisplayServer.CursorSetShape(DisplayServer.CursorShape.Hsize);
                GetViewport().SetInputAsHandled();
                break;
        }

        if (_initialResizePosition is null)
            return;

        // If the resizing is in progress
        switch (Mode)
        {
            case Mode.Size:
            {
                var parentSize = _parent.Size;
                _parent.Size = GetNewSize(parentSize);
                _parent.Position = GetNewPosition(_parent.Position, parentSize);
                break;
            }

            case Mode.MinimumSize:
                _parent.CustomMinimumSize = GetNewSize(_parent.CustomMinimumSize);
                break;
        }
    }

    private void OnMouseClick(InputEventMouseButton @event)
    {
        var handle = GetHoveredHandle();

        if (handle is not null && !ActiveHandles.HasFlag(handle.Value))
            return;

        if (@event.IsPressed() && handle is not null && _initialResizePosition is not null)
        {
            _initialResizePosition = GetGlobalMousePosition();
            _initialParentSize = Mode is Mode.Size ? _parent.Size : _parent.CustomMinimumSize;
            _initialParentPosition = _parent.Position;
            _handleBeingResized = handle;
        }
        else if (!@event.IsPressed())
        {
            _initialResizePosition = null;
            _handleBeingResized = null;
        }
    }
    #endregion

    #region Utility methods
    /// <summary>
    /// Returns the new position of the parent node, based on the mouse position
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private Vector2 GetNewSize(Vector2 size)
    {
        var newSize = size;

        var isCornerActive = ActiveHandles.HasFlag(_handleBeingResized!.Value);
        if (!isCornerActive)
            return newSize;

        Vector2 initialParentSize = _initialParentSize!.Value,
                initialResizePosition = _initialResizePosition!.Value;

        switch (_handleBeingResized!.Value)
        {
            case Handle.BottomRight:
                newSize = initialParentSize + (GetMousePosition() - initialResizePosition);
                break;

            case Handle.TopLeft:
                newSize = initialParentSize - (GetMousePosition() - initialResizePosition);
                break;

            case Handle.TopRight:
            {
                var x = initialParentSize.X + (GetMousePosition().X - initialResizePosition.X);
                var y = initialParentSize.Y - (GetMousePosition().Y - initialResizePosition.Y);
                newSize = new(x, y);
                break;
            }

            case Handle.BottomLeft:
            {
                var x = initialParentSize.X - (GetMousePosition().X - initialResizePosition.X);
                var y = initialParentSize.Y + (GetMousePosition().Y - initialResizePosition.Y);
                newSize = new(x, y);
                break;
            }

            case Handle.Left:
            {
                var x = initialParentSize.X - (GetMousePosition().X - initialResizePosition.X);
                newSize = new(x, newSize.Y);
                break;
            }

            case Handle.Right:
            {
                var x = initialParentSize.X + (GetMousePosition().X - initialResizePosition.X);
                newSize = new(x, newSize.Y);
                break;
            }

            case Handle.Bottom:
            {
                var y = initialParentSize.Y + (GetMousePosition().Y - initialResizePosition.Y);
                newSize = new(newSize.X, y);
                break;
            }

            case Handle.Top:
            {
                var y = initialParentSize.Y - (GetMousePosition().Y - initialResizePosition.Y);
                newSize = new(newSize.X, y);
                break;
            }
        }

        return RespectMinMaxSize(newSize);
    }

    private Vector2 GetMousePosition() => ClampToViewport
        ? GetGlobalMousePosition().Clamp(Vector2.Zero, GetViewportRect().Size)
        : GetGlobalMousePosition();

    /// <summary>
    /// Returns the new position of the parent node, based on the new size of the parent node and in which handle is being resized
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    private Vector2 GetNewPosition(Vector2 position, Vector2 size)
    {
        Vector2 initialParentPosition = _initialParentPosition!.Value,
                initialParentSize = _initialParentSize!.Value;

        switch (_handleBeingResized!.Value)
        {
            case Handle.TopLeft or Handle.Left:
                position = initialParentPosition - (size - initialParentSize);
                break;

            case Handle.TopRight or Handle.Right or Handle.Top:
            {
                var x = initialParentPosition.X;
                var y = initialParentPosition.Y - (size.Y - initialParentSize.Y);
                position = new(x, y);
                break;
            }

            case Handle.BottomLeft:
            {
                var x = initialParentPosition.X - (size.X - initialParentSize.X);
                var y = initialParentPosition.Y;
                position = new(x, y);
                break;
            }
        }

        return position;
    }

    /// <summary>
    /// Returns the handle that is hovered by the mouse. If the mouse is not hovering any handle, returns null.
    /// </summary>
    /// <returns></returns>
    private Handle? GetHoveredHandle()
    {
        var mousePosition = GetGlobalMousePosition();
        var parentGlobalPosition = _parent.GlobalPosition;
        var parentGlobalRectSize = _parent.GetGlobalRect().Size;

        bool isNearYTop = IsNear(mousePosition.Y, parentGlobalPosition.Y),
             isNearXLeft = IsNear(mousePosition.X, parentGlobalPosition.X),
             isNearYBottom = IsNear(mousePosition.Y, parentGlobalPosition.X + parentGlobalRectSize.Y),
             isNearXRight = IsNear(mousePosition.X, parentGlobalPosition.X + parentGlobalRectSize.X);

        if (isNearYTop && isNearXLeft)
            return Handle.TopLeft;
        if (isNearYBottom && isNearXLeft)
            return Handle.BottomLeft;
        if (isNearYTop && isNearXRight)
            return Handle.TopRight;
        if (isNearYBottom && isNearXRight)
            return Handle.BottomRight;

        var parentSize = _parent.Size;
        var isInsideContainer = mousePosition.X >= parentGlobalPosition.X - BorderWidth
            && mousePosition.X <= parentGlobalPosition.X + parentSize.X + BorderWidth
            && mousePosition.Y >= parentGlobalPosition.Y - BorderWidth
            && mousePosition.Y <= parentGlobalPosition.Y + parentSize.Y + BorderWidth;

        if (isNearYTop && isInsideContainer)
            return Handle.Top;
        if (isNearYBottom && isInsideContainer)
            return Handle.Bottom;
        if (isNearXLeft && isInsideContainer)
            return Handle.Left;
        if (isNearXRight && isInsideContainer)
            return Handle.Right;

        return null;
    }

    /// <summary>
    /// Returns if the values are near each other, based on the border_width
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    private bool IsNear(float value1, float value2) => Math.Abs(value1 - value2) <= BorderWidth;

    /// <summary>
    /// Returns the new size, respecting the min and max size
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns></returns>
    private Vector2 RespectMinMaxSize(Vector2 newSize)
    {
        if (MinSize.X is not 0 && newSize.X < MinSize.X)
            newSize.X = MinSize.X;

        if (MinSize.Y is not 0 && newSize.Y < MinSize.Y)
            newSize.Y = MinSize.Y;

        if (MaxSize.X is not 0 && newSize.X > MaxSize.X)
            newSize.X = MaxSize.X;

        if (MaxSize.Y is not 0 && newSize.Y > MaxSize.Y)
            newSize.Y = MaxSize.Y;

        return newSize;
    }
    #endregion
}