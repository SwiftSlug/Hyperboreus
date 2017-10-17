using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/CrawlerAttack")]
public class CrawlerAttackAction : Action {

    //private float lastJumped = 0;

    public override void Act(StateController controller)
    {
        CrawlerAttack(controller);
    }

    private void CrawlerAttack(StateController controller)
    {
        controller.navMeshAgent.speed = controller.runSpeed;
        controller.navMeshAgent.destination = controller.target.transform.position;


        //Debug.Log("Last Jumped = " + controller.lastJumped);
        //Debug.Log("Time = " + Time.time);
        if ( (controller.transform.position - controller.target.transform.position).magnitude < 2.0)
        {
            controller.navMeshAgent.destination = controller.transform.position;    //  Target close enough stop moving
        }
        else
        {
            controller.navMeshAgent.destination = controller.target.transform.position; //  Target too far move to target
        }
        
        JumpAttack(controller);            

    }

    private void JumpAttack(StateController controller)
    {

        if (Time.time > (controller.lastJumped + controller.attackCooldown)) //  Call jump only every 5 seconds
        {
            if ((controller.transform.position - controller.target.transform.position).magnitude < 10.0)    //  Check within max range
            {
                if ((controller.transform.position - controller.target.transform.position).magnitude > 5.0) //  Check within min range
                {
                    controller.lastJumped = Time.time;
                    Debug.Log("Jump !");
                }
            }            
        }

            

        //Rigidbody rb = controller.GetComponent<Rigidbody>();

        //controller.navMeshAgent.isStopped = true;
        //controller.navMeshAgent.Stop(true);
        //rb.useGravity = true;



        //controller.GetComponent<Rigidbody>().AddForce( controller.transform.up * controller.jumpDistance , ForceMode.Force);

        //controller.GetComponent<Rigidbody>().AddForce(new Vector3(0, 500, 0), ForceMode.Impulse);

        
        //controller.GetComponent<Rigidbody>().velocity = controller.transform.up * controller.jumpDistance;
    }

}