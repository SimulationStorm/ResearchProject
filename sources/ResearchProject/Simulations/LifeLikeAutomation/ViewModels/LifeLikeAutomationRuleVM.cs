using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings.Interfaces;
using System;

public class LifeLikeAutomationRuleVM : ObservableObject, INotifyStateChanged
{
    public object? State { get; }

    #region Fields
    private readonly bool[] _neighborCountsToBorn = new bool[LifeLikeAutomationRule.RuleLength],
                            _neighborCountsToSurvival = new bool[LifeLikeAutomationRule.RuleLength];
    #endregion

    public LifeLikeAutomationRuleVM(LifeLikeAutomationRule? rule = null)
    {
        if (rule is not null)
            SetRule(rule);
    }

    // Todo: May be commands?
    #region Public methods
    public void SetBornWhen(int neighborCount, bool isBorn)
    {
        ValidateNeighborCount(neighborCount);

        if (_neighborCountsToBorn[neighborCount] == isBorn)
            return;

        _neighborCountsToBorn[neighborCount] = isBorn;
        OnPropertyChanged(nameof(State));
    }

    public void SetSurvivalWhen(int neighborCount, bool isSurvival)
    {
        ValidateNeighborCount(neighborCount);

        if (_neighborCountsToSurvival[neighborCount] == isSurvival)
            return;

        _neighborCountsToSurvival[neighborCount] = isSurvival;
        OnPropertyChanged(nameof(State));
    }

    public bool IsBornWhen(int neighborCount)
    {
        ValidateNeighborCount(neighborCount);
        return _neighborCountsToBorn[neighborCount];
    }

    public bool IsSurvivalWhen(int neighborCount)
    {
        ValidateNeighborCount(neighborCount);
        return _neighborCountsToSurvival[neighborCount];
    }

    public void SetRule(LifeLikeAutomationRule rule)
    {
        for (var neighborCount = 0; neighborCount < LifeLikeAutomationRule.RuleLength; neighborCount++)
        {
            _neighborCountsToBorn[neighborCount] = rule.IsBornWhen(neighborCount);
            _neighborCountsToSurvival[neighborCount] = rule.IsSurvivalWhen(neighborCount);
        }

        OnPropertyChanged(nameof(State));
    }

    public LifeLikeAutomationRule AsRule() => new(_neighborCountsToBorn, _neighborCountsToSurvival);
    #endregion

    private static void ValidateNeighborCount(int neighborCount)
    {
        if (neighborCount is < 0 or > LifeLikeAutomationRule.RuleLength - 1)
            throw new ArgumentException($"Should be in range 0 and 8; was = {neighborCount}.", nameof(neighborCount));
    }
}