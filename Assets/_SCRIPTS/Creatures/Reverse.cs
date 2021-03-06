﻿using UnityEngine;
using System.Collections;
using System;

public class Reverse : CreatureBase
{
    public AudioClip[] gutturalSweeps;
    public AudioClip horn, howl, lowGrowl;

    //ANIMATION 
    public override void Footstep()
    {
        audio.PlayOneShot(gutturalSweeps[UnityEngine.Random.Range(0, gutturalSweeps.Length)]);
    }

    public void Horn()
    {
        audio.PlayOneShot(horn);
    }

    public void Howl()
    {
        audio.PlayOneShot(lowGrowl);
    }

    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position =  RandomGroundPosition(-13.5f, 13.5f, -13.5f, 13.5f);

        targetPosition = Vector3.zero;

        audio.PlayOneShot(howl);

        base.SpawnVisual();
    }

  protected override void ChangeState(CreatureState state)
    {
        if (state==CreatureState.Shocked){ 
            Horn();
        }
        else if (state==CreatureState.Fleeing){
            Howl();
        }
        base.ChangeState(state);
    }

    void Update()
    {
        Lookat();
        switch (currState)
        {
            case CreatureState.Default:
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

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
                // Move/scale

                break;
        }
    }
}
