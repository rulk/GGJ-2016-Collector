using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Wall : NetworkBehaviour
{
    public float ttl;
	
	// Update is called once per frame
	void Update ()
    {
        if (!isServer)
            return;
        
        ttl -= Time.deltaTime;
        if (ttl < 0.0f)
            Destroy(gameObject);
	}
}
