using System;
using UnityEngine;

public enum GamePhase
{
    Start, Afternoon, Dusk, Night, Latenight, Dawn, End
}

public class GameManager : ManagerBase
{
    public static GameManager Instance;
    
    public event Action<GamePhase> PhaseLoaded;
    public event Action<GamePhase> PhaseUnloaded;

    public GamePhase CurrPhase { get; private set; }
    public float CurrPhaseTime { get; private set; }
    public float[] PhaseLengths { get; private set; }
    public bool debugMode;

    public float minPhaseLengthInSeconds;
    public float maxPhaseLengthInSeconds;

    bool gameOver;

	void Awake()
	{
        Instance = this;
        InitializePhaseLengths();
    }

    void Start()
    {
        // Fire events for loading Start
        PhaseLoaded(CurrPhase);

        if (debugMode)
        {
            InitializeDebugMode();
        }
    }

    void Update()
	{
		// Advance time
		CurrPhaseTime += Time.deltaTime;
		if (CurrPhaseTime >= PhaseLengths[(int)CurrPhase])
		{
            PhaseTransition();
        }
    }

    void InitializeDebugMode()
    {
        //PhaseLengths[(int)GamePhase.Night] = Mathf.Infinity;
    }

    void InitializePhaseLengths()
    {
        int numPhases = Enum.GetValues(typeof(GamePhase)).Length;
        PhaseLengths = new float[numPhases];
        for (int i = 0; i < numPhases; ++i)
        {
            PhaseLengths[i] = UnityEngine.Random.Range(minPhaseLengthInSeconds, maxPhaseLengthInSeconds);
        }

        // No limit for Start or End
        PhaseLengths[(int)GamePhase.Start] = Mathf.Infinity;
        PhaseLengths[(int)GamePhase.End] = Mathf.Infinity;
    }

    public void PressedStart()
    {
        if (CurrPhase == GamePhase.Start)
        {
            PhaseTransition();
        }
    }

    public void GameOver()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;
        Debug.LogError("GAME OVER!!!");

        /*
        rising noises
        disable ability to use flashlight
        sudden death
        eyes close to black + fade out blur
        freeze game time
        wait a bit
        restart scene
        -> opening eyes
         */
    }

    void PhaseTransition()
    {
        Debug.Log("Transition to: " + (CurrPhase + 1).ToString());
        CurrPhaseTime = 0;
        PhaseUnloaded(CurrPhase);
        PhaseLoaded(++CurrPhase);
	}

    protected override void OnPhaseLoad(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Start:
                break;
            case GamePhase.Afternoon:
                break;
            case GamePhase.Dusk:
                break;
            case GamePhase.Night:
                break;
            case GamePhase.Latenight:
                break;
            case GamePhase.Dawn:
                break;
            case GamePhase.End:
                break;
        }
    }

    protected override void OnPhaseUnload(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Start:
                break;
            case GamePhase.Afternoon:
                break;
            case GamePhase.Dusk:
                break;
            case GamePhase.Night:
                break;
            case GamePhase.Latenight:
                break;
            case GamePhase.Dawn:
                break;
            case GamePhase.End:
                break;
        }
    }
}
