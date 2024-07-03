using System.Linq;

public class CreatureActionProcessor : AliveCreatureProcessor
{
    private static readonly ICreatureAction[] _actions = new ICreatureAction[]
    {
        new CreatureRestAction(),
        new CreatureMoveAction(),
    };

    protected override void ProcessAliveCreature(Creature creature)
    {
        Process(creature);

        var actionsCanBeExecuted = _actions.Where(a => a.CanExecute(creature));
        var actionToExecute = actionsCanBeExecuted.RandomElementThreadSafe();
        actionToExecute.Execute(creature);
    }
}

//public class CreatureGrowthProcessor : IProcessor<Creature>
//{
//    public void Process(Creature cell)
//    {
//        var creature = cell.Creature!;

//        if (creature.GrowthStage != CreatureGrowthStage.Youth)
//            return;
//    }
//}

public interface ICreatureAction
{
    bool CanExecute(Creature creature);

    void Execute(Creature creature);
}

// What if we create the following mechanism:
// Creature will search for "cousins". It will move, while it is not founded
// Это позволит собираться тварям в колонии

public class CreatureMoveAction : ICreatureAction
{
    private Cell? _unoccupiedCell;

    public bool CanExecute(Creature creature) =>
        (_unoccupiedCell = creature.HabitatCell.ExistingNeighbors.FirstOrDefault(n => n.Creature == null)) != null;

    public void Execute(Creature creature)
    {
        // Механизм выбора направления движения ?
        //if (creature.ParentCell.IsSide)
        //{
        //}

        // Пока создание кушает - двигаться не может

        // Если поблизости есть создания, тогда ... ?

        // Если создание будет двигаться в том направлении, где больше пищи, где теплее/холоднее (ближе к target temperature)?

        creature.HabitatCell.Creature = null;

        creature.HabitatCell = _unoccupiedCell!;
        _unoccupiedCell!.Creature = creature;

        //creature.Properties.Energy -= 1;
    }
}

public class CreatureRestAction : ICreatureAction
{
    public bool CanExecute(Creature creature) => true;

    public void Execute(Creature creature) { }
}

public class CreatureShareSubstancesAction : ICreatureAction
{
    public bool CanExecute(Creature creature) => true;

    public void Execute(Creature creature)
    {
    }
}