using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class ObjectUI
{
    public string objectName;
    public GameObject[] object_TF;
}
public class UIManager : MonoBehaviourPunCallbacks
{
    public ObjectUI[] objectUI;
    public Sprite[] _PlayerSprite;
    public Image _PlayerImage, _PlayerCoolTime;
    public GameObject _MainCanvasUI, _PlayerUICanvas, _GameEnding;
    public PhotonView _PV;
    public Text _PlayerName, _Timer, _GameEndingText;

    public int screenWidth, screenHeight;
    public bool screenAllSizeBool;
    

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        StartSetting();
    }

    public void ScreenAllSize(bool allSize)
    {
        screenAllSizeBool = allSize;
    }

    public void ScreenSizeWidth(int width)
    {
        screenWidth = width;
    }

    public void ScreenSizeHeight(int height)
    {
        screenHeight = height;
    }

    public void ScreenSetting()
    {
        Screen.SetResolution(screenWidth, screenHeight, screenAllSizeBool);
    }

    public void StartSetting()
    {
        for (int i = 0; i < objectUI[0].object_TF.Length; i++)
        {
            objectUI[0].object_TF[i].SetActive(true);
        }

        for (int i = 0; i < objectUI[1].object_TF.Length; i++)
        {
            objectUI[1].object_TF[i].SetActive(false);
        }


        screenWidth = 1280;
        screenHeight = 720;
        screenAllSizeBool = false;
        Screen.SetResolution(screenWidth, screenHeight, screenAllSizeBool);
    }

    public void GameWaitSceneTF()
    {
        for (int i = 0; i < objectUI[2].object_TF.Length; i++)
        {
            objectUI[2].object_TF[i].SetActive(true);
        }
        for (int i = 0; i < objectUI[3].object_TF.Length; i++)
        {
            objectUI[3].object_TF[i].SetActive(false);
        }
    }

    public void PlayerImageSetting(int imageNum)
    {
        _PlayerImage.sprite = _PlayerSprite[imageNum];
    }

    [PunRPC]
    public void GameWaitScene(int playersNum)
    {
        for(int i = 0; i < playersNum; i++)
        {
            if (!objectUI[4].object_TF[i].activeSelf)
            {
                objectUI[4].object_TF[i].SetActive(true);
            }
        }

        if (playersNum >= 4)
        {
            _MainCanvasUI.SetActive(false);
            _PlayerUICanvas.SetActive(true);
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

    public void GameTimer(float timer)
    {
        _Timer.text = string.Format("{0:N1}", timer);
    }
}
