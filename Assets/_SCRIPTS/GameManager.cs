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

    [SerializeField] float minPhaseLengthInSeconds;
    [SerializeField] float maxPhaseLengthInSeconds;

	void Awake()
	{
		if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

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

    void Start()
    {
        //// Fire events for loading .Afternoon
        //PhaseLoaded(CurrPhase);
    }

    void Update()
	{
		// Advance time
		CurrPhaseTime += Time.deltaTime;
		if (CurrPhaseTime >= PhaseLengths[(int)CurrPhase])
		{
            CurrPhaseTime = 0;
            PhaseTransition();
        }
    }

    public void PressedStart()
    {
        PhaseTransition();
    }

    public void End(){
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
		PhaseUnloaded(CurrPhase);
		PhaseLoaded(++CurrPhase);
        Debug.Log("Transition to: " + CurrPhase.ToString());
	}

    protected override void OnPhaseLoad(GamePhase phase)
    {
        switch (phase)
        {
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
