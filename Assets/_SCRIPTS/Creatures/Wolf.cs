using UnityEngine;
using System.Collections;
using System;

public class Wolf : CreatureBase
{
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        Vector3 pos = RandomGroundPosition();
        if (UnityEngine.Random.value > 0.3f)
        {
            pos = new Vector3(pos.x * 1.5f, 0, pos.z * 1.5f);
        }
        transform.position = pos;

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
