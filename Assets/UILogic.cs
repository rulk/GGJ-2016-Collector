using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UILogic : MonoBehaviour {

    [SerializeField]
    RectTransform parentManna;

    [SerializeField]
    RectTransform childManna;

    void Update()
    {
        if (Player.s_localPlayer == null) return;
        int manna = Player.s_localPlayer.manna;
        float prc = ((float)manna )/Player.maxManna;

        var r = childManna.offsetMax;
        r.x = - parentManna.rect.width * (1.0f - prc);
        childManna.offsetMax = r;
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
