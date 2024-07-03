using System;
using System.Linq;

public class CellCreatureSpawningProcessor : IProcessor<Cell>
{
    private WorldEnvironment _worldEnv = null!;

    public CellCreatureSpawningProcessor(WorldEnvironment worldEnv) => _worldEnv = worldEnv;

    public void Process(Cell cell)
    {
        // If cell is already occupied/habited by a creatures
        if (cell.Creature != null)
            return;
        //Possibility condition
        else if (Random.Shared.NextDouble() > 0.00001)
            return;

        // Substances condition
        //var totalSubstances = new SubstancesContainer();

        //cell.ExistingNeighborsAndSelf.ForEach(cell => totalSubstances.Add(cell.Substances));

        //var substancesCondition = totalSubstances[Substance.Water] > 50
        //                           && totalSubstances[Substance.Organics] > 30
        //                           && totalSubstances[Substance.Minerals] > 10;

        //if (substancesCondition == false)
        //    return;

        // Temperature condition
        //var averageTemperatureAmongNeighbors = cell.ExistingNeighborsAndSelf.Sum(cell => cell.Temperature) / cell.ExistingNeighborsAndSelf.Count;
        //var temperatureCondition = averageTemperatureAmongNeighbors > 20;

        //if (temperatureCondition == false)
        //    return;

        // Spawning
        
    }
}

public static class CreatureFactory
{
    public static Creature Create(Cell cell, CreatureBreathType breathType)
    {
        var geneticMaterial = new CreatureGenotype
        {
            DietType = CreatureDietType.PhotoAutotrophic,
            FoodSubstanceCapacity = 10,

            BreathType = breathType,
            BreathSubstanceCapacity = 10,

            ThermoregulationType = CreatureThermoregulationType.ColdBlooded,
            ThermoregulationRate = 5,
            TargetTemperature = -40,
            TolerantTemperatureInterval = new Interval
            {
                Min = new Endpoint
                {
                    Type = EndpointType.Including,
                    Value = 0
                },
                Max = new Endpoint
                {
                    Type = EndpointType.Including,
                    Value = 20
                }
            },

            AgeLimit = 1000
        };

        var properties = new CreatureProperties
        {
            LiveState = CreatureLiveState.Alive,
            ActivityState = CreatureActivityState.Wakefullness,
            BreathState = CreatureBreathState.Inhale,
            MetabolismState = CreatureMetabolismState.Eating,

            Age = 0,
            Health = 100,
            Energy = 100,
            Temperature = cell.ExistingNeighborsAndSelf.Sum(c => c.Temperature) / cell.ExistingNeighborsAndSelf.Count,
            BodySubstances =
            {
                [Substance.Water] = 50,
                [Substance.Organics] = 30,
                [Substance.Minerals] = 10
            }
        };

        var creature = new Creature
        {
            HabitatCell = cell,
            Genotype = geneticMaterial,
            Properties = properties
        };

        cell.Creature = creature;

        return creature;
    }
}

//public class CreatureGeneticMaterialFactory
//{
//    // We need create here 'average' balanced genetic material
//    // Здесь будет выбрать такой тип дыхания/питания, который
//    // лучше всего подходит в данной среде.
//    private CreatureGeneticMaterial Create() => new CreatureGeneticMaterial
//    {
//        DietType = GetDietType(),
//    };

//    private static CreatureDietType GetDietType() => CreatureDietTypeExtensions.AllDietTypes.RandomElementThreadSafe();
//}