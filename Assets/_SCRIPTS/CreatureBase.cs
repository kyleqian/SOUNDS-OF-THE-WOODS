using UnityEngine;

public enum CreatureState
{
    Default, Shocked, Fleeing
}

public abstract class CreatureBase : MonoBehaviour
{
    public bool spawned;
    protected const float SECONDS_TO_SHOCK = 0.5f;
    protected const float SECONDS_TO_FLEE = 2f;
    protected const float SECONDS_TO_UNSHOCK = 1f;
    protected float shockTimer;
    protected CreatureState currState;

    public virtual void Spawn(Vector3 location)
    {
        // Change state to Default
        ChangeState(CreatureState.Default);

        // Set to spawned
        spawned = true;

        // Fade in/activation/initial position should be handled by subclass implementation
    }

    public virtual void Despawn()
    {
        // Set to despawned
        spawned = false;

        // Fade out/deactivation should be handled by subclass implementation
    }

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
