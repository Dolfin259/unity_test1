using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("コンティニュー番号")] public int continueNum;
    [Header("音")] public AudioClip se;
    [Header("プレイヤー判定")] public PlayerTriggerCheck trigger;

    public GameObject fxhit;

    private bool on = false;
    private Vector3 defaultPos;

    void Start()
    {
        if(trigger == null)
        {
            Debug.Log("インスペクターの設定が足りません");
            Destroy(this);
        }
        defaultPos = transform.position;
    }

    void Update()
    {
     //プレイヤーが範囲に入った
     if(trigger.isOn && !on)
     {
        GameManager.instance.continueNum = continueNum;
        GameManager.instance.PlaySE(se);
        Instantiate(fxhit , transform.position , transform. rotation);
        on = true;
     }   
    }
}
