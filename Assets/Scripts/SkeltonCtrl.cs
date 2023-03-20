using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeltonCtrl : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spRenderer;
    private Rigidbody2D rb2d;
    private GameObject player;

    public float speed = 10;
    public int hp = 3;

    //当たり判定
    private HitChecker sChecker; //横の当たり判定
    private HitChecker gChecker; //地面の当たり判定

    private bool isAttack = false;
    private bool isIdle = false;
    private bool isDead = false;

    public GameObject fxhit;

    //SE
    AudioSource audiosource;
    [SerializeField] AudioClip HitSE;


    void Start()
    {
        player = GameObject.Find("player");
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        sChecker = transform.Find("sideChecker").gameObject.GetComponent<HitChecker>();//横のチェック
        gChecker = transform.Find("groundChecker").gameObject.GetComponent<HitChecker>();//地面のチェック

        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float x = 1;

        if (this.transform.eulerAngles.y == 180)
        {
            x = 1;
        }
        else
        {
            x = -1;
        }

        float velX = rb2d.velocity.x;
        float velY = rb2d.velocity.y;

        //一定速度以上にならないよう調整(Mathf.Abs = 絶対値)
        if( Mathf.Abs(velX) > 4 ){
            if( velX > 4f ){
                rb2d.velocity = new Vector2( 4f , velY );
            }
            if( velX < -4f ){
                rb2d.velocity = new Vector2( -4f , velY );
            }
        }

        CheckValue();

        if(sChecker.isPlayerHit ){

            if( !isAttack ){
                StartCoroutine("Attack");
            }

            anim.SetTrigger("TrgAttack");
            rb2d.velocity = new Vector2(0,0);
        }

        if( !isIdle & !isAttack & !isDead){
            anim.SetBool("isWalk" , true );
            rb2d.AddForce(Vector2.right * x * speed); 
            }

        else{
            anim.SetBool("isWalk" , false );
            rb2d.velocity = new Vector2(0,0);
        }
        
    }

    private void CheckValue(){
        //地面にヒットしていない時、かつ待機状態ではない時
        if( !gChecker.isGroundHit & !isIdle ){
            gChecker.isGroundHit = true;
            StartCoroutine("ChangeRotate");
        }
      
        //敵にヒットしている時、かつ待機状態ではない時
        if( sChecker.isEnemyHit & !isIdle ){
            sChecker.isEnemyHit = false;
            StartCoroutine("ChangeRotate");
        }

        //横判定が地面にヒットしている時、かつ待機状態ではない時
      if( sChecker.isGroundHit & !isIdle ){
          sChecker.isGroundHit = false;
          StartCoroutine("ChangeRotate");
      }
    }
        //ダメージ
      public void OnDamage(int damage)
     {
            hp -= damage;
            audiosource.PlayOneShot(HitSE);
            anim.SetTrigger("isHit");
            if (hp <= 0)
        {
        StartCoroutine("Dead");
        }
     }

    IEnumerator ChangeRotate(){
        isIdle = true;
        
        yield return new WaitForSeconds(2.0f);

        if( this.transform.eulerAngles.y == 180 )
        {
            this.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0,180,0);
        }
        isIdle = false; 
    }

    IEnumerator Attack(){
            isAttack = true;
            anim.SetTrigger("TrgAttack");
            yield return new WaitForSeconds(6.0f);
            isAttack = false;
        }
    IEnumerator Dead(){
            hp = 0;
            isDead = true;
            anim.SetTrigger("TrgDead");
            yield return new WaitForSeconds(1.5f);

            Instantiate(fxhit , transform.position , transform.rotation);

        Destroy( this.gameObject );
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
