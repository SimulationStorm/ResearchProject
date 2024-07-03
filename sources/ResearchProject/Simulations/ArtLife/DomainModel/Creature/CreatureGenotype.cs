public class CreatureGenotype
{
    //public SubstancesContainer BodySubstanceAmounts { get; init; } = new();



    public CreatureDietType DietType { get; set; }
    public int FoodSubstanceCapacity { get; set; }
    //public int NeedForFood { get; set; }
    //public int FoodDigestionRate { get; set; }



    public CreatureBreathType BreathType { get; set; }
    public int BreathSubstanceCapacity { get; set; }
    //public int NeedForBreath { get; set; }
    //public int BreathRate { get; set; }



    public CreatureThermoregulationType ThermoregulationType { get; set; }
    /// <summary>
    /// Активен для теплокровных
    /// 
    /// Скорость теплообмена, ед. / тик
    /// </summary>
    public double ThermoregulationRate { get; set; }
    /// <summary>
    /// Активен для теплокровных
    /// 
    /// Целевая температура тела существа.
    /// Если текущая отличается от целевой, у существа отнимается единица здоровья
    /// </summary>
    public double TargetTemperature { get; set; }
    public Interval TolerantTemperatureInterval { get; set; }



    public CreatureReproductionType ReproductionType { get; set; }



    public int AgeLimit { get; set; }
}