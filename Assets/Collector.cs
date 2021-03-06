﻿using UnityEngine;
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
    public int playerNum;

    [SyncVar]
    public float hp;
    
    public const float maxHp = 50.0f;

    public const float stunDuration = 4.0f;

    public const float withResourceSpeedMult = 0.8f;

    float tillExplosionEnd;

    float stunRemaning = 0.0f;

    [SerializeField]
    GameObject resourcePrefab;

    NavMeshAgent agent;
    Rigidbody rigidBody;
    // Use this for initialization
	void Start ()
    {
        tillExplosionEnd = 0.0f;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;

        if (!isServer)
            return;

        hp = maxHp;
        agent.enabled = true;
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

        if(tillExplosionEnd > 0.0f && rigidBody.isKinematic == false)
        {
            tillExplosionEnd -= Time.deltaTime;
            if(tillExplosionEnd <=0.0f)
            {
                rigidBody.isKinematic = true;
                agent.enabled = true;
            }
        }

        float resourceDistance = float.MaxValue;
#if UNITY_EDITOR
        if (stunRemaning > 0.0f )
#else
        if (stunRemaning > 0.0f || GGJNetworkManager.players.Count != 2)
#endif
        {
            stunRemaning -= Time.deltaTime;
            agent.ResetPath();
            return;
        }

        if (resource != null)
        {
            resourceDistance = Vector3.Distance(transform.position, resource.transform.position);
        }

        if (agent.enabled == true)
        {
            if (target != null)
            {
                float targetPos = Vector3.Distance(transform.position, target.transform.position);
                if (targetPos < resourceDistance || draggedResource != null || (resource != null && resource.getTarget() != this))
                {
                    agent.SetDestination(target.transform.position);
                }
                else if (draggedResource == null)
                {
                    agent.SetDestination(resource.transform.position);
                }
            }
            else if (resource != null && draggedResource == null)
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

            if (resource != null && resourceDistance < 1.5f)
            {
                if (resource.follow(this))
                    draggedResource = resource;
            }

            if (draggedResource != null)
            {
                float homeDistance = Vector3.Distance(homeEmitter.transform.position, transform.position);
                if (homeDistance < 1.5f)
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
                else
                {
                    speed *= withResourceSpeedMult;
                }
            }
        }

        resource = null;
        agent.speed =  speed;
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

    public void ApplyDammage(float dmg)
    {
        hp -= dmg; 
        if(hp < 0)
        {
            stunRemaning = stunDuration;
            hp = maxHp;
            if (draggedResource != null)
            {
                draggedResource.follow(null);
                draggedResource = null;
            }
        }
    }

    public void affectByExplosion(float duration)
    {
        tillExplosionEnd = duration;
        rigidBody.isKinematic = false;
        agent.enabled = false;
    }
}
