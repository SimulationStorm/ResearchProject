using System.Collections.Generic;

public class UniversalAutomationKindBuilder
{
    private string _name = string.Empty;

    private int _repetitionsNumber;

    private readonly ISet<UniversalAutomationState> _states = new HashSet<UniversalAutomationState>();

    private UniversalAutomationState _defaultState = null!;

    private readonly IList<UniversalAutomationRuleSet> _ruleSets = new List<UniversalAutomationRuleSet>();

    public UniversalAutomationKindBuilder HasName(string name)
    {
        _name = name;
        return this;
    }

    public UniversalAutomationKindBuilder HasRuleSetsRepetitionsNumber(int repetitionsNumber)
    {
        _repetitionsNumber = repetitionsNumber;
        return this;
    }

    public UniversalAutomationKindBuilder HasDefaultState(UniversalAutomationState state)
    {
        _states.Add(state);
        _defaultState = state;
        return this;
    }

    public UniversalAutomationKindBuilder HasState(UniversalAutomationState state)
    {
        _states.Add(state);
        return this;
    }

    public UniversalAutomationKindBuilder HasRuleSet(UniversalAutomationRuleSet ruleSet)
    {
        _ruleSets.Add(ruleSet);
        return this;
    }

    public UniversalAutomationKind Build() => new(_name, _repetitionsNumber, _states, _defaultState, _ruleSets);
}