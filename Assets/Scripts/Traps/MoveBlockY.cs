using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockY : MonoBehaviour
{
    [SerializeField] float speed = 2f ;  //移動するブロックのスピード
    [SerializeField] float moveRange = 5f; //移動の範囲
    private Vector3 startPosition; //初期の生成位置

    private bool moveup=true; //移動方向

    void Start()
    {
        startPosition = transform.position; //初期位置を保存
    } 

    void Update()
    {
        Move();
    }

    void Move()
    {
        if(moveup)
        {
            //右方向に移動する
            transform.position += Vector3.up * speed *Time.deltaTime;
            //もし現在地が右端に達したら
            if(transform.position.y >= startPosition.y + moveRange)
                moveup = false;
        }
        else
        {
            //左方向にspeedの速さで移動する
            transform.position -= Vector3.up* speed * Time.deltaTime;
            //もし左端に達したら
            if(transform.position.y <= startPosition.y - moveRange)
            moveup = true;

        }
        
     }

}
