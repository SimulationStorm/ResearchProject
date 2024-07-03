public class CreatureDigestionProcessor : CreatureMetabolismProcessorBase
{
    protected override void ProcessAliveCreature(Creature creature)
    {
        if (creature.Properties.MetabolismState != CreatureMetabolismState.Digestion)
            return;

        Substance foodSubstance = GetFoodSubstance(creature),
                  excrectionSubstance = GetExcretionSubstance(creature);

        creature.Properties.SubstancesInProcessing[foodSubstance] -= 1;
        creature.Properties.SubstancesToExcrete[excrectionSubstance] += 1;

        if (creature.Properties.SubstancesInProcessing[foodSubstance] == 0)
            creature.Properties.MetabolismState = CreatureMetabolismState.Excretion;
    }
}