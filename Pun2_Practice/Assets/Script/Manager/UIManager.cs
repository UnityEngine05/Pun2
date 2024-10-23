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
        yield return StartCoroutine(TextTyping("���ٺ� : ��� �����ؾ� ����?"));
        yield return StartCoroutine(TextTyping("�ӽ��� : ���⼭ ������ų ����� ���� �� ����"));
        _GameEndingImage.sprite = _GameStartSprite[1];
        yield return StartCoroutine(TextTyping("������ : ����..."));
        yield return StartCoroutine(TextTyping("���ѿ� : ���..?"));
        yield return StartCoroutine(TextTyping("���ٺ� : ��.. ������..?"));
        yield return StartCoroutine(TextTyping("�ӽ��� : Ȥ�� �𸣴� �ϴ� �����غ�."));
        _GameEndingImage.sprite = _GameStartSprite[2];
        yield return StartCoroutine(TextTyping("������ : ��...��.. �Ӹ���.. ����.."));
        _GameEndingImage.sprite = _GameStartSprite[3];
        yield return StartCoroutine(TextTyping("������ : ���� ���� �־�������..?"));
        yield return StartCoroutine(TextTyping("�ӽ��� : ����������! �����̴�"));
        _GameEndingImage.sprite = _GameStartSprite[4];
        yield return StartCoroutine(TextTyping("���ٺ� : ���ƾ�.. ���� �����̴�.."));
        yield return StartCoroutine(TextTyping("���ѿ� : ��¥ �����̾�...������.."));
        _GameEndingImage.sprite = _GameStartSprite[5];
        yield return StartCoroutine(TextTyping("������ : ��...����... ���� ����....�Ѱž�..?"));
        yield return StartCoroutine(TextTyping("�ӽ��� : �Ű澲����, �� ���� ������ ��"));
        yield return StartCoroutine(TextTyping("���ѿ� : ������ ������. �츮�� �ذ����ٰ�"));
        yield return StartCoroutine(TextTyping("���ٺ� : �ϴ� �츮 ����"));
        yield return StartCoroutine(TextTyping("������ : ��..��...�˾Ҿ�"));
        yield return StartCoroutine(TextTyping("�ӽ��� : �� �� ����!"));
        yield return StartCoroutine(TextTyping("���ѿ� : �ֵ�� �� ���� �� ��!~"));
        _GameEndingImage.sprite = _GameEndingSprite[0];
        yield return StartCoroutine(TextTyping("*������*"));
        _GameEndingImage.sprite = _GameStartSprite[6];
        yield return StartCoroutine(TextTyping("���ٺ� : ����..."));
        yield return StartCoroutine(TextTyping("��...��..."));
        yield return StartCoroutine(TextTyping("������ : ����....��....��....."));
        yield return StartCoroutine(TextTyping("���ٺ� : ��..����..."));
        _GameEndingImage.sprite = _GameStartSprite[7];
        yield return StartCoroutine(TextTyping("���ٺ� : ����...��..����...?"));
        yield return StartCoroutine(TextTyping("���ٺ� : ���� �Ҹ���.."));
        _GameEndingImage.sprite = _GameStartSprite[8];
        yield return StartCoroutine(TextTyping("������ : ��ģģ��:���ƾƾƾƾ�!!!!!!!!!"));
        yield return StartCoroutine(TextTyping("��!!��!!"));
        yield return StartCoroutine(TextTyping("��!!!"));
        yield return StartCoroutine(TextTyping("���ٺ� : ��...����..."));
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
        yield return StartCoroutine(TextTyping("..��..����..."));
        yield return StartCoroutine(TextTyping("�Ͼ�........."));
        _GameEndingImage.sprite = _GameEndingSprite[1];
        yield return StartCoroutine(TextTyping("����......"));
        yield return StartCoroutine(TextTyping("....�󸶳�...�� �����ϴ°ž�..."));
        yield return StartCoroutine(TextTyping("����... ��¥ ����� �װڳ�..."));
        _GameEndingImage.sprite = _GameEndingSprite[2];
        yield return StartCoroutine(TextTyping("���...���⼭...�� ��...."));
        yield return StartCoroutine(TextTyping("�� �ӿ�... �ִ� �� ������...."));
        yield return StartCoroutine(TextTyping("���� �� ����...����..."));
        _GameEndingImage.sprite = _GameEndingSprite[3];
        yield return StartCoroutine(TextTyping("���� �տ� ���̴� ��..."));
        yield return StartCoroutine(TextTyping("���� ����..?"));
        yield return StartCoroutine(TextTyping("..��..! ���� �¾�!"));
        yield return StartCoroutine(TextTyping("GPS�δ� �и� �����...!"));
        _GameEndingImage.sprite = _GameEndingSprite[4];
        yield return StartCoroutine(TextTyping("���ݸ� ���� ??��..!"));
        yield return StartCoroutine(TextTyping("�� �����̾�..! ���...���.."));
        yield return StartCoroutine(TextTyping("���� �¾�..?"));
        yield return StartCoroutine(TextTyping("�ʹ� ���� �����ѵ�..!����.."));
        yield return StartCoroutine(TextTyping("�߹� ������..!"));
        _GameEndingImage.sprite = _GameEndingSprite[5];
        yield return StartCoroutine(TextTyping("��...��."));
        yield return StartCoroutine(TextTyping("����...."));
        yield return StartCoroutine(TextTyping("....."));
        yield return StartCoroutine(TextTyping("....."));
        yield return StartCoroutine(TextTyping("�����ߴ�!"));
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
