using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;

public enum PlayerCharaters
{
    ±èÇý³ª,
    Á¤´Ùº½,
    ÀÓ½½Âù,
    °­ÇÑ¿ï
}
public enum PlayerTeam
{
    ÆÄ±«ÀÚ,
    »ýÁ¸ÀÚ
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
    public float moveX, moveY, moveSpeed, playerNoMoveTimer, stunSecond, gameTimer,
        fixSpeed, checkSpeed, coolTime,
        fixTimer, checkTimer, maxCoolTime;
    [HideInInspector]
    public bool foldOut, playerNoMove, fixOnOff;
    public Vector3 playerPos, playerSize;
    public Quaternion playerRot;

    public PhotonView _PV;
    public Animator _Animator;
    public ObjectFix _ObjectFixCollision;

    public GameObject _FixCanvas;
    public Image _FixGauge;

    // Start is called before the first frame update
    void Awake()
    {
        if (_PV.IsMine)
        {
            _ObjectFixCollision = null;
        }
    }
    void Start()
    {
        PlayerSelect();
        gameTimer = 600;
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
            GameTimerSeconds();
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


            if (stunSecond > 0)
            {
                stunSecond -= Time.deltaTime;
            }

            if(GameManager.Instance.objectBrokenNum >= 4)
            {
                GameEndingFunction("Á¤½Å³ª°£ Ä£±¸\n½Â¸®!");
            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, playerPos, 10 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, playerRot, 10 * Time.deltaTime);
            transform.localScale = playerSize;
        }
    }

    public void GameTimerSeconds()
    {
        gameTimer -= Time.deltaTime;
        GameManager.Instance._UIManager.GameTimer(gameTimer);
        if (gameTimer <= 0)
        {
            GameEndingFunction("°íÄ¡´Â Ä£±¸\n½Â¸®!");
        }
    }


    public void GameEndingFunction(string message)
    {
        playerNoMove = true;
        GameManager.Instance._UIManager._GameEnding.SetActive(true);
        GameManager.Instance._UIManager._GameEndingText.text = message;
    }
    void PlayerMove()
    {
        if (playerNoMove) return;

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
            case PlayerCharaters.±èÇý³ª:
                PlayerAnimator(1);
                break;
            case PlayerCharaters.Á¤´Ùº½:
                _Animator.SetTrigger("C1");
                PlayerAnimator(1);
                if (fixOnOff && Input.GetKeyDown(key_3) && coolTime <= 0)
                {
                    fixSpeed = 1.5f;
                    coolTime = maxCoolTime;
                }
                break;
            case PlayerCharaters.ÀÓ½½Âù:
                _Animator.SetTrigger("C2");
                PlayerAnimator(1);
                break;
            case PlayerCharaters.°­ÇÑ¿ï:
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
                GameManager.Instance.PlayerNameTextSetting("±èÇý³ª");
                _PlayerCharaters = PlayerCharaters.±èÇý³ª;
                _PlayerTeam = PlayerTeam.ÆÄ±«ÀÚ;
                transform.position = GameManager.Instance.spawnPoint[0].transform.position;
                moveSpeed = 6.5f;
                fixSpeed = 1;
                maxCoolTime = 0;
                break;
            case 2001:
                GameManager.Instance._UIManager.PlayerImageSetting(1);
                GameManager.Instance.PlayerNameTextSetting("Á¤´Ùº½");
                _PlayerCharaters = PlayerCharaters.Á¤´Ùº½;
                _PlayerTeam = PlayerTeam.»ýÁ¸ÀÚ;
                transform.position = GameManager.Instance.spawnPoint[1].transform.position;
                moveSpeed = 5.5f;
                fixSpeed = 1.2f;
                maxCoolTime = 30;
                break;
            case 3001:
                GameManager.Instance._UIManager.PlayerImageSetting(2);
                GameManager.Instance.PlayerNameTextSetting("ÀÓ½½Âù");
                _PlayerCharaters = PlayerCharaters.ÀÓ½½Âù;
                _PlayerTeam = PlayerTeam.»ýÁ¸ÀÚ;
                transform.position = GameManager.Instance.spawnPoint[2].transform.position;
                moveSpeed = 5;
                fixSpeed = 1;
                maxCoolTime = 40;
                break;
            case 4001:
                GameManager.Instance._UIManager.PlayerImageSetting(3);
                GameManager.Instance.PlayerNameTextSetting("°­ÇÑ¿ï");
                _PlayerCharaters = PlayerCharaters.°­ÇÑ¿ï;
                _PlayerTeam = PlayerTeam.»ýÁ¸ÀÚ;
                transform.position = GameManager.Instance.spawnPoint[3].transform.position;
                moveSpeed = 5f;
                fixSpeed = 1;
                maxCoolTime = 30;
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

        if(_PlayerCharaters == PlayerCharaters.°­ÇÑ¿ï)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                _PV.RPC("PlayerStun", RpcTarget.AllViaServer, collision.gameObject.GetComponent<Player>());
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player _CrazyPlayer = collision.gameObject.GetComponent<Player>();
            if (_PlayerCharaters == PlayerCharaters.°­ÇÑ¿ï && coolTime <= 0)
            {
                if (_CrazyPlayer._PlayerTeam == PlayerTeam.ÆÄ±«ÀÚ)
                {
                    coolTime = maxCoolTime;
                }
                else
                {

                }
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                if (_CrazyPlayer._PlayerTeam == PlayerTeam.ÆÄ±«ÀÚ)
                {
                    playerNoMove = true;
                }
            }
        }
    }

    [PunRPC]
    public void PlayerStun(Player player)
    {
        if (_PlayerTeam == PlayerTeam.ÆÄ±«ÀÚ && stunSecond <= 0 && player._PlayerCharaters == PlayerCharaters.°­ÇÑ¿ï)
        {
            playerNoMove = true;
            stunSecond = 30;
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
            

        if (_PlayerCharaters == PlayerCharaters.±èÇý³ª && collision.gameObject.CompareTag("Object"))
        {
            _ObjectFixCollision._HpCanvas.SetActive(false);
        }
        else if (_PlayerCharaters == PlayerCharaters.Á¤´Ùº½)
        {
            fixSpeed = 1.2f;
        }
    }
    public void ObjectFix()
    {
        if (!fixOnOff) return;

        if (_PlayerCharaters == PlayerCharaters.±èÇý³ª)
        {
            if (Input.GetKey(key_1))
            {
                _ObjectFixCollision._PV.RPC("ObjectHpAttack", RpcTarget.AllViaServer, fixSpeed);
                if(_ObjectFixCollision.hp <= 0)
                {
                    GameManager.Instance._SoundManager.EffectSoundPlay(3);
                    _ObjectFixCollision._PV.RPC("HideObject", RpcTarget.AllViaServer, null);
                }
            }
            else if (Input.GetKey(key_2))
            {
                
            }
        }
        else if (_PlayerCharaters == PlayerCharaters.Á¤´Ùº½ ||
            _PlayerCharaters == PlayerCharaters.ÀÓ½½Âù || _PlayerCharaters == PlayerCharaters.°­ÇÑ¿ï)
        {
            if (Input.GetKey(key_1))
            {
                if(_ObjectFixCollision.check && _ObjectFixCollision.hp < _ObjectFixCollision.maxHp)
                {
                    _ObjectFixCollision._PV.RPC("ObjectHpHeal", RpcTarget.AllViaServer, fixSpeed);
                    if (_ObjectFixCollision.hp >= _ObjectFixCollision.maxHp)
                    {
                        GameManager.Instance._SoundManager.EffectSoundPlay(2);
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
                }
            }
        }
    }
}
