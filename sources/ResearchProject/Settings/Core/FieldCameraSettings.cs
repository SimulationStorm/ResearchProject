public static class FieldCameraSettings
{
    public const double
        MinViewScale = 0.01, // Minimum zoom out, should be dividable by step
        MaxViewScale = 10, // Maximum zoom in, should be dividable by step
        InitialViewScale = 1,
        
        ZoomStep = 0.1,
        MoveStep = 0.5,
        PositionSmoothingSpeed = 20;

    public const bool
        PositionSmoothingEnabled = false,
        LimitSmoothed = false;
}