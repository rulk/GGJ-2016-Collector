using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GGJNetworkManager : NetworkManager
{
   

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<Player>().pos = numPlayers;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
