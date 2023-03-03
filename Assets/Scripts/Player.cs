using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float speed = 25f;
    public float jumpForce = 750f;
    public int at = 1; //攻撃力
    public int hp = 5; //HP

    private GameObject enemy;

    public Transform attackpoint;
    public float attackRadius;

    public LayerMask enemyLayer;
    public LayerMask groundLayer;

    private Animator anim;

    private SpriteRenderer spRenderer;

    private bool isGround;

    private bool isDead = false;

    public float ControlLostTime; //制御不能になる時間
    
    void Start()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.spRenderer = GetComponent<SpriteRenderer>();
        enemy = GameObject.FindWithTag("Enemy");
        ControlLostTime = 0f;
        
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

        //攻撃
        void Attack()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackpoint.position, attackRadius, enemyLayer);

            foreach (Collider2D hitEnemy in hitEnemys)
            {
                Debug.Log(hitEnemy.gameObject.name+"に攻撃");
                hitEnemy.GetComponent<SkeltonCtrl>().OnDamage(at);
            }
            
            anim.SetTrigger("isAttack");
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
    }

    void OnDrawGizmosSelected() //攻撃範囲
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackpoint.position, attackRadius);
        }

    void onDamage(GameObject enemy) //ダメージ処理
    {
        hp--;
        
        rb2d.velocity = new Vector2(4f , 7f);//ノックバック
        ControlLostTime = 0.5f;

        Debug.Log($"ダメージを受けました。現在のhp:{hp}");
        anim.SetTrigger("TrgHit");

        if(hp <= 0)
        {
            OnDead();
        }
    }

    void OnDead()//死亡処理
    {
            hp = 0;
            isDead = true;
            anim.Play("Knight_Death");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
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

    void OnTriggerEnter2D(Collider2D col)
    {
    if (col.gameObject.tag == "Enemy")
        {
            Debug.Log(col.gameObject.name + "のTriggerが通過");
            onDamage(col.gameObject);
        }
    }
}
