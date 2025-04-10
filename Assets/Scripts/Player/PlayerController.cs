using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // プレイヤーRigidbody
    private SpriteRenderer sprite;  // プレイヤーのスプライト
    private Animator anim;      // プレイヤーのアニメイター
    [SerializeField] private GameObject hookshot;   // HookShotゲームオブジェクト
    private HookShot _hook;     // HookShotスクリプト
    private float hookShotPosX;    // 右向き状態でのフックショットのx座標

    [SerializeField] private IsGroundCheck GrCheck;
    [SerializeField] private float moveSpeed = 2f;

    //[SerializeField] private bool CanMoveinAir = false;     // 空中で動けるか

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        _hook = hookshot.GetComponent<HookShot>();  
        hookShotPosX = hookshot.transform.localPosition.x;
    }

    void Update()
    {   
        if(GameManager.Instance.CanMovePlayer){
            anim.SetBool("isHookShotMove", _hook.isHookShotMove);
            if(Input.GetMouseButtonDown(0)){
                Vector3 mousePosition = Input.mousePosition;
                Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
                Debug.Log((transform.position.x <= target.x) ? "右向き" : "左向き");
                TurnPlayer(transform.position.x <= target.x);       // プレイヤーより右をクリックしたら右向き
            }

            if(GrCheck.isGround && !_hook.isHookShotMove){  // 動いてるとき移動不可と設定！
                float nowHorizontal = Input.GetAxis("Horizontal");  // 水平軸入力情報を得る
                rb.velocity = new Vector2(nowHorizontal * moveSpeed, rb.velocity.y);

                // 水平軸の絶対値が一定値以上なら歩いている
                anim.SetBool("isWalk", Mathf.Abs(nowHorizontal) >= 0.5f);

                // フックショット発射中は振り向き不可
                if(!_hook.isHookShotMove){
                    if(nowHorizontal > 0){
                        TurnPlayer(true);
                    }else if(nowHorizontal < 0){
                        TurnPlayer(false);
                    }
                }
            }else{
                anim.SetBool("isWalk", false);
            }
        }
    }

    public void ResetHookFromPlayer(){
        StartCoroutine(_hook.ResetHook());
    }

    //プレイヤーを左右反転させる関数
    private void TurnPlayer(bool lookRight){
        sprite.flipX = !lookRight;
        if(lookRight){
            // 右向きに即した位置取りに
            hookshot.transform.localPosition = new Vector3(hookShotPosX, hookshot.transform.localPosition.y, hookshot.transform.localPosition.z);
        }else{
            hookshot.transform.localPosition = new Vector3(-1*hookShotPosX, hookshot.transform.localPosition.y, hookshot.transform.localPosition.z);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "WireArrivalPoint"){     // プレイヤーがワイヤ終着点についたなら
            _hook.PlayerArriveWireEndPoint();
        }
    }
    
    /*
    private void OnCollisionEnter2D(Collision2D other){
    }
    */
}
