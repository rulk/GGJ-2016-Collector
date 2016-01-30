using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UILogic : MonoBehaviour {

    [SerializeField]
    RectTransform parentManna;

    [SerializeField]
    RectTransform childManna;

    [SerializeField]
    Text score;

    [SerializeField]
    Text time;

    [SerializeField]
    Text winnerMsg;
    void Start()
    {
        winnerMsg.enabled = false;
    }
    void Update()
    {
        if (Player.s_localPlayer == null) return;
        int manna = Player.s_localPlayer.manna;
        float prc = ((float)manna )/Player.maxManna;

        var r = childManna.offsetMax;
        r.x = - parentManna.rect.width * (1.0f - prc);
        childManna.offsetMax = r;

        if(Player.players != null && Player.players.Count ==2)
        {
            score.text = Player.players[0].resources.ToString() + "/" + Player.players[1].resources.ToString();
        }
        

        if (ServerStateSynch.instance != null)
        {
            if(ServerStateSynch.instance.timeLeft <= 0.0f)
            {
                if (winnerMsg.enabled == false)
                {
                    winnerMsg.enabled = true;
                    Player other = null;
                    for(int i=0; i< Player.players.Count; i++)
                    {
                        if(Player.s_localPlayer != Player.players[i])
                        {
                            other = Player.players[i];
                        }
                    }

                    if(other.resources == Player.s_localPlayer.resources)
                    {
                        winnerMsg.text = "Tie!";
                    }
                    if (other.resources > Player.s_localPlayer.resources)
                    {
                        winnerMsg.text = "Defeat!";
                    }
                    if (other.resources < Player.s_localPlayer.resources)
                    {
                        winnerMsg.text = "Victory!";
                    }

                }
            }
            else
            {
                int min = (int)(ServerStateSynch.instance.timeLeft / 60.0);
                int sec = (int)(ServerStateSynch.instance.timeLeft - min*60.0f);
                time.text = min.ToString() + ":" + sec.ToString();
            }
        }
    }

	public void onSlowDown()
    {
        Player.s_localPlayer.nextAction = Player.Action.Slow;
    }
     
    public void onHaste()
    {
        Player.s_localPlayer.nextAction = Player.Action.Haste;
    }

    public void onDamage()
    {
        Player.s_localPlayer.nextAction = Player.Action.Damage;
        
    }

    public void OnWall()
    {
        Player.s_localPlayer.nextAction = Player.Action.Wall;
    }

    public void onCP()
    {
        Player.s_localPlayer.nextAction = Player.Action.CP;
    }
}
