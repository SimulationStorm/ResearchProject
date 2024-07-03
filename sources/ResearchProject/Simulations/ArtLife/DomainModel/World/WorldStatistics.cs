using System.Linq;

public class WorldStatistics
{
    #region Properties
    public double AverageTemperature { get; private set; }

    public double AverageAcidity { get; private set; }

    public SubstancesContainer TotalSubstanceAmounts { get; init; } = new();
    #endregion

    private readonly WorldField _worldField;

    public WorldStatistics(WorldField worldField) => _worldField = worldField;

    #region Public methods
    public void Update()
    {
        CalculateAverageTemperature();
        CalculateAverageAcidity();
        CountSubstanceTotalAmounts();
    }

    public void Reset()
    {
        AverageTemperature = 0;
        TotalSubstanceAmounts.Reset();
    }
    #endregion

    #region Private methods
    private void CalculateAverageTemperature() => AverageTemperature = _worldField.Cells.Average(cell => cell.Temperature);

    private void CalculateAverageAcidity() => AverageAcidity = _worldField.Cells.Average(cell => cell.Acidity);

    private void CountSubstanceTotalAmounts()
    {
        foreach (var substance in SubstanceExtensions.AllSubstances)
            TotalSubstanceAmounts[substance] = _worldField.Cells.Sum(cell => cell.Substances[substance]);
    }
    #endregion
}