using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFix : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxHp, hp, timer;
    public bool broken, check, attack;
    public GameObject _HpCanvas;
    public Image _Hp;
    public PhotonView _PV;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 10;
        hp = maxHp;
        broken = false;
        check = false;
        _HpCanvas.SetActive(false);
        attack = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _Hp.fillAmount = hp / maxHp;
        if(!attack) _HpCanvas.SetActive(false);

        if (hp <= 0)
        {
            broken = true;
            hp = 0;
        }
        else if (hp > 0)
        {
            broken = false;
        }

        PlayerCheck();
    }

    public void PlayerCheck()
    {
        if (check)
        {
            timer += Time.deltaTime;
            _HpCanvas.SetActive(true);


            if (hp >= maxHp && timer >= 3)
            {
                _HpCanvas.SetActive(false);
                timer = 0;
                check = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hp);
        }
        else
        {
            hp = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void CheckObject()
    {
        check = true;
    }

    public void ObjectAttack()
    {
        _HpCanvas.SetActive(true);
        if (hp > 0)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
    }
}
