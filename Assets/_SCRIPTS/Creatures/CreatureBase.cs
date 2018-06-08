using UnityEngine;
using System;
using System.Collections;

public enum CreatureState
{
    Default, Shocked, Fleeing
}

public abstract class CreatureBase : MonoBehaviour
{
    public bool Spawned;//{ get; private set; }
    const float SECONDS_TO_SHOCK = 0.5f;
    const float SECONDS_TO_FLEE = 2f;
    protected const float SECONDS_TO_UNSHOCK = 3f; // Has to be greater than SECONDS_TO_FLEE
    protected float shockTimer;
    [SerializeField]
    protected CreatureState currState;
    protected Animator animator;
    protected Vector3 target;
    public float speed = 0.9f;
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
        animator.SetFloat("offset", UnityEngine.Random.Range(0, 1.2f));

        SpawnVisual();

        Spawned = true;
    }

    public virtual void Despawn()
    {
        if (!Spawned && !gameObject.activeSelf)
        {
            return;
        }
        Spawned = false;

        DespawnVisual(() =>
        {
            gameObject.SetActive(false);

        });


    }

    protected virtual void SpawnVisual()
    {
        // Enable GameObject
        gameObject.SetActive(true);

        ChangeState(CreatureState.Default);

        // Fade in
        SpriteRenderer s = transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartCoroutine(Fade(s, 0, 1, null));
    }
    protected virtual void DespawnVisual(Action a)
    {
        StartCoroutine(DespawnCo(a));
    }

    IEnumerator DespawnCo(Action a)
    {
        if (currState != CreatureState.Fleeing)
        {
            Debug.Log(gameObject.name + " change state!");
            ChangeState(CreatureState.Fleeing);
            yield break;
        }

        Debug.Log("finally flee!");
        SpriteRenderer s = transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartCoroutine(Fade(s, 1, 0, a, 2));
        yield return null;
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

    protected void Flee()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * 2 * Time.deltaTime);
        if (transform.position == target && Spawned && !gameObject.activeSelf)
        {
            Debug.Log("despawn GOD DAMIMIT!!");
            Despawn();
        }
    }

    protected Vector3 FleePosition()
    {
        float x1, x2, z1, z2;
        if (transform.position.x < 0)
        {
            x1 = -8; x2 = -6;
        }
        else
        {
            x1 = 8; x2 = 6;
        }
        if (transform.position.z < 0)
        {
            z1 = -8; z2 = -6;
        }
        else
        {
            z1 = 8; z2 = 6;
        }
        return new Vector3(UnityEngine.Random.Range(x1, x2), 0, UnityEngine.Random.Range(z1, z2));
    }

    protected Vector3 RandomGroundPosition(float x1 = -7, float x2 = 7, float z1 = -6, float z2 = 6)
    {
        float x = UnityEngine.Random.Range(x1, x2);
        float z;
        if (Mathf.Abs(x) < 5) z = UnityEngine.Random.value > 0.5f ? UnityEngine.Random.Range(z1, z1 + 1) : UnityEngine.Random.Range(z2 - 1, z2);
        else z = UnityEngine.Random.Range(z1, z2);
        return new Vector3(x, 0, z);
    }

    protected void Lookat()
    {
        Vector3 relativePos = Vector3.zero - transform.position;
        transform.rotation = Quaternion.LookRotation(-relativePos, Vector3.up);
    }

    protected virtual void ChangeState(CreatureState state)
    {
        Debug.LogWarning(gameObject.name + " change state to: " + state);

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
                target = FleePosition();
                animator.SetTrigger("flee");
                break;
        }
    }

    protected IEnumerator Fade(SpriteRenderer s, float alpha1, float alpha2, Action after, float maxTime = 2f)
    {
        s.color = new Color(1, 1, 1, alpha1);

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
