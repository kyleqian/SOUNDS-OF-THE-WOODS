﻿using UnityEngine;
using System.Collections;
using System;

public class Deer : CreatureBase
{
    float idleWalkingTimer;
    bool idleWalking;

    Vector3 GetTarget()
    {
        float x1, x2, z1, z2;


        if (transform.position.x < 0)
        {
            x1 = -0f; x2 = 1;
        }
        else
        {
            x1 = -1; x2 = 0f;
        }

        if (transform.position.z < 0)
        {
            z1 = 0f; z2 = 1;
        }
        else
        {
            z1 = -1; z2 = 0f;
        }
        Vector3 left = transform.position + Vector3.left;
        return new Vector3(left.x + UnityEngine.Random.Range(x1, x2), 0, left.z + UnityEngine.Random.Range(z1, z2));/// + UnityEngine.Random.Range(x1, x2) .!--.!--  + UnityEngine.Random.Range(z1, z2)
    }

    protected override void SpawnVisual()
    {
        idleWalkingTimer = UnityEngine.Random.Range(3, 7.5f);
        idleWalking = false;

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
        if (currState == CreatureState.Default)
        {
            animator.SetBool("walk", false);
        }
        if (state == CreatureState.Fleeing)
        {
            speed *= 1.5f;
        }
        base.ChangeState(state);
    }




    public override void Footstep()
    {
        int length = SoundManager.Instance.footsteps.Length;
        audio.PlayOneShot(SoundManager.Instance.footsteps[UnityEngine.Random.Range(0, length)]);
    }


    void Update()
    {
        Lookat();
        switch (currState)
        {
            case CreatureState.Default:
                if (idleWalkingTimer < 0)
                {
                    if (idleWalking)
                    {
                        animator.SetBool("walk", false);
                    }
                    else
                    {
                        targetPosition = GetTarget();
                        animator.SetBool("walk", true);
                    }
                    idleWalking = !idleWalking;
                    idleWalkingTimer = UnityEngine.Random.Range(2, 7.5f);
                }
                if (idleWalking)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    if (transform.position == targetPosition)
                    {
                        idleWalkingTimer = 0;
                    }
                }
                idleWalkingTimer -= Time.deltaTime;

                break;
            case CreatureState.Shocked:
                // Revert back to Default if shocked for too long without fleeing
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
