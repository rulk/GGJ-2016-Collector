using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class MainMenuLogic : MonoBehaviour {


    [SerializeField]
    InputField field;

    public void onLocalServer()
    {
        GGJNetworkManager.singleton.StartHost();
    }

    public void onConnectToServer()
    {
        connectrTo(field.text);
    }

    public void connectLoacla()
    {
        connectrTo("localhost");
    }

    public void connectSr()
    {
        connectrTo("sr.ddns.ms");
    }

    public void connect55()
    {
        connectrTo("192.168.1.55");
    }

    public void connect79()
    {
        connectrTo("192.168.1.79");
    }

    void connectrTo(string addr)
    {
        GGJNetworkManager.singleton.networkAddress = addr;
        GGJNetworkManager.singleton.StartClient();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void StartOnlineMatch()
    {
        GGJNetworkManager.singleton.StartMatchMaker();
        GGJNetworkManager.singleton.matchMaker.ListMatches(0, 20, "", OnMatchList);
    }

    public void OnMatchList(ListMatchResponse matchListResponse)
    {
        if (matchListResponse.success && matchListResponse.matches != null && matchListResponse.matches.Count > 0)
        {
            GGJNetworkManager.singleton.matchMaker.JoinMatch(matchListResponse.matches[0].networkId, "", GGJNetworkManager.singleton.OnMatchJoined);
        }
        else
        {
            var manager = GGJNetworkManager.singleton;
            GGJNetworkManager.singleton.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", GGJNetworkManager.singleton.OnMatchCreate);
        }
    }
}
