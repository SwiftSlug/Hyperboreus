using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CanSeePlayer")]
public class CanSeePlayerDecision : Decision
{

    public float detectionRadius = 100.0f;

    public override bool Decide(StateController controller)
    {
        bool canSeePlayer = CanSeePlayer(controller);
        return canSeePlayer;
    }

    private bool CanSeePlayer(StateController controller)
    {
        Ray hit;
        
        //Physics.OverlapSphere()

        return false;
    }

}
