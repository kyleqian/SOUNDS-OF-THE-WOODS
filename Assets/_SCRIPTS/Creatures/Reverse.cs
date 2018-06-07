using UnityEngine;

public class Reverse : CreatureBase
{
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position = RandomGroundPosition();

        ChangeState(CreatureState.Default);

        // Enable GameObject
        gameObject.SetActive(true);

        // Fade in
        SpriteRenderer s = transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartCoroutine(Fade(s, 0, 1, null));
    }

    protected override void DespawnVisual()
    {
        if (currState != CreatureState.Fleeing)
        {
            ChangeState(CreatureState.Fleeing);
        }

        // Fade out
        SpriteRenderer s = transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartCoroutine(Fade(s, 1, 0, () =>
        {
            // After fade out, disable GameObject
            gameObject.SetActive(false);
        }));
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
