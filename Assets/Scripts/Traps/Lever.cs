using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private float life_time = 1.5f; // 出現する時間
    [SerializeField] private GameObject disappearingBlock; // 出現/消失するオブジェクト
    private float timer = 0f;
    private bool isPlayerTriggered = false;

    // Startメソッドの初期化処理
    void Start() {
        if (disappearingBlock != null) {
            disappearingBlock.SetActive(false); // 初期状態で非表示
        }
    }

    // Playerが触れたときに呼ばれるメソッド
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) { // Playerタグを持つオブジェクトに触れた場合
            if (disappearingBlock != null && !isPlayerTriggered) {
                Debug.Log("レバー触れた！カウント開始!");
                disappearingBlock.SetActive(true); // オブジェクトを表示
                isPlayerTriggered = true;
                timer = 0f; // タイマーをリセット
            }
        }
    }

    // 毎フレーム呼び出されるUpdateメソッド
    void Update() {

        // プレイヤーが動けない状態 = 死の時
        if(!GameManager.Instance.CanMovePlayer){
            // 設定をリセット
            isPlayerTriggered = false;
            timer = 0f;
            disappearingBlock.SetActive(false);
        }

        if (isPlayerTriggered) {
            timer += Time.deltaTime;
            if (timer > life_time) {
                if (disappearingBlock != null) {
                    var _pl = disappearingBlock.transform.Find("Player");
                    if(_pl != null){
                        // 子供にプレイヤーがいるならフックを離す
                        _pl.GetComponent<PlayerController>().ResetHookFromPlayer();
                    }
                    disappearingBlock.SetActive(false); // オブジェクトを非表示
                }
                isPlayerTriggered = false; // トリガーをリセット
            }
        }
    }
}

