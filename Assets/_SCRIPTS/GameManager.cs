using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] float minPhaseLengthInSeconds;
	[SerializeField] float maxPhaseLengthInSeconds;

	public static GameManager instance;
	public enum GamePhase { Afternoon, Dusk, Night, Latenight, Dawn, End }
	public GamePhase currPhase;
	public float currPhaseTime;

	public event Action<GamePhase> OnPhaseLoad;
	public event Action<GamePhase> OnPhaseUnload;

	float[] phaseLengths;

	void Awake()
	{
		instance = this;

		for (int i = 0; i < Enum.GetNames(typeof(GamePhase)).Length; ++i)
		{
			phaseLengths[i] = UnityEngine.Random.Range(minPhaseLengthInSeconds, maxPhaseLengthInSeconds);
		}

		// No limit for .End
		phaseLengths[phaseLengths.Length - 1] = Mathf.Infinity;
	}

	void Update()
	{
		// Advance time
		currPhaseTime += Time.deltaTime;
		if (currPhaseTime >= phaseLengths[(int)currPhase])
		{
			PhaseTransition();
		}
	}

	void PhaseTransition()
	{
		currPhaseTime = 0;
		OnPhaseUnload(currPhase);
		OnPhaseLoad(++currPhase);

		switch (currPhase)
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
