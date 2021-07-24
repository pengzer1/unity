//점수 관리 및 스테이지 관리

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public playermove player;
    // Start is called before the first frame update
    public void NextStage()
    {
        stageIndex++;

        totalPoint += stagePoint;
        stagePoint = 0;
    }
    public void HealthDown()
    {
        if (health > 0)
            health--;
        else
        {
            //죽는 이펙트
            player.OnDie();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            health--;

            //player 원위치
            collision.attachedRigidbody.velocity = Vector2.zero;
            collision.transform.position = new Vector3(-7.5f, 2.5f, -1);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
