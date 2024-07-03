public class CreatureProperties
{
    public CreatureLiveState LiveState { get; set; }

    public CreatureBreathState BreathState { get; set; }

    public CreatureMetabolismState MetabolismState { get; set; }

    public CreatureActivityState ActivityState { get; set; }

    //public CreatureGrowthStage GrowthStage { get; set; } = null!;

    public int Age { get; set; } 

    public int Health { get; set; }

    public int Energy { get; set; }

    public double Temperature { get; set; }

    public int FoodSatisfaction { get; set; }

    public int BreathSatisfaction { get; set; }

    public int TemperatureSatisfaction { get; set; }

    //public double AciditySatisfaction { get; set; }

    public SubstancesContainer BodySubstances { get; init; } = new();

    public SubstancesContainer SubstancesInProcessing { get; init; } = new();

    public SubstancesContainer SubstancesToExcrete { get; init; } = new();

    // Вряд ли можно здесь хранить ссылку на соседа(ей)...
    // Ведь они должны быть рядом физически (на соседних клетках)
}

// Нужна ли нам возможность для сравнения генотипа? И если генотип схож более чем на N % - это родственное создание.
