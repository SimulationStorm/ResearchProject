using System.Collections.Generic;

public class UniversalAutomationRuleSetBuilder
{
    private int _applicationsNumber;

    private readonly IList<UniversalAutomationRule> _rules = new List<UniversalAutomationRule>();

    public UniversalAutomationRuleSetBuilder HasApplicationsNumber(int applicationsNumber)
    {
        _applicationsNumber = applicationsNumber;
        return this;
    }

    public UniversalAutomationRuleSetBuilder HasRule(UniversalAutomationRule rule)
    {
        _rules.Add(rule);
        return this;
    }

    public UniversalAutomationRuleSetBuilder HasRules(IEnumerable<UniversalAutomationRule> rules)
    {
        foreach (var rule in rules)
            _rules.Add(rule);

        return this;
    }

    public UniversalAutomationRuleSet Build() => new(_applicationsNumber, _rules);
}