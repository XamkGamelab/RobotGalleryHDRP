using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class NPC : CharacterBase
{
    public bool UseLookAtIK = true;
    protected NavMeshAgent agent => GetComponent<NavMeshAgent>();
    protected Animator anim => GetComponentInChildren<Animator>();

    private (float min, float max) waitMinMaxSeconds = (2f, 5f);
    private float nearDestinationTresholdDistance = 0.5f;
    private Transform currentDestination;
    private List<PatrolPoint> patrolPoints = new List<PatrolPoint>();

    //Additional inverse kinematics for turning the head towards paintings:
    private Vector3 lookAtOffset = new Vector3(0, 1.5f, 0);
    private LookAtIK lookAtIK = null;

    private void Start()
    {
        //Add IK component to animator object if UseLookAtIK
        if (UseLookAtIK)
            lookAtIK = anim.gameObject.AddComponent<LookAtIK>();

        patrolPoints = GameObject.FindObjectsOfType<PatrolPoint>(true).ToList();
        currentDestination = SetDestinationToRandomPatrolPoint();
        //Set new look target if lookAtIK not null
        lookAtIK?.SetIKLookTarget(currentDestination.position + currentDestination.forward + lookAtOffset);

        StartCoroutine(WanderingRoutine());
    }

    private IEnumerator WanderingRoutine()
    {
        while (true)
        {
            //Set new random destination if close enough...
            if (Vector3.Distance(transform.position, currentDestination.position) < nearDestinationTresholdDistance)
            {
                //Wait couple of seconds in destination:
                yield return new WaitForSeconds(Random.Range(waitMinMaxSeconds.min, waitMinMaxSeconds.max));

                //Set new destination:
                currentDestination = SetDestinationToRandomPatrolPoint();
                //Set new look target if lookAtIK not null
                lookAtIK?.SetIKLookTarget(currentDestination.position + currentDestination.forward + lookAtOffset);
            }
            //...or wait for next frame
            else
                yield return null;
        }
    }

    private Transform SetDestinationToRandomPatrolPoint()
    {
        Transform randomDestination = patrolPoints[Random.Range(0, patrolPoints.Count)].transform;
        agent.SetDestination(randomDestination.position);
        return randomDestination;
    }

    private void Update()
    {
        //Switch between idle/walk based on agent velocity magnitude
        anim.SetFloat("AgentSpeed", agent.velocity.magnitude);
    }
}
