using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Collector : NetworkBehaviour
{

    POI target = null;
    float distanceToPOI;

    Resource resource;
    Resource draggedResource;

    float maxSpeed;

    float speed;

    [SerializeField]
    Transform homeEmitter;

    [SerializeField]
    int playerNum;


    [SerializeField]
    GameObject resourcePrefab;

    NavMeshAgent agent;
    // Use this for initialization
	void Start ()
    {
        if (!isServer)
            return;
        
        agent = GetComponent<NavMeshAgent>();
        maxSpeed = agent.speed;
        POI.OnPoiIsActive += addPOI;
        Resource.OnResourceOnTheGround += resourceOnTheGround;
    }
    
    void OnDestroy()
    {
        if (!isServer)
            return;

        POI.OnPoiIsActive -= addPOI;
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        if (!isServer)
            return;
        float resourceDistance = float.MaxValue;

        if (resource != null)
        {
            resourceDistance = Vector3.Distance(transform.position, resource.transform.position);
        }

        if (target != null )
        {
            float targetPos = Vector3.Distance(transform.position, target.transform.position);
            if (targetPos < resourceDistance)
            {
                agent.SetDestination(target.transform.position);
            }
            else
            {
                agent.SetDestination(resource.transform.position);
            }
        }
        else if(resource != null )
        {
            agent.SetDestination(resource.transform.position);
        }
        else
        {
            agent.SetDestination(homeEmitter.position);
        }

        if (target != null)
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

        if(resource != null && resourceDistance < 1.5f)
        {
            resource.follow(this);
            draggedResource = resource;
        }

        if(draggedResource != null)
        {
            float homeDistance = Vector3.Distance(homeEmitter.transform.position, transform.position);
            if(homeDistance < 1.5f)
            {
                Destroy(draggedResource.gameObject);
                draggedResource = null;

                GameObject go = (GameObject)Instantiate(resourcePrefab, Vector2.zero, Quaternion.identity);
                NetworkServer.Spawn(go);

                if (GGJNetworkManager.players[playerNum] != null)
                {
                    GGJNetworkManager.players[playerNum].resources += 1;
                }
            }
        }

        resource = null;
        agent.speed = speed;
        speed = maxSpeed;
    }

    public void addPOI(POI poi)
    {
        if (!isServer)
            return;

        if (poi.playerNum != playerNum)
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

    public void resourceOnTheGround(Resource res)
    {
        resource = res;
    }

    public void ApplySpeedChange(float mult)
    {
        speed *= mult;
    }
}
