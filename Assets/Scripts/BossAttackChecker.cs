using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackChecker : MonoBehaviour
{
    public bool isGroundHit;

    public bool isPlayerHit;

    public bool isEnemyHit;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D col)//接触した
    {
        //地面判定
        if (col.gameObject.name == "Ground")
        {
            isGroundHit = true;
        }
        //プレイヤー判定
        if (col.gameObject.name == "Player")
        {
            isPlayerHit = true;
        }
    }

    void OnTriggerExit2D( Collider2D col )
    {

        //地面から離れた
        if( col.gameObject.name == "Ground"){
            isGroundHit = false;
        }
        //プレイヤーから離れた
         if( col.gameObject.name == "Player"){
            isPlayerHit = false;
        }

    }
}
