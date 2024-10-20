using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;

public enum PlayerCharaters
{
    김혜나,
    정다봄,
    임슬찬,
    강한울
}
public enum PlayerTeam
{
    파괴자,
    생존자
}

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    [HideInInspector]
    public PlayerCharaters _PlayerCharaters;
    [HideInInspector]
    public PlayerTeam _PlayerTeam;
    [HideInInspector]
    public KeyCode key_1, key_2, key_3;
    [HideInInspector]
    public float moveX, moveY, moveSpeed, playerNoMoveTimer, gameTimer,
        fixSpeed, checkSpeed, coolTime,
        fixTimer, checkTimer, maxCoolTime;
    [HideInInspector]
    public bool foldOut, playerNoMove, fixOnOff, gameEnding;
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
        gameEnding = false;
        PlayerSelect();
        gameTimer = 600;
        GameManager.Instance._SoundManager.BGMSoundPlay(1);
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
                GameEndingFunction();
            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, playerPos, 10 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, playerRot, 10 * Time.deltaTime);
            transform.localScale = playerSize;
        }
    }


    public void GameEndingFunction()
    {
        GameManager.Instance._UIManager.UIManagerCoroutine();
        gameEnding = true;
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
            case PlayerCharaters.김혜나:
                PlayerAnimator(1);
                break;
            case PlayerCharaters.정다봄:
                _Animator.SetTrigger("C1");
                PlayerAnimator(1);
                if (fixOnOff && Input.GetKeyDown(key_3) && coolTime <= 0)
                {
                    fixSpeed = 1.5f;
                    coolTime = maxCoolTime;
                    GameManager.Instance._SoundManager.EffectSoundPlay(8);
                }
                break;
            case PlayerCharaters.임슬찬:
                _Animator.SetTrigger("C2");
                PlayerAnimator(1);
                break;
            case PlayerCharaters.강한울:
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
                GameManager.Instance.PlayerNameTextSetting("김혜나");
                _PlayerCharaters = PlayerCharaters.김혜나;
                _PlayerTeam = PlayerTeam.파괴자;
                transform.position = GameManager.Instance.spawnPoint[0].transform.position;
                moveSpeed = 6.5f;
                fixSpeed = 1;
                maxCoolTime = 0;
                GameManager.Instance._UIManager._Skill_Image.enabled = false;
                break;
            case 2001:
                GameManager.Instance._UIManager.PlayerImageSetting(1);
                GameManager.Instance.PlayerNameTextSetting("정다봄");
                _PlayerCharaters = PlayerCharaters.정다봄;
                _PlayerTeam = PlayerTeam.생존자;
                transform.position = GameManager.Instance.spawnPoint[1].transform.position;
                moveSpeed = 5.5f;
                fixSpeed = 1.2f;
                maxCoolTime = 10;
                GameManager.Instance._UIManager.SkillIconSetting(0);
                break;
            case 3001:
                GameManager.Instance._UIManager.PlayerImageSetting(2);
                GameManager.Instance.PlayerNameTextSetting("임슬찬");
                _PlayerCharaters = PlayerCharaters.임슬찬;
                _PlayerTeam = PlayerTeam.생존자;
                transform.position = GameManager.Instance.spawnPoint[2].transform.position;
                moveSpeed = 5;
                fixSpeed = 1;
                maxCoolTime = 40;
                GameManager.Instance._UIManager.SkillIconSetting(1);
                break;
            case 4001:
                GameManager.Instance._UIManager.PlayerImageSetting(3);
                GameManager.Instance.PlayerNameTextSetting("강한울");
                _PlayerCharaters = PlayerCharaters.강한울;
                _PlayerTeam = PlayerTeam.생존자;
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
            if (_CrazyPlayer._PlayerTeam == PlayerTeam.파괴자)
            {
                playerNoMove = true;
            }
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
            

        if (_PlayerCharaters == PlayerCharaters.김혜나 && collision.gameObject.CompareTag("Object"))
        {
            _ObjectFixCollision._HpCanvas.SetActive(false);
        }
        else if (_PlayerCharaters == PlayerCharaters.정다봄)
        {
            fixSpeed = 1.2f;
        }
    }
    public void ObjectFix()
    {
        if (!fixOnOff) return;

        if (_PlayerCharaters == PlayerCharaters.김혜나)
        {
            if (Input.GetKey(key_1))
            {
                _ObjectFixCollision._PV.RPC("ObjectHpAttack", RpcTarget.AllViaServer, fixSpeed);
                if(_ObjectFixCollision.hp <= 0)
                {
                    GameManager.Instance._SoundManager.EffectSoundPlay(3);
                    _ObjectFixCollision._PV.RPC("HideObject", RpcTarget.AllViaServer, null);
                    GameManager.Instance._UIManager.FixTextFunction("고장! 고장!");
                }
            }
            else if (Input.GetKey(key_2))
            {
                
            }
        }
        else if (_PlayerCharaters == PlayerCharaters.정다봄 ||
            _PlayerCharaters == PlayerCharaters.임슬찬 || _PlayerCharaters == PlayerCharaters.강한울)
        {
            if (Input.GetKey(key_1))
            {
                if(_ObjectFixCollision.check && _ObjectFixCollision.hp < _ObjectFixCollision.maxHp)
                {
                    _ObjectFixCollision._PV.RPC("ObjectHpHeal", RpcTarget.AllViaServer, fixSpeed);
                    if (_ObjectFixCollision.hp >= _ObjectFixCollision.maxHp)
                    {
                        GameManager.Instance._SoundManager.EffectSoundPlay(2);
                        GameManager.Instance._UIManager.FixTextFunction("수리 완료");
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
                        GameManager.Instance._UIManager.FixTextFunction("수리가 필요하다.");
                    }
                    else
                    {
                        GameManager.Instance._UIManager.FixTextFunction("수리가 필요하지 않다.");
                    }
                }
            }
        }
    }
}
