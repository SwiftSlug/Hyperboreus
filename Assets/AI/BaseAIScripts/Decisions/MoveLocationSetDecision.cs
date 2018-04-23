using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/MoveLocationSetDecision")]
public class MoveLocationSetDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return MoveLocationSet(controller);
    }

    private bool MoveLocationSet(StateController controller)
    {

        if (controller.moveCommandLocation != Vector3.zero) //  May be better to use a move to bool instead of the zero comparison
        {
            //Debug.Log("MoveLocation is Set !");
            return true;
        }
        else
        {
            //Debug.Log("MoveLocation is not Set !");
            return false;
        }

    }

}

