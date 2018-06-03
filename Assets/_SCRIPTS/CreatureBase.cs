using UnityEngine;

public enum CreatureState
{
    Default, Shocked, Fleeing
}

public abstract class CreatureBase : MonoBehaviour
{
    protected const float SECONDS_TO_SHOCK = 0.5f;
    protected const float SECONDS_TO_FLEE = 2f;
    protected CreatureState currState;

    public virtual void Spawn(Vector3 location)
    {
        // Initialize location
        transform.position = location;

        // Change state to Default
        ChangeState(CreatureState.Default);

        // Enable GameObject
        gameObject.SetActive(true);

        // Fade in
        // TODO
    }

    public virtual void Despawn()
    {
        // Fade out
        // TODO

        // Disable GameObject when faded out
        // TODO
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
        }
    }

    protected abstract void ChangeState(CreatureState state);
}
