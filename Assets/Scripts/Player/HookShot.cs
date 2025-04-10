using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HookShot : MonoBehaviour
{
    private LineRenderer line;  // ワイヤーの肝
    private EdgeCollider2D col; // エッジコライダー
    [SerializeField] private Rigidbody2D plRb;  // プレイヤーのRigidbody
    [SerializeField] private TrailRenderer trRe;    // ちょっとリッチにする用の軌跡レンダラー(Playerから設定)
    private Transform parentTrans;  // 親のTransform
    [SerializeField] public bool isHookShotMove {get; private set;} = false;   // 全体でフックショットの動作中か
    private bool isShotHook = false;    // フックショットを発射したか
    private bool isArriveHook = false;  // フックが障害物についたか
    private bool isWrapHook = false;   // フックを巻き戻しているか
    private bool playerArrivedEndPoint = false;     // プレイヤーがフックの終着地点にいるか
    private float lirendScale = 1;        // Linerendererのワイヤ点座標のスケール
    private List<Vector2> linePoints = new List<Vector2>() {Vector2.zero, Vector2.zero};   // ワイヤーの頂点のリスト
    [SerializeField] private GameObject arrivalPoint;   // フックの到着点に作成する感知用オブジェクトのPrefab
    private GameObject _arrivalPoint;        // 上記arrivalPoint、生成したオブジェクト保存用

    [SerializeField] float hookShotSpeed = 5f;  // フックを飛ばす速度
    [SerializeField] float wrapSpeed = 5f;  // フックを巻き取る速度
    [SerializeField] float shotLimit = 2f;  // フックの持続時間
    [SerializeField] float playerGravityScale = 3;  // プレイヤーにかかる重力の大きさ
    private float overWrapLimit = 0.5f;     // バグで過度に巻きつけすぎた際に強制停止する限界の距離

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();
        parentTrans = transform.parent.transform;
        lirendScale = parentTrans.localScale.x; // 親のスケールはx,y同じにすること！
        plRb.gravityScale = playerGravityScale;
        trRe.emitting = false;  // 軌跡をoffに
    }

    private void Update()
    {   
        if(GameManager.Instance.CanMovePlayer){
            if(Input.GetMouseButtonDown(0)){
                if(!isHookShotMove && !isShotHook){    // フックショットする処理
                    // マウスからの入力を取得
                    Vector3 mousePosition = Input.mousePosition;
                    Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
                    Vector3 local_targ = new Vector3(target.x - transform.position.x, target.y - transform.position.y, target.z - transform.position.z);
                    // マウスポインタ方向への単位ベクトル
                    Vector3 normalize_targ = local_targ.normalized;
                    ShotHook(normalize_targ);
                }
            }else if(isHookShotMove && !isWrapHook && isArriveHook && !playerArrivedEndPoint){  // フック巻きつけの処理
                    // すぐ離した際も少し動くように、離す処理の前に記載
                    // 親オブジェクトの位置をもとにワイヤ終着点のワールド座標を生成
                    Vector2 worldArrivalPos = new Vector2(linePoints[1].x + parentTrans.position.x/lirendScale, linePoints[1].y + parentTrans.position.y/lirendScale);
                    WrapHook(worldArrivalPos);
            }else if(isHookShotMove && (isWrapHook || playerArrivedEndPoint) && !Input.GetMouseButton(0)){   // マウスが離されているなら、フックを離す処理
                    StartCoroutine(ResetHook());
                    plRb.gravityScale = playerGravityScale;     // 重力on
            }
        }else{
            //Debug.Log("動けないよ！");
            plRb.velocity = Vector2.zero;
            StartCoroutine(ResetHook());
            trRe.emitting = false;
        }
    }

    // 伸ばしているフックショットをリセットする
    public IEnumerator ResetHook(){
        Debug.Log("フックをリセットしたよ");
        linePoints[1] = Vector2.zero;
        line.SetPositions(Vector2to3(linePoints));
        col.SetPoints(linePoints);
        // 各種フックショットフラグもおろす
        isArriveHook = false;
        isWrapHook = false;
        isShotHook = false;
        playerArrivedEndPoint = false;
        // 衝突感知オブジェクトも削除
        Destroy(_arrivalPoint);
        // 親関係も削除
        parentTrans.SetParent(null);
        // 軌跡も切る
        StartCoroutine(TrailOff());

        plRb.gravityScale = playerGravityScale;

        // 即離した際のバグ挙動修正のため1F猶予
        yield return null;
        isHookShotMove = false;
    }
    private void MakeLinePoints(Vector2 endPos){
        Vector2 startPos = new Vector2(0, 0);   // 始点(手元)
        linePoints[0] = startPos;
        linePoints[1] = endPos;
    }

    /// Vector2をVector3に変換(LineRendererのSetPosition用)
    private Vector3[] Vector2to3(List<Vector2> vec2){
        Vector3[] tmp = {Vector3.zero, Vector3.zero};
        for(int i=0;i < vec2.Count;i++){
            tmp[i] = new Vector3 (vec2[i].x, vec2[i].y, 0);
        }
        return tmp; 
    }

    /// フックショットを発射するための関数。_ShotHookコルーチンを呼び出す。
    /// targetVectorは単位ベクトルを渡し、その方向にフックが飛ぶ
    public void ShotHook(Vector2 targetVector){
        if(!isShotHook) isShotHook = true;  // 飛ばしているなら飛ばしておく
        isHookShotMove = true;
        // フックショット発射時空中停止
        plRb.velocity = Vector2.zero;
        plRb.gravityScale = 0;
        StartCoroutine(_ShotHook(targetVector));
    }

    private IEnumerator _ShotHook(Vector2 targetVector){
        Debug.Log("伸ばしてます！");
        Vector2 wirePoint = new Vector2(0, 0);
        float limitTimeCount = 0f;     // ワイヤー持続時間
        while(isShotHook){
            wirePoint += targetVector * hookShotSpeed * Time.deltaTime; // ワイヤを伸ばす
            // 位置を適用
            MakeLinePoints(wirePoint);
            line.SetPositions(Vector2to3(linePoints));
            col.SetPoints(linePoints);

            limitTimeCount += Time.deltaTime;   // タイマー加算
            if(limitTimeCount >= shotLimit) StartCoroutine(ResetHook());    // 持続時間を超過したならリセット

            yield return null;
        }
    }
    private void WrapHook(Vector2 wireEndPoint){
        Debug.Log("巻きます！ 目指すは " + wireEndPoint);
        isWrapHook = true;
        // 巻きつけ始めたら軌跡表示
        trRe.emitting = true;
        StartCoroutine(_WrapHook(wireEndPoint));
    }

    private IEnumerator _WrapHook(Vector2 wireEndPoint){
        Vector2 velocityVector = linePoints[1].normalized;   // 移動方向のベクトルをワイヤ到着地点に
        plRb.gravityScale = 0;  // 移動中無重力に
        Vector2 worldLimitWrapPos = wireEndPoint*lirendScale;
        bool _goRight = worldLimitWrapPos.x >= transform.position.x;
        bool _goUp = worldLimitWrapPos.y >= transform.position.y;
        Vector2 wirePoint = new Vector2(0, 0);
        while(isWrapHook){
            // プレイヤーの位置から元々の位置に固定するように
            wirePoint = new Vector2(wireEndPoint.x - parentTrans.position.x/lirendScale, wireEndPoint.y - parentTrans.position.y/lirendScale);
            // ワイヤーの描画
            MakeLinePoints(wirePoint);
            line.SetPositions(Vector2to3(linePoints));
            col.SetPoints(linePoints);
            
            plRb.velocity = velocityVector * wrapSpeed;

            // playerが移動限界距離範囲に存在したら、巻き取り停止
            // その判定をする関数
            JudgeisBugMove(_goRight, _goUp, worldLimitWrapPos);
            yield return null;
        }
        // 巻きつけが終了したら軌跡をオフに
        if(!isWrapHook){
            StartCoroutine(TrailOff());
        }
    }

    private void JudgeisBugMove(bool _right, bool _up, Vector2 wireEndPos){      // 巻き取りがバグの動きか判定
        bool isBug = false;
        if(_right && !isBug){
            isBug = (wireEndPos.x + overWrapLimit) <= transform.position.x;
        }else{
            isBug = (wireEndPos.x - overWrapLimit) >= transform.position.x;
        }

        if(_up && !isBug){
            isBug = (wireEndPos.y + overWrapLimit) <= transform.position.y;
        }else{
            isBug = (wireEndPos.y - overWrapLimit) >= transform.position.y;
        }

        // バグ判定時のイベント
        if(isBug){
            Debug.Log("バグ判定！");
            PlayerArriveWireEndPoint();
        }
    }

    private IEnumerator TrailOff(){
        yield return new WaitForSeconds(0.5f);
        trRe.emitting = false;
    }

    public void PlayerArriveWireEndPoint(){
        if(isHookShotMove && isWrapHook){
            Debug.Log("プレイヤー到着!");
            playerArrivedEndPoint = true;
            isWrapHook = false;
            plRb.velocity = Vector2.zero;   // 到着したら速度をゼロに
        }  
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(isHookShotMove && other.gameObject.tag == "Wall"){
            isShotHook = false;  // 壁に付いたらショットしきった
            isArriveHook = true;  // 巻き戻しに遷移
            Debug.Log(other.bounds.ClosestPoint(this.transform.position));
            // 衝突地点 LineRendererの到着点から計算
            var _hitPos = lirendScale * (linePoints[1] + (Vector2)parentTrans.position + (Vector2)transform.position);
            _hitPos -= linePoints[1].normalized * 0.5f;     // めりこみ加味して少しワイヤーの手前側に
            Debug.Log("ここにヒット:" + _hitPos);
            if(_arrivalPoint != null)Destroy(_arrivalPoint);
            _arrivalPoint = Instantiate(arrivalPoint,_hitPos,Quaternion.identity);      // 衝突地オブジェクト生成

            parentTrans.SetParent(other.transform);
        }
    }
}
