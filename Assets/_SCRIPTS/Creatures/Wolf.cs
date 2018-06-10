using UnityEngine;
using System.Collections;
using System;

public class Wolf : CreatureBase
{
    float angle;
    float radius;

    public AudioClip flee;
    public AudioClip[] growl, run;

    //ANIMATION STUFF
    public override void Footstep()
    {
        int length = run.Length;
        audio.PlayOneShot(run[UnityEngine.Random.Range(0, length)]);
    }

    public void Flee(){
        audio.PlayOneShot(flee);
    }

    public void Howl(){
        audio.PlayOneShot(growl[UnityEngine.Random.Range(0,growl.Length)]);
    }


    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        Vector3 pos = RandomGroundPosition();
        if (UnityEngine.Random.value > 0.3f)
        {
            pos = new Vector3(pos.x * 1.5f, 0, pos.z * 1.5f);
        }
        transform.position = pos;

        radius = Vector3.Distance(transform.position, Vector3.zero);

        speed = UnityEngine.Random.Range(0.2f, 0.5f);

        base.SpawnVisual();


    }


    protected override void ChangeState(CreatureState state)
    {
        if (state==CreatureState.Shocked) Howl();
        else if (state==CreatureState.Fleeing) Flee();

        base.ChangeState(state);
    }

    

    void Update()
    {

        Lookat();
        switch (currState)
        {
            case CreatureState.Default:
                angle -= speed * Time.deltaTime;
                var offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
                transform.position = Vector3.zero + offset;

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
