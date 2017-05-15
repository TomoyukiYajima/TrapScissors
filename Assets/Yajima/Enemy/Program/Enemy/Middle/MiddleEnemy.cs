using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MiddleEnemy : Enemy3D {

    #region シリアライズ
    [SerializeField]
    protected bool m_IsLiftWait = false;        // 動物を持ってくる時に、一度待機するか
    #endregion

    #region protected変数
    protected float m_MoveDegree = 0.0f;        // 移動角度
    protected GameObject m_DiscoverObj;         // 発見したオブジェクト
    protected Enemy3D m_Animal = null;          // 発見した動物
                                                //protected DiscoverMoveState m_DiscoverMoveState =
                                                //    DiscoverMoveState.DiscoverMove_Animal;            // 発見状態
    #endregion

    #region 列挙クラス
    //// 餌
    //protected enum DiscoverMoveState
    //{
    //    DiscoverMove_Animal,      // 発見移動
    //    DiscoverMove_Lift,      // 持ち上げ状態
    //    DiscoverMove_TakeAway   // 持ち帰り状態
    //}
    #endregion

    #region 状態関数
    //protected override void Idel(float deltaTime)
    //{
    //    //if (InObject("RabbitEnemy", out m_DiscoverObj))
    //    // if (IsInTrapAnimal(out m_DiscoverObj))

    //    // トラバサミの持っているターゲットを取得
    //    var trapName = "RabbitEnemy";//"sample1(Clone)";
    //    // 餌である動物を見つけたら、動物のもとに行く
    //    if (InObject(trapName, out m_DiscoverObj))
    //    {
    //        // えさ発見状態に遷移
    //        //m_DiscoverObj
    //        //m_DSNumber = DSNumber.DISCOVERED_FEED_NUMBER;
    //        //ChangeState(State.Discover, AnimationNumber.ANIME_DISCOVER_NUMBER);
    //        ChangeDiscoverMoveState(DiscoverMoveState.DiscoverMove_Animal);
    //        //var enemy = m_DiscoverObj.GetComponent<Enemy3D>();
    //        //if (enemy == null) return;
    //        //m_Animal = enemy;
    //        // 移動ポイントを動物に変更
    //        ChangeMovePoint(m_DiscoverObj.transform.position);
    //        ChangeSpriteColor(Color.magenta);
    //        return;


    //        //// えさ発見状態に遷移
    //        //var trap = m_DiscoverObj.GetComponent<Trap_Small>():
    //        //if (trap == null) return;
    //        ////var animal = trap.t
    //        ////m_DSNumber = DSNumber.DISCOVERED_FEED_NUMBER;
    //        ////ChangeState(State.Discover, AnimationNumber.ANIME_DISCOVER_NUMBER);
    //        //ChangeDiscoverMoveState(DiscoverMoveState.DiscoverMove_Animal);
    //        ////var enemy = m_DiscoverObj.GetComponent<Enemy3D>();
    //        ////if (enemy == null) return;
    //        ////m_Animal = enemy;
    //        //// 移動ポイントを動物に変更
    //        //ChangeMovePoint(m_DiscoverObj.transform.position);
    //        //ChangeSpriteColor(Color.magenta);
    //        //return;
    //    }

    //    // 親の移動
    //    base.Idel(deltaTime);
    //}

    //protected override void DiscoverMove(float deltaTime)
    //{
    //    switch (m_DiscoverMoveState)
    //    {
    //        case DiscoverMoveState.DiscoverMove_Animal: DiscoverMoveAnimal(deltaTime); break;
    //        case DiscoverMoveState.DiscoverMove_Lift: DiscoverMoveLift(deltaTime); break;
    //        case DiscoverMoveState.DiscoverMove_TakeAway: DiscoverMoveTakeAway(deltaTime); break;
    //    }
    //    //ChangeMovePoint(m_Animal.transform.position);
    //    //m_Agent.Resume();
    //}
    #endregion

    #region 発見状態派生関数
    //// 発見状態の変更
    //protected void ChangeDiscoverMoveState(DiscoverMoveState dState)
    //{
    //    ChangeState(State.DiscoverMove, AnimationNumber.ANIME_DISCOVER_NUMBER);
    //    // 同じ行動なら返す
    //    if (m_DiscoverMoveState == dState) return;
    //    m_DiscoverMoveState = dState;
    //    m_StateTimer = 0.0f;
    //}

    //protected void DiscoverMoveAnimal(float deltaTime)
    //{
    //    //// オブジェクトとの方向を計算
    //    //var addPos = Vector3.up * (m_DiscoverObj.transform.localScale.y * 0.8f);
    //    //var dir = ObjectDirection(m_DiscoverObj, addPos);

    //    //var length = Vector3.Distance(
    //    //    this.transform.position,
    //    //    m_DiscoverObj.transform.position + addPos
    //    //    );

    //    var length = Vector3.Distance(
    //        m_Agent.destination, this.transform.position
    //        );

    //    if (length < 0.5f)
    //    {
    //        // 持ち上げ状態に遷移
    //        ChangeDiscoverMoveState(DiscoverMoveState.DiscoverMove_Lift);
    //        ChangeSpriteColor(Color.yellow);
    //        return;
    //    }
    //}

    // 持ち上げ行動
    //protected void DiscoverMoveLift(float deltaTime)
    //{
    //    // 仮
    //    if (m_StateTimer < 1.0f) return;
    //    //// 持った動物の親を子供に追加する
    //    //m_Animal.transform.parent.parent = gameObject.transform;
    //    //m_Animal.EnemyLift();

    //    //var q = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    //    //m_Animal.transform.parent.rotation = 
    //    //    m_Animal.transform.rotation = q;

    //    //m_Animal.transform.position = Vector3.zero;
    //    // 持ち上げ終了したら、持ち帰り状態に遷移
    //    ChangeDiscoverMoveState(DiscoverMoveState.DiscoverMove_TakeAway);
    //    // 生成ボックスの移動ポイント取得
    //    var box = this.transform.parent.GetComponentInParent<EnemyCreateBox>();
    //    // 初期位置に移動
    //    ChangeMovePoint(box.GetMovePoint(0).position);
    //    m_Agent.Resume();
    //}

    // 持ち帰り行動
    //protected virtual void DiscoverMoveTakeAway(float deltaTime)
    //{
    //    var length = Vector3.Distance(
    //        m_Agent.destination, this.transform.position
    //        );
    //    // 移動ポイントに到達したら消える
    //    if (length > 0.5f) return;
    //    ChangeState(State.DeadIdel, AnimationNumber.ANIME_DEAD_NUMBER);
    //    // ステータスの初期化
    //    InitState();
    //}
    #endregion

    #region override関数
    public override void SoundNotice(Transform point)
    {
        base.SoundNotice(point);
    }
    // トラバサミに当たった時の行動です
    protected override void TrapHitAction()
    {
        ChangeState(State.Runaway, AnimationNumber.ANIME_RUNAWAY_NUMBER);
        ChangeSpriteColor(Color.white);
    }
    #endregion

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(MiddleEnemy), true)]
    [CanEditMultipleObjects]
    public class MiddleEnemyEditor : Enemy3DEditor
    {
        SerializedProperty IsLiftWait;

        protected override void OnChildEnable()
        {
            IsLiftWait = serializedObject.FindProperty("m_IsLiftWait");
        }

        protected override void OnChildInspectorGUI()
        {
            MiddleEnemy middleEnemy = target as MiddleEnemy;

            EditorGUILayout.LabelField("〇中型動物のステータス");
            // bool
            IsLiftWait.boolValue = EditorGUILayout.Toggle("持ち帰る時に待機するか", middleEnemy.m_IsLiftWait);
        }
    }
#endif
    #endregion
}
