using System;
using UnityEngine;

public enum GamePhase
{
    Afternoon, Dusk, Night, Latenight, Dawn, End
}

public class GameManager : ManagerBase
{
    public static GameManager Instance;
    
    public event Action<GamePhase> PhaseLoaded;
    public event Action<GamePhase> PhaseUnloaded;

    public GamePhase CurrPhase { get; private set; }
    public float CurrPhaseTime { get; private set; }

    [SerializeField] float minPhaseLengthInSeconds;
    [SerializeField] float maxPhaseLengthInSeconds;

	float[] phaseLengths;

	void Awake()
	{
		if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        int numPhases = Enum.GetNames(typeof(GamePhase)).Length;
        phaseLengths = new float[numPhases];
        for (int i = 0; i < numPhases; ++i)
		{
			phaseLengths[i] = UnityEngine.Random.Range(minPhaseLengthInSeconds, maxPhaseLengthInSeconds);
		}

		// No limit for .End
        phaseLengths[(int)GamePhase.End] = Mathf.Infinity;

        // Fire events for loading .Afternoon
        PhaseLoaded(CurrPhase);
	}

    void Update()
	{
		// Advance time
		CurrPhaseTime += Time.deltaTime;
		if (CurrPhaseTime >= phaseLengths[(int)CurrPhase])
		{
            CurrPhaseTime = 0;
            PhaseTransition();
        }
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
