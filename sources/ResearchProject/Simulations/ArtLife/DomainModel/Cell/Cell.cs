using System.Collections.Generic;
using System.Linq;

public class Cell
{
    public const int MaxNeighborsCount = 8;

    public IDictionary<CellNeighborPosition, Cell?> Neighbors { get; init; } = new Dictionary<CellNeighborPosition, Cell?>()
    {
        [CellNeighborPosition.Left] = null,
        [CellNeighborPosition.LeftTop] = null,
        [CellNeighborPosition.Top] = null,
        [CellNeighborPosition.RightTop] = null,
        [CellNeighborPosition.Right] = null,
        [CellNeighborPosition.RightBottom] = null,
        [CellNeighborPosition.Bottom] = null,
        [CellNeighborPosition.LeftBottom] = null,
    };

    public IReadOnlyCollection<Cell> ExistingNeighbors { get; set; } = null!;

    public IReadOnlyCollection<Cell> ExistingNeighborsAndSelf { get; set; } = null!;

    //public bool IsSide => ExistingNeighbors.Count < MaxNeighborsCount;

    public double Temperature { get; set; }

    public double Acidity { get; set; }

    // From zero to one. When there is sun around
    public double SunlightIntensity { get; set; }

    public SubstancesContainer Substances { get; init; } = new();

    public Creature? Creature { get; set; }
}

// TODO: Move it somewhere
public static class CellHelpers
{
    public static IEnumerable<Cell> GetSurroundingCellsHavingSubstance(Cell cell, Substance substance) =>
        GetCellsHavingSubstance(cell.ExistingNeighborsAndSelf, substance);

    public static IEnumerable<Cell> GetCellsHavingSubstance(IEnumerable<Cell> cells, Substance substance) =>
        cells.Where(c => c.Substances[substance] > 0);
}