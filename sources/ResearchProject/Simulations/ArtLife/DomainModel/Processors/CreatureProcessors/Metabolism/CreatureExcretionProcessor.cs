public class CreatureExcretionProcessor : CreatureMetabolismProcessorBase
{
    protected override void ProcessAliveCreature(Creature creature)
    {
        if (creature.Properties.MetabolismState != CreatureMetabolismState.Excretion)
            return;

        var surroundingCells = creature.HabitatCell.ExistingNeighborsAndSelf;

        var excretionSubstance = GetExcretionSubstance(creature);

        var substanceAmountToExcrete = creature.Properties.SubstancesToExcrete[excretionSubstance];
        creature.Properties.SubstancesToExcrete[excretionSubstance] = 0;

        // TODO: Optimize this (as in substance sharing processor)
        while (substanceAmountToExcrete-- > 0)
            surroundingCells.RandomElementThreadSafe().Substances[excretionSubstance] += 1;

        creature.Properties.MetabolismState = CreatureMetabolismState.Eating;
    }
}