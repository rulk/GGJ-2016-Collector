using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GGJNetworkManager : NetworkManager
{

    static public Player[] players = new Player[2];

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (numPlayers < 2)
        {
            var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<Player>().pos = numPlayers;
            players[player.GetComponent<Player>().pos] = player.GetComponent<Player>();
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}
