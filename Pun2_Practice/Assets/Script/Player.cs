using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;

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
public class Player : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public PlayerCharaters _PlayerCharaters;
    [HideInInspector]
    public PlayerTeam _PlayerTeam;

    [HideInInspector]
    public float moveX, moveY, moveSpeed;
    [HideInInspector]
    public bool foldOut, playerNoMove;

    public PhotonView _PV;
    public Animator _Animator;

    // Start is called before the first frame update
    void Start()
    {
        playerNoMove = false;
        PlayerSelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if(_PV.IsMine)
        {
            PlayerMove();

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                new Vector3(transform.position.x, transform.position.y, -10), Time.deltaTime * 2);
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
                break;
            case PlayerCharaters.���ٺ�:
                _Animator.SetTrigger("C1");
                if(moveY < 0)
                {
                    _Animator.SetBool("Walk", true);
                }
                else if (moveY > 0)
                {
                    _Animator.SetBool("BackWalk", true);
                }
                else
                {
                    _Animator.SetBool("Walk", false);
                    _Animator.SetBool("BackWalk", false);
                }
                break;
            case PlayerCharaters.�ӽ���:
                break;
            case PlayerCharaters.���ѿ�:
                break;
            default:
                break;
        }
    }



    public void PlayerSelect()
    {
        if (!_PV.IsMine) return;

        Camera.main.transform.position = transform.position;

        if (_PV.ViewID == 1001)
        {
            _PlayerCharaters = PlayerCharaters.������;
            _PlayerTeam = PlayerTeam.�ı���;
            transform.position = GameManager.Instance.spawnPoint[0].transform.position;
        }
        else if (_PV.ViewID == 2001)
        {
            _PlayerCharaters = PlayerCharaters.���ٺ�;
            _PlayerTeam = PlayerTeam.������;
            transform.position = GameManager.Instance.spawnPoint[1].transform.position;
        }
        else if (_PV.ViewID == 3001)
        {
            _PlayerCharaters = PlayerCharaters.�ӽ���;
            _PlayerTeam = PlayerTeam.������;
            transform.position = GameManager.Instance.spawnPoint[2].transform.position;
        }
        else if (_PV.ViewID == 4001)
        {
            _PlayerCharaters = PlayerCharaters.���ѿ�;
            _PlayerTeam = PlayerTeam.������;
            transform.position = GameManager.Instance.spawnPoint[3].transform.position;
        }
        else
        {

        }
    }
}
