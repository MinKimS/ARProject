using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AttackItemCtrl : MonoBehaviour
{
    // 카메라의 transform
    public Transform camTr;

    // 공격 아이템의 Rigidbody
    Rigidbody rb;

    // 마우스 위치
    Vector3 mousePos;

    // 던지는 아이템 정보
    [System.Serializable]
    public struct weaponType
    {
        public AudioClip[] sound;   // 아이템 소리
        public GameObject weaphone; // 아이템 오브젝트
    }
    public weaponType[] WeaponList;

    // 블럭의 인덱스
    int Bindex;

    // 미니게임 바구니 오브젝트
    public GameObject basket;

    // 미니게임 점수
    public int score;

    // 떨어진 횟수
    public int fall = 0;
    
    // 돈 증가량 텍스트
    public Text numText;

    // 소환될 블럭
    public GameObject[] miniObj;
    GameObject bObj;

    // 리플레이 버튼
    public GameObject replayBtn;

    // 메뉴이동 버튼
    public GameObject MenuBtn;

    // 공중에 블럭이 생성되지 않기위해 타겟아이템 인식 확인
    public bool Find = false;

    // 튜토리얼 공던지는 소리
    public AudioClip ballAudio;

    private void Start()
    {
        // 상태 메세지를 설정
        MainMng.instance.setText("이미지를 인식시켜주세요.");
        // 현재 오브젝트의 rigidbody를 가져옴
        rb = GetComponent<Rigidbody>();

        // 미니게임인 경우
        if (SceneManager.GetActiveScene().name == "MiniGame")
        {
            // 처음 블럭을 랜덤으로 설정
            Bindex = Random.Range(0, 7);
        }

        // 실제 게임인 경우 상점에서 선택된 아이템으로 무기와 데미지, 무기 소리설정
        if (SceneManager.GetActiveScene().name == "InGame")
        {
            // 축구공
            if (MainMng.instance.selected == 0)
            {
                // 가장 약함
                GameMng.instance.attackItem = WeaponList[0].weaphone;
                GameMng.instance.damage = 0.03f;
                GameMng.instance.ads.clip = WeaponList[0].sound[0];
            }
            // 책
            if (MainMng.instance.selected == 1)
            {
                GameMng.instance.attackItem = WeaponList[1].weaphone;
                GameMng.instance.damage = 0.05f;
                // 2개의 소리중 하나를 랜덤으로 재생소리로 지정
                GameMng.instance.ads.clip = WeaponList[1].sound[Random.Range(0, 2)];
            }
            // 휴지
            if (MainMng.instance.selected == 2)
            {
                GameMng.instance.attackItem = WeaponList[2].weaphone;
                GameMng.instance.damage = 0.07f;
                // 2개의 소리중 하나를 랜덤으로 재생소리로 지정
                GameMng.instance.ads.clip = WeaponList[2].sound[Random.Range(0, 2)];
            }
            // 부츠
            if (MainMng.instance.selected == 3)
            {
                GameMng.instance.attackItem = WeaponList[3].weaphone;
                GameMng.instance.damage = 0.1f;
                GameMng.instance.ads.clip = WeaponList[3].sound[0];
            }
            // 돌
            if (MainMng.instance.selected == 4)
            {
                GameMng.instance.attackItem = WeaponList[4].weaphone;
                GameMng.instance.damage = 0.12f;
                GameMng.instance.ads.clip = WeaponList[4].sound[0];
            }
            // 총
            if (MainMng.instance.selected == 5)
            {
                GameMng.instance.attackItem = WeaponList[5].weaphone;
                GameMng.instance.damage = 0.3f;
                // 5개의 소리중 하나를 랜덤으로 재생소리로 지정
                GameMng.instance.ads.clip = WeaponList[5].sound[Random.Range(0, 5)];
            }
            // 러버덕
            if (MainMng.instance.selected == 6)
            {
                // 가장 강함
                GameMng.instance.attackItem = WeaponList[6].weaphone;
                GameMng.instance.damage = 0.8f;
                // 3개의 소리중 하나를 랜덤으로 재생소리로 지정
                GameMng.instance.ads.clip = WeaponList[6].sound[Random.Range(0, 3)];
            }
        }
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            GameMng.instance.ads.clip = ballAudio;
        }    
    }
    private void LateUpdate()
    {
        // 미니게임에서 공을 3번 떨어트린 경우
        if (SceneManager.GetActiveScene().name == "MiniGame" && fall == 3)
        {
            // 점수를 돈으로 추가
            MainMng.instance.money += score*10;
            numText.text = "돈이 " + score*10 + " 증가했다!!";

            // 리플레이, 메뉴이동버튼 활성화
            replayBtn.SetActive(true);
            MenuBtn.SetActive(true);

            // 던지는 블럭 아이템 비활성화
            gameObject.SetActive(false);
        }

        // 공이 날아간 순간에는 위치가 초기화 되지 않도록 함
        if (rb.isKinematic == false)
            return;

        // 카메라와 떨어진 정도를 저장
        Vector3 offset = camTr.forward * 0.4f - camTr.up * 0.12f;
        // 카메라에서 offset만큼 떨어진 위치로 공격아이템 위치 지정
        transform.position = camTr.position + offset;
        // 공격아이템의 방향을 카메라의 방향으로 설정
        transform.rotation = camTr.rotation;

        // 마우스 드래그한 방향으로 공격아이템 던진다
        if(Input.GetMouseButtonDown(0))
        {
            // 왼쪽 마우스가 클릭된 위치를 저장
            mousePos = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            // 설명창이 비활성화인경우에만 실행해 설명을 보는 동안 블럭이 날아가지 않도록 함
            if (!MainMng.instance.uiShow)
            {
                // 실제 게임인 경우 SphereCollider 활성화
                if (SceneManager.GetActiveScene().name == "InGame")
                    gameObject.GetComponent<SphereCollider>().enabled = true;

                // 현재위치에서 기존위치가지 이동한 값
                Vector3 deltaPos = mousePos - Input.mousePosition;
                // deltaPos의 길이를 저장
                float len = deltaPos.magnitude;

                // 현재위치에서 계속있지 않게 설정
                rb.isKinematic = false;
                // 아이템이 카메라의 전방 대각선 방향으로 포물선으로 던져짐
                rb.AddForce((camTr.forward + camTr.up).normalized * len * 0.3f);
                // 아이템의 회전력 설정
                rb.AddTorque(-deltaPos.y, deltaPos.x, 0);

                // 실제게임인 경우
                if (SceneManager.GetActiveScene().name == "InGame")
                {
                    // 여러소리가 나는 공의 소리를 설정
                    // 책이나 휴지인 경우
                    if (MainMng.instance.selected == 1 || MainMng.instance.selected == 2)
                        GameMng.instance.ads.clip = WeaponList[2].sound[Random.Range(0, 2)];
                    // 돌인 경우
                    if (MainMng.instance.selected == 5)
                        GameMng.instance.ads.clip = WeaponList[5].sound[Random.Range(0, 5)];

                    // 러버덕인 경우
                    if (MainMng.instance.selected == 6)
                        GameMng.instance.ads.clip = WeaponList[6].sound[Random.Range(0, 3)];
                }

                // 본게임이거나 튜토리얼인 경우
                if (SceneManager.GetActiveScene().name == "InGame" || SceneManager.GetActiveScene().name == "Tutorial")
                {
                    // 오디오소스가 활성화되어있으면 소리 재생
                    if (GameMng.instance.ads.enabled == true)
                    {
                        // 오디오 소리 재생
                        GameMng.instance.ads.PlayOneShot(GameMng.instance.ads.clip, 0.5f);
                    }
                }

                // 미니게임이고 타겟이미지를 찾은 경우
                if (SceneManager.GetActiveScene().name == "MiniGame" && Find == true)
                {
                    // 던진 공을 던진 위치에 생성
                    Invoke("PutBlock", 0.5f);
                }
                // 공을 원래 위치로 이동
                Invoke("ResetBall", 0.5f);
            }
        }
    }

    // 공격아이템, 블럭을 원래 위치로 이동
    void ResetBall()
    { 
        // 미니게임인 경우
        if (SceneManager.GetActiveScene().name == "MiniGame")
        {
            // 현재 블럭을 비활성화
            WeaponList[Bindex].weaphone.SetActive(false);
            
            //정해진 7개의 블럭 중 하나를 랜덤으로 활성화
            Bindex = Random.Range(0, 7);
            WeaponList[Bindex].weaphone.SetActive(true);
        }
        // 실제 게임이나 튜토리얼인경우
        else
        {
            // 공격 아이템 활성화
            gameObject.SetActive(true);
            // 실제 게임인 경우 SphereCollider 비활성화해서 몬스터에 접근했을때 공격처리가 되지 않도록 설정
            if(SceneManager.GetActiveScene().name == "InGame")
                gameObject.GetComponent<SphereCollider>().enabled = false;
        }

        // 처음 상태로 되돌림
        rb.isKinematic = true;  // 공 위치를 원래 위치로 되돌림
        rb.velocity = Vector3.zero; // 이동 속도를 0으로 설정
        rb.angularVelocity = Vector3.zero; // 회전하는 속도를 0으로 설정
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 보스에 닿으면 공격 아이템 비활성화
        if (SceneManager.GetActiveScene().name != "MiniGame")
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 몬스터나 아기 몬스터에 닿으면 공격 아이템 비활성화
        if (SceneManager.GetActiveScene().name != "MiniGame" && (other.gameObject.tag == "Monster" || other.gameObject.tag == "BabyMonster"))
        {
            gameObject.SetActive(false);
        }
    }

    // 블럭이 놓아지는 함수
    public void PutBlock()
    {
        // 던진 블럭을 던져진 위치에 생성
        bObj = Instantiate(miniObj[Bindex], WeaponList[Bindex].weaphone.transform.position, WeaponList[Bindex].weaphone.transform.rotation);

        // 바구니의 자식 오브젝트로 설정
        bObj.transform.parent = miniObj[7].transform;
    }

    // 타겟 이미지가 있으면 블럭 활성화
    public void OnTargetFound()
    {
        //타겟이미지 인식을 참으로 설정
        Find = true;
        WeaponList[Bindex].weaphone.SetActive(true);
        // 팝업 메세지 텍스트 공백설정
        MainMng.instance.setTextVoid();
    }

    // 타겟 이미지가 없으면 비활성화
    public void OnTargetLost()
    {
        // 타겟이미지 인식을 거짓으로 설정
        Find = false;
    }
}
