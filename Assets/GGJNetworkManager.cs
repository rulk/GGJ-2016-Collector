using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GGJNetworkManager : NetworkManager
{

    static public List<Player> players = new List<Player>();

    const float totalTime = 3.0f;
   

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (numPlayers < 2)
        {
            var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<Player>().pos = numPlayers;
            player.GetComponent<Player>().manna = Player.maxManna;
            players.Add(player.GetComponent<Player>());           
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        if(numPlayers == 1)
        {
            ServerStateSynch.instance.timeLeft = ServerStateSynch.totalTime;
        }
    }

}
