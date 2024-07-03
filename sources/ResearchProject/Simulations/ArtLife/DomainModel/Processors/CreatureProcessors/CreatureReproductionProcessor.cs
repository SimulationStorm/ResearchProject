public class CreatureReproductionProcessor : AliveCreatureProcessor
{
    protected override void ProcessAliveCreature(Creature creature)
    {
        // Here will be some complex logic.
        // Нужен ли механизм для удерживания тварей вместе для спаривания?
        // А ведь можно сделать ещё механизм откладывания яиц
        // Also, as I said before, we need mechanism to "couple" creatures in colonies.

        switch (creature.Genotype.ReproductionType)
        {
            case CreatureReproductionType.Division:
                ProcessDivision(creature);
                break;
            case CreatureReproductionType.Pairing:
                ProcessPairing(creature);
                break;
        }
    }

    private void ProcessDivision(Creature creature)
    {
        // Создание копии этого же создания с изменением одного случайного гена
    }

    private void ProcessPairing(Creature creature)
    {
        // Новое создание создаётся так: берётся половина генов от одного создания, половина генов от другого.
    }
}