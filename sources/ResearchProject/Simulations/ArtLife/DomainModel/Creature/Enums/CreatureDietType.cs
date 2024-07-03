// Autotrophs - consume inorganic substances and produce organic substances
// Heterotrophs - consume organic substances and produce inorganic substances (^ vice versa)

using System;
using System.Collections.Generic;

public enum CreatureDietType
{
    // They use energy of sun to do substance processing
    PhotoAutotrophic,
    PhotoHeterotrophic,

    // They use energy from destruction substances to do substance processing
    ChemoAutotrophic,
    ChemoHeterotrophic,

    // Mixotrophs are able to use both autotrophic and heterotrophic methods of nutrition, depending on the available resources. (photo/chemo+auto/hetero)
    Mixotrophic,

    Saprotrophic, // Saprotrophs - consume parts of dead organisms or organics and produce inorganic substances
    Parasitic, // Parasites live at the expense of other host-organisms; they consume organic substances from the host bodies and produce inorganic substances
}

public static class CreatureDietTypeExtensions
{
    public static IEnumerable<CreatureDietType> AllDietTypes { get; } = Enum.GetValues<CreatureDietType>();
}