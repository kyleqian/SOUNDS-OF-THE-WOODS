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
    const float ATTACK_RADIUS = 2f; // How close is Game Over?
    const float SECONDS_TO_SHOCK = 0.5f;
    const float SECONDS_TO_FLEE = 1.5f;
    protected const float SECONDS_TO_UNSHOCK = 1.6f; // Has to be greater than SECONDS_TO_FLEE
    protected float unshockTimer; // How long a creature has been shocked (used to reset creature to default if shocked but not long enough to flee)
    protected CreatureState currState;
    protected Animator animator;
    protected Vector3 targetPosition;
    public float speed = 0.9f;

    protected AudioSource audio;

    protected void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    public virtual void Spawn()
    {
        if (Spawned)
        {
            return;
        }

        animator.SetFloat("offset", UnityEngine.Random.Range(0, 1.2f));

        ChangeState(CreatureState.Default);
        SpawnVisual();

        Spawned = true;
        gameObject.SetActive(true);
    }

    public virtual void Despawn()
    {
        if (!Spawned)
        {
            return;
        }

        ChangeState(CreatureState.Fleeing);
        DespawnVisual(() =>
        {
            Spawned = false;
            gameObject.SetActive(false);
        });
    }

    protected virtual void SpawnVisual()
    {
        // Fade in
        //SpriteRenderer s = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //StartCoroutine(Fade(s, 0, 1, null));
    }

    protected virtual void DespawnVisual(Action callback)
    {
        StartCoroutine(Flee(callback));

        // Fade out
        //SpriteRenderer s = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //StartCoroutine(Fade(s, 1, 0, a, 2));
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
                Despawn();
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
                unshockTimer = 0;
            }
        }
    }

    IEnumerator Flee(Action callback)
    {
        targetPosition = FleePosition();

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * 2 * Time.deltaTime);
            yield return null;
        }

        callback();
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

        return new Vector3(UnityEngine.Random.Range(x1, x2), transform.position.y, UnityEngine.Random.Range(z1, z2));
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

    public abstract void Footstep();
    protected virtual void ChangeState(CreatureState state)
    {
        Debug.Log(gameObject.name + " change state to: " + state);
        if (state == currState)
        {
            return;
        }

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

    // TODO: LOL hacky inheritance
    void LateUpdate()
    {
        //if (GetType() != typeof(Deer) && GetType() != typeof(Squirrel))
        //{
        //    Debug.Log(gameObject.GetType());
        //    Debug.Log(Vector3.Distance(transform.position, Camera.main.transform.position));
        //}

        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= ATTACK_RADIUS)
        {
            // TODO: Hacky check if friendly
            if (GetType() != typeof(Deer) && GetType() != typeof(Squirrel))
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    //protected IEnumerator Fade(SpriteRenderer s, float alpha1, float alpha2, Action after, float maxTime = 2f)
    //{
    //    s.color = new Color(1, 1, 1, alpha1);

    //    for (float i = 0; i < maxTime; i += Time.deltaTime)
    //    {
    //        s.color = Color.Lerp(new Color(1, 1, 1, alpha1), new Color(1, 1, 1, alpha2), i / maxTime);
    //        yield return null;
    //    }

    //    if (after != null)
    //    {
    //        after();
    //    }
    //}
}
