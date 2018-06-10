using UnityEngine;
using System.Collections;
using System;


public class Decoy : CreatureBase
{

    public AudioClip[] squeaks;


    public void Start()
    {
        audio.Play();
    }
    public override void Footstep()
    {
        audio.PlayOneShot(squeaks[UnityEngine.Random.Range(0, squeaks.Length)], 1);
    }
    public void StopSong()
    {
        audio.enabled = false;
        audio.enabled = true;
    }
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position = RandomGroundPosition();

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
