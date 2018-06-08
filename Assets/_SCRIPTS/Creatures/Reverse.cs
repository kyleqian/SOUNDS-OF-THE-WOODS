using UnityEngine;
using System.Collections;
using System;


public class Reverse : CreatureBase
{
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position = RandomGroundPosition();

         target = Vector3.zero;

        base.SpawnVisual();
    }

    void Update()
    {
        Lookat();
        switch (currState)
        {
            case CreatureState.Default:
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);


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

                break;
        }
    }
}
