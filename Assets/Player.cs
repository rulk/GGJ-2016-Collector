using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour
{

    public static Player s_localPlayer = null;
    public const float oneMannaPerSec = 0.25f;
    float tillNextManna;
    public const int maxManna = 50;
    public enum Action
    {
        None=0,
        Slow,
        Haste,
        Damage,
        Wall,
        CP
    }

    public static int[] mannaCost = new int[] { 0, 20, 15, 25, 30, 5};

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

    [SerializeField]
    public GameObject obstacle;

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
            Vector3 ppos = Camera.main.transform.position;
            ppos.z *= -1;
            Camera.main.transform.position = ppos;
        }
        s_localPlayer = this;
        Camera.main.transform.rotation = qant;
    }

    static public List<Player> players = new List<Player>();

    void Awake()
    {
        players.Add(this);
    }

    void OnDestroy()
    {
        players.Remove(this);
        GGJNetworkManager.players.Remove(this);
        Debug.Log("Player Destroyed!");
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

        tillNextManna -= Time.deltaTime;
        if (tillNextManna < 0.0f)
        {
            if (manna < maxManna)
                manna += 1;

            tillNextManna = oneMannaPerSec;
        }
         
        //if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && nextAction != Action.None)
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS) 
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && nextAction != Action.None)
#else
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0) && nextAction != Action.None)
#endif
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 target = ray.origin;
            target.y = 1.0f;
            if (mannaCost[(int)nextAction] > manna) return;
            manna -= mannaCost[(int)nextAction];
            switch (nextAction)
            {
                case Action.None:
                    break;
                case Action.Slow:                   
                    CmdAoE(target, 0.33f, 3.5f,6.0f,2.5f,0.0f,0);
                    break;
                case Action.Haste:                   
                    CmdAoE(target, 1.60f, 2.0f,5.0f, 1.5f, 0.0f,1);
                    break;
                case Action.Damage:                    
                    CmdAoE(target, 1.0f, 3.5f, 4.0f, 2.0f, 20.0f,2);
                    break;
                case Action.CP:                   
                    CmdspanPOI(target, pos, 5.0f);
                    break;
                case Action.Wall:
                    CmdspanWall(target,6.0f);
                    break;
            }

            //nextAction = Action.None;
        }
	}

    [Command]
    void CmdAoE(Vector3 target, float speedRate, float duration, float diameter, float delay, float hpPerSec, int color)
    {
        target.y = -0.42f;
        GameObject go = (GameObject)Instantiate(AoEPrefab, target, Quaternion.identity);
        go.GetComponent<AoELogic>().speedMult = speedRate;
        go.GetComponent<AoELogic>().duration = duration;
        go.GetComponent<AoELogic>().delay = delay;
        go.GetComponent<AoELogic>().active = false;
        go.GetComponent<AoELogic>().hpPerSec = hpPerSec;
        go.GetComponent<AoELogic>().color = color;
       go.transform.localScale = new Vector3(diameter, 0.5f, diameter);
        NetworkServer.Spawn(go);
    }

    [Command]
    void CmdspanPOI(Vector3 position, int playerNum, float ttl)
    {
        Quaternion qat = new Quaternion();
        qat.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
        GameObject go = (GameObject)Instantiate(POIPrefab,position,qat);
        go.GetComponent<POI>().playerNum = playerNum;
        go.GetComponent<POI>().timeToLive = ttl;
        NetworkServer.Spawn(go);
    }

    [Command]
    void CmdspanWall(Vector3 position, float ttl)
    {
        Quaternion qat = new Quaternion();
        qat.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
        GameObject go = (GameObject)Instantiate(obstacle, position, qat);
        go.GetComponent<Wall>().ttl = ttl;
        NetworkServer.Spawn(go);
    }
}
