using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MiddleEnemy : Enemy3D {

    #region 変数
    #region シリアライズ
    [SerializeField]
    protected bool m_IsLiftWait = false;        // 動物を持ってくる時に、一度待機するか
    [SerializeField]
    protected GameObject m_AttackCollider;      // 攻撃判定
    #endregion

    #region protected変数
    protected float m_MoveDegree = 0.0f;        // 移動角度
    protected GameObject m_DiscoverObj;         // 発見したオブジェクト
    protected Enemy3D m_Animal = null;          // 発見した動物
                                                //protected DiscoverMoveState m_DiscoverMoveState =
                                                //    DiscoverMoveState.DiscoverMove_Animal;            // 発見状態
    #endregion
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

    #region 関数
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

    protected override void DiscoverPlayer(float deltaTime)
    {
        //// プレイヤーとのベクトルを計算して、移動ポイントを更新する
        var vec = m_Player.transform.position - this.transform.position;
        var dir = vec.normalized;
        //var point = m_Player.transform.position + dir * 10.0f;
        //m_Agent.destination = point;
        //m_Agent.speed = m_Speed * 1.5f;
        // velocity に移動量を渡すー＞ナビメッシュが自動で速度の補正をしてしまい、
        // 動物が追いつけなくなっているため
        m_Agent.velocity = dir * m_DiscoverSpeed; //m_Speed;
        m_Agent.destination = m_Player.transform.position;

        base.DiscoverPlayer(deltaTime);
    }
    #endregion

    #region override関数
    public override void SoundNotice(Transform point)
    {
        base.SoundNotice(point);
    }
    //// トラバサミに当たった時の行動です
    //protected override void TrapHitAction()
    //{
    //    base.TrapHitAction();

    //    ChangeState(State.Runaway, AnimationNumber.ANIME_RUNAWAY_NUMBER);
    //    ChangeSpriteColor(Color.white);
    //}

    // 小さいトラバサミに衝突した時の行動です
    protected override void SmallTrapHitAction()
    {
        //ChangeState(State.Runaway, AnimationNumber.ANIME_RUNAWAY_NUMBER);
        ChangeTrapHitState(TrapHitState.TrapHit_Runaway);
        ChangeSpriteColor(Color.white);
    }

    // 大きいトラバサミに衝突した時の行動です
    protected override void BigTrapHitAction()
    {
        ////base.SmallTrapHitAction();
        //// 死亡待機状態に遷移
        //ChangeState(State.DeadIdel, AnimationNumber.ANIME_DEAD_NUMBER);
        //// 肉UIの生成
        //CreateMeat(AnimalMeat.MeatNumber.SMALL_NUMBER);
        ////// トラバサミが解放されたときの行動
        ////TrapReleaseAction();
        ////// ステータスの初期化
        ////InitState();

        // 肉UIの生成
        CreateMeat(AnimalMeat.MeatNumber.SMALL_NUMBER);
        // トラバサミが解放されたときの行動
        TrapReleaseAction();
        // 動物の消去
        DeadAnimal();
    }
    protected override void AnimalHit(GameObject animal)
    {
        base.AnimalHit(animal);
        m_Mark.ExclamationMark();
    }
    #endregion
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
        SerializedProperty AttackCollider;

        protected override void OnChildEnable()
        {
            IsLiftWait = serializedObject.FindProperty("m_IsLiftWait");
            AttackCollider = serializedObject.FindProperty("m_AttackCollider");
        }

        protected override void OnChildInspectorGUI()
        {
            MiddleEnemy middleEnemy = target as MiddleEnemy;

            EditorGUILayout.LabelField("〇中型動物のステータス");
            // bool
            IsLiftWait.boolValue = EditorGUILayout.Toggle("持ち帰る時に待機するか", middleEnemy.m_IsLiftWait);
            AttackCollider.objectReferenceValue = EditorGUILayout.ObjectField("攻撃判定", middleEnemy.m_AttackCollider, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
