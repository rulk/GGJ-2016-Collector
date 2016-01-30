using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class AoELogic : NetworkBehaviour
{
    //[SyncVar]
    public float speedMult;

    //[SyncVar]
    public float duration;

    void OnTriggerStay(Collider collider)
    {
        if (!isServer)
            return;
        var collector = collider.gameObject.GetComponent<Collector>();

        if (collector != null)
        {
            collector.ApplySpeedChange(speedMult);
        }
    }

    void Update()
    {
        if (!isServer)
            return;

        duration -= Time.deltaTime;
        if(duration < 0)
        {
            Destroy(gameObject);
        }
    }
}
