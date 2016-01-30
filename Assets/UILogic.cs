using UnityEngine;
using System.Collections;

public class UILogic : MonoBehaviour {

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

    public void onCP()
    {
        Player.s_localPlayer.nextAction = Player.Action.CP;
    }
}
