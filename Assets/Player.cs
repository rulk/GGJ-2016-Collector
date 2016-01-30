using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
 
    [SyncVar]
    public int pos;

    [SyncVar]
    public int resources;

    [SyncVar]
    public int manna;

    [SerializeField]
    public GameObject POIPrefab;

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
            Camera.main.transform.rotation = qant;
        }
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

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 target = ray.origin;
            target.y = 1.0f;
            CmdspanPOI(target, pos);
        }
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
