using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;

[System.Serializable]
public class ObjectUI
{
    public string objectName;
    public GameObject[] object_TF;
}
public class UIManager : MonoBehaviourPunCallbacks
{
    public ObjectUI[] objectUI;
    public Sprite[] _PlayerSprite, _Skill_Icon, _GameEndingSprite, _GameStartSprite;
    public Image _PlayerImage, _PlayerCoolTime, _Skill_Image, _GameEndingImage, _SkipImage;
    public GameObject _MainCanvasUI, _PlayerUICanvas, _GameEnding, _FixTextImage,
        skipPlayerNum, skipPlayerNum_2;
    public PhotonView _PV;
    public TMP_Text _PlayerName, _GameEndingText, _FixText;

    public float fixTextHideTime;
    public int screenWidth, screenHeight, skipPlayer;
    public bool screenAllSizeBool, startGameStoryEnd, endingCoroutine;
    

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        StartSetting();
        startGameStoryEnd = false;
        skipPlayer = 0;
        endingCoroutine = false;
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

    IEnumerator GameStart(Player player)
    {
        _GameEnding.SetActive(true);
        player.gameEnding = true;
        _GameEndingImage.sprite = _GameStartSprite[0];
        yield return StartCoroutine(TextTyping("정다봄 : 어떻게 진정해야 하지?"));
        yield return StartCoroutine(TextTyping("임슬찬 : 여기서 진정시킬 방법은 없는 것 같아"));
        _GameEndingImage.sprite = _GameStartSprite[1];
        yield return StartCoroutine(TextTyping("김혜나 : 으으..."));
        yield return StartCoroutine(TextTyping("강한울 : 어라..?"));
        yield return StartCoroutine(TextTyping("정다봄 : 어.. 괜찮아..?"));
        yield return StartCoroutine(TextTyping("임슬찬 : 혹시 모르니 일단 조심해봐."));
        _GameEndingImage.sprite = _GameStartSprite[2];
        yield return StartCoroutine(TextTyping("김혜나 : 아...아.. 머리가.. 아파.."));
        _GameEndingImage.sprite = _GameStartSprite[3];
        yield return StartCoroutine(TextTyping("김혜나 : 무슨 일이 있었던거지..?"));
        yield return StartCoroutine(TextTyping("임슬찬 : 괜찮아졌다! 다행이다"));
        _GameEndingImage.sprite = _GameStartSprite[4];
        yield return StartCoroutine(TextTyping("정다봄 : 으아아.. 정말 다행이다.."));
        yield return StartCoroutine(TextTyping("강한울 : 진짜 다행이야...정말로.."));
        _GameEndingImage.sprite = _GameStartSprite[5];
        yield return StartCoroutine(TextTyping("김혜나 : 내...내가... 무슨 짓을....한거야..?"));
        yield return StartCoroutine(TextTyping("임슬찬 : 신경쓰지마, 넌 나을 생각만 해"));
        yield return StartCoroutine(TextTyping("강한울 : 괜찮아 괜찮아. 우리가 해결해줄게"));
        yield return StartCoroutine(TextTyping("정다봄 : 일단 우리 자자"));
        yield return StartCoroutine(TextTyping("김혜나 : 어..어...알았어"));
        yield return StartCoroutine(TextTyping("임슬찬 : 자 다 자자!"));
        yield return StartCoroutine(TextTyping("강한울 : 애들아 다 좋은 꿈 꿔!~"));
        _GameEndingImage.sprite = _GameEndingSprite[0];
        yield return StartCoroutine(TextTyping("*다음날*"));
        _GameEndingImage.sprite = _GameStartSprite[6];
        yield return StartCoroutine(TextTyping("정다봄 : 쿨쿨..."));
        yield return StartCoroutine(TextTyping("쾅...쾅..."));
        yield return StartCoroutine(TextTyping("김혜나 : 으으....아....아....."));
        yield return StartCoroutine(TextTyping("정다봄 : 으..으음..."));
        _GameEndingImage.sprite = _GameStartSprite[7];
        yield return StartCoroutine(TextTyping("정다봄 : 으음...무..뭐지...?"));
        yield return StartCoroutine(TextTyping("정다봄 : 무슨 소리야.."));
        _GameEndingImage.sprite = _GameStartSprite[8];
        yield return StartCoroutine(TextTyping("김혜나 : 미친친구:으아아아아아!!!!!!!!!"));
        yield return StartCoroutine(TextTyping("쾅!!쾅!!"));
        yield return StartCoroutine(TextTyping("쾅!!!"));
        yield return StartCoroutine(TextTyping("정다봄 : 허...허헉..."));
        _GameEndingImage.sprite = _GameEndingSprite[0];
        yield return new WaitForSeconds(0.5f);
        _GameEnding.SetActive(false);
        player.gameEnding = false;
    }


    IEnumerator GameEnding()
    {
        endingCoroutine = true;
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
        yield return StartCoroutine(TextTyping("좀만 더 참자...얘들아..."));
        _GameEndingImage.sprite = _GameEndingSprite[3];
        yield return StartCoroutine(TextTyping("저기 앞에 보이는 산..."));
        yield return StartCoroutine(TextTyping("저기 맞지..?"));
        yield return StartCoroutine(TextTyping("..어..! 저기 맞아!"));
        yield return StartCoroutine(TextTyping("GPS로는 분명 저기야...!"));
        _GameEndingImage.sprite = _GameEndingSprite[4];
        yield return StartCoroutine(TextTyping("조금만 버텨 ??아..!"));
        yield return StartCoroutine(TextTyping("곧 도착이야..! 허억...허억.."));
        yield return StartCoroutine(TextTyping("여기 맞아..?"));
        yield return StartCoroutine(TextTyping("너무 길이 위험한데..!헉헉.."));
        yield return StartCoroutine(TextTyping("발밑 조심해..!"));
        _GameEndingImage.sprite = _GameEndingSprite[5];
        yield return StartCoroutine(TextTyping("헉...헉."));
        yield return StartCoroutine(TextTyping("드디어...."));
        yield return StartCoroutine(TextTyping("....."));
        yield return StartCoroutine(TextTyping("....."));
        yield return StartCoroutine(TextTyping("도착했다!"));
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

    public void UIManagerCoroutine(string _CoroutineName, Player player)
    {
        skipPlayerNum.SetActive(false);
        skipPlayerNum_2.SetActive(false);
        switch (_CoroutineName)
        {
            case "GameStart":
                StartCoroutine(GameStart(player));
                break;
            case "GameEnding":
                StartCoroutine(GameEnding());
                break;
            default
                : break;
        }
    }

    [PunRPC]
    public void SkipScene()
    {
        skipPlayer++;

        if(skipPlayer == 1)
        {
            skipPlayerNum.SetActive(true);
        }
        else if(skipPlayer == 2)
        {
            skipPlayerNum_2.SetActive(true);
            StopCoroutine(GameStart(null));
            StopCoroutine(GameEnding());
            _GameEnding.SetActive(false);
            if(endingCoroutine)
            {
                QuitGame();
            }
        }
        else if(skipPlayer == 3)
        {
            skipPlayer = 0;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
