using UnityEngine;
using System.Collections;

public class ChaseState : FSMState
{
    private NPCTankController controller;

    public ChaseState(Transform[] wp, NPCTankController controller) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Chasing;

        
        attackDistance = 200f;
        chaseDistance = 450f;

        this.controller = controller;
        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        //TO IMPLEMENT
        //Set the target position as the player position
        //Check the distance with player tank
        float distance = Vector3.Distance(player.position, npc.position);
        if(distance <= attackDistance)
        {
            controller.SetTransition(Transition.ReachPlayer);
        }

        float chaseRange = Vector3.Distance(player.position, npc.position);
        if(chaseRange > chaseDistance +200f)
        {
            controller.SetTransition(Transition.LostPlayer);
        }

        if (controller.Health < (controller.maxHealth/2))
        {
            controller.SetTransition(Transition.HalfHealth);
        }

        //When the distance is near, call the appropriate transition using SetTransition(Transition t) from NPCTankController 
        //Also check when the player becomes too far, call the appropriate transition using SetTransition(Transition t) from NPCTankController 
    }

    public override void Act(Transform player, Transform npc)
    {
        //TO IMPLEMENT
        //Rotate to the target point
        controller.MoveToTarget(player.position, curSpeed, curRotSpeed);
        controller.PointTurret();
        //Rotate turret
        //Go Forward
    }
}
