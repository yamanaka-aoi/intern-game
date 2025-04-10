using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BackGroundFader : MonoBehaviour
{
    [SerializeField] private float fadeTime = 2f;   // フェードイン・アウトする時間
    private Image img;  // 黒背景
    void Start()
    {
        img = GetComponent<Image>();
        FadeIn();
    }

    public void FadeIn(){
        // 最初黒く
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        img.DOFade(0, fadeTime).SetEase(Ease.InCubic).OnComplete(() =>
        {
            // フェードしきったら操作可能
            GameManager.Instance.SetCanMovePlayer(true);
        });
    }

    // フェードアウトし、シーン遷移もしてしまう
    public void FadeOutandSceneChange(string sceneName){
        if(GameManager.Instance.CanMovePlayer){
            GameManager.Instance.SetCanMovePlayer(false);
            img.DOFade(1, fadeTime).SetEase(Ease.OutCubic).OnComplete(() => {
                SceneManager.LoadScene(sceneName);
            });
        }  
    }
}
