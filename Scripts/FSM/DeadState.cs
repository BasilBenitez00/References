using UnityEngine;
using System.Collections;

public class DeadState : FSMState
{
    private NPCTankController controller;

    public DeadState(NPCTankController controller) 
    {
        stateID = FSMStateID.Dead;
        this.controller = controller;
    }

    public override void Reason(Transform player, Transform npc)
    {

    }

    public override void Act(Transform player, Transform npc)
    {
        //Do Nothing for the dead state
    }
}
