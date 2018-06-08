using UnityEngine;
using System.Collections;
using System;


public class Shapeshifter : CreatureBase
{
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position = RandomGroundPosition();

        base.SpawnVisual();
    }



    void Update()
    {
        Lookat();
        switch (currState)
        {
            case CreatureState.Default:
                // Move around

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
