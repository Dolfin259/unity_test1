using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitChecker : MonoBehaviour
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
        if (col.gameObject.name == "StageMap")
        {
            isGroundHit = true;
        }
        //プレイヤー判定
        if (col.gameObject.name == "Player")
        {
            isPlayerHit = true;
        }

        //敵判定
        if (col.gameObject.tag == "Enemy")
        {
            isEnemyHit = true;
        }
    }

    void OnTriggerExit2D( Collider2D col ){

        //ステージマップから離れた
        if( col.gameObject.name == "StageMap"){
            isGroundHit = false;
        }
        //プレイヤーから離れた
         if( col.gameObject.name == "Player"){
            isPlayerHit = false;
        }

        //敵から離れた
         if( col.gameObject.tag == "Enemy"){
            isEnemyHit = false;
        }


    }
}
