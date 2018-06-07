using UnityEngine;
using System;
using System.Collections;

public enum CreatureState
{
    Default, Shocked, Fleeing
}

public abstract class CreatureBase : MonoBehaviour
{
    public bool Spawned { get; private set; }
    const float SECONDS_TO_SHOCK = 0.5f;
    const float SECONDS_TO_FLEE = 2f;
    protected const float SECONDS_TO_UNSHOCK = 3f; // Has to be greater than SECONDS_TO_FLEE
    protected float shockTimer;
    protected CreatureState currState;
    protected Animator animator;

    protected void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
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

    protected Vector3 RandomGroundPosition()
    {
        float x = UnityEngine.Random.Range(-7, 7);
        float z;
        if (Mathf.Abs(x) < 5) z = UnityEngine.Random.value > 0.5f ? UnityEngine.Random.Range(-6, -5) : UnityEngine.Random.Range(5, 6);
        else z = UnityEngine.Random.Range(-6, 6);
        return new Vector3(x, 0, z);
    }

    protected void Lookat()
    {
        Vector3 relativePos = Vector3.zero - transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
    }

    protected virtual void ChangeState(CreatureState state)
    {
        Debug.Log("Change state to: " + state);

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

    protected IEnumerator Fade(SpriteRenderer s, float alpha1, float alpha2, Action after)
    {
        s.color = new Color(1, 1, 1, alpha1);
        float maxTime = 1f;

        for (float i = 0; i < maxTime; i += Time.deltaTime)
        {
            s.color = Color.Lerp(new Color(1, 1, 1, alpha1), new Color(1, 1, 1, alpha2), i / maxTime);
            yield return null;
        }

        if (after != null)
        {
            after();
        }
    }
}
