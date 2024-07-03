public abstract class DeadCreatureProcessor : IProcessor<Creature>
{
    public virtual void Process(Creature creature)
    {
        if (creature.Properties.LiveState == CreatureLiveState.Alive)
            return;
    }

    protected abstract void ProcessDeadCreature(Creature creature);
}