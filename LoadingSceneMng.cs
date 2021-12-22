using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneMng : MonoBehaviour
{
    public static string nextScene;     // 이동될 씬이름

    [SerializeField]
    Image progressBar;      // 로딩 진행 바

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    // 씬을 로드할때 로딩시간에 로링씬을 보여줌
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        // 로딩 씬 불러오기
        SceneManager.LoadScene("Loading");
    }
    IEnumerator LoadScene()
    {
        yield return null;
        
        // 비동기적 방식으로 씬 불러옴
        // 씬을 불러오는 진행상황을 
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        
        // nextScene이 준비되도 이동하지 않고 기다림
        op.allowSceneActivation = false;
        float timer = 0.0f;     // 시간측정

        // 씬 로딩이 끝날때까지 반복
        while(!op.isDone)
        {
            yield return null;  // 화면의 로딩바 진행
            timer += Time.deltaTime;

            // 90%까지 진행될때까지 로딩진행도에 따라서 진행바를 채움
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if(progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            // 나머지 10%를 1초에 거쳐서 채운뒤 씬 로드
            else
            {
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                // 로딩 진행 바가 다 채워지면 nextScene을 불러옴
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
