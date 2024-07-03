using System;

public class CellSubstancesSharingProcessor : IProcessor<Cell>
{
    public void Process(Cell cell)
    {
        // May be add here some degree of possibility to share?
        // For stableness of world

        foreach (var substance in SubstanceExtensions.AllSubstances)
        {
            var weight = substance.Weight();
            var amount = cell.Substances[substance];

            if (amount == 0)
                return;

            var totalShareAmount = (int)(ArtLifeSettings.SubstanceShareFactor / weight);

            if (totalShareAmount > amount)
                totalShareAmount = amount;

            // Variant 1:

            //while (totalShareAmount-- > 0)
            //    cell.ExistingNeighbors.RandomElementUsing(Random.Shared).Substances[substance] += 1;

            //cell.Substances[substance] -= totalShareAmount;

            // Variant 2:

            int neighborsCountToShareWith,
                neighborShareAmount;

            if (totalShareAmount < cell.ExistingNeighbors.Count)
            {
                neighborsCountToShareWith = totalShareAmount;
                neighborShareAmount = 1;
            }
            else
            {
                neighborsCountToShareWith = cell.ExistingNeighbors.Count;
                neighborShareAmount = totalShareAmount / neighborsCountToShareWith;
            }

            cell.Substances[substance] -= neighborShareAmount * neighborsCountToShareWith;

            while (neighborsCountToShareWith-- > 0)
                cell.ExistingNeighbors.RandomElementUsing(Random.Shared).Substances[substance] += neighborShareAmount;
        }
    }
}