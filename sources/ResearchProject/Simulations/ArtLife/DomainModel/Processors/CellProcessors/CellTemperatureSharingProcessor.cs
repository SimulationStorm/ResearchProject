using System;
using System.Linq;

public class CellTemperatureSharingProcessor : IProcessor<Cell>
{
    private readonly WorldEnvironment _worldEnv;

    public CellTemperatureSharingProcessor(WorldEnvironment worldEnv) => _worldEnv = worldEnv;

    public void Process(Cell cell)
    {
        cell.Temperature += _worldEnv.TemperatureStep;

        var surroundingCells = cell.ExistingNeighbors;

        var surroundingCellAverageTemperature = surroundingCells.Average(c => c.Temperature);
        var temperatureDifference = surroundingCellAverageTemperature - cell.Temperature;
        if (temperatureDifference == 0)
            return;

        var temperatureShareAmount = Math.Sign(temperatureDifference) * ArtLifeSettings.TemperatureShareAmount;
        if (Math.Abs(temperatureDifference) < ArtLifeSettings.TemperatureShareAmount)
            temperatureShareAmount = temperatureDifference;

        cell.Temperature += temperatureShareAmount;

        var temperatureShareAmountPerCell = temperatureShareAmount / surroundingCells.Count;
        foreach (var neighborCell in surroundingCells)
            neighborCell.Temperature -= temperatureShareAmountPerCell;
    }
}