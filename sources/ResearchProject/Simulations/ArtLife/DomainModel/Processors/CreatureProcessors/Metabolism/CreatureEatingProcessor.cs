using System;
using System.Linq;

public class CreatureEatingProcessor : CreatureMetabolismProcessorBase
{
    private readonly WorldEnvironment _environment;

    public CreatureEatingProcessor(WorldEnvironment environment) => _environment = environment;

    protected override void ProcessAliveCreature(Creature creature)
    {
        if (creature.Properties.MetabolismState != CreatureMetabolismState.Eating)
            return;

        // IGNORE WATER FOR NOW
        // Water - neccessary substance every (almost every) creature consumes
        // var cellsHavingWater = CellHelpers.GetNearestCellsHavingSubstance(creature.ParentCell, Substance.Water);

        var eatingResult = false;

        switch (creature.Fenotype.DietType)
        {
            case CreatureDietType.PhotoAutotrophic:
            case CreatureDietType.PhotoHeterotrophic:
            case CreatureDietType.ChemoAutotrophic:
            case CreatureDietType.ChemoHeterotrophic:
                eatingResult = ProcessPhotoOrChemotrophicEating(creature);
                break;
            case CreatureDietType.Saprotrophic:
                eatingResult = ProcessSaprotrophicEating(creature); break;
            case CreatureDietType.Parasitic:
                eatingResult = ProcessParasiticEating(creature); break;
        }

        if (eatingResult && creature.Properties.FoodSatisfaction < ArtLifeSettings.CreatureSatisfactionMaxValue)
            creature.Properties.FoodSatisfaction++;
        else
            creature.Properties.FoodSatisfaction--;
    }

    private bool ProcessPhotoOrChemotrophicEating(Creature creature)
    {
        if (CanPhotoOrChemotrophEat(creature) == false)
            return false;

        var foodSubstance = GetFoodSubstanceByDietType(creature.Fenotype.DietType);

        var cellsHavingFoodSubstance = CellHelpers.GetSurroundingCellsHavingSubstance(creature.HabitatCell, foodSubstance);
        if (cellsHavingFoodSubstance.Any() == false)
        {
            TryChangeDietType(creature);
            return false;
        }

        var cellHavingFoodSubstance = cellsHavingFoodSubstance.RandomElementThreadSafe();

        cellHavingFoodSubstance.Substances[foodSubstance] -= 1;
        creature.Properties.SubstancesInProcessing[foodSubstance] += 1;

        if (creature.Properties.SubstancesInProcessing[foodSubstance] == creature.Genotype.FoodSubstanceCapacity)
            creature.Properties.MetabolismState = CreatureMetabolismState.Digestion;

        return true;
    }

    private bool CanPhotoOrChemotrophEat(Creature creature) => creature.Fenotype.DietType switch
    {
        CreatureDietType.PhotoAutotrophic => CanPhototrophCreatureEat(creature),
        CreatureDietType.PhotoHeterotrophic => CanPhototrophCreatureEat(creature),

        CreatureDietType.ChemoAutotrophic => CanChemotrophCreatureEat(creature),
        CreatureDietType.ChemoHeterotrophic => CanChemotrophCreatureEat(creature),

        _ => throw new NotImplementedException()
    };
    private bool CanPhototrophCreatureEat(Creature creature) => _environment.TimeOfDay == TimeOfDay.Day;
    // This is temporary decision. In truth, they should use energy of other substances to make digestion
    private bool CanChemotrophCreatureEat(Creature creature) => true;

    private bool TryChangeDietType(Creature creature)
    {
        if (creature.Genotype.DietType != CreatureDietType.Mixotrophic)
            return false;

        CreatureDietType currentDietType = creature.Fenotype.DietType,
                         newDietType = GetOppositeDietType(currentDietType);

        creature.Fenotype.DietType = newDietType;

        return true;
    }

    private CreatureDietType GetOppositeDietType(CreatureDietType dietType) => dietType switch
    {
        CreatureDietType.PhotoAutotrophic => CreatureDietType.PhotoHeterotrophic,
        CreatureDietType.PhotoHeterotrophic => CreatureDietType.PhotoAutotrophic,

        CreatureDietType.ChemoAutotrophic => CreatureDietType.ChemoHeterotrophic,
        CreatureDietType.ChemoHeterotrophic => CreatureDietType.ChemoAutotrophic,

        _ => throw new NotImplementedException()
    };

    private bool ProcessSaprotrophicEating(Creature creature)
    {
        var cell = creature.HabitatCell;

        var nearestDeadCreatures = cell.ExistingNeighborsAndSelf
            .Where(c => c.Creature != null)
            .Select(c => c.Creature!)
            .Where(c => c.Properties.LiveState == CreatureLiveState.Dead)
            .Where(c => c.Properties.BodySubstances[Substance.Organics] > 0);
        
        if (nearestDeadCreatures.Any() == false)
            return false;

        var deadCreature = nearestDeadCreatures.RandomElementThreadSafe();
        
        deadCreature.Properties.BodySubstances[Substance.Organics] -= 1;
        creature.Properties.SubstancesInProcessing[Substance.Organics] += 1;

        if (creature.Properties.SubstancesInProcessing[Substance.Organics] == creature.Genotype.FoodSubstanceCapacity)
            creature.Properties.MetabolismState = CreatureMetabolismState.Digestion;

        return true;
    }

    private bool ProcessParasiticEating(Creature creature)
    {
        // Паразит должен следовать за хозяином?

        var neighborCreatures = creature.HabitatCell.ExistingNeighbors
            .Where(c => c.Creature != null)
            .Select(c => c.Creature!);

        if (neighborCreatures.Any() == false)
            return false;

        var donor = neighborCreatures.RandomElementThreadSafe();


        // Проверка на наличие всех веществ тела существа.
        // Если пусто - существо необходимо убрать с поля мира
        //if (creature.Properties.BodySubstances[Substance.Organics]) { }

        // берем в-ва и ...

        return true;
    }
}