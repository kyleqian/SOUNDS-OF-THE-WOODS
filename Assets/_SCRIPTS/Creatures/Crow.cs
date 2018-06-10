using UnityEngine;
using System.Collections;
using System;

public class Crow : CreatureBase
{
    float angle;
    float radius;

    public AudioClip[] caws;
    public AudioClip bugle;
    

    public override void Footstep()
    {
        audio.PlayOneShot(caws[UnityEngine.Random.Range(0, caws.Length)]);
    }

    public void Bugle(){
        audio.PlayOneShot(bugle);
    }
    
    protected override void SpawnVisual()
    {
        //Randomize position, scale, and speed for variety
        transform.position = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(4.53f, 6.85f), UnityEngine.Random.Range(-3f, 3f));
        transform.localScale = Vector3.one * UnityEngine.Random.Range(1, 1.32f);
        speed = UnityEngine.Random.Range(0.1f, 0.35f);

        radius = Vector3.Distance(transform.position, Vector3.zero);

        base.SpawnVisual();
    }


    void Update()
    {

        switch (currState)
        {
            case CreatureState.Default:
                // Move around
                angle += speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Sin(angle) * radius, transform.position.y, Mathf.Cos(angle) * radius);
                transform.forward = transform.position;

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

                break;
        }
    }
}
