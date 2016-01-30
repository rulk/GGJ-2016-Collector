using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Collector : NetworkBehaviour
{

    static public Collector s_myCollector;
    POI target = null;
    float distanceToPOI;

    [SerializeField]
    Transform homeEmitter;

    NavMeshAgent agent;
    // Use this for initialization
	void Start ()
    {
        if (!isServer)
            return;
        agent = GetComponent<NavMeshAgent>();
        s_myCollector = this;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (!isServer)
            return;

        if (target != null )
        {
            agent.SetDestination(target.transform.position);
            if (!agent.pathPending)
            {
                float dist = Vector3.Distance(transform.position, target.transform.position);
                if (dist <= 1.5f)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        Destroy(target.gameObject);
                        target = null;
                    }
                }
            }
        }
        else
        {
            agent.SetDestination(homeEmitter.position);

        }
	}

    public void addPOI(POI poi)
    {
        if (!isServer)
            return;

        float dist = Vector3.Distance(poi.transform.position, transform.position);
        if (target == null)
        {
            target = poi;
            distanceToPOI = dist;
        }
        else
        {
            if(dist < distanceToPOI)
            {
                target = poi;
                distanceToPOI = dist;
            }
        }
    }
}
