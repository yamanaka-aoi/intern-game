using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapControll : MonoBehaviour
{
    // 矢の方向を表す列挙体
    public enum ArrowDirection{
        Up,
        Right,
        Left,
        Down
    };

    [SerializeField] private GameObject ArrowPoint; // 矢の発射座標オブジェクト
    [SerializeField] private GameObject Arrow; // 矢のプレハブ
    [SerializeField] private float speed = 30f; // 矢のスピード
    [SerializeField] private float arrowContinueTime = 5f;   // 矢の持続時間
    [SerializeField] private ArrowDirection direction = ArrowDirection.Up; //0:上 1:右 2:下 3:左
    [SerializeField] private bool isThroughWall = false;    // 矢が壁をすり抜けるか

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // プレイヤーがトリガーに入ったか確認
        {
            // 矢をArrowPointの位置から発射
            GameObject arrowInstance = Instantiate(Arrow, ArrowPoint.transform.position, ArrowPoint.transform.rotation);
            
            Rigidbody2D arrowRb = arrowInstance.GetComponent<Rigidbody2D>();

            arrowInstance.GetComponent<Arrow>().SetisThroughWall(isThroughWall);
            if (arrowRb != null)
            {
                // 方向をdirectionによって決め,矢を発射
                switch (direction){
                case ArrowDirection.Up:
                 arrowRb.velocity = Vector2.up * speed;
                 break;
                case ArrowDirection.Right:
                 arrowRb.velocity = Vector2.right * speed;
                 break;
                case ArrowDirection.Down:
                 arrowRb.velocity = Vector2.down * speed;
                 break;
                case ArrowDirection.Left:
                 arrowRb.velocity = Vector2.left * speed;
                 break;
                }

            }

            // 一定時間後に矢を破壊
            Destroy(arrowInstance, arrowContinueTime);
        }
    }
}
