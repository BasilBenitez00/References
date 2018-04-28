using UnityEngine;
using System.Collections;

public class AttackState : FSMState
{

    private NPCTankController controller;

    public AttackState(Transform[] wp, NPCTankController controller) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Attacking;
        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        attackDistance = 200f;
        chaseDistance = 450f;
        this.controller = controller;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        //TO IMPLEMENT
        float distance = Vector3.Distance(player.position, npc.position);
        if(distance > attackDistance + 20f)
        {
            controller.SetTransition(Transition.SawPlayer);   
        }
        if (controller.Health < (controller.maxHealth / 2))
        {
            controller.SetTransition(Transition.HalfHealth);
        }
        //Check the distance with the player tank

        //When the distance is near, call the appropriate transition using SetTransition(Transition t) from NPCTankController 
        //Also check when the player becomes too far, call the appropriate transition using SetTransition(Transition t) from NPCTankController 
    }

    public override void Act(Transform player, Transform npc)
    {
        //TO IMPLEMENT
        //Set the target position as the player position
        //Rotate to the target point
        controller.PointTurret();
        controller.ShootBullet();
        //Rotate turret
        //Shoot bullet towards the player

    }
}
