using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterCtrl : MonoBehaviour
{
    // 죽을때 새끼를 낳을 확률
    public float layRate = 0;
    // 새끼 오브젝트
    public GameObject baby;
    // 보스의 체력바
    public GameObject hpBar;
    // 튜토리어 클리어 체크
    bool Tcheck = false;

    // 몬스터가 움직일 위치
    Vector3 moveDir;
    // 몬스터가 움직일 방향
    float angle;

    // 몬스터와 음식사이의 거리
    float distance;

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌 대상이 공이 아닌경우 OnCollisionEnter의 아래 코드 실행 안함
        if (collision.collider.tag != "AttackItem")
            return;

        // 이펙트 발생 위치
        Vector3 colPos = collision.contacts[0].point;

        // 보스공격시 체력감소
        if (this.gameObject.tag == "Boss")
        {
            // 이펙트 발생
            GameMng.instance.PlayEffect(colPos, 0);

            //Tutorial인 경우
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                // 공을 맞을 경우 체력감소
                GameMng.instance.imgHp[0].fillAmount -= 0.1f;

                // 튜토리얼을 클리어하지 않고 튜토리얼의 보스체력을 0으로 만든경우
                if (hpBar.GetComponent<Image>().fillAmount <= 0 && !Tcheck)
                {
                    // 팝업메세지를 2초동안 출력
                    MainMng.instance.PopupMsg("튜토리얼 클리어!!", 2);

                    // 재시작 버튼 활성화
                    GameMng.instance.replayBtn.SetActive(true);
                    // 메뉴로 이동 버튼 활성화
                    GameMng.instance.goMenuBtn.SetActive(true);

                    // 튜토리얼을 클리어 상태로 설정
                    Tcheck = true;
                }
            }
            //실제게임인 경우 보스의 체력감소
            else
            {
                // 패배한 경우가 아닌경우 보스체력 감소
                if(GameMng.instance.gameState != GameState.Defeat)
                {
                    //마지막스테이지
                    if (GameMng.instance.curStage == 3)
                    {
                        // 데미지의 0.2배씩 감소
                        GameMng.instance.imgHp[3].fillAmount -= GameMng.instance.damage * 0.2f;
                    }
                    //Stage3
                    else if (GameMng.instance.curStage == 2)
                    {
                        // 데미지의 0.25배씩 감소
                        GameMng.instance.imgHp[2].fillAmount -= GameMng.instance.damage * 0.25f;
                    }
                    //Stage2
                    else if (GameMng.instance.curStage == 1)
                    {
                        // 데미지의 0.5배씩 감소
                        GameMng.instance.imgHp[1].fillAmount -= GameMng.instance.damage * 0.5f;
                    }
                    //Stage1
                    else if (GameMng.instance.curStage == 0)
                    {
                        // 데미지 만큼 감소
                        GameMng.instance.imgHp[0].fillAmount -= GameMng.instance.damage;
                    }

                }

                // 보스의 체력이 0과 같거나 작은 경우
                if (hpBar.GetComponent<Image>().fillAmount <= 0)
                {
                    // 마지막 스테이지까지 클리어한 경우 리플레이, 메뉴로 이동 버튼 활성화
                    if (GameMng.instance.imgHp[3].fillAmount == 0 || SceneManager.GetActiveScene().name == "Tutorial")
                    {
                        // 게임 상태를 End로 설정
                        GameMng.instance.gameState = GameState.End;

                        // 리플레이 버튼 활성화
                        GameMng.instance.replayBtn.SetActive(true);
                        // 메뉴로 이동 버튼 활성화
                        GameMng.instance.goMenuBtn.SetActive(true);

                        // 처음 게임 클리어 시 미니게임 해금 표시
                        if (MainMng.instance.gameClear == false)
                        {
                            // 팝업메시지를 4초동안 출력
                            MainMng.instance.PopupMsg("미니게임이 해금되었습니다", 4);
                        }
                        // 게임 클리어 팝업메세지 표시
                        else
                        {
                            // 팝업메시지를 2초동안 출력
                            MainMng.instance.PopupMsg("스테이지 클리어!!", 2);
                        }

                        // 돈 증가
                        GameMng.instance.AddMoney(500);
                        // 공격아이템 비활성화
                        GameMng.instance.attackItem.SetActive(false);
                        // 오디오 비활성화
                        GameMng.instance.ads.enabled = false;
                        // 게임 클리어를 true로 변경해 한번더 클리어시 미니게임 해금 비표시
                        MainMng.instance.gameClear = true;
                    }
                    // 마지막이 아닌 스테이지 클리어한 경우
                    else
                    {
                        // 게임 상태를 승리로 설정
                        GameMng.instance.gameState = GameState.Win;

                        // 마지막 스테이지가 아니면 다음스테이지로 이동
                        if (GameMng.instance.curStage < 4)
                            GameMng.instance.GoNextStage();
                    }

                    // 현재 오브젝트를 비활성화
                    gameObject.SetActive(false);
                }
            }
        }
        // 튜토리얼인 경우에만
        else
        {
            // 3초후 몬스터를 되살림
            Invoke("Revive", 3);

            //공에 맞으면 이펙트 발생 및 몬스터 비활성화
            GameMng.instance.PlayEffect(this.gameObject.transform.position, 1);
            // 몬스터 비활성화
            gameObject.SetActive(false);
        }
    }

    // 본게임에서 몬스터와 보스가 겹칠때를 방지하기 위해 보스이외에는 Trigger로 설정
    private void OnTriggerEnter(Collider other)
    {
        // 음식에 몬스터가 접근하면 음식의 체력 감소
        if (other.gameObject.tag == "Food")
        {
            //음식에 접근한 몬스터나 아기몬스터 삭제
            Destroy(this.gameObject);

            //마지막스테이지
            if (GameMng.instance.curStage == 3)
            {
                // 0.07만큼 감소
                GameMng.instance.foodImgHp[3].fillAmount -= 0.07f;
            }
            //Stage3
            else if (GameMng.instance.curStage == 2)
            {
                // 0.06만큼 감소
                GameMng.instance.foodImgHp[2].fillAmount -= 0.06f;
            }
            //Stage2
            else if (GameMng.instance.curStage == 1)
            {
                // 0.04만큼 감소
                GameMng.instance.foodImgHp[1].fillAmount -= 0.04f;
            }
            //Stage1
            else if (GameMng.instance.curStage == 0)
            {
                // 0.03만큼 감소
                GameMng.instance.foodImgHp[0].fillAmount -= 0.03f;
            }
        }

        // 몬스터가 공에 맞은 경우
        if(this.gameObject.tag == "Monster")
        {
            if (other.gameObject.tag == "AttackItem")
            {
                // 공에 맞으면 이펙트 발생
                GameMng.instance.PlayEffect(this.gameObject.transform.position, 1);

                // 돈 증가
                GameMng.instance.AddMoney(10);

                // 죽을때 새끼 몬스터를 소환
                //Stage1
                if (GameMng.instance.curStage == 0)
                {
                    // 가장 낮은 확률로 죽을때 새끼를 4마리 소환
                    if (Random.Range(0.0f, 1.0f) < layRate)
                    {
                        for (int i = 0; i < 4; i++)
                            SpawnBaby();
                    }
                }
                //Stage2
                else if (GameMng.instance.curStage == 1)
                {
                    // 새끼 5마리 소환
                    if (Random.Range(0.0f, 1.0f) < layRate)
                    {
                        for (int i = 0; i < 5; i++)
                            SpawnBaby();
                    }
                }
                //Stage3
                else if (GameMng.instance.curStage == 2)
                {
                    // 새끼 5마리 소환
                    if (Random.Range(0.0f, 1.0f) < layRate)
                    {
                        for (int i = 0; i < 5; i++)
                            SpawnBaby();
                    }
                }
                //마지막스테이지
                else if (GameMng.instance.curStage == 3)
                {
                    // 가장 높은 확률로 죽을때 새끼 3마리 소환
                    if (Random.Range(0.0f, 1.0f) < layRate)
                    {
                        for (int i = 0; i < 3; i++)
                            SpawnBaby();
                    }
                }

                // 아기 몬스터 소환 후 몬스터 삭제
                Destroy(this.gameObject);
            }
        }
        // 아기 몬스터가 공에 맞은 경우
        if (this.gameObject.tag == "BabyMonster")
        {
            if (other.gameObject.tag == "AttackItem")
            {
                // 공에 맞으면 이펙트 발생
                GameMng.instance.PlayEffect(this.gameObject.transform.position, 1);

                // 돈 증가
                GameMng.instance.AddMoney(5);
                // 공에 맞은 아기 몬스터 삭제
                Destroy(this.gameObject);
            }
        }

        // 패배 시 팝업 메세지 출력
        if (GameMng.instance.gameState == GameState.Defeat)
        {
            MainMng.instance.PopupMsg("패배", 4);
        }
    }

    private void Start()
    {
        // 튜토리얼이 아닌경우
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            // 몬스터와 아기몬스터인경우
            if (this.gameObject.tag == "Monster" || this.gameObject.tag == "BabyMonster")
            {
                // 소환되고 7초가 지나면 삭제
                Destroy(this.gameObject, 7.0f);
            }
        }
    }

    private void Update()
    {
        // 튜토리얼이 아닌경우
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            //최종보스가 죽은후 남아있는 몬스터와 아기몬스터 삭제
            if (GameMng.instance.imgHp[3].fillAmount == 0)
            {
                // 몬스터태그가 있는 오브젝트 삭제
                GameObject tempObj = GameObject.FindWithTag("Monster");
                Destroy(tempObj);

                // 아기몬스터태그가 있는 오브젝트 삭제
                GameObject tempBabyuObj = GameObject.FindWithTag("BabyMonster");
                Destroy(tempBabyuObj);
            }

            // 게임을 패배한 경우
            if (GameMng.instance.gameState == GameState.Defeat)
            {
                // 메뉴로 돌아기기 버튼 활성화
                GameMng.instance.goMenuBtn.SetActive(true);
                // 리플레이 버튼 활성화
                GameMng.instance.replayBtn.SetActive(true);

                // 패배 오브젝트 활성화
                for (int i = 0; i < GameMng.instance.Defeat.Length; i++)
                    GameMng.instance.Defeat[i].SetActive(true);
                
                // 공격아이템 비활성화
                GameMng.instance.attackItem.SetActive(false);
                // 오디오 비활성화
                GameMng.instance.ads.enabled = false;
            }

            // 몬스터가 음식으로 이동
            if (this.gameObject.tag == "Monster")
            {
                // 몬스터가 움직일 위치 설정
                moveDir = GameMng.instance.target.transform.position - this.gameObject.transform.position;
                // y값은 0으로 설정
                moveDir.y = 0;
                // 몬스터가 움직일 방향 설정
                angle = Vector3.SignedAngle(transform.forward, moveDir.normalized, Vector3.up);
                transform.Rotate(0, angle, 0);

                // 음식과 몬스터간의 거리
                distance = Vector3.Distance(gameObject.transform.position, GameMng.instance.target.transform.position);
                // 음식과 거리가 멀수록 속도가 빨라지고 가까워지면 속도가 느려짐
                Vector3 deltaPos = moveDir.normalized * GameMng.instance.speed * distance * Time.deltaTime;
                // 음식으로 이동
                transform.Translate(deltaPos, Space.World);
            }
            // 아기몬스터가 음식으로 이동
            if (this.gameObject.tag == "BabyMonster")
            {
                // 몬스터가 움직일 위치 설정
                moveDir = GameMng.instance.target.transform.position - this.gameObject.transform.position;
                // y값은 0으로 설정
                moveDir.y = 0;
                // 몬스터가 움직일 방향 설정
                angle = Vector3.SignedAngle(transform.forward, moveDir.normalized, Vector3.up);
                transform.Rotate(0, angle, 0);

                // 음식과 몬스터간의 거리
                distance = Vector3.Distance(gameObject.transform.position, GameMng.instance.target.transform.position);
                // 음식과 거리가 멀수록 속도가 빨라지고 가까워지면 속도가 느려짐
                Vector3 deltaPos = moveDir.normalized * GameMng.instance.speed * distance * Time.deltaTime * 2.0f;
                // 음식으로 몬스터보다 2배 빠르게 이동
                transform.Translate(deltaPos, Space.World);
            }
        }
    }

    // 죽은 몬스터 되살리기 ( 튜토리얼 용 )
    void Revive()
    {
        // 게임 오브젝트 활성화
        gameObject.SetActive(true);
    }

    // 아기 몬스터 소환
    void SpawnBaby()
    {
        // 몬스터 생성
        GameObject obj = Instantiate(baby);

        // 스폰위치 설정
        Vector3 randPos;
        randPos.x = Random.Range(-0.1f, 0.1f);  // x 값은 -0.1에서 0.1 범위의 임의의 값으로 설정
        randPos.y = 0;                          // y 값은 0으로 설정
        randPos.z = Random.Range(-0.1f, 0.1f);  // z 값은 -0.1에서 0.1 범위의 임의의 값으로 설정

        // 부모 몬스터 위치에 randPos떨어진 만큼의 위치로 설정
        obj.transform.position = gameObject.transform.position + randPos;

        // 현재 stage의 자식오브젝트로 소환
        obj.transform.SetParent(GameMng.instance.stages[GameMng.instance.curStage].transform, true);
    }
}
