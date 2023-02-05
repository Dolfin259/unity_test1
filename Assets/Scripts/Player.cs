using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float speed = 25f;
    public float jumpForce = 750f;

    public Transform attackpoint;
    public float attackRadius;
    public LayerMask enemyLayer;

    public LayerMask groundLayer;

    private Animator anim;

    private SpriteRenderer spRenderer;

    private bool isGround;
    
    void Start()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.spRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); //キー入力で水平方向移動

        anim.SetFloat("Speed",Mathf.Abs(x * speed)); //キー入力があるときだけアニメーション再生

        if(Input.GetButtonDown("Jump") & isGround){
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

       //スプライトの向きを変える
        if(x < 0)
            {spRenderer.flipX = true;}

        else if(x > 0)
         {spRenderer.flipX = false;}
        
        rb2d.AddForce(Vector2.right * x * speed);

        //攻撃
        if(Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
        void Attack()
        {
            Debug.Log("攻撃");
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackpoint.position,attackRadius,enemyLayer);
        }
     void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackpoint.position,attackRadius);
        }
    }

    void FixedUpdate()
    {
        isGround = false;

        float x = Input.GetAxisRaw("Horizontal");

        Vector2 groundPos =  //自分の立っているポジション
        new Vector2(
            transform.position.x ,
            transform.position.y
            );

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
        if( col.gameObject.tag == "Damage" ){

            anim.SetBool("Damage",true);

            Debug.Log("Damage");
        }
        else{
            anim.SetBool ("Damage",false);
            }
        
    }
}
