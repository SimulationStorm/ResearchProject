using System;

public abstract class CreatureMetabolismProcessorBase : AliveCreatureProcessor
{
    protected static Substance GetFoodSubstance(Creature creature) => GetFoodSubstanceByDietType(creature.Fenotype.DietType);
    protected static Substance GetExcretionSubstance(Creature creature) => GetExcretionSubstanceByDietType(creature.Fenotype.DietType);

    protected static Substance GetFoodSubstanceByDietType(CreatureDietType dietType) => dietType switch
    {
        CreatureDietType.PhotoAutotrophic => Substance.Minerals,
        CreatureDietType.PhotoHeterotrophic => Substance.Organics,

        CreatureDietType.ChemoAutotrophic => Substance.Minerals,
        CreatureDietType.ChemoHeterotrophic => Substance.Organics,

        CreatureDietType.Saprotrophic => Substance.Organics,

        _ => throw new NotImplementedException()
    };

    protected static Substance GetExcretionSubstanceByDietType(CreatureDietType dietType) => dietType switch
    {
        CreatureDietType.PhotoAutotrophic => Substance.Organics,
        CreatureDietType.PhotoHeterotrophic => Substance.Minerals,

        CreatureDietType.ChemoAutotrophic => Substance.Organics,
        CreatureDietType.ChemoHeterotrophic => Substance.Minerals,

        CreatureDietType.Saprotrophic => Substance.Minerals,

        _ => throw new NotImplementedException()
    };
}