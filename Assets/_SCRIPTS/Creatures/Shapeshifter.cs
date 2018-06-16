using UnityEngine;
using System.Collections;
using System;


public class Shapeshifter : CreatureBase
{
    public AudioClip[] branches;

    public AudioClip somethingBadTeleport;


    public void BadTeleport(){
        audio.pitch=UnityEngine.Random.Range(0.9f, 1.05f);
        audio.PlayOneShot(somethingBadTeleport);
    }
    public override void Footstep()
    {
        audio.PlayOneShot(branches[UnityEngine.Random.Range(0, branches.Length)]);
    }
 
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position = RandomGroundPosition(-13.5f, 13.5f, -13.5f, 13.5f );

        targetPosition = Vector3.zero;

        base.SpawnVisual();
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
