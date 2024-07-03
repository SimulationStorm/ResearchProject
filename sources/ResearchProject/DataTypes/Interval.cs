public readonly struct Interval
{
    public Endpoint Min { get; init; }

    public Endpoint Max { get; init; }

    public Interval(Endpoint min, Endpoint max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(double value) =>
        (Min.Type is EndpointType.Including && value >= Min.Value || Min.Type is EndpointType.Excluding && value > Min.Value)
        && (Max.Type is EndpointType.Including && value <= Max.Value || Max.Type is EndpointType.Excluding && value < Max.Value);
}