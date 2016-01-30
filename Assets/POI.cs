using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class POI : NetworkBehaviour
{
    public delegate void PoiIsActive( POI poi );

    public static event PoiIsActive OnPoiIsActive;

    [SyncVar]
    public int playerNum;
    
	// Use this for initialization
	public override void OnStartClient()
    {
      
            if (playerNum == 0)
                GetComponent<MeshRenderer>().material.color = Color.red;
            else
                GetComponent<MeshRenderer>().material.color = Color.blue;
                    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isServer)
            return;

        if(OnPoiIsActive != null)
            OnPoiIsActive(this);
	
	}
}
