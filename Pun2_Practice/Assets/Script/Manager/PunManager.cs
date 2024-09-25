using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunManager : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void GameStartButton()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnectedToMaster()
    {
        
        PhotonNetwork.JoinOrCreateRoom("PlayGame", new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        GameManager.Instance.SendUIManagerPlayersNum(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("½ÇÆÐ");
    }
}
