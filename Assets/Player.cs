using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour
{

    public static Player s_localPlayer = null;
    public enum Action
    {
        None=0,
        Slow,
        Haste,
        Damage,
        Wall,
        CP
    }

    public Action nextAction;

    [SyncVar]
    public int pos;

    [SyncVar]
    public int resources;

    [SyncVar]
    public int manna;

    [SerializeField]
    public GameObject POIPrefab;

    [SerializeField]
    public GameObject AoEPrefab;

    public override void OnStartLocalPlayer()
    {
        Quaternion qant = new Quaternion();
        if (pos == 0)
        {
            qant.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
           
        }
        else
        {
            qant.eulerAngles = new Vector3(90.0f, 180.0f, 0.0f);
           
        }
        s_localPlayer = this;
        Camera.main.transform.rotation = qant;
    }
    // Use this for initialization
    void Start ()
    {
	   
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0) && nextAction != Action.None)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 target = ray.origin;
            target.y = 1.0f;
            switch(nextAction)
            {
                case Action.None:
                    break;
                case Action.Slow:
                    CmdAoE(target, 0.75f, 2.0f);
                    break;
                case Action.Haste:
                    CmdAoE(target, 1.15f, 2.0f);
                    break;
                case Action.CP:
                    CmdspanPOI(target, pos);
                    break;
            }

            nextAction = Action.None;
        }
	}

    [Command]
    void CmdAoE(Vector3 target, float speedRate, float duration)
    {
        target.y = -0.42f;
        GameObject go = (GameObject)Instantiate(AoEPrefab, target, Quaternion.identity);
        go.GetComponent<AoELogic>().speedMult = speedRate;
        go.GetComponent<AoELogic>().duration = duration;
        NetworkServer.Spawn(go);
    }

    [Command]
    void CmdspanPOI(Vector3 position, int playerNum)
    {
        Quaternion qat = new Quaternion();
        qat.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
        GameObject go = (GameObject)Instantiate(POIPrefab,position,qat);
        go.GetComponent<POI>().playerNum = playerNum;
        NetworkServer.Spawn(go);
    }
}
