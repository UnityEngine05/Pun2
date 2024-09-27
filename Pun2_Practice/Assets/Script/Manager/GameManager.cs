using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public UIManager _UIManager;
    public Player _Player;
    public NetworkManager _NetworkManager;
    public GameObject[] spawnPoint;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void SendUIManagerPlayersNum(int playersNum)
    {
        _UIManager.GameWaitSceneTF();
        _UIManager._PV.RPC("GameWaitScene", RpcTarget.AllViaServer, playersNum);
    }
}