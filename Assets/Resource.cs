using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Resource : NetworkBehaviour
{
    public delegate void ResourceOnTheGround(Resource res);

    public static event ResourceOnTheGround OnResourceOnTheGround;

    NavMeshAgent agent;
    Rigidbody rigidBody;

    Collector target;

    float tillExplosionEnd = 0.0f;

    public void affectByExplosion(float duration)
    {
        tillExplosionEnd = duration;
        rigidBody.isKinematic = false;
        agent.enabled = false;
    }

    void Start()
    {
        tillExplosionEnd = 0.0f;
        agent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        agent.enabled = false;
        if (!isServer)
            return;
        agent.enabled = true;
    }

    void Update()
    {
        if (!isServer)
            return;

        if (tillExplosionEnd > 0.0f && rigidBody.isKinematic == false)
        {
            tillExplosionEnd -= Time.deltaTime;
            if (tillExplosionEnd <= 0.0f)
            {
                rigidBody.isKinematic = true;
                agent.enabled = true;
            }
        }

        if ( OnResourceOnTheGround != null)
        {
            OnResourceOnTheGround(this);
        }

        if(target != null && agent.enabled == true)
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
    public Collector getTarget()
    {
        return target;
    }
}
