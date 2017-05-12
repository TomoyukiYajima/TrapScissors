using UnityEngine;
using System.Collections;

public class Trap_Player : MonoBehaviour {

    public GameObject target;
    private NavMeshAgent agent;
    //private bool arrived = false;
    private Animator animator;
    public float charaDistance;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        //if(!arrived)
        //{
            if(agent.remainingDistance < charaDistance)
            {
                agent.Stop();
                animator.SetFloat("speed", 0.0f);
                //arrived = true;
            }
            else
            {
                agent.Resume();
                animator.SetFloat("speed", 1.0f);

                //if (Vector3.Distance(transform.position, player.transform.position) > 10.0f)
                //{
                //    agent.Resume();
                //    agent.SetDestination(player.transform.position);
                //    arrived = false;
                //}
            }
            agent.SetDestination(target.transform.position);
        //}
	}

    void OnAnimatorIK(int layerIndex){
        var weight = Vector3.Dot(transform.forward, target.transform.position - transform.position);

        if (weight < 0)
        {
            weight = 0;
        }
        animator.SetLookAtWeight(0.8f, 0, 1, 1, 0.5f);
        animator.SetLookAtPosition(target.transform.position + Vector3.up * 1.5f);
    }
}
