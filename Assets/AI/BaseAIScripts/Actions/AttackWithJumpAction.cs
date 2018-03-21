using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/AttackWithJump")]
public class AttackWithJumpAction : Action
{
    public override void Act(StateController controller)
    {
        CrawlerAttackWithJump(controller);
    }

    private void CrawlerAttackWithJump(StateController controller)
    {
        //  Ensure that the controller has a target before attempting to move and attack
        if (controller.target == null)
        {
            return;
        }
        if (!controller.isServer)
        {
            //  Do nothing if the call does not come from the server
            return;
        }

        controller.navMeshAgent.destination = controller.target.transform.position;

        float distanceToTarget = (controller.transform.position - controller.target.transform.position).magnitude;

        //  AI movement towards target
        if (distanceToTarget < controller.stopDistance)
        {
            //  Target close enough stop moving
            controller.navMeshAgent.destination = controller.transform.position;
        }
        else
        {
            //  Target too far away move to target
            controller.navMeshAgent.destination = controller.target.transform.position;
        }

        //  AI Attacking players
        if (controller.target.GetComponent<PlayerStats>())
        {

            if (distanceToTarget < controller.attackDistance)
            {
                //  Only attack target if within attack range   
                if (Time.time > (controller.lastAttack + controller.attackCooldown))
                {
                    //  Call attack after cooldown
                    controller.target.GetComponent<PlayerStats>().CmdDamage(controller.attackDamage);
                    controller.lastAttack = Time.time;
                }
            }
        }
        //  AI attack for buildings
        else if (controller.target.GetComponent<TestBuildingController>())
        {
            Debug.Log("Targetted building for attack");
            if (distanceToTarget < controller.buildingAttackDistance)
            {
                //  Only attack target if within attack range   
                if (Time.time > (controller.lastAttack + controller.attackCooldown))
                {
                    //  Call attack after cooldown
                    controller.target.GetComponent<TestBuildingController>().CmdDamage(controller.attackDamage);
                    controller.lastAttack = Time.time;
                    Debug.Log("Damage Called on building !");
                }
            }
        }

        //  Set speed back to normal running speed if the animation has stopped running
        if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyJumpAnimation"))   
        {
            controller.navMeshAgent.speed = controller.runSpeed;
        }
        //  Set the speed to jump attack speed if the animation is running
        else
        {
            controller.navMeshAgent.speed = controller.runSpeed * 100;
        }

        if (controller.gameObject.GetComponent<AIStats>().isTrapped == false)
        {
            //  Only perform the jump attack if the AI is not trapped
            JumpAttack(controller);
        }
        else
        {
            //Debug.Log("I can't jump, I'm trapped!");
        }

    }

    private void JumpAttack(StateController controller)
    {

        if (Time.time > (controller.lastJumped + controller.jumpCooldown)) //  Call jump only every 5 seconds
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