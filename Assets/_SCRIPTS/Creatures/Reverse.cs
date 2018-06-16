using UnityEngine;
using System.Collections;
using System;


public class Reverse : CreatureBase
{
    public AudioClip[] gutturalSweeps;
    public AudioClip horn, howl;

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
        audio.PlayOneShot(howl);
    }


    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position =  RandomGroundPosition(-13.5f, 13.5f, -13.5f, 13.5f );

        // TODO: Hacky patch because Reverse doesn't show up
        transform.Translate(0, -0.35f, 0);

        targetPosition = Vector3.zero;

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
