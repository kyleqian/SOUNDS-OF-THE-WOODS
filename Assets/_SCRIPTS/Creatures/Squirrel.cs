using UnityEngine;
using System.Collections;
using System;

public class Squirrel : CreatureBase
{


    float time;
    bool walk;

    Vector3 getTarget()
    {
        float x1, x2, z1, z2;
        if (transform.position.x < 0)
        {
            x1 = -1.6f; x2 = 2;
        }
        else
        {
            x1 = -2; x2 = 1.6f;
        }
        if (transform.position.z < 0)
        {
            z1 = -1.6f; z2 = 2;
        }
        else
        {
            z1 = -2; z2 = 1.6f;
        }
        return new Vector3(transform.position.x + UnityEngine.Random.Range(x1, x2), 0, transform.position.z + UnityEngine.Random.Range(z1, z2));
    }
    protected override void SpawnVisual()
    {

        time = UnityEngine.Random.Range(2, 5.5f);
        targetPosition = getTarget();
        walk = false;

        // Choose random location at X distance from player
        Vector3 pos = RandomGroundPosition();
        if (UnityEngine.Random.value > 0.5f)
        {
            pos = new Vector3(pos.x * 0.7f, 0, pos.z * 0.7f);
        }
        transform.position = pos;

        base.SpawnVisual();
    }


    protected override void ChangeState(CreatureState state)
    {
        if (currState == CreatureState.Default) animator.SetBool("walk", false);
        base.ChangeState(state);
    }


    void Update()
    {
        Lookat();
        switch (currState)
        {
            case CreatureState.Default:
                if (time < 0)
                {
                    if (walk)
                        animator.SetBool("walk", false);
                    else
                    {
                        targetPosition = getTarget();
                        animator.SetBool("walk", true);
                    }
                    walk = !walk;
                    time = UnityEngine.Random.Range(2, 7.5f);
                }
                if (walk)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    if (transform.position == targetPosition)
                    {
                        time = 0;
                    }
                }
                time -= Time.deltaTime;
                break;
            case CreatureState.Shocked:
                // Revert back to Default if shocked for too long
                // without fleeing
                unshockTimer += Time.deltaTime;
                if (unshockTimer >= SECONDS_TO_UNSHOCK)
                {
                    unshockTimer = 0;
                    ChangeState(CreatureState.Default);
                }
                break;
            case CreatureState.Fleeing:
              
                break;
        }
    }
}
