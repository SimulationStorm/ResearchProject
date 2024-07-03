using System;
using System.Collections.Generic;
using System.Linq;

public class UniversalAutomationRuleSet
{
    #region Properties
    public int ApplicationsNumber { get; }

    public IEnumerable<UniversalAutomationRule> Rules { get; }
    #endregion

    public UniversalAutomationRuleSet(int applicationsNumber, IEnumerable<UniversalAutomationRule> rules)
    {
        ApplicationsNumber = applicationsNumber > 0 ? applicationsNumber
            : throw new ArgumentException("Should be greater than 0.", nameof(applicationsNumber));

        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        Rules = rules.Any() ? rules : throw new ArgumentException("Should contain at least one rule.", nameof(rules));
    }
}