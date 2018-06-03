using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
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

    protected abstract void OnPhaseLoad(GamePhase phase);
    protected abstract void OnPhaseUnload(GamePhase phase);
}
