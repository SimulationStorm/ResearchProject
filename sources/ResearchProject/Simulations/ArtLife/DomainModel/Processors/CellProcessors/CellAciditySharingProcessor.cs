using System.Linq;
using System;

public class CellAciditySharingProcessor : IProcessor<Cell>
{
    public void Process(Cell cell)
    {
        var surroundingCells = cell.ExistingNeighbors;

        var surroundingCellAverageAcidity = surroundingCells.Average(c => c.Acidity);
        var acidityDifference = surroundingCellAverageAcidity - cell.Acidity;
        if (acidityDifference == 0)
            return;

        var acidityShareAmount = Math.Sign(acidityDifference) * ArtLifeSettings.AcidityShareAmount;
        if (Math.Abs(acidityDifference) < ArtLifeSettings.AcidityShareAmount)
            acidityShareAmount = acidityDifference;

        cell.Acidity += acidityShareAmount;

        var acidityShareAmountPerCell = acidityShareAmount / surroundingCells.Count;
        foreach (var neighborCell in surroundingCells)
            neighborCell.Acidity -= acidityShareAmountPerCell;
    }
}