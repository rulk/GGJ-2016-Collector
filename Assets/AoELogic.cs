using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class AoELogic : NetworkBehaviour
{
    //[SyncVar]
    public float speedMult;

    //[SyncVar]
    public float duration;

    public float delay;

    public float hpPerSec;

    [SyncVar]
    public int color;

    [SyncVar]
    public bool active;

    void OnTriggerStay(Collider collider)
    {
        if (!isServer)
            return;
        if (delay > 0.0f)
            return;
        var collector = collider.gameObject.GetComponent<Collector>();

        if (collector != null)
        {
            collector.ApplySpeedChange(speedMult);
            collector.ApplyDammage(Time.fixedDeltaTime * hpPerSec);
        }
    }

    bool proxyActive = false;
    bool firstUpdate = true;
    void Update()
    {
        if( firstUpdate)
        {
           switch(color)
            {
                case 0: GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.2f, 0.0f, 0.2f);break;
                case 1: GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.2f, 0.2f); break;
                case 2: GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.0f, 0.0f, 0.2f); break;
            }
                
            firstUpdate = false;

        }
        if( proxyActive == false && active == true)
        {
            proxyActive = true;
            switch (color)
            {
                case 0: GetComponent<MeshRenderer>().material.color = new Color(0.0f, 1f, 0.0f, 0.2f); break;
                case 1: GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 1f, 0.2f); break;
                case 2: GetComponent<MeshRenderer>().material.color = new Color(1f, 0.0f, 0.0f, 0.2f); break;
            }
        }

        if (!isServer)
            return;

        if (delay < 0.0f)
        {
            duration -= Time.deltaTime;
            if (duration < 0)
            {
                Destroy(gameObject);
            }
        }
        else
            delay -= Time.deltaTime;

        if(delay < 0.0f && active == false)
            active = true;
    }
}
