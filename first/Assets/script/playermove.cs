using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    public GameManager GameManager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsulecollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //�ȱ� Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.35)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

    void FixedUpdate()
    {
        //Move Speed By Key Control
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        //Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        if (rigid.velocity.y < 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.6f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            //공격
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
                //데미지
                OnDamaged(collision.transform.position);
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        GameManager.HealthDown();
        //layer ����
        gameObject.layer = 9;

        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 5, ForceMode2D.Impulse);

        //animation
        anim.SetTrigger("damaged");

        Invoke("OffDamaged", 2);
    }

    void OffDamaged()
    {
        //layer ����
        gameObject.layer = 8;

        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsulecollider.isTrigger = true;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("item"))
        {
            //점수
            bool isBronze = collision.gameObject.name.Contains("bronzecoin");
            bool isSilver = collision.gameObject.name.Contains("silvercoin");
            bool isGold = collision.gameObject.name.Contains("goldcoin");

            if (isBronze)
                GameManager.stagePoint += 50;
            else if (isSilver)
                GameManager.stagePoint += 100;
            else if (isGold)
                GameManager.stagePoint += 200;
            // 동전 사라짐
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //Next Stage
            GameManager.NextStage();
        }
    }

    void OnAttack(Transform enemy)
    {
        GameManager.stagePoint += 100;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        monstermove enemyMove = enemy.GetComponent<monstermove>();
        enemyMove.OnDamaged();
    }
}
