using UnityEngine;

public class Crow : CreatureBase
{
    protected override void SpawnVisual()
    {
        //choose random location at X distance from player


        ChangeState(CreatureState.Default);

        // Enable GameObject
        parentObject.SetActive(true);

        // Fade in
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        StartCoroutine(Fade(s, 0, 1, null));

    }

    protected override void DespawnVisual()
    {
        if (currState != CreatureState.Fleeing)
        {
            ChangeState(CreatureState.Fleeing);
        }

        // Fade out
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        StartCoroutine(Fade(s, 1, 0, () =>
        {
            // After fade out, disable GameObject
            parentObject.SetActive(false);
        }));
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
