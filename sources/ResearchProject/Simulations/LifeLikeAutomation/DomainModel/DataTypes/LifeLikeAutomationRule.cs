using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public partial class LifeLikeAutomationRule : IEquatable<LifeLikeAutomationRule>
{
	#region Constants
	public const int RuleLength = 9;

	public static readonly LifeLikeAutomationRule Empty = FromString("b/s");
	#endregion

	#region Fields
	[GeneratedRegex("^b(?<born>0?1?2?3?4?5?6?7?8?)\\/s(?<survival>0?1?2?3?4?5?6?7?8?)$",
		RegexOptions.IgnoreCase | RegexOptions.Compiled)]
	private static partial Regex _cellBornSurvivalRuleRegex();

	private readonly IReadOnlyList<bool> _bornRule,
							             _survivalRule;
	#endregion

	public LifeLikeAutomationRule(IReadOnlyList<bool> bornRule, IReadOnlyList<bool> survivalRule)
	{
		if (bornRule?.Count is not RuleLength)
			throw new ArgumentException("Should be a bool array containing 9 values.", nameof(bornRule));

		if (survivalRule?.Count is not RuleLength)
			throw new ArgumentException("Should be a bool array containing 9 values.", nameof(survivalRule));

		_bornRule = bornRule;
		_survivalRule = survivalRule;
	}

	#region Public methods 
	public bool IsBornWhen(int neighbors) => _bornRule[neighbors];

	public bool IsSurvivalWhen(int neighbors) => _survivalRule[neighbors];

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.Append('b');
		for (var n = 0; n < RuleLength; n++)
			if (_bornRule[n])
				sb.Append(n);

		sb.Append("/s");
		for (var n = 0; n < RuleLength; n++)
			if (_survivalRule[n])
				sb.Append(n);

		return sb.ToString();
	}

    public static LifeLikeAutomationRule FromString(string rule)
	{
		ArgumentNullException.ThrowIfNull(rule);

		if (_cellBornSurvivalRuleRegex().IsMatch(rule) == false)
			throw new ArgumentException($"Should match to regex {_cellBornSurvivalRuleRegex()}.", nameof(rule));

		var groups = _cellBornSurvivalRuleRegex().Match(rule).Groups;

		string bornRule = groups["born"].Value,
			   survivalRule = groups["survival"].Value;

        bool[] bornNeighborCounts = new bool[RuleLength],
               survivalNeighborCounts = new bool[RuleLength];

		foreach (var neighborCount in bornRule)
            bornNeighborCounts[neighborCount - '0'] = true;

        foreach (var neighborCount in survivalRule)
            survivalNeighborCounts[neighborCount - '0'] = true;

        return new(bornNeighborCounts, survivalNeighborCounts);
    }

    public override bool Equals(object? obj) => Equals(obj as LifeLikeAutomationRule);

    public bool Equals(LifeLikeAutomationRule? other) => ToString() == other?.ToString();

    public override int GetHashCode() => ToString().GetHashCode();
    #endregion
}