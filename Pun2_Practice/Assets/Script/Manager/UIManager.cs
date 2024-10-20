using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[System.Serializable]
public class ObjectUI
{
    public string objectName;
    public GameObject[] object_TF;
}
public class UIManager : MonoBehaviourPunCallbacks
{
    public ObjectUI[] objectUI;
    public Sprite[] _PlayerSprite, _Skill_Icon, _GameEndingSprite;
    public Image _PlayerImage, _PlayerCoolTime, _Skill_Image, _GameEndingImage;
    public GameObject _MainCanvasUI, _PlayerUICanvas, _GameEnding, _FixTextImage;
    public PhotonView _PV;
    public Text _PlayerName, _GameEndingText, _FixText;

    public float fixTextHideTime;
    public int screenWidth, screenHeight;
    public bool screenAllSizeBool;
    

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        StartSetting();
    }

    void LateUpdate()
    {
        if (_FixTextImage.activeSelf) 
        {
            fixTextHideTime += Time.deltaTime;
            if(fixTextHideTime >= 3)
            {
                _FixTextImage.SetActive(false);
            }
        }
    }

    public void FixTextFunction(string text)
    {
        fixTextHideTime = 0;
        _FixTextImage.SetActive(true);
        _FixText.text = text;
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

    public void SkillIconSetting(int num)
    {
        _Skill_Image.sprite = _Skill_Icon[num];
    }

    IEnumerator GameEnding()
    {
        _GameEnding.SetActive(true);
        _GameEndingImage.sprite = _GameEndingSprite[0];
        yield return StartCoroutine(TextTyping("..헉..헉헉..."));
        yield return StartCoroutine(TextTyping("하아........."));
        _GameEndingImage.sprite = _GameEndingSprite[1];
        yield return StartCoroutine(TextTyping("헉헉......"));
        yield return StartCoroutine(TextTyping("....얼마나...더 가야하는거야..."));
        yield return StartCoroutine(TextTyping("으아... 진짜 힘들어 죽겠네..."));
        _GameEndingImage.sprite = _GameEndingSprite[2];
        yield return StartCoroutine(TextTyping("허억...여기서...좀 더...."));
        yield return StartCoroutine(TextTyping("산 속에... 있는 것 같은데...."));
        yield return StartCoroutine(TextTyping("좀만 더 참자...애들아..."));
        _GameEndingImage.sprite = _GameEndingSprite[3];
        yield return StartCoroutine(TextTyping("저기 앞에 보이는 산..."));
        yield return StartCoroutine(TextTyping("저기 맞지..?"));
        yield return StartCoroutine(TextTyping("..어..! 저기 맞아! gps로는 분명 저기야...!"));
        yield return StartCoroutine(TextTyping("gps로는 분명 저기야...!"));
        _GameEndingImage.sprite = _GameEndingSprite[4];
        yield return StartCoroutine(TextTyping("조금만 버텨 ??아..!"));
        yield return StartCoroutine(TextTyping("곧 도착이야..! 허억...허억.."));
        yield return StartCoroutine(TextTyping("여기 맞아..? 너무 길이 위험한데..!헉헉.."));
        yield return StartCoroutine(TextTyping("발밑 조심해..!"));
        _GameEndingImage.sprite = _GameEndingSprite[5];
        yield return StartCoroutine(TextTyping("헉...헉."));
        yield return StartCoroutine(TextTyping("드디어...."));
        yield return StartCoroutine(TextTyping("....."));
        yield return StartCoroutine(TextTyping("....."));
        yield return StartCoroutine(TextTyping("도착했다!"));
        GameManager.Instance._NetworkManager.LeaveRoom();
        yield return new WaitForSeconds(1);
        QuitGame();
    }

    IEnumerator TextTyping(string text)
    {
        _GameEndingText.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            _GameEndingText.text += text[i];
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);
    }

    [PunRPC]
    public void UIManagerCoroutine()
    {
        StartCoroutine(GameEnding());
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
