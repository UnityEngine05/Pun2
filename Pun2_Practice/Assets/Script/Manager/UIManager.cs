using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] objectSettingUI_T, objectSettingUI_F;
    void Awake()
    {
        for (int i = 0; i < objectSettingUI_T.Length; i++)
        {
            objectSettingUI_T[i].SetActive(true);
        }

        for (int i = 0; i < objectSettingUI_F.Length; i++)
        {
            objectSettingUI_F[i].SetActive(false);
        }
    }
}
