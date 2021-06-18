using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monstermove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsulecollider;

    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsulecollider = GetComponent<CapsuleCollider2D>();

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        //������
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //�ٴ����� �ȶ������� �ϱ�
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        //Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    void Think()
    {
        //next Action
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

        //���� animation
        anim.SetInteger("WalkSpeed", nextMove);

        //���� sprite
        if (nextMove == 1)
        {
            spriteRenderer.flipX = true;
        }
        else if (nextMove == -1)
        {
            spriteRenderer.flipX = false;
        }
    }

    void Turn()
    {
        nextMove = nextMove * -1;
        CancelInvoke();
        Invoke("Think", 5);

        if (nextMove == 1)
        {
            spriteRenderer.flipX = true;
        }
        else if (nextMove == -1)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsulecollider.isTrigger = true;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        Invoke("DeActive", 5f);
    }

    void DeActive()
    {
        Debug.Log("앙 기모띠");
        gameObject.SetActive(false);
    }
}
