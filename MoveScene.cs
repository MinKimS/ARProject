using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    // 버튼에 입력된 씬으로 이동
    public void OnClickLoadScene(string SceneName)
    {
        // 로딩씬을 거쳐서 SceneName으로 씬 이동
        LoadingSceneMng.LoadScene(SceneName);
    }

    // 종료버튼
    public void Quit()
    {
        // 어플리케이션 종료
        Application.Quit();
        print("게임종료");
    }

    // 플레이 버튼
    public void PlayBtn()
    {
        // 게임을 한번도 클리어한적이 없으면 본게임으로 이동
        if (MainMng.instance.gameClear == false)
        {
            // InGame으로 이동
            LoadingSceneMng.LoadScene("InGame");
        }
        // 본 게임을 한번이라도 클리어한 경우 게임선택 활성화
        else
        {
            // 게임선택창 활서와
            gameObject.SetActive(true);
        }
    }
}
