using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackTrigger : MonoBehaviour
{
    [SerializeField]
    private BossManager bossManager;

    [SerializeField]
    private BossManager.BossAttackType attackType;

    void OnTriggerEnter2D(Collider2D col)
    {
        //プレイヤー判定
        if(col.gameObject.name == "Player")
        {
            if(bossManager != null)
            {
                Debug.Log("ボス攻撃判定がプレイヤーに触れました:{attackType}");
                bossManager.SetAttackType(attackType);
            }
        }
    }
}
