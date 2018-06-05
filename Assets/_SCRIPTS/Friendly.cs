using UnityEngine;

public class Friendly : CreatureBase
{
    protected override void SpawnVisual()
    {
        // Initialize at some location

        ChangeState(CreatureState.Default);

        // Enable GameObject
        parentObject.SetActive(true);

        // Fade in

    }

    protected override void DespawnVisual()
    {
        if (currState != CreatureState.Fleeing)
        {
            ChangeState(CreatureState.Fleeing);
        }

        // Fade out

        // After fade out, disable GameObject
        parentObject.SetActive(false);
    }

    void Update()
    {
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
