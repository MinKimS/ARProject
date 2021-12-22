using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetText : MonoBehaviour
{
    // 타겟 이미지가 있으면 활성화
    public void OnTargetFound()
    {
        // 현재 오브젝트 활성화
        gameObject.SetActive(true);

        // 튜토리얼인 경우 설명활성화
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            // 설명창을 활성화
            GameMng.instance.explain.SetActive(true);
            // 이미지 인식 팝업메세지 공백으로 설정
            MainMng.instance.setTextVoid();
        }
    }

    // 타겟 이미지가 없으면 비활성화
    public void OnTargetLost()
    {
        // 현재 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
