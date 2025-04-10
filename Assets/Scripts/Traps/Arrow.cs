using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
 {
    private bool _isThroughWall = false;        // 矢が持つ壁をすり抜けるかどうかの判定
    private bool willDestroy = false;       // すぐ消去する

    private void Update()
    {
        // プレイヤーが死んだら消去
        if(!GameManager.Instance.CanMovePlayer && !willDestroy){
            willDestroy = true;
            Destroy(this.gameObject, 0.4f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // プレイヤーに当たったときの処理
            Debug.Log("Player hit!");
            GameManager.Instance.LoadGame();  
        }
        else if (collision.CompareTag("Wall") && !_isThroughWall)
        {
            // 壁に当たったときの処理
            Destroy(gameObject);
        }
    }

    public void SetisThroughWall(bool isThrough){
        _isThroughWall = isThrough;
    }
}

