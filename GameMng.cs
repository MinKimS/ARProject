using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 상태
public enum GameState
{
    Ready,
    Begin,
    End,
    Defeat,
    Win
}
public class GameMng : MonoBehaviour
{
    // GameMng의 전역 클래스
    static public GameMng instance;
    
    // 게임 설명 ( 튜토리얼 용 )
    public GameObject explain;

    // 게임 상태를 준비로 설정
    public GameState gameState = GameState.Ready;

    // 리플레이 버튼
    public GameObject replayBtn;
    // 메뉴로 돌아가기 버튼
    public GameObject goMenuBtn;

    // 패배시 나오는 오브젝트
    public GameObject[] Defeat;

    // 보유한 돈 텍스트
    public Text numText;

    // 스테이지
    public GameObject[] stages;
    public GameObject[] foods;
    public int curStage = 0;

    // 무기
    public GameObject attackItem;
    // 무기의 데미지 ( 기본 = 0.03 )
    public float damage;

    // 보스 체력
    public Image[] imgHp;
    // 음식 체력
    public Image[] foodImgHp;

    // 음식 위치
    public GameObject target;
    // 몬스터 이동 속도
    public float speed = 0.5f;

    // 공 던지는 소리 오디오
    public AudioSource ads;

    // 이펙트
    public ParticleSystem[] Eff;

    // 인식된 이미지 개수
    public int foundCheck = 0;

    void Start()
    {
        // instance가 null이면 자기자신을 할당
        if (instance == null)
        {
            instance = this;
        }

        // 튜토리얼이 아닌 경우
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            // 보유한 돈 텍스트를 설정
            numText.text = MainMng.instance.money + "";
        }
        ads = GetComponent<AudioSource>();
    }

    // 다음 스테이지로 이동
    public void GoNextStage()
    {
        StartCoroutine(ChangeStage());
    }
    IEnumerator ChangeStage()
    {
        // 현재 스테이지의 몬스터와 음식 비활성화
        stages[curStage].SetActive(false);
        foods[curStage].SetActive(false);

        // 다음 스테이지로 설정
        ++curStage;
        // 0.5초후 다음 스테이지의 몬스터와 음식 활성화
        if(curStage < stages.Length)
        {
            yield return new WaitForSeconds(0.5f);
            stages[curStage].SetActive(true);
            foods[curStage].SetActive(true);
        }
        // 게임 상태를 Begin으로 설정
        gameState = GameState.Begin;
    }

    //음식이 인식되었을때 기본설정
    public void OnDetected()
    {
        // 현재 음식 스테이지 활성화
        foods[curStage].SetActive(true);

        // 게임상태를 Ready인 경우
        if (gameState == GameState.Ready)
        {
            // 게임 상태를 Begin으로 설정
            gameState = GameState.Begin;
            // 공격 아이템 활성화
            attackItem.SetActive(true);
        }

        // 찾은 개수 증가
        foundCheck++;
        // 두개의 이미지가 인식되면 팝업텍스트를 전부 지우기
        if (foundCheck == 2)
        {
            MainMng.instance.setTextVoid();
        }
    }

    // 돈 증가 및 돈 보유량 텍스트 설정
    public void AddMoney(int num)
    {
        MainMng.instance.money += num;
        numText.text = MainMng.instance.money + "";
    }

    // 이펙트 발생
    public void PlayEffect(Vector3 pos, int index)
    {
        // 이펙트 생성
        ParticleSystem ps = Instantiate(Eff[index]);
        // 이펙트 위치 설정
        ps.transform.position = pos;
    }

}
