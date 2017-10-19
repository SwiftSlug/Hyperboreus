using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/CrawlerAttack")]
public class CrawlerAttackAction : Action {

    public override void Act(StateController controller)
    {
        CrawlerAttack(controller);
    }

    private void CrawlerAttack(StateController controller)
    {
        
        controller.navMeshAgent.destination = controller.target.transform.position;
       
        if ( (controller.transform.position - controller.target.transform.position).magnitude < controller.stopDistance)
        {
            controller.navMeshAgent.destination = controller.transform.position;    //  Target close enough stop moving
            // Player damage function goes here ********************
        }
        else
        {
            controller.navMeshAgent.destination = controller.target.transform.position; //  Target too far move to target
        }
        
        if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyJumpAnimation"))   //  Is the jump animation running
        {
            controller.navMeshAgent.speed = controller.runSpeed;
        }
        else
        {
            controller.navMeshAgent.speed = controller.runSpeed * 100;
        }

        JumpAttack(controller);            

    }

    private void JumpAttack(StateController controller)
    {

        if (Time.time > (controller.lastJumped + controller.attackCooldown)) //  Call jump only every 5 seconds
        {
            if ((controller.transform.position - controller.target.transform.position).magnitude < 10.0)    //  Check within max jump range
            {
                if ((controller.transform.position - controller.target.transform.position).magnitude > 5.0) //  Check within min jump range
                {

                    controller.lastJumped = Time.time;
                    //controller.jumpAnimation.Play();

                    controller.navMeshAgent.speed = controller.runSpeed * 100;
                    controller.animator.SetTrigger("jump");
                    

                    //Debug.Log("Jump !");
                }
            }            
        }
    }

}