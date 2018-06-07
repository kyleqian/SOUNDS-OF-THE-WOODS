using UnityEngine;
using System;
using System.Collections;
public class Friendly : CreatureBase
{
    protected override void SpawnVisual()
    {
        //choose random location at X distance from player
        

        ChangeState(CreatureState.Default);

        // Enable GameObject
        parentObject.SetActive(true);

        // Fade in
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        StartCoroutine(fade(s, 0, 1, null));

    }
    IEnumerator fade(SpriteRenderer s, float alpha1, float alpha2, Action after)
    {
        s.color = new Color(1, 1, 1, alpha1);
        float maxTime = 1f;
        for (float i = 0; i < maxTime; i += Time.deltaTime)
        {
            s.color = Color.Lerp(new Color(1, 1, 1, alpha1), new Color(1, 1, 1, alpha2), i / maxTime);
            yield return null;
        }
        if (after!=null)
        after();
    }

    protected override void DespawnVisual()
    {
        if (currState != CreatureState.Fleeing)
        {
            ChangeState(CreatureState.Fleeing);
        }

        // Fade out
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        StartCoroutine(fade(s, 1, 0, () =>
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
