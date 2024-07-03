using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public class WorldEnvPanelVM : ObservableObject, IPanelViewModel, IUnsubscribe
{
    #region Properties
    public bool IsShown { get; set; } = true;

    public int YearNumber => _worldEnv.YearNumber;
    public YearSeason YearSeason => _worldEnv.YearSeason;
    public int YearSeasonProgress => _worldEnv.YearSeasonProgress;

    public int DayNumber => _worldEnv.DayNumber;
    public TimeOfDay TimeOfDay => _worldEnv.TimeOfDay;
    public int TimeOfDayProgress => _worldEnv.TimeOfDayProgress;
    #endregion

    private readonly WorldEnvironment _worldEnv;

    public WorldEnvPanelVM(ArtLifeModel artLifeModel)
    {
        _worldEnv = artLifeModel.WorldEnvironment;

        int prevYearNumber = _worldEnv.YearNumber,
            prevYearSeasonProgress = _worldEnv.YearSeasonProgress,
            prevDayNumber = _worldEnv.DayNumber,
            prevTimeOfDayProgress = _worldEnv.TimeOfDayProgress;

        var prevYearSeason = _worldEnv.YearSeason;
        var prevTimeOfDay = _worldEnv.TimeOfDay;

        TriggerBinder.OnPropertyChanged(this, artLifeModel, o => o.State, _ =>
        {
            if (prevYearSeason != _worldEnv.YearSeason)
            {
                OnPropertyChanged(nameof(YearSeason));
                prevYearSeason = _worldEnv.YearSeason;
            }

            if (prevYearSeasonProgress != _worldEnv.YearSeasonProgress)
            {
                OnPropertyChanged(nameof(YearSeasonProgress));
                prevYearSeasonProgress = _worldEnv.YearSeasonProgress;
            }

            if (prevYearNumber != _worldEnv.YearNumber)
            {
                OnPropertyChanged(nameof(YearNumber));
                prevYearNumber = _worldEnv.YearNumber;
            }

            if (prevTimeOfDay != _worldEnv.TimeOfDay)
            {
                prevTimeOfDay = _worldEnv.TimeOfDay;
                OnPropertyChanged(nameof(TimeOfDay));
            }

            if (prevTimeOfDayProgress != _worldEnv.TimeOfDayProgress)
            {
                prevTimeOfDayProgress = _worldEnv.TimeOfDayProgress;
                OnPropertyChanged(nameof(TimeOfDayProgress));
            }

            if (prevDayNumber != _worldEnv.DayNumber)
            {
                prevDayNumber = _worldEnv.DayNumber;
                OnPropertyChanged(nameof(DayNumber));
            }
        });
    }

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}