using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;

public enum PlayerCharaters
{
    ������,
    ���ٺ�,
    �ӽ���,
    ���ѿ�
}
public enum PlayerTeam
{
    �ı���,
    ������
}

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    [HideInInspector]
    public PlayerCharaters _PlayerCharaters;
    [HideInInspector]
    public PlayerTeam _PlayerTeam;
    [HideInInspector]
    public KeyCode key_1, key_2, key_3, key_4;
    [HideInInspector]
    public float moveX, moveY, moveSpeed, playerNoMoveTimer, gameTimer,
        fixSpeed, checkSpeed, coolTime,
        fixTimer, checkTimer, maxCoolTime, skipTimer;
    [HideInInspector]
    public bool foldOut, playerNoMove, fixOnOff, gameEnding, skipPlayerBool;
    [HideInInspector]
    public Vector3 playerPos, playerSize;
    [HideInInspector]
    public Quaternion playerRot;

    public PhotonView _PV;
    public Animator _Animator;
    public ObjectFix _ObjectFixCollision;
    public SpriteRenderer _SpriteRenderer;

    public GameObject _FixCanvas;
    public Image _FixGauge;

    // Start is called before the first frame update
    void Awake()
    {
        if (_PV.IsMine)
        {
            _ObjectFixCollision = null;
            _SpriteRenderer = this.GetComponent<SpriteRenderer>();
        }
    }
    void Start()
    {
        gameEnding = true;
        skipPlayerBool = false;
        skipTimer = 0;
        PlayerSelect();
        gameTimer = 600;
        GameManager.Instance._SoundManager.BGMSoundPlay(1);
        if(_PV.IsMine)
        {
            GameManager.Instance._UIManager.UIManagerCoroutine("GameStart", this.GetComponent<Player>());
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.localScale);
            stream.SendNext(_PlayerCharaters);
            stream.SendNext(_PlayerTeam);
        }
        else
        {
            playerPos = (Vector3)stream.ReceiveNext();
            playerRot = (Quaternion)stream.ReceiveNext();
            playerSize = (Vector3)stream.ReceiveNext();
            _PlayerCharaters = (PlayerCharaters)stream.ReceiveNext();
            _PlayerTeam = (PlayerTeam)stream.ReceiveNext();
        }
    }

    void LateUpdate()
    {
        if(_PV.IsMine)
        {
            if (gameEnding && !skipPlayerBool)
            {
                if (skipTimer >= 3)
                {
                    skipPlayerBool = true;
                    GameManager.Instance._UIManager._PV.RPC("SkipScene", RpcTarget.AllViaServer, null);
                }
            }

            if(Input.GetKey(key_4))
            {
                skipTimer += Time.deltaTime;
            }
            if(Input.GetKeyUp(key_4))
            {
                skipTimer = 0;
            }
            GameManager.Instance._UIManager._SkipImage.fillAmount = skipTimer / 3;
            if(GameManager.Instance._UIManager.skipPlayer == 2)
            {
                gameEnding = false;
                playerNoMove = false;
                GameManager.Instance._UIManager._PV.RPC("SkipScene", RpcTarget.AllViaServer, null);
            }
        }
        

        if (gameEnding) return;

        if (_PV.IsMine)
        {
            PlayerMove();
            CameraMove();
            ObjectFix();
            CoolTimeImageTimer();

            if (playerNoMove)
            {
                playerNoMoveTimer += Time.deltaTime;
                if (playerNoMoveTimer >= 3)
                {
                    playerNoMove = false;
                    playerNoMoveTimer = 0;
                }
            }

            if(GameManager.Instance.objectBrokenNum >= 2)
            {
                GameManager.Instance._UIManager.UIManagerCoroutine("GameEnding", null);
                gameEnding = true;
                skipTimer = 0;
                skipPlayerBool = false;
            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, playerPos, 10 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, playerRot, 10 * Time.deltaTime);
            transform.localScale = playerSize;
        }
    }
    void PlayerMove()
    {
        if (playerNoMove) return;


        this._SpriteRenderer.sortingOrder = -(int)transform.position.y;

        moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);

        if (moveX != 0 || moveY != 0)
        {
            if(!GameManager.Instance._SoundManager._EffectAudioSource.isPlaying)
            {
                GameManager.Instance._SoundManager.EffectSoundPlayStop(true);
            }
        }
        else
        {
            GameManager.Instance._SoundManager.EffectSoundPlayStop(false);
        }

        switch (_PlayerCharaters)
        {
            case PlayerCharaters.������:
                PlayerAnimator(1);
                break;
            case PlayerCharaters.���ٺ�:
                _Animator.SetTrigger("C1");
                PlayerAnimator(1);
                if (fixOnOff && Input.GetKeyDown(key_3) && coolTime <= 0)
                {
                    fixSpeed = 1.5f;
                    coolTime = maxCoolTime;
                    GameManager.Instance._SoundManager.EffectSoundPlay(8);
                }
                break;
            case PlayerCharaters.�ӽ���:
                _Animator.SetTrigger("C2");
                PlayerAnimator(1);
                break;
            case PlayerCharaters.���ѿ�:
                _Animator.SetTrigger("C3");
                PlayerAnimator(-1);
                break;
            default:
                break;
        }
    }

    void CameraMove()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                new Vector3(transform.position.x, transform.position.y, -10), Time.deltaTime * 3);
    }

    void CoolTimeImageTimer()
    {
        GameManager.Instance._UIManager._PlayerCoolTime.fillAmount = coolTime / maxCoolTime;
        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }
    }

    void PlayerAnimator(int side)
    {
        if (moveY < 0)
        {
            _Animator.SetBool("Walk", true);
            _Animator.SetBool("BackWalk", false);
            if (moveX > 0)
            {
                RWalkAnimator(1 * side);
            }
            else if(moveX < 0)
            {
                RWalkAnimator(-1 * side);
            }
            else
            {
                _Animator.SetBool("SideWalk", false);
            }
        }
        else if (moveY > 0)
        {
            _Animator.SetBool("BackWalk", true);
            _Animator.SetBool("Walk", false);
            if (moveX < 0)
            {
                RWalkAnimator(-1 * side);
            }
            else if (moveX > 0)
            {
                RWalkAnimator(1 * side);
            }
            else
            {
                _Animator.SetBool("SideWalk", false);
            }
        }
        else if (moveX > 0)
        {
            RWalkAnimator(1 * side);
        }
        else if (moveX < 0)
        {
            RWalkAnimator(-1 * side);
        }
        else
        {
            _Animator.SetBool("Walk", false);
            _Animator.SetBool("BackWalk", false);
            _Animator.SetBool("SideWalk", false);
        }
    }

    void RWalkAnimator(int scale)
    {
        _Animator.SetBool("SideWalk", true);
        transform.localScale = new Vector3(scale, 1, 1);
        if(scale < 0)
        {
            _FixGauge.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            _FixGauge.rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void PlayerSelect()
    {
        if (!_PV.IsMine) return;

        playerNoMove = false;
        Camera.main.transform.position = transform.position;
        playerNoMoveTimer = 0;
        _FixCanvas.SetActive(false);


        switch (_PV.ViewID)
        {
            case 1001:
                GameManager.Instance._UIManager.PlayerImageSetting(0);
                GameManager.Instance.PlayerNameTextSetting("������");
                _PlayerCharaters = PlayerCharaters.������;
                _PlayerTeam = PlayerTeam.�ı���;
                transform.position = GameManager.Instance.spawnPoint[0].transform.position;
                moveSpeed = 6.5f;
                fixSpeed = 1;
                maxCoolTime = 0;
                GameManager.Instance._UIManager._Skill_Image.enabled = false;
                break;
            case 2001:
                GameManager.Instance._UIManager.PlayerImageSetting(1);
                GameManager.Instance.PlayerNameTextSetting("���ٺ�");
                _PlayerCharaters = PlayerCharaters.���ٺ�;
                _PlayerTeam = PlayerTeam.������;
                transform.position = GameManager.Instance.spawnPoint[1].transform.position;
                moveSpeed = 5.5f;
                fixSpeed = 1.2f;
                maxCoolTime = 10;
                GameManager.Instance._UIManager.SkillIconSetting(0);
                break;
            case 3001:
                GameManager.Instance._UIManager.PlayerImageSetting(2);
                GameManager.Instance.PlayerNameTextSetting("�ӽ���");
                _PlayerCharaters = PlayerCharaters.�ӽ���;
                _PlayerTeam = PlayerTeam.������;
                transform.position = GameManager.Instance.spawnPoint[2].transform.position;
                moveSpeed = 5;
                fixSpeed = 1;
                maxCoolTime = 40;
                GameManager.Instance._UIManager.SkillIconSetting(1);
                break;
            case 4001:
                GameManager.Instance._UIManager.PlayerImageSetting(3);
                GameManager.Instance.PlayerNameTextSetting("���ѿ�");
                _PlayerCharaters = PlayerCharaters.���ѿ�;
                _PlayerTeam = PlayerTeam.������;
                transform.position = GameManager.Instance.spawnPoint[3].transform.position;
                moveSpeed = 5f;
                fixSpeed = 1;
                maxCoolTime = 30;
                GameManager.Instance._UIManager.SkillIconSetting(2);
                break;
            default:
                break;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_PV.IsMine) return;


        if (collision.gameObject.CompareTag("Object"))
        {
            checkTimer = 0;
            fixOnOff = true;
            ObjectFix _ObjectFix_ = collision.gameObject.GetComponent<ObjectFix>();
            _ObjectFixCollision = _ObjectFix_;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player _CrazyPlayer = collision.gameObject.GetComponent<Player>();
            if (_CrazyPlayer._PlayerTeam == PlayerTeam.�ı���)
            {
                playerNoMove = true;
            }
        }

        if(collision.gameObject.CompareTag("Door"))
        {
            switch (collision.gameObject.name)
            {
                case "ȯ��ǹ�":
                    transform.position = new Vector3(0.15f, 18, 0);
                    break;
                case "�Žǹ�":
                    transform.position = new Vector3(0.5f, 2.25f, 0);
                    break;
                case "�Žǹ�_2":
                    transform.position = new Vector3(-8.25f, -3.5f, 0);
                    break;
                case "�ξ���":
                    transform.position = new Vector3(-31, 0, 0);
                    break;
                default
                    : break;
            }
            GameManager.Instance._SoundManager.EffectSoundPlay(1);
            Camera.main.transform.position = transform.position;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!_PV.IsMine) return;

        _FixCanvas.SetActive(false);
        if (collision.gameObject.CompareTag("Object"))
        {
            fixOnOff = false;
            _ObjectFixCollision.attack = false;
        }
            

        if (_PlayerCharaters == PlayerCharaters.������ && collision.gameObject.CompareTag("Object"))
        {
            _ObjectFixCollision._HpCanvas.SetActive(false);
        }
        else if (_PlayerCharaters == PlayerCharaters.���ٺ�)
        {
            fixSpeed = 1.2f;
        }
    }
    public void ObjectFix()
    {
        if (!fixOnOff) return;

        if (_PlayerCharaters == PlayerCharaters.������)
        {
            if (Input.GetKey(key_1))
            {
                _ObjectFixCollision._PV.RPC("ObjectHpAttack", RpcTarget.AllViaServer, fixSpeed);
                if(_ObjectFixCollision.hp <= 0)
                {
                    GameManager.Instance._SoundManager.EffectSoundPlay(3);
                    _ObjectFixCollision._PV.RPC("HideObject", RpcTarget.AllViaServer, null);
                    GameManager.Instance._UIManager.FixTextFunction("����! ����!");
                }
            }
            else if (Input.GetKey(key_2))
            {
                
            }
        }
        else if (_PlayerCharaters == PlayerCharaters.���ٺ� ||
            _PlayerCharaters == PlayerCharaters.�ӽ��� || _PlayerCharaters == PlayerCharaters.���ѿ�)
        {
            if (Input.GetKey(key_1))
            {
                if(_ObjectFixCollision.check && _ObjectFixCollision.hp < _ObjectFixCollision.maxHp)
                {
                    _ObjectFixCollision._PV.RPC("ObjectHpHeal", RpcTarget.AllViaServer, fixSpeed);
                    if (_ObjectFixCollision.hp >= _ObjectFixCollision.maxHp)
                    {
                        GameManager.Instance._SoundManager.EffectSoundPlay(2);
                        GameManager.Instance._UIManager.FixTextFunction("���� �Ϸ�");
                    }
                }
            }
            else if (Input.GetKey(key_2))
            {
                _FixCanvas.SetActive(true);
                checkTimer += Time.deltaTime * checkSpeed;
                _FixGauge.fillAmount = checkTimer / 2;
                if (checkTimer >= 2)
                {
                    _ObjectFixCollision._PV.RPC("ObjectHpRefresh", RpcTarget.AllViaServer);
                    _FixCanvas.SetActive(false);
                    if (_ObjectFixCollision.broken)
                    {
                        GameManager.Instance._UIManager.FixTextFunction("������ �ʿ��ϴ�.");
                    }
                    else
                    {
                        GameManager.Instance._UIManager.FixTextFunction("������ �ʿ����� �ʴ�.");
                    }
                }
            }
        }
    }
}
