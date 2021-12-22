using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMng : MonoBehaviour
{
    // MainMng의 전역 클래스
    static public MainMng instance;
    
    // 구입
    public bool[] bought;

    // 선택한 아이템 번호
    public int selected = 0;

    // 소유한 돈
    public int money;

    // 최초 게임 클리어 확인
    public bool gameClear = false;
    
    // 팝업메세지 텍스트
    public Text Text;

    // 팝업메세지 오브젝트
    public GameObject stateMsg;

    // 설명 창 활성화상태인지 확인
    public bool uiShow;

    // 오브젝트의 오디오소스
    public AudioSource Mads;
    // 배경음악
    public AudioClip[] BGM;

    void Start()
    {
        // 오브젝트의 오디오 소스를 가져온다.
        Mads = GetComponent<AudioSource>();

        // BGM 실행
        Mads.Play();
        // 돈을 0원으로 설정
        money = 0;

        // instance가 null이면 자기자신을 할당
        if (instance == null)
        {
            instance = this;
        }

        // MainMng가 있는 오브젝트와 팝업메세지 오브젝트를 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(stateMsg);
        
        // 기본 아이템을 제외하고 비구매 아이템으로 설정
        for (int i = 1; i < bought.Length; i++)
        {
            bought[i] = false;
        }
    }
    private void OnEnable()
    {
        // 델리게이트 체인 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 씬이 로드될때마다 씬에 맞는 음악으로 설정
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 튜토리얼인 경우
        if (scene.name == "Tutorial")
        {
            // 정해진 BGM으로 설정
            Mads.clip = BGM[0];
            // 음악 재생
            instance.Mads.Play();
        }
        // 본게임인 경우
        else if (scene.name == "InGame")
        {
            // 정해진 BGM으로 설정
            instance.Mads.clip = instance.BGM[2];
            // 음악 재생
            instance.Mads.Play();
        }
        // 미니게임인 경우
        else if (scene.name == "MiniGame")
        {
            // 정해진 BGM으로 설정
            instance.Mads.clip = instance.BGM[1];
            // 음악 재생
            instance.Mads.Play();
        }
        // 메뉴인 경우
        else if (scene.name == "Menu")
        {
            // 정해진 BGM으로 설정
            instance.Mads.clip = instance.BGM[3];
            // 음악 재생
            instance.Mads.Play();
        }
        // Shop인 경우
        else if (scene.name == "Shop")
        {
            // 정해진 BGM으로 설정
            instance.Mads.clip = instance.BGM[4];
            // 음악 재생
            instance.Mads.Play();
        }
    }

    //메세지를 화면에 time만큼 띄운다
    public void PopupMsg(string msg, int time)
    {
        StartCoroutine(ShowMsg(msg, time));
    }
    IEnumerator ShowMsg(string msg, int time)
    {
        // msg를 텍스트로 설정
        Text.text = msg;
        // time만큼 지난 후 text를 공백으로 설정
        yield return new WaitForSeconds(time);
        Text.text = "";
    }

    // 팝업메세지의 텍스트를 공백으로 설정
    public void setTextVoid()
    {
        Text.text = "";
    }

    // 팝업메세지의 텍스트를 설정
    public void setText(string text)
    {
        Text.text = text;
    }
}
