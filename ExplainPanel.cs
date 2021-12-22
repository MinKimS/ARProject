using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainPanel : MonoBehaviour
{
    // 설명패널 오브젝트
    public GameObject[] exPanel;

    // 다음 설명 버튼
    public GameObject RightBtn;
    // 이전 설명 버튼
    public GameObject LeftBtn;

    // 설명패널 인덱스
    public int exIndex = 0;

    // 닫기 버튼
    public GameObject exBtn;

    // 설명 창을 보여준다
    public void ShowExPanel()
    {
        // 전에 활성화되있던 설명패널을 비활성화
        exPanel[exIndex].SetActive(false);
        // 설명패널의 인덱스를 0으로 설정
        exIndex = 0;
        // 첫번째 설명패널을 활성화
        exPanel[0].SetActive(true);

        // 이전 버튼 비활성화
        LeftBtn.SetActive(false);
        // 다음 버튼 활성화
        RightBtn.SetActive(true);

        // 설명창 활성화
        gameObject.SetActive(true);
        
        // 설명창이 활성화상태를 참으로 설정
        MainMng.instance.uiShow = true;
    }
    // 설명 창을 감춘다
    public void HideExPanel()
    {
        // 설명창 비활성화
        gameObject.SetActive(false);

        Invoke("uiShowFalse", 0.2f);
    }

    // 다음 버튼
    public void Right()
    {
        // 현재 설명패널을 비활성화
        exPanel[exIndex].SetActive(false);

        // 다음 설명 패널을 활성화
        ++exIndex;
        exPanel[exIndex].SetActive(true);

        // 마지막 패널이 활성화상태면 다음버튼을 비활성화
        if (exPanel[exPanel.Length-1].activeSelf == true)
        {
            RightBtn.SetActive(false);
        }

        // 두번째 패널이 활성화상태면 이전버튼 활성화
        if (exPanel[1].activeSelf == true)
        {
            LeftBtn.SetActive(true);
        }
    }

    // 이전 버튼
    public void Left()
    {
        // 현재 설명패널을 비활성화
        exPanel[exIndex].SetActive(false);

        // 이전 설명 패널을 비활성화
        --exIndex;
        exPanel[exIndex].SetActive(true);

        // 첫번재 패널이 활성화상태면 이전버튼을 비활성화
        if (exPanel[0].activeSelf == true)
        {
            LeftBtn.SetActive(false);
        }

        // 마지막의 이전 패널이 활성화 상태면 다음버튼 활성화
        if (exPanel[exPanel.Length - 2].activeSelf == true)
        {
            RightBtn.SetActive(true);
        }
    }

    // 설명 창의 활성화상태를 거짓으로 설정
    void uiShowFalse()
    {
        MainMng.instance.uiShow = false;
    }
}
