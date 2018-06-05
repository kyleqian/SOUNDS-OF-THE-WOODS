using UnityEngine;

public enum CreatureState
{
    Default, Shocked, Fleeing
}

public abstract class CreatureBase : MonoBehaviour
{
    public bool Spawned { get; private set; }

    const float SECONDS_TO_SHOCK = 0.5f;
    const float SECONDS_TO_FLEE = 2f;
    protected const float SECONDS_TO_UNSHOCK = 1f;
    protected float shockTimer;
    protected CreatureState currState;
    protected GameObject parentObject;
    protected Animator animator;

    protected void Awake()
    {
        parentObject = transform.parent.gameObject;
        animator = GetComponent<Animator>();
    }

    public virtual void Spawn()
    {
        if (Spawned)
        {
            return;
        }

        // Fade in/activate/set initial position
        SpawnVisual();

        Spawned = true;
    }

    public virtual void Despawn()
    {
        if (!Spawned)
        {
            return;
        }
        
        // Fade out/deactivate
        DespawnVisual();

        Spawned = false;
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

    protected virtual void ChangeState(CreatureState state)
    {
        currState = state;
        switch (currState)
        {
            case CreatureState.Default:
                animator.SetTrigger("idle");
                break;
            case CreatureState.Shocked:
                animator.SetTrigger("shocked");
                break;
            case CreatureState.Fleeing:
                animator.SetTrigger("flee");
                break;
        }
    }
}
