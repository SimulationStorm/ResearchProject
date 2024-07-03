using System;
using System.Linq;

public class CreatureMovementProcessor : AliveCreatureProcessor
{
    protected override void ProcessAliveCreature(Creature creature)
    {
        if (creature.Properties.MetabolismState == CreatureMetabolismState.Eating
            || creature.Properties.BreathState == CreatureBreathState.Inhale)
            return;

        int foodSatisfaction = creature.Properties.FoodSatisfaction,
            breathSatisfaction = creature.Properties.BreathSatisfaction,
            temperatureSatisfaction = creature.Properties.TemperatureSatisfaction;

        if (foodSatisfaction == ArtLifeSettings.CreatureSatisfactionMaxValue
            && breathSatisfaction == ArtLifeSettings.CreatureSatisfactionMaxValue
            && temperatureSatisfaction == ArtLifeSettings.CreatureSatisfactionMaxValue)
            return;

        var isMoved = false;

        if (foodSatisfaction < breathSatisfaction && foodSatisfaction < temperatureSatisfaction)
        {
            var foodSubstance = GetFoodSubstanceByDietType(creature.Fenotype.DietType);

            var surroundingCells = creature.HabitatCell.ExistingNeighbors;
            var unoccupiedSurroundingCells = surroundingCells.Where(c => c.Creature == null);
            if (unoccupiedSurroundingCells.Any())
            {
                var whereIsMoreFood = unoccupiedSurroundingCells.MaxBy(c => c.Substances[foodSubstance])!;
                MoveCreatureToCell(creature, whereIsMoreFood);

                isMoved = true;
            }
        }
        
        if (isMoved == false && breathSatisfaction < foodSatisfaction && breathSatisfaction < temperatureSatisfaction)
        {
            var breathSubstance = GetInhaleSubstance(creature);

            var surroundingCells = creature.HabitatCell.ExistingNeighbors;
            var unoccupiedSurroundingCells = surroundingCells.Where(c => c.Creature == null);
            if (unoccupiedSurroundingCells.Any())
            {
                var whereIsMoreBreathSubstance = unoccupiedSurroundingCells.MaxBy(c => c.Substances[breathSubstance])!;
                MoveCreatureToCell(creature, whereIsMoreBreathSubstance);

                isMoved = true;
            }
        }
        
        if (isMoved == false)
        {
            var surroundingCells = creature.HabitatCell.ExistingNeighbors;
            var unoccupiedSurroundingCells = surroundingCells.Where(c => c.Creature == null);
            if (unoccupiedSurroundingCells.Any())
            {
                var whereIsBetterTemperature = unoccupiedSurroundingCells.FirstOrDefault(c => creature.Genotype.TolerantTemperatureInterval.Contains(c.Temperature));
                if (whereIsBetterTemperature != null)
                    MoveCreatureToCell(creature, whereIsBetterTemperature);
            }
        }
    }

    protected static Substance GetFoodSubstanceByDietType(CreatureDietType dietType) => dietType switch
    {
        CreatureDietType.PhotoAutotrophic => Substance.Minerals,
        CreatureDietType.PhotoHeterotrophic => Substance.Organics,

        CreatureDietType.ChemoAutotrophic => Substance.Minerals,
        CreatureDietType.ChemoHeterotrophic => Substance.Organics,

        CreatureDietType.Saprotrophic => Substance.Organics,

        _ => throw new NotImplementedException()
    };

    private static Substance GetInhaleSubstance(Creature creature) => creature.Fenotype.BreathType switch
    {
        CreatureBreathType.Aerobic => Substance.Oxygen,
        CreatureBreathType.Anaerobic => Substance.CarbonDioxide,

        _ => throw new NotImplementedException()
    };

    private void MoveCreatureToCell(Creature creature, Cell cell)
    {
        creature.HabitatCell.Creature = null;
        creature.HabitatCell = cell;
        cell.Creature = creature;
    }

    //private static bool CheckCreatureSatisfaction(Creature creature) =>
    //    creature.Properties.FoodSatisfaction > 0
    //    && creature.Properties.BreathSatisfaction > 0
    //    && creature.Properties.TemperatureSatisfaction > 0;
}