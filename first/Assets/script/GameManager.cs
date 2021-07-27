//점수 관리 및 스테이지 관리

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

            //Stage UI 설정
            UIStage.text = "Stage " + (stageIndex + 1);
        }
        else
        {//게임 클리어
         //시간 정지
            Time.timeScale = 0;
            //결과
            Debug.Log("게임 클리어!");
            //Retry 버튼 UI 활성화
            Text btnText = RestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Game Clear!";
            RestartBtn.SetActive(true);
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 1, 1, 0.2f);
        }
        else
        {
            UIhealth[0].color = new Color(1, 1, 1, 0.2f);
            //죽는 이펙트
            player.OnDie();
            //죽는 UI
            Debug.Log("죽었습니다!");
            //Retry 버튼 UI 활성화
            RestartBtn.SetActive(true);
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

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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
        //Point UI 업데이트 설정
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }
}
