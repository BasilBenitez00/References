using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageState : FSMState
{
    public NPCTankController controller;

    private float shootRate;
    private float bonusSpeed;

    public RageState(NPCTankController controller)
    {
      
        stateID = FSMStateID.Rage;

        attackDistance = 200f;
        chaseDistance = 450f;
        shootRate = 4f;
        bonusSpeed = 300f;
        

        this.controller = controller;
    }


    public override void Act(Transform player, Transform npc)
    {
        controller.RageEffect(shootRate, bonusSpeed);
        
        
    }

    public override void Reason(Transform player, Transform npc)
    {
        
    }
}
