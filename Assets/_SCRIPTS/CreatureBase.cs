using UnityEngine;

public enum CreatureState
{
    Default, Shocked, Fleeing
}

public abstract class CreatureBase : MonoBehaviour
{
    public bool Spawned { get; private set; }
    protected const float SECONDS_TO_SHOCK = 0.5f;
    protected const float SECONDS_TO_FLEE = 2f;
    protected const float SECONDS_TO_UNSHOCK = 1f;
    protected float shockTimer;
    protected CreatureState currState;

    public virtual void Spawn()
    {
        ChangeState(CreatureState.Default);
        Spawned = true;

        // Fade in/activate/set initial position
        SpawnVisual();
    }

    public virtual void Despawn()
    {
        Spawned = false;

        // Fade out/deactivate
        DespawnVisual();
    }

    protected abstract void SpawnVisual();
    protected abstract void DespawnVisual();

    // Called by flashlight with value indicating how long
    // the creature has been continuously looked at
    public virtual void ISeeYou(float forThisLong)
    {
        // Change state
        if (forThisLong >= SECONDS_TO_FLEE)
        {
            if (currState == CreatureState.Shocked)
            {
                ChangeState(CreatureState.Fleeing);
            }
        }
        else if (forThisLong >= SECONDS_TO_SHOCK)
        {
            if (currState == CreatureState.Default)
            {
                ChangeState(CreatureState.Shocked);
            }
            else if (currState == CreatureState.Shocked)
            {
                shockTimer = 0;
            }
        }
    }

    protected abstract void ChangeState(CreatureState state);
}
