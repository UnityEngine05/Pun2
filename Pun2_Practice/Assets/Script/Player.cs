using UnityEngine;
using Photon.Pun;
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
    public KeyCode key_1, key_2;

    [HideInInspector]
    public float moveX, moveY, moveSpeed, fixSpeed, checkSpeed, fixTimer, checkTimer;
    [HideInInspector]
    public bool foldOut, playerNoMove, fixOnOff;

    public PhotonView _PV;
    public Animator _Animator;
    public ObjectFix _ObjectFixCollision;

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
        OtherPlayerValueinformation();
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_PlayerCharaters);
        }
        else
        {
            _PlayerCharaters = (PlayerCharaters)stream.ReceiveNext();
        }
    }

    void LateUpdate()
    {
        if(_PV.IsMine)
        {
            PlayerMove();
            CameraMove();
            ObjectFix();
        }
    }
    void PlayerMove()
    {
        if (playerNoMove) return;

        moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);


        switch (_PlayerCharaters)
        {
            case PlayerCharaters.������:
                PlayerAnimator(1);
                break;
            case PlayerCharaters.���ٺ�:
                _Animator.SetTrigger("C1");
                PlayerAnimator(1);
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

    void PlayerAnimator(int side)
    {
        if (moveY < 0)
        {
            _Animator.SetBool("Walk", true);
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
    }


    public void OtherPlayerValueinformation()
    {
        switch (_PlayerCharaters)
        {
            case PlayerCharaters.������:
                _PlayerCharaters = PlayerCharaters.������;
                _PlayerTeam = PlayerTeam.�ı���;
                break;
            case PlayerCharaters.���ٺ�:
                _PlayerCharaters = PlayerCharaters.���ٺ�;
                _PlayerTeam = PlayerTeam.������;
                break;
            case PlayerCharaters.�ӽ���:
                _PlayerCharaters = PlayerCharaters.�ӽ���;
                _PlayerTeam = PlayerTeam.������;
                break;
            case PlayerCharaters.���ѿ�:
                _PlayerCharaters = PlayerCharaters.���ѿ�;
                _PlayerTeam = PlayerTeam.������;
                break;
            default:
                break;
        }
    }


    public void PlayerSelect()
    {
        if (!_PV.IsMine) return;

        playerNoMove = false;
        Camera.main.transform.position = transform.position;
        

        switch(_PV.ViewID)
        {
            case 1001:
                GameManager.Instance._UIManager.PlayerImageSetting(0);
                GameManager.Instance.PlayerNameTextSetting("������");
                _PlayerCharaters = PlayerCharaters.������;
                _PlayerTeam = PlayerTeam.�ı���;
                transform.position = GameManager.Instance.spawnPoint[0].transform.position;
                moveSpeed = 6.5f;
                break;
            case 2001:
                GameManager.Instance._UIManager.PlayerImageSetting(1);
                GameManager.Instance.PlayerNameTextSetting("���ٺ�");
                _PlayerCharaters = PlayerCharaters.���ٺ�;
                _PlayerTeam = PlayerTeam.������;
                transform.position = GameManager.Instance.spawnPoint[1].transform.position;
                moveSpeed = 6.5f;
                break;
            case 3001:
                GameManager.Instance._UIManager.PlayerImageSetting(2);
                GameManager.Instance.PlayerNameTextSetting("�ӽ���");
                _PlayerCharaters = PlayerCharaters.�ӽ���;
                _PlayerTeam = PlayerTeam.������;
                transform.position = GameManager.Instance.spawnPoint[2].transform.position;
                moveSpeed = 5;
                break;
            case 4001:
                GameManager.Instance._UIManager.PlayerImageSetting(3);
                GameManager.Instance.PlayerNameTextSetting("���ѿ�");
                _PlayerCharaters = PlayerCharaters.���ѿ�;
                _PlayerTeam = PlayerTeam.������;
                transform.position = GameManager.Instance.spawnPoint[3].transform.position;
                moveSpeed = 5f;
                break;
            default:
                break;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_PV.IsMine) return;

        checkTimer = 0;
        fixOnOff = true;
        ObjectFix _ObjectFix_ = collision.gameObject.GetComponent<ObjectFix>();
        _ObjectFixCollision = _ObjectFix_;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (!_PV.IsMine) return;

        fixOnOff = false;
        _ObjectFixCollision.attack = false;

        if (_PlayerCharaters == PlayerCharaters.������)
        {
            _ObjectFixCollision._HpCanvas.SetActive(false);
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
                }
            }
            else if (Input.GetKey(key_2))
            {
                checkTimer += Time.deltaTime * checkSpeed;
                if (checkTimer >= 2)
                {
                    _ObjectFixCollision._PV.RPC("CheckObject", RpcTarget.AllViaServer);
                }
            }
        }
    }
}
