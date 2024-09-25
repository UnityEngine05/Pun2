using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectUI
{
    public string objectName;
    public GameObject[] object_TF;
}
public class UIManager : MonoBehaviour
{
    public ObjectUI[] objectUI;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        StartSetting();
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

        Screen.SetResolution(1280, 720, false);
    }

    public void GameWaitScene(int playersNum)
    {
        for (int i = 0; i < objectUI[2].object_TF.Length; i++)
        {
            objectUI[2].object_TF[i].SetActive(true);
        }
        for (int i = 0; i < objectUI[3].object_TF.Length; i++)
        {
            objectUI[3].object_TF[i].SetActive(false);
        }

        for(int i = 0; i < playersNum; i++)
        {
            if (!objectUI[4].object_TF[i].active)
            {
                objectUI[4].object_TF[i].SetActive(true);
            }
        }
    }
}
