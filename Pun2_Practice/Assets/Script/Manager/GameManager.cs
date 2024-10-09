using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public UIManager _UIManager;
    public NetworkManager _NetworkManager;
    public GameObject[] spawnPoint;

    public int objectBrokenNum;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
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

    public void Start()
    {
        objectBrokenNum = 0;
    }

    public void SendUIManagerPlayersNum(int playersNum)
    {
        _UIManager.GameWaitSceneTF();
        _UIManager._PV.RPC("GameWaitScene", RpcTarget.AllViaServer, playersNum);
    }

    public void PlayerNameTextSetting(string playerName)
    {
        _UIManager._PlayerName.text = playerName;
    }
}