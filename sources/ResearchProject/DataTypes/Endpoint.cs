public readonly struct Endpoint
{
    public EndpointType Type { get; init; }

    public double Value { get; init; }

    public Endpoint(EndpointType type, double value)
    {
        Type = type;
        Value = value;
    }
}