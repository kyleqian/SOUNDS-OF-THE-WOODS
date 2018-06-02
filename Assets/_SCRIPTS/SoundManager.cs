using UnityEngine;

public class SoundManager : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.Instance.PhaseLoaded += OnPhaseLoad;
        GameManager.Instance.PhaseUnloaded += OnPhaseUnload;
    }

    void OnDisable()
    {
        GameManager.Instance.PhaseLoaded -= OnPhaseLoad;
        GameManager.Instance.PhaseUnloaded -= OnPhaseUnload;
    }

    void OnPhaseLoad(GamePhase phase)
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

    void OnPhaseUnload(GamePhase phase)
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
