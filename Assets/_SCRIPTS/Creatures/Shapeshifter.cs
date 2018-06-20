using UnityEngine;

public class Shapeshifter : CreatureBase
{
    public AudioClip[] branches;
    public AudioClip somethingBadTeleport;
    int TimesTeleported;

    public void BadTeleport()
    {
        audio.pitch = Random.Range(0.9f, 1.05f);
        audio.PlayOneShot(somethingBadTeleport);
    }

    public override void Footstep()
    {
        audio.PlayOneShot(branches[Random.Range(0, branches.Length)]);
    }
 
    protected override void SpawnVisual()
    {
        // Choose random location at X distance from player
        transform.position = RandomGroundPosition(-13.5f, 13.5f, -13.5f, 13.5f);

        targetPosition = Vector3.zero;

        base.SpawnVisual();
    }

    protected override void ChangeState(CreatureState state)
    {
        if (state == CreatureState.Shocked && currState != CreatureState.Shocked)
        {
            if (TimesTeleported++ < 2)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6.5f);
            }
        }

        base.ChangeState(state);
    }

    void Update()
    {
        Lookat();
        switch (currState)
        {
            case CreatureState.Default:
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                break;
            case CreatureState.Shocked:
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
