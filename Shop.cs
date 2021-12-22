using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // 아이템 정보
    [System.Serializable]
    public struct ItemType
    {
        public string name;             // 아이템 이름
        public int cost;                // 아이템 가격
        public GameObject BuyBtn;       // 구매버튼
        public GameObject soldOut;      // 매진표시 오브젝트
        public GameObject selectItem;   // 아이템 선택버튼
        public Image selectItemImage;   // 아이템 선택버튼의 이미지
    }
    public ItemType[] itemInfo;

    public Text numText;    // 코인 보유량 텍스트 표시

    // 버튼 색깔
    Color normalColor;  // 일반 버튼 색
    Color selectColor;  // 선택된 버튼 색

    void Start()
    {
        // 팝업메세지가 안보이는 것을 방지
        MainMng.instance.stateMsg.SetActive(false); // 팝업메세지 비활성화
        MainMng.instance.stateMsg.SetActive(true); // 팝업메세지 활성화

        // 상태메세지의 텍스트를 비어있게 만듬
        MainMng.instance.setTextVoid();
        // 보유한 금액을 보유금액란에 표시
        numText.text = MainMng.instance.money + "";

        // 버튼 색깔 설정
        normalColor = new Color(255f, 255f, 255f, 255f);    // 하얀색
        selectColor = new Color(0f, 255f, 255f, 255f);      // 청록색
        
        // 모든 아이템을 설정
        for(int i =0;i<itemInfo.Length;i++)
        {
            // 아이템 버튼을 normalColor로 변경
            itemInfo[i].selectItemImage.color = normalColor;

            // 구입한 아이템에 매진표시
            if (MainMng.instance.bought[i] == true)
            {
                // 매진표시 오브젝트를 활성화
                itemInfo[i].soldOut.SetActive(true);
            }
        }

        // 선택한 아이템의 색 변경
        itemInfo[MainMng.instance.selected].selectItemImage.color = selectColor;

    }

    // 클릭한 버튼의 num번째 아이템 선택
    public void SelectItem(int num)
    {
        // 현재 클릭한 아이템 버튼
        GameObject selectObj = EventSystem.current.currentSelectedGameObject;

        // 구입을 한 아이템 중 클릭한 아이템을 선택
        if (selectObj.name == itemInfo[num].selectItem.name && MainMng.instance.bought[num] == true)
        {
            // 아이템 버튼을 normalColor로 변경
            for (int i = 0; i <itemInfo.Length; i++)
            {
                // 모든 아이템버튼을 일반 버튼 색으로 변경
                itemInfo[i].selectItemImage.color = normalColor;
            }

            // 선택한 아이템의 버튼 색 변경
            itemInfo[num].selectItemImage.color = selectColor;
            // 선택한 아이템의 번호를 저장
            MainMng.instance.selected = num;
        }
    }

    // 클릭한 버튼의 num번째 아이템 구매
    public void Buy(int num)
    {
        // 현재 클릭한 아이템 버튼
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;

        // 보유한 돈이 구매할 아이템 가격과 같거나 많은 경우
        if(MainMng.instance.money >= itemInfo[num].cost)
        {
            // 구입버튼을 누른 아이템을 구입
            if(clickObj.name == itemInfo[num].name)
            {
                itemInfo[num].BuyBtn.GetComponent<Button>().interactable = false;
                MinusMoney(itemInfo[num].cost);

                // 구입한 아이템에 매진표시
                if(MainMng.instance.bought[num] == false)
                {
                    itemInfo[num].soldOut.SetActive(true);
                    // 한번 구입하면 더이상 살 수 없음
                    MainMng.instance.bought[num] = true;
                }
            }
        }
        // 돈이 부족한 경우
        else
        {   // 얼마나 돈이 부족한지 2초동안 나온다.
            MainMng.instance.PopupMsg("돈 부족 : " + (itemInfo[num].cost - MainMng.instance.money), 2);
        }
    }

    // 아이템 가격만큼 코인감소와 코인 보유량 텍스트 변경
    public void MinusMoney(int num)
    {
        // money를 아이템 가격만큼 감소
        MainMng.instance.money -= num;
        // 코인 보유량 텍스트를 변경
        numText.text = MainMng.instance.money + "";
    }
}
