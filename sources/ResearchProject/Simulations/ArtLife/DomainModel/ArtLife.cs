using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Godot;

public class ArtLife
{
    #region Properties
    public WorldField WorldField { get; init; } = null!;

    public WorldEnvironment WorldEnvironment { get; init; } = null!;

    public WorldStatistics WorldStatistics { get; init; } = null!;
    #endregion

    #region Fields
    private IEnumerable<IProcessor<Cell>> _cellProcessors = null!;

    private IEnumerable<IProcessor<Creature>> _creatureProcessors = null!;
    #endregion

    #region Setup
    public ArtLife(Vector2I fieldSize)
    {
        WorldField = new WorldField(fieldSize);
        WorldEnvironment = new WorldEnvironment();
        WorldStatistics = new WorldStatistics(WorldField);

        SynchronizeFieldAndEnvironment();

        SetupCellProcessors();
        SetupCreatureProcessors();
    }

    private void SetupCellProcessors() => _cellProcessors = new IProcessor<Cell>[]
    {
        //new CellSubstancesSharingProcessor(),
        //new CellTemperatureSharingProcessor(WorldEnvironment),
        //new CellAciditySharingProcessor(),
        //new CellCreatureSpawningProcessor(WorldEnvironment)
    };

    private void SetupCreatureProcessors() => _creatureProcessors = new IProcessor<Creature>[]
    {
        // Группировка систем

        new CreatureThermoregulationProcessor(),

        new CreatureEatingProcessor(WorldEnvironment),
        new CreatureDigestionProcessor(),
        new CreatureExcretionProcessor(),

        new CreatureBreathingProcessor(),

        new CreatureReproductionProcessor(),

        new CreatureMovementProcessor(),

        new CreatureLiveStateProcessor()
    };
    #endregion

    #region Public methods
    public void Advance()
    {
        foreach (var cellProcessor in _cellProcessors)
        {
            Parallel.ForEach(WorldField.EvenChunks, chunk =>
            {
                foreach (var cell in chunk)
                    cellProcessor.Process(cell);
            });
            Parallel.ForEach(WorldField.OddChunks, chunk =>
            {
                foreach (var cell in chunk)
                    cellProcessor.Process(cell);
            });
        }

        foreach (var creatureProcessor in _creatureProcessors)
        {
            Parallel.ForEach(WorldField.EvenChunks, chunk =>
            {
                var creatures = chunk.Where(cell => cell.Creature != null).Select(cell => cell.Creature!);
                foreach (var creature in creatures)
                    creatureProcessor.Process(creature);
            });
            Parallel.ForEach(WorldField.OddChunks, chunk =>
            {
                var creatures = chunk.Where(cell => cell.Creature != null).Select(cell => cell.Creature!);
                foreach (var creature in creatures)
                    creatureProcessor.Process(creature);
            });
        }

        WorldEnvironment.Update();
        WorldStatistics.Update();
    }

    public void Reset(Vector2I? newFieldSize = null)
    {
        if (newFieldSize is not null)
        {
            WorldField.Size = newFieldSize.Value;

            FillWithSubstances();

            SynchronizeFieldAndEnvironment();
        }
        else
        {
            WorldField.Reset();
        }

        WorldEnvironment.Reset();

        WorldStatistics.Update();
    }

    public void FillWithSubstances()
    {
        var initialSubstances = ArtLifeSettings.InitialCellSubstances;
        Parallel.ForEach(WorldField.Cells, cell => cell.Substances.Add(initialSubstances));

        WorldStatistics.Update();
    }
    #endregion

    private void SynchronizeFieldAndEnvironment() =>
        Parallel.ForEach(WorldField.Cells, cell => cell.Temperature = WorldEnvironment.YearSeason.MinTemperature());
}