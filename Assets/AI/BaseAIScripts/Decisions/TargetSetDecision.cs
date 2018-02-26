﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TargetSetDecision")]
public class TargetSetDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return MoveLocationSet(controller);
    }

    private bool MoveLocationSet(StateController controller)
    {

        if (controller.target != false) //  May be better to use a move to bool instead of the zero comparison
        {
            //Debug.Log("MoveLocation Set !");
            return true;
        }
        else
        {
            return false;
        }

    }
}
