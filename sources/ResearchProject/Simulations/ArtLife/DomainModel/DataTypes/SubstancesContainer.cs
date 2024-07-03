using System.Collections.Generic;

public class SubstancesContainer : Dictionary<Substance, int>
{
    public SubstancesContainer() => Reset();

    public void Reset()
    {
        foreach (var substance in SubstanceExtensions.AllSubstances)
            this[substance] = 0;
    }

    public void Add(SubstancesContainer other)
    {
        foreach (var substance in SubstanceExtensions.AllSubstances)
            this[substance] += other[substance];
    }

    public void Subtract(SubstancesContainer other)
    {
        foreach (var substance in SubstanceExtensions.AllSubstances)
            this[substance] -= other[substance];
    }
}