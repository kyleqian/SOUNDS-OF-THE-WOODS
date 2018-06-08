using UnityEngine;
using System.Collections;
using System;


public class Raccoon : CreatureBase
{

    //How long the racoon appears before popping down
    float appearingTime;
    //is the racoon popping?
    bool popping;

    void initAppearingTime()
    {
        appearingTime = UnityEngine.Random.Range(4, 9.5f);
    }
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position = RandomGroundPosition();
        initAppearingTime();
        base.SpawnVisual();

    }

    Vector3 getTarget()
    {

        float x1, x2, z1, z2;
        if (transform.position.x < 0)
        {
            x1 = -0.5f; x2 = 1.5f;
        }
        else
        {
            x1 = -1.5f; x2 = 0.5f;
        }
        if (transform.position.z < 0)
        {
            z1 = -0.5f; z2 = 1.5f;
        }
        else
        {
            z1 = -1.5f; z2 = 0.5f;
        }
        return new Vector3(transform.position.x + UnityEngine.Random.Range(x1, x2), 0, transform.position.z + UnityEngine.Random.Range(z1, z2));

    }
    IEnumerator appearCloser()
    {
        
        //do things
        float maxTime = UnityEngine.Random.Range(0.5f, 1.4f);
        for (float i = 0; i < maxTime; i+=Time.deltaTime)
        {
            float t=i/maxTime;
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(0,-1.61f,t*t), transform.position.z);
            yield return null;
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f,3.3f));

        Vector3 target = getTarget();
        maxTime = UnityEngine.Random.Range(0.5f, 2f);
        for (float i = 0; i < maxTime; i+=Time.deltaTime)
        {
            float t=i/maxTime;
            transform.position = new Vector3(target.x,  Mathf.Lerp(-1.61f,0,t*t), target.z);
            yield return null;
        }
        popping=false;

        //init time
        initAppearingTime();
    }
    void Update()
    {
        Lookat();
        switch (currState)
        {
            case CreatureState.Default:
                // Pops down, pokes back up. Every time it pokes, it gets closer
                if (appearingTime < 0 && !popping)
                {
                    popping=true;
                    StartCoroutine(appearCloser());
                }
                else
                {
                    appearingTime -= Time.deltaTime;
                }
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
                Flee();
                break;
        }
    }
}
