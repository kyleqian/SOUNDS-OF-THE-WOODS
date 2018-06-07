using UnityEngine;

public class Squirrel : CreatureBase
{
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        Vector3 pos = RandomGroundPosition();
        if (Random.value > 0.5f)
        {
            pos = new Vector3(pos.x * 0.7f, 0, pos.z * 0.7f);
        }
        transform.position = pos;

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
