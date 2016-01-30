using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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
}
