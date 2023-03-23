using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator anim;

    public float speed = 3;
    public int jumpForce = 50;
    public int hp = 10;
    public GameObject ShockWavePrefab;
    public Transform ShotPoint; 

    Transform player;
    public LayerMask GroundLayer;

    private bool isDead;
    

    //当たり判定
    private HitChecker sChecker; //遠距離攻撃の当たり判定
    private HitChecker bChecker; //遠距離攻撃の当たり判定

    public GameObject fxhit;
    
    //SE
    AudioSource audiosource;
    [SerializeField] AudioClip AttackSE;
    [SerializeField] AudioClip DamageSE;
    [SerializeField] AudioClip DeadSE;
    [SerializeField] AudioClip JumpSE;
   

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        sChecker = transform.Find("ShockWaveChecker").gameObject.GetComponent<HitChecker>();
        bChecker = transform.Find("BladeChecker").gameObject.GetComponent<HitChecker>();
        audiosource = GetComponent<AudioSource>();   
    }

    public enum BossAttackType
    {
        None,
        ShockWave,
        Blade,
    };

    private BossAttackType currentAttackType = BossAttackType.None;

    void Attack1()//遠距離攻撃
    {
        anim.SetTrigger("isAttack1");
        Instantiate(ShockWavePrefab,ShotPoint.position,transform.rotation);
    }

    void Attack2()//近距離攻撃
    {
        anim.SetTrigger("isAttack2");
    }

    //実行する攻撃のタイプを設定
    public void SetAttackType(BossAttackType attackType)
    {
        currentAttackType = attackType;
    }

    void Update()
    {
        float x = rb2d.velocity.x;
        ChangeRotate();

        switch(currentAttackType)
        {
            case BossAttackType.None:
            //何もしない
            break;

            case BossAttackType.ShockWave:
            //遠距離攻撃
            Invoke("Attack1",1);
            //攻撃が済んだので何もしない状態に戻る
            currentAttackType = BossAttackType.None;
            break;

            case BossAttackType.Blade:
            //近距離攻撃
            Invoke("Attack2",1);
            //攻撃が済んだので何もしない状態に戻る
            currentAttackType = BossAttackType.None;
            break;
        }
    }
        
    void Movement()//移動処理
       {
        anim.SetBool("Run",true);
        Vector2 target = new Vector2(player.position.x, rb2d.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb2d.position,target,speed * Time.fixedDeltaTime);
        rb2d.MovePosition(newPos);
       }

    void ChangeRotate()//スプライトの向きを変える
    {
        Vector3 localEulerAngles = transform.localEulerAngles;

        if(player.transform.position.x > this.transform.position.x)
        {
            localEulerAngles.y = 180;
        }    
        else
        {
            localEulerAngles.y = 0;
        } 
        transform.localEulerAngles = localEulerAngles;
    }

    void FixedUpdate()
    {

        Movement();

        float velX = rb2d.velocity.x;
        float velY = rb2d.velocity.y;

        //一定速度以上にならないよう調整(Mathf.Abs = 絶対値)
        if( Mathf.Abs(velX) > 4 )
        {
            if( velX > 4f ){
                rb2d.velocity = new Vector2( 4f , velY );
            }
            if( velX < -4f ){
                rb2d.velocity = new Vector2( -4f , velY );
            }
        }
    }
}

