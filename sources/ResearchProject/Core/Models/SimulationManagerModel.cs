using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;
using System;

public partial class SimulationManagerModel : ObservableObject, IUnsubscribe
{
    #region Properties
    /// <summary>
    /// The current running state of simulation
    /// </summary>
    [ObservableProperty]
    private SimulationRunningState _simulationRunningState;

    /// <summary>
    /// The current mode of simulation
    /// </summary>
    [ObservableProperty]
    private SimulationMode _simulationMode;

    /// <summary>
    /// Iterations of simulation per second
    /// </summary>
    [ObservableProperty]
    private int _iterationsPerSecond;

    /// <summary>
    /// If set to true, then the field will not be redrawn according to the current state of the simulation
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FieldRedrawRequired))]
    private bool _skipFrames;

    /// <summary>
    /// Number of frames to skip, when SkipFrames is set to true
    /// </summary>
    [ObservableProperty]
    private int _framesToSkip;

    /// <summary>
    /// Number of the current simulation iteration
    /// </summary>
    [ObservableProperty]
    private int _iterationNumber;

    /// <summary>
    /// True, when field it is necessary to redraw field according to the current state of the simulation
    /// </summary>
    public bool FieldRedrawRequired
    {
        get
        {
            if (!SkipFrames)
                return true;

            if (FramesToSkip > SimulationSettings.MaxFramesToSkip)
                return false;

            return _framesSkipped >= FramesToSkip;
        }
    }

    /// <summary>
    /// The current simulation model
    /// </summary>
    public SimulationModel? CurrentSimulationModel { get; private set; }
    #endregion

    private readonly FieldStateModel _fieldStateModel;

    public SimulationManagerModel(FieldStateModel fieldStateModel)
    {
        _fieldStateModel = fieldStateModel;

        SimulationRunningState = SimulationRunningState.Stopped;
        SimulationMode = SimulationSettings.InitialSimulationMode;
        IterationsPerSecond = SimulationSettings.InitialIterationsPerSecond;
        FramesToSkip = SimulationSettings.InitialFramesToSkip;
        SkipFrames = SimulationSettings.InitialSkipFrames;

        SwitchSimulationMode(SimulationMode);

        // TODO: Is this right?
        App.Process += Process;

        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.FieldSize, newFieldSize =>
        {
            StopAndResetIterationNumber();
            CurrentSimulationModel!.Reset(newFieldSize);
        });
    }

    #region Public methods 
    public void ResetSimulation()
    {
        StopAndResetIterationNumber();
        CurrentSimulationModel!.Reset();
    }

    public void SwitchSimulationMode(SimulationMode simulationMode)
    {
        StopAndResetIterationNumber();
        CurrentSimulationModel?.Unsubscribe();

        CurrentSimulationModel = (SimulationModel)Activator.CreateInstance
        (
            SimulationSettings.SimulationModelTypesByMode[simulationMode],
            _fieldStateModel.FieldSize
        )!;

        SimulationMode = simulationMode;
    }
    #endregion

    #region Simulation cycle
    private TimeSpan _lastUpdateTime;
    private double _updateElapsedTime;

    private int _framesSkipped;

    // TODO: Refactor/split
    private void Process()
    {
        if (SimulationRunningState is SimulationRunningState.Stopped)
            return;

        if (!IsAdvanceRequired())
            return;

        if (!CurrentSimulationModel!.CanAdvance())
        {
            SimulationRunningState = SimulationRunningState.Stopped;
            return;
        }

        CurrentSimulationModel!.Advance();
        IterationNumber++;

        _updateElapsedTime = 0;

        UpdateFramesToSkip();
    }

    private bool IsAdvanceRequired()
    {
        // Checking for unlimited advancements
        var isAdvanceRequired = IterationsPerSecond > SimulationSettings.MaxIterationsPerSecond;

        if (isAdvanceRequired is false)
        {
            var currentTime = DateTime.Now.TimeOfDay;
            _updateElapsedTime += currentTime.TotalMilliseconds - _lastUpdateTime.TotalMilliseconds;
            _lastUpdateTime = currentTime;

            var delayBetweenIterationsInMs = 1000 / IterationsPerSecond;
            isAdvanceRequired = _updateElapsedTime >= delayBetweenIterationsInMs;
        }

        return isAdvanceRequired;
    }

    private void UpdateFramesToSkip()
    {
        if (SkipFrames)
            _framesSkipped = _framesSkipped >= FramesToSkip ? 0 : _framesSkipped + 1;
        else
            _framesSkipped = 0;
    }
    #endregion

    private void StopAndResetIterationNumber()
    {
        SimulationRunningState = SimulationRunningState.Stopped;
        IterationNumber = 0;
    }

    public void Unsubscribe()
    {
        CurrentSimulationModel?.Unsubscribe();
        App.Process -= Process;
        TriggerBinder.Unbind(this);
    }
}