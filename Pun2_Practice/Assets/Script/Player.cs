using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;

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
            case PlayerCharaters.김혜나:
                break;
            case PlayerCharaters.정다봄:
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
            case PlayerCharaters.임슬찬:
                break;
            case PlayerCharaters.강한울:
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
            _PlayerCharaters = PlayerCharaters.김혜나;
            _PlayerTeam = PlayerTeam.파괴자;
            transform.position = GameManager.Instance.spawnPoint[0].transform.position;
        }
        else if (_PV.ViewID == 2001)
        {
            _PlayerCharaters = PlayerCharaters.정다봄;
            _PlayerTeam = PlayerTeam.생존자;
            transform.position = GameManager.Instance.spawnPoint[1].transform.position;
        }
        else if (_PV.ViewID == 3001)
        {
            _PlayerCharaters = PlayerCharaters.임슬찬;
            _PlayerTeam = PlayerTeam.생존자;
            transform.position = GameManager.Instance.spawnPoint[2].transform.position;
        }
        else if (_PV.ViewID == 4001)
        {
            _PlayerCharaters = PlayerCharaters.강한울;
            _PlayerTeam = PlayerTeam.생존자;
            transform.position = GameManager.Instance.spawnPoint[3].transform.position;
        }
        else
        {

        }
    }
}
