using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class WorldField : IGetCell<Cell>
{
    #region Properties
    public Vector2I Size
    {
        get => new(_columns, _rows);
        set => Resize(value);
    }

    public IReadOnlyCollection<Cell> Cells { get; private set; } = null!;

    public IReadOnlyCollection<IReadOnlyCollection<Cell>> EvenChunks { get; private set; } = null!;

    public IReadOnlyCollection<IReadOnlyCollection<Cell>> OddChunks { get; private set; } = null!;
    #endregion

    #region Fields
    private int _columns, _rows;

    private Cell[,] _cellsMatrix = null!;
    #endregion

    public WorldField(Vector2I size) => Resize(size);

    #region Public methods
    public Cell GetCell(int x, int y) => _cellsMatrix[x, y];

    public void Reset()
    {
        Parallel.ForEach(Cells, cell =>
        {
            cell.Temperature = 0;
            cell.Acidity = 0;
            cell.Creature = null;
            cell.Substances.Reset();
        });
    }
    #endregion

    #region Private methods
    private void Resize(Vector2I size)
    {
        _columns = size.X;
        _rows = size.Y;

        CreateCells();
        ConnectCells();
        SplitCellsMatrixToChunks();
    }

    private void CreateCells()
    {
        _cellsMatrix = new Cell[_columns, _rows];

        for (int y = 0; y < _rows; y++)
            for (int x = 0; x < _columns; x++)
                _cellsMatrix[x, y] = new Cell();

        Cells = _cellsMatrix.Cast<Cell>().Shuffle().ToList();
    }

    private void ConnectCells()
    {
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _columns; x++)
            {
                bool isLeftEdge = x == 0,
                     isRightEdge = x == _columns - 1,
                     isTopEdge = y == 0,
                     isBottomEdge = y == _rows - 1;

                var cell = _cellsMatrix[x, y];

                // Left-top neighbor
                if (isLeftEdge == false && isTopEdge == false)
                    cell.Neighbors[CellNeighborPosition.LeftTop] = _cellsMatrix[x - 1, y - 1];

                // Top neighbor
                if (isTopEdge == false)
                    cell.Neighbors[CellNeighborPosition.Top] = _cellsMatrix[x, y - 1];

                // Right-top neighbor
                if (isRightEdge == false && isTopEdge == false)
                    cell.Neighbors[CellNeighborPosition.RightTop] = _cellsMatrix[x + 1, y - 1];

                // Left neighbor
                if (isLeftEdge == false)
                    cell.Neighbors[CellNeighborPosition.Left] = _cellsMatrix[x - 1, y];

                // Right neighbor
                if (isRightEdge == false)
                    cell.Neighbors[CellNeighborPosition.Right] = _cellsMatrix[x + 1, y];

                // Left-bottom neighbor
                if (isLeftEdge == false && isBottomEdge == false)
                    cell.Neighbors[CellNeighborPosition.LeftBottom] = _cellsMatrix[x - 1, y + 1];

                // Bottom neighbor
                if (isBottomEdge == false)
                    cell.Neighbors[CellNeighborPosition.Bottom] = _cellsMatrix[x, y + 1];

                // Right-bottom neighbor
                if (isRightEdge == false && isBottomEdge == false)
                    cell.Neighbors[CellNeighborPosition.RightBottom] = _cellsMatrix[x + 1, y + 1];

                var neighborList = cell.Neighbors.Values.Where(n => n != null).Shuffle().ToList();
                cell.ExistingNeighbors = neighborList!;

                neighborList.Add(cell);
                cell.ExistingNeighborsAndSelf = neighborList!;
            }
        }
    }

    private void SplitCellsMatrixToChunks()
    {
        var chunkCount = System.Environment.ProcessorCount * 2;

        var chunks = _cellsMatrix.SplitToVerticalChunks(chunkCount);
        chunks.Shuffle();
        var shuffledChunks = chunks.Select(chunk => { chunk.Shuffle(); return chunk; });
        var indexedChunks = shuffledChunks.Zip(Enumerable.Range(0, chunkCount));

        EvenChunks = indexedChunks.Where(pair => pair.Second % 2 == 0).Select(pair => pair.First.ToList()).ToList();
        OddChunks = indexedChunks.Where(pair => pair.Second % 2 != 0).Select(pair => pair.First.ToList()).ToList();
    }
    #endregion
}