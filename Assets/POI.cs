using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class POI : NetworkBehaviour
{
    [SyncVar]
    public int playerNum;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isServer)
            return;

        if (Collector.s_myCollector != null)
        {
            Collector.s_myCollector.addPOI(this);
        }
	
	}
}
