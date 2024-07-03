using System;

public class WorldEnvironment
{
    #region Properties
    public int IterationNumber { get; private set; }

    public int YearNumber { get; private set; }
    public YearSeason YearSeason { get; private set; }
    public int YearSeasonProgress { get; private set; }

    public int DayNumber { get; private set; }
    public TimeOfDay TimeOfDay { get; private set; }
    public int TimeOfDayProgress { get; private set; }

    public double TemperatureStep { get; private set; }
    #endregion

    #region Fields
    private YearSeason _prevYearSeason;

    private TimeOfDay _prevTimeOfDay;
    #endregion

    public WorldEnvironment() => Reset();

    #region Public methods
    public void Update()
    {
        UpdateTimeOfDay();
        UpdateYearSeason();
        UpdateTemperatureStep();

        IterationNumber++;
    }

    public void Reset()
    {
        IterationNumber = 0;

        YearNumber = 0;
        YearSeasonProgress = 0;

        _prevYearSeason = YearSeason.Spring;
        YearSeason = YearSeason.Spring;

        DayNumber = 0;
        TimeOfDayProgress = 0;

        _prevTimeOfDay = TimeOfDay.Morning;
        TimeOfDay = TimeOfDay.Morning;

        TemperatureStep = GetNormalTemperatureDifference(YearSeason.Spring) / TimeOfDay.DurationInTicks();
    }
    #endregion

    #region Private methods
    private void UpdateTimeOfDay()
    {
        if (TimeOfDayProgress != TimeOfDay.DurationInTicks())
        {
            TimeOfDayProgress++;
            return;
        }

        TimeOfDayProgress = 0;

        _prevTimeOfDay = TimeOfDay;

        TimeOfDay = TimeOfDay.Next();
        if (TimeOfDay == TimeOfDay.Morning)
        {
            DayNumber++;
            YearSeasonProgress++;
        }
    }

    private void UpdateYearSeason()
    {
        if (YearSeasonProgress != YearSeason.DurationInDays())
            return;

        YearSeasonProgress = 0;

        _prevYearSeason = YearSeason;

        YearSeason = YearSeason.Next();
        if (YearSeason == YearSeason.Spring)
            YearNumber++;
    }

    private void UpdateTemperatureStep()
    {
        if (TimeOfDay == _prevTimeOfDay)
            return;

        TemperatureStep = GetTemperatureStepForTimeOfDay(TimeOfDay);
    }

    private double GetTemperatureStepForTimeOfDay(TimeOfDay timeOfDay)
    {
        var temperatureStep = GetTemperatureDifference() / timeOfDay.DurationInTicks();

        if (timeOfDay == TimeOfDay.Morning)
            return temperatureStep;
        else if (timeOfDay == TimeOfDay.Evening)
            return -temperatureStep;
        return 0;
    }

    private double GetTemperatureDifference() => IsTemperatureTransitionSmoothed()
        ? GetNormalTemperatureDifference(YearSeason) : GetSmoothingTemperatureDifference(_prevYearSeason, YearSeason);

    private bool IsTemperatureTransitionSmoothed() => YearSeasonProgress > ArtLifeSettings.DaysToSmoothTemperature;

    private static double GetNormalTemperatureDifference(YearSeason yearSeason) => Math.Abs(yearSeason.MinTemperature() - yearSeason.MaxTemperature());

    private static double GetSmoothingTemperatureDifference(YearSeason first, YearSeason second)
    {
        double tempFrom = first.MinTemperature(),
               tempTo = second.MaxTemperature();

        return tempFrom > tempTo ? tempFrom - tempTo : -(tempFrom - tempTo);
    }
    #endregion
}