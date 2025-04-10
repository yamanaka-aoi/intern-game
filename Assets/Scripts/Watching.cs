using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watching : MonoBehaviour
{
    public float rotationInterval = 2.0f;
    private float timer = 0;
    private bool _isFlipping;
    // Start is called before the first frame update
    void Start()
    {
        FlippingSetting();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= rotationInterval)
        {
            Rotation();
            timer = 0; // タイマーをリセット
        }
    }
    private void Rotation()
{
   if(_isFlipping == true) {
    this.transform.rotation = Quaternion.Euler(0,180f,180);
    _isFlipping = false;
   }else if(_isFlipping == false){
    this.transform.rotation = Quaternion.Euler(0,0f,180);
   _isFlipping = true;
   }
   
}
private void FlippingSetting()
    {
        // 現在の回転を取得
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Y軸の回転が180の場合はfalse、0の場合はtrueに設定
        if (Mathf.Approximately(currentRotation.y, 180f))
        {
            _isFlipping = false;
        }
        else if (Mathf.Approximately(currentRotation.y, 0f))
        {
            _isFlipping = true;
        }
    }
}
