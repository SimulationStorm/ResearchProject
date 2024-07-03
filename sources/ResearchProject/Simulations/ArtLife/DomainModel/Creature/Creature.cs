public class Creature
{
    public Cell HabitatCell { get; set; } = null!;

    public CreatureGenotype Genotype { get; set; } = null!;

    public CreatureFenotype Fenotype { get; set; } = null!;

    public CreatureProperties Properties { get; set; } = null!;
}