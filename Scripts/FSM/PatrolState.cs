using UnityEngine;
using System.Collections;

public class PatrolState : FSMState
{
  

	Vector3 velocity;
    private NPCTankController controller;
    public PatrolState(Transform[] wp, NPCTankController controller) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Patrolling;

        curSpeed = 200f;
        curRotSpeed = 2f;
        attackDistance = 200f;
        chaseDistance = 450f;
        this.controller = controller;
        
    }

    public override void Reason(Transform player, Transform npc)
    {
        //TO IMPLEMENT
        //Check the distance with player tank
        float distance = Vector3.Distance(player.position, npc.position);
        if(distance <= chaseDistance)
        {
            controller.SetTransition(Transition.SawPlayer);
        }
        //When the distance is near, call the appropriate transition using SetTransition(Transition t) from NPCTankController 

        if (controller.Health < (controller.maxHealth / 2))
        {
            controller.SetTransition(Transition.HalfHealth);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //TO IMPLEMENT
        //Find another random patrol point if the current destination point is reached
        controller.MoveToTarget(destPos,curSpeed,curRotSpeed);
        float distance = Vector3.Distance(destPos, npc.position);
        if(distance <= 20f)
        {
            FindNextPoint();
        }
       
		//Rotate the tank and the turret
		//Go Forward
    }

  
    }