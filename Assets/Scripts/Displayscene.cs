using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Displayscene : MonoBehaviour
{

  public TextMeshProUGUI sceneNameText; // TextMeshProUGUIオブジェクト

    // 初期化
  void Start()
  {
    // 現在のシーン名を初期化時に設定
    UpdateSceneName();
    sceneNameText = GetComponent<TextMeshProUGUI>();
  }

  // シーン名を更新するメソッド
  private void UpdateSceneName()
  {
    if (sceneNameText != null)
      {
        sceneNameText.text = SceneManager.GetActiveScene().name;
      }
  }
}
