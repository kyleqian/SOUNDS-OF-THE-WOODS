using UnityEngine;
using System.Collections;
using System;

public class Bunny : CreatureBase
{

    float time;
    bool walk;


    Vector3 getTarget()
    {
        return new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, UnityEngine.Random.Range(-2f, 2f));
    }
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        time = UnityEngine.Random.Range(0, 5.5f);
        walk = false;

        base.SpawnVisual();
    }


    protected override void ChangeState(CreatureState state)
    {
        if (currState == CreatureState.Default) animator.SetBool("walk", false);
        base.ChangeState(state);
    }

    void Update()
    {
        switch (currState)
        {
            case CreatureState.Default:
                if (time < 0)
                {
                    if (walk)
                        animator.SetBool("walk", false);
                    else
                    {
                        target = getTarget();
                        animator.SetBool("walk", true);
                    }

                    walk = !walk;
                    time = UnityEngine.Random.Range(2, 7.5f);
                }
                if (walk)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                }
                time -= Time.deltaTime;
                break;
            case CreatureState.Shocked:
                // Revert back to Default if shocked for too long
                // without fleeing
                shockTimer += Time.deltaTime;
                if (shockTimer >= SECONDS_TO_UNSHOCK)
                {
                    ChangeState(CreatureState.Default);
                }
                break;
            case CreatureState.Fleeing:
                // Move/scale
                Flee();
                time = 0;
                break;
        }
    }
}
