using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class ServerStateSynch : NetworkBehaviour
{
    [SyncVar]
    public float timeLeft;

    public const float totalTime = 3.0f*60;

    public static ServerStateSynch instance;

    void Awake()
    {
        instance = this;
        if (isServer)
            timeLeft = totalTime;
    }

    [Server]
	void Update ()
    {
	    if(GGJNetworkManager.players.Count == 0)
        {
            timeLeft = totalTime;
        }

        if(GGJNetworkManager.players.Count == 2)
        {
            timeLeft -= Time.deltaTime;
        }
	}
}
