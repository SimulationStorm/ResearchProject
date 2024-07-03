using Godot;

public partial class ArtLifeModel : SimulationModel
{
    #region Properties
    public WorldStatistics WorldStatistics => _artLife.WorldStatistics;

    public IGetCell<Cell> FieldCellGetter { get; init; }

    public WorldEnvironment WorldEnvironment => _artLife.WorldEnvironment;
    #endregion

    #region Protected methods
    protected override void DoAdvance() => _artLife.Advance();

    protected override void DoReset(Vector2I? newFieldSize) => _artLife.Reset(newFieldSize);
    #endregion

    private readonly ArtLife _artLife = null!;

    public ArtLifeModel(Vector2I fieldSize)
    {
        _artLife = new(fieldSize);

        _artLife.FillWithSubstances();

        FieldCellGetter = _artLife.WorldField;
    }
}