using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float speed = 25f;
    public float jumpForce = 750f;
    public int at = 1; //攻撃力
    private const int Maxhp = 5;
    public int hp = Maxhp; //HP

    private GameObject enemy;

    public Transform attackpoint;
    public float attackRadius;

    public LayerMask enemyLayer;
    public LayerMask groundLayer;

    private Animator anim;

    private SpriteRenderer spRenderer;

    private bool isGround;

    private bool isDead = false;
    private bool isContinue = false;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    
    private float ControlLostTime; //制御不能になる時間

    public GameObject fxhit;

    //SE
    AudioSource audiosource;
    [SerializeField] AudioClip AttackSE;
    [SerializeField] AudioClip DamageSE;
    [SerializeField] AudioClip DeadSE;
    [SerializeField] AudioClip JumpSE;
   
    void Start()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.spRenderer = GetComponent<SpriteRenderer>();
        enemy = GameObject.FindWithTag("Enemy");
        ControlLostTime = 0f;
        audiosource = GetComponent<AudioSource>();
        anim.keepAnimatorControllerStateOnDisable = true;
    }
    
    void Update()
    {

        if(isDead)
        {
            return;
        }

        bool ControlTime = ControlLostTime <= 0f; //制御不能時間の時、横移動できない
        if(ControlTime == false)
        {
            float speed = 0f;
        }
            float x = Input.GetAxisRaw("Horizontal"); //キー入力で水平方向移動
            anim.SetFloat("Speed",Mathf.Abs(x * speed)); //キー入力があるときだけアニメーション再生

        if(Input.GetButtonDown("Jump") & isGround & ControlTime)
        {
            anim.SetBool("isJump",true); //ジャンプアニメーションon
            rb2d.AddForce(Vector2.up * jumpForce);
            audiosource.PlayOneShot(JumpSE);
        }

        if( isGround ){ //地面にいる時はジャンプアニメーションoff

            anim.SetBool("isJump",false);
            anim.SetBool("isFall",false);
        }

        float velX = rb2d.velocity.x;
        float velY = rb2d.velocity.y;

        if( velY > 0.5f )
        { //velocityが上向きに働いていたらジャンプ中
            anim.SetBool("isJump",true);
        }

        if( velY < -0.1f )
        { //velocityが下向きに働いていたら落下中
            anim.SetBool("isFall",true);
        }

        //一定速度以上にならないよう調整(Mathf.Abs = 絶対値)
        if( Mathf.Abs(velX) > 5 )
        {
            if( velX > 5.0f )
            {
                rb2d.velocity = new Vector2( 5.0f , velY );
            }
            if( velX < -5.0f )
            {
                rb2d.velocity = new Vector2( -5.0f , velY );
            }
        }

        if(ControlTime)//制御可能な時のみ速度を加える
        {
        rb2d.AddForce(Vector2.right * x * speed);
        }

       //スプライトの向きを変える
        Vector3 localEulerAngles = transform.localEulerAngles;
        if(x < 0)
        {
            localEulerAngles.y = 180;
        }
        else if(x > 0)
        {
            localEulerAngles.y = 0;
        } 
         transform.localEulerAngles = localEulerAngles;

        void OnDrawGizmosSelected() //攻撃範囲
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackpoint.position, attackRadius);
        }

        //攻撃
        void Attack()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackpoint.position, attackRadius, enemyLayer);

            foreach (Collider2D hitEnemy in hitEnemys)
            {
                Debug.Log(hitEnemy.gameObject.name+"に攻撃");

                var skeltonCtrl = hitEnemy.GetComponent<SkeltonCtrl>();
                if (skeltonCtrl != null)
                {
                    skeltonCtrl.OnDamage(at);
                }
                else
                {
                var bossCtrl = hitEnemy.GetComponent<BossManager>();
                    if (bossCtrl != null)
                    {
                        bossCtrl.OnDamage(at);
                    }
                }
            }
            
            anim.SetTrigger("isAttack");
            audiosource.PlayOneShot(AttackSE);
        }

            if(Input.GetButtonDown("Fire1"))
            {
                Attack();
            }

            if(0f < ControlLostTime)
            {
                ControlLostTime -= Time.deltaTime;
                GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
            }
            else
            {
            GetComponent<SpriteRenderer>().color = Color.white;  
            }

            //コンティニュー時の点滅
            if(isContinue)
        {
            //明滅ついている時に戻る
            if(blinkTime > 0.2f)
            {
                spRenderer.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅消えている時
            else if(blinkTime > 0.1f)
            {
                spRenderer.enabled = false;
            }
            //明滅ついているとき
            else
            {
                spRenderer.enabled = true;
            }

            //1秒たったら明滅終わり
            if(continueTime > 1.0f)
            {
                isContinue = false;
                blinkTime = 0f;
                continueTime = 0f;
                spRenderer.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }

        }
    }

    public bool IsContinueWaiting()
    {
        return IsDeadAnimEnd();
    }

    //ダウンアニメーションが完了しているかどうか
    private bool IsDeadAnimEnd()
    {
        if(isDead && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("Knight_Death"))
            {
                if(currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
                return false;
    }

    public void ContinuePlayer()
    {
        isDead = false;
        Instantiate(fxhit , transform.position , transform. rotation);
        anim.Play("Knight_idle");
        isContinue = true;

        //HPを最大まで回復させておく
        hp = Maxhp;
    }

    public void onDamage(GameObject enemy) //ダメージ処理
    {
        hp--;

        Debug.Log($"forward:{gameObject.transform.forward}");
        float knockBackX = gameObject.transform.forward.z < 0f ? 4f : -4f;
        rb2d.velocity = new Vector2(knockBackX , 7f);//ノックバック

        ControlLostTime = 0.5f;

        Debug.Log($"ダメージを受けました。現在のhp:{hp}");
        anim.SetTrigger("TrgHit");
        audiosource.PlayOneShot(DamageSE);
        

        if(hp <= 0)
        {
            onDead();
        }
    }

    void onDead()//死亡処理
    {
        hp = 0;
        isDead = true;
        anim.Play("Knight_Death");
        audiosource.PlayOneShot(DeadSE);

        Invoke("Destroy",0.5f);//死亡時にウェイトを作る
    }

    public int GetHP() //HP処理
    {
        return hp;
    }

    void FixedUpdate()
    {

        if(isDead)
        {
            return;
        }

        isGround = false;

        float x = Input.GetAxisRaw("Horizontal");

        Vector2 groundPos =  //自分の立っているポジション
        new Vector2(
            transform.position.x ,transform.position.y);
            
        //地面判定エリア
        Vector2 groundArea = new Vector2(0.5f , 0.4f);

        Debug.DrawLine( groundPos + groundArea , groundPos - groundArea , Color.red);

        //壁判定エリア
        Vector2 WallArea1 = new Vector2(x * 0.8f,1.5f);
        Vector2 WallArea2 = new Vector2(x * 0.3f,1.0f);

        Debug.DrawLine( groundPos + WallArea1 , groundPos + WallArea2 , Color.blue);

        //坂道判定エリア
        Vector2 WallArea3 = new Vector2(x * 1.4f,0.6f);
        Vector2 WallArea4 = new Vector2(x * 0.8f,0.1f);

        Debug.DrawLine( groundPos + WallArea3 , groundPos + WallArea4 , Color.blue);
        
        isGround =
        Physics2D.OverlapArea(
            groundPos + groundArea ,
            groundPos - groundArea ,
            groundLayer
        );
    }

    void OnCollisionEnter2D( Collision2D col )
    {
        if( col.gameObject.tag == "Enemy" )
        {
            onDamage(col.gameObject);
            Debug.Log(col.gameObject.name+"に初めて接触");
        }
    }
}

