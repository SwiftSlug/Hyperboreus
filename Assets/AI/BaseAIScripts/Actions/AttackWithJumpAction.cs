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

        controller.navMeshAgent.destination = controller.target.transform.position;

        if ((controller.transform.position - controller.target.transform.position).magnitude < controller.stopDistance)
        {
            controller.navMeshAgent.destination = controller.transform.position;    //  Target close enough stop moving        

            if (Time.time > (controller.lastAttack + controller.attackCooldown)) //  Call attack only every 5 seconds
            {
                controller.target.GetComponent<PlayerStats>().CmdDamage(controller.attackDamage);
                controller.lastAttack = Time.time;
                //Debug.Log("Attack");
            }

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