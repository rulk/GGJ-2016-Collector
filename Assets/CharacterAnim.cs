using UnityEngine;
using System.Collections;

public class CharacterAnim : MonoBehaviour {


    Animator animator;

    [SerializeField]
    NavMeshAgent agent;

    Quaternion initRot;
    int previousDirection;
    
    [SerializeField]
    public float s, c;

    [SerializeField]
    RuntimeAnimatorController you;
    [SerializeField]
    RuntimeAnimatorController notYou;

    // Use this for initialization
    void Start ()
    {
        
        animator = GetComponent<Animator>();
        var collector = GetComponentInParent<Collector>();
        if(collector != null)
        {
            if (collector.playerNum == Player.s_localPlayer.pos)
            {
                animator.runtimeAnimatorController = you;
            }
            else
            {
                animator.runtimeAnimatorController = notYou;
            }
        }
        
        previousDirection = animator.GetInteger("Direction");
        initRot = transform.rotation;
    }
	enum Direction
    {
        North = 0 ,
        South = 1,
        West = 2,
        East = 3
    };

    

    static string[] StateNames = new string[] { "N","S","W","E"}; 
	// Update is called once per frame
	void Update ()
    {
        Vector3 velocity = agent.velocity;
        transform.rotation = initRot;
        velocity.y = 0;
        velocity.Normalize();

        s = velocity.z;
        c = velocity.x;

        int direction = 0;

        if(Mathf.Abs(c) > Mathf.Abs(s))
        {
            if(c > 0.0f)
            {
                direction = (int)Direction.East;
            }
            else
            {
                direction = (int)Direction.West;
            }
        }
        else
        {
            if(s > 0.0f)
            {
                direction = (int)Direction.North;
            }
            else
            {
                direction = (int)Direction.South;
            }
        }

        if (previousDirection != direction)
        {
            animator.Play(StateNames[direction]);
            previousDirection = direction;
        }
    }
}
