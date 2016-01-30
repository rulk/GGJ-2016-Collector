using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Resource : NetworkBehaviour
{
    public delegate void ResourceOnTheGround(Resource res);

    public static event ResourceOnTheGround OnResourceOnTheGround;

    NavMeshAgent agent;

    Collector target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isServer)
            return;

        if ( OnResourceOnTheGround != null)
        {
            OnResourceOnTheGround(this);
        }

        if(target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    public bool follow(Collector collect)
    {
        if (collect == null)
        {
            target = collect;
            return true;
        }
        if (target == null)
        {
            target = collect;
            return true;
        }
        return false;
    }
}
