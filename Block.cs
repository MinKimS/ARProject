using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{  
    // attackCtrl의 객체
    AttackItemCtrl attackCtrl;
    
    // 현재 오브젝트의 rigidbody
    Rigidbody rb;

    // 점수 증가 여부 체크
    bool check = false;

    // 오브젝트의 오디오소스
    public AudioSource bAds;

    void Start()
    {
        // Block오브젝트의 attackCtrl 스크립트를 가져옴
        attackCtrl = GameObject.Find("Block").GetComponent<AttackItemCtrl>();
        // 현재 오브젝트의 Rigidbody를 가져옴
        rb = gameObject.GetComponent<Rigidbody>();
        bAds = gameObject.GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        // 타겟이 인식이 안된경우에는 중력 비활성화해서 바구니에서 떨어지지 않도록 함
        if(attackCtrl.Find == false)
        {
            rb.useGravity = false;
        }
        // 타겟이 인식이 된경우 바구니에서 떨어질 수 있도록 함
        else
        {
            rb.useGravity = true;
        }

        // 블럭의 y값이 바구니의 0.01f 아래보다 경우
        if (transform.position.y < attackCtrl.basket.transform.position.y - 0.01f)
        {
            // 떨어진 횟수 증가
            ++attackCtrl.fall;

            // 총 3번 떨어진 경우 게임오버 팝업메세지 출력
            if (attackCtrl.fall == 3)
            {
                MainMng.instance.PopupMsg("GAME OVER", 3);
            }

            // 떨어진 블럭 삭제
            Destroy(gameObject);

        }
        else
        {
            // 바구니에 올라갔다가 떨어지는 경우를 대비해 0.15초 후 점수 상승
            Invoke("Increase", 0.5f);
        }
    }

    // 점수 증가 및 점수 표시
    void Increase()
    {
        // 한번만 점수가 증가하도록 함
        if (!check)
        {
            // 점수 상승 음악 실행
            bAds.PlayOneShot(bAds.clip, 0.5f);
            // 점수 상승
            ++attackCtrl.score;
            // 점수 표시
            attackCtrl.numText.text = "점수 : " + attackCtrl.score;
            // 실행여부를 true로 설정
            check = true;
        }
    }
}
