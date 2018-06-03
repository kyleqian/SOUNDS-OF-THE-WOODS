using UnityEngine;

public class Friendly : CreatureBase
{
    protected override void ChangeState(CreatureState state)
    {
        currState = state;
        switch (state)
        {
            case CreatureState.Default:
                // Play animation

                // Move?

                break;
            case CreatureState.Shocked:
                // Play animation
                
                // Stop moving
                
                break;
            case CreatureState.Fleeing:
                // Play animation

                // Move?

                break;
        }
    }
}
