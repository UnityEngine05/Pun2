using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFix : MonoBehaviourPunCallbacks
{
    public float maxHp, hp, timer;
    public bool broken, check, attack, layer_;
    public GameObject _HpCanvas;
    public Image _Hp;
    public PhotonView _PV;
    public SpriteRenderer _SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 10;
        broken = false;
        check = false;
        _HpCanvas.SetActive(false);
        attack = false;
        _SpriteRenderer = this.GetComponent<SpriteRenderer>();

        if (layer_) return;
        this._SpriteRenderer.sortingOrder = -(int)transform.position.y;
        if(transform.position.y == 0) this._SpriteRenderer.sortingOrder = -(int)transform.position.y - 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _Hp.fillAmount = hp / maxHp;

        if (hp <= 0)
        {
            if (!broken)
            {
                GameManager.Instance.objectBrokenNum--;
            }
            broken = true;
            hp = 0;
        }
        else if (hp >= maxHp)
        {
            if (broken)
            {
                GameManager.Instance.objectBrokenNum++;
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
        hp -= Time.deltaTime * fixSpeed;
        ObjectAttack();
    }

    [PunRPC]
    public void ObjectHpHeal(float fixSpeed)
    {
        hp += Time.deltaTime * fixSpeed;
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
