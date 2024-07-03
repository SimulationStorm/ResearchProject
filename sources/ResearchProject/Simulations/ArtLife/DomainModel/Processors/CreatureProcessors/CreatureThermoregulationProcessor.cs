using System;
using System.Linq;

public class CreatureThermoregulationProcessor : AliveCreatureProcessor
{
	protected override void ProcessAliveCreature(Creature creature)
    {
        // По умолчанию, создание будет нагреваться/охлаждаться до температуры окружающих клеток
        ProcessCreatureAndSurroundingCellsTemperatureSharing(creature);

        if (creature.Genotype.ThermoregulationType == CreatureThermoregulationType.WarmBlooded)
            ProcessWarmBloodedThermoregulation(creature);

        // Помещать такие feature/gene-specific checkings в самих процессорах этих генов или в liveStateProcessor???
        // Вероятно, в сами процессоры; это группировка по feature'ам
        var isTemperatureTolerant = creature.Genotype.TolerantTemperatureInterval.Contains(creature.HabitatCell.Temperature);
        if (isTemperatureTolerant && creature.Properties.TemperatureSatisfaction < ArtLifeSettings.CreatureSatisfactionMaxValue)
            creature.Properties.TemperatureSatisfaction++;
        else
            creature.Properties.TemperatureSatisfaction--;
    }

    // TODO: Extract this repeating temperature sharing code in some static utility
	private static void ProcessCreatureAndSurroundingCellsTemperatureSharing(Creature creature)
	{
        var surroundingCells = creature.HabitatCell.ExistingNeighborsAndSelf;

        var surroundingCellsAverageTemperature = surroundingCells.Average(c => c.Temperature);
        var temperatureDifference = surroundingCellsAverageTemperature - creature.Properties.Temperature;
        if (temperatureDifference == 0)
            return;

        var temperatureShareAmount = Math.Sign(temperatureDifference) * ArtLifeSettings.TemperatureShareAmount;
        if (Math.Abs(temperatureDifference) < ArtLifeSettings.TemperatureShareAmount)
            temperatureShareAmount = temperatureDifference;

        creature.Properties.Temperature += temperatureShareAmount;

        var temperatureShareAmountPerCell = temperatureShareAmount / surroundingCells.Count;
        foreach (var neighborCell in surroundingCells)
            neighborCell.Temperature -= temperatureShareAmountPerCell;
    }

	private static void ProcessWarmBloodedThermoregulation(Creature creature)
	{
        var temperatureDifference = creature.Genotype.TargetTemperature - creature.Properties.Temperature;
        if (temperatureDifference == 0)
            return;

        var thermoregulationRate = creature.Genotype.ThermoregulationRate;

        var temperatureShareAmount = Math.Sign(temperatureDifference) * thermoregulationRate;
        if (Math.Abs(temperatureDifference) < thermoregulationRate)
            temperatureShareAmount = temperatureDifference;

        creature.Properties.Temperature += temperatureShareAmount;
    }
}