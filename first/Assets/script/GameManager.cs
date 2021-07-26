//점수 관리 및 스테이지 관리

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public playermove player;
    public GameObject[] Stages;
    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject RestartBtn;

    // Start is called before the first frame update
    public void NextStage()
    {
        //Stage 변경
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else
        {//게임 클리어
         //시간 정지
            Time.timeScale = 0;
            //결과
            Debug.Log("게임 클리어!");
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }
    public void HealthDown()
    {
        if (health > 1)
            health--;
        else
        {
            //죽는 이펙트
            player.OnDie();
            //죽는 UI
            Debug.Log("죽었습니다!");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health > 1)
            {
                //player 원위치
                PlayerReposition();
            }
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.VelocityZero();
        player.transform.position = new Vector3(-7.5f, 2.5f, -1);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
