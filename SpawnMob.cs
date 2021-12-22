using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMob : MonoBehaviour
{
    bool unCheck = true;
    public GameObject mob;      // 소환될 몬스터
    public GameObject parent;   // 부모 오브젝트
    public GameObject hpBar;    // 현재 스테이지 보스의 체력바

    void LateUpdate()
    {
        // 게임이 시작되고 한번도 실행이 안된경우
        // 음식이 인식되고 몬스터를 인식할때도 실행되도록 update에서 실행
        if (GameMng.instance.gameState == GameState.Begin && unCheck)
        {
            // 1초후에 1.0에서 3.0까지의 범위의 임의의 값마다 Spawn함수 실행
            InvokeRepeating("Spawn", 1.0f, Random.Range(1.0f, 3.0f));
            unCheck = false;
        }

        // 음식의 hp를 확인
        FoodHP();
    }

    void Spawn()
    {
        // 몬스터 생성
        GameObject obj = Instantiate(mob);

        // 스폰위치 설정
        Vector3 randPos;
        randPos.x = Random.Range(-0.1f, 0.1f);
        randPos.y = 0;
        randPos.z = Random.Range(-0.1f, 0.1f);

        // 소환될 몬스터의 위치 설정
        obj.transform.position = parent.transform.position + randPos;

        // 현재 stage의 자식오브젝트로 소환
        obj.transform.parent = parent.transform;

        // 보스의 피가 0이하가 되면 몬스터 소환 중단
        if (hpBar.GetComponent<Image>().fillAmount <= 0)
        {
            // 이 스크립트의 invoke함수 중지 
            CancelInvoke();
        }
    }

    public void OnDetected()
    {
        // 찾은 갯수 증가
        GameMng.instance.foundCheck++;

        // 두개의 이미지가 인식되는 경우
        if (GameMng.instance.foundCheck == 2)
        {
            // 팝업텍스트를 공백으로 설정
            MainMng.instance.setTextVoid();
        }

        // 현재 스테이지 활성화
        GameMng.instance.stages[GameMng.instance.curStage].gameObject.SetActive(true);
    }

    //음식의 체력이 0이되면 몬스터 소환 종료
    void FoodHP()
    {
        // 모든 스테이지의 음식체력 확인
        for (int i = 0; i < GameMng.instance.foodImgHp.Length; i++)
        {
            // 스테이지의 음식의 체력이 0인지 확인
            if (GameMng.instance.foodImgHp[i].fillAmount == 0)
            {
                // 이 스크립트의 invoke함수 중지 
                CancelInvoke();
                // 게임상태를 패배로 설정
                GameMng.instance.gameState = GameState.Defeat;
                break;
            }
        }
    }
}
