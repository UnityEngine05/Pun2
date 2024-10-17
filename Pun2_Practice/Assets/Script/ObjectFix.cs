using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFix : MonoBehaviourPunCallbacks
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

        if (hp <= 0)
        {
            if (!broken)
            {
                GameManager.Instance.objectBrokenNum++;
            }
            broken = true;
            hp = 0;
        }
        else if (hp >= maxHp)
        {
            if (broken)
            {
                GameManager.Instance.objectBrokenNum--;
            }
            broken = false;
        }

        PlayerCheck();
    }

    [PunRPC]
    public void HideObject()
    {
        _HpCanvas.SetActive(false);
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

    [PunRPC]
    public void ObjectHpRefresh()
    {
        check = true;
    }

    [PunRPC]
    public void ObjectHpAttack(float fixSpeed)
    {
        hp -= ( Time.deltaTime * fixSpeed );
        ObjectAttack();
    }

    [PunRPC]
    public void ObjectHpHeal(float fixSpeed)
    {
        hp += ( Time.deltaTime * fixSpeed );
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
