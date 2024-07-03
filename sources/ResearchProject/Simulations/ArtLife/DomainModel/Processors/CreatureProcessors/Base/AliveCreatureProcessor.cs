public abstract class AliveCreatureProcessor : IProcessor<Creature>
{
    public void Process(Creature creature)
    {
        if (creature.Properties.LiveState != CreatureLiveState.Dead)
            ProcessAliveCreature(creature);
    }

    protected abstract void ProcessAliveCreature(Creature creature);
}