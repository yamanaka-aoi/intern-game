using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    float timeCount = 0; // 経過時間
    [SerializeField] float shotAngle = 0; // 発射角度
    [SerializeField] float shotSpeed = 2;//発射速度

    [SerializeField] float interval = 2;//発射間隔
    [SerializeField] GameObject shotBullet; // 発射する弾

    void Start()
    {
        // 何も書かない
    }

    void Update()
    {
        // 前フレームからの時間の差を加算
        timeCount += Time.deltaTime;

        // 0.1秒を超えているか
        if (timeCount > interval)
        {
            timeCount = 0; // 再発射のために時間をリセット

            //shotAngle += 15;

            // GameObjectを新たに生成する
            // 第一引数：生成するGameObject
            // 第二引数：生成する座標
            // 第三引数：生成する角度
            // 戻り値：生成したGameObject
            GameObject createObject = Instantiate(shotBullet, transform.position, Quaternion.identity);

            // 生成したGameObjectに設定されている、Bulletスクリプトを取得する
            Bullet1 bulletScript = createObject.GetComponent<Bullet1>();

            // BulletスクリプトのInitを呼び出す
            bulletScript.Init(shotAngle, shotSpeed);
        }
    }
}

