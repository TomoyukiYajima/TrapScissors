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
    protected override void DiscoverPlayer(float deltaTime)
    {
        // プレイヤーとのベクトルを計算して、移動ポイントを更新する
        var vec = m_Player.transform.position - this.transform.position;
        var dir = vec.normalized;
        // velocity に移動量を渡すー＞ナビメッシュが自動で速度の補正をしてしまい、
        // 動物が追いつけなくなっているため
        m_Agent.velocity = dir * m_DiscoverSpeed;
        m_Agent.destination = m_Player.transform.position;

        base.DiscoverPlayer(deltaTime);
    }
    #endregion

    #region override関数
    protected override void SetAnimator()
    {
        base.SetAnimator();
        // 攻撃アニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_ATTACK_NUMBER] = "Attack";
        // トラバサミ衝突時のアニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_TRAP_HIT_NUMBER] = "SmallTrapHit";
        // 追跡アニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_CHASE_NUMBER] = "Chase";
        // 壁衝突時のアニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_WALL_HIT_NUMBER] = "WallHit";
        // ぴよりアニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_FAINT_NUMBER] = "Faint";
    }
    //public override void SoundNotice(Transform point)
    //{
    //    base.SoundNotice(point);
    //}

    // 小さいトラバサミに衝突した時の行動です
    protected override void SmallTrapHitAction()
    {
        ChangeTrapHitState(TrapHitState.TrapHit_Runaway);
        m_Agent.isStopped = false;
    }

    // 大きいトラバサミに衝突した時の行動です
    protected override void BigTrapHitAction()
    {
        // トラバサミが解放されたときの行動
        TrapReleaseAction();
        // 死亡状態に変更
        //ChangeState(State.DeadIdel, AnimatorNumber.ANIMATOR_DEAD_NUMBER);
        ChangeAnimation(AnimatorNumber.ANIMATOR_DEAD_NUMBER);
        // トラバサミを空っぽにする
        m_SmallTrap = null;
        // ナビメッシュエージェント関連の初期化
        m_Agent.isStopped = true;
        m_Agent.enabled = false;
        if (m_MovePoints.Length > 0)
            m_MovePointPosition = m_MovePoints[0].position;
        // 自身の衝突判定をオンにする
        m_Collider.enabled = false;
        m_RayPoint.gameObject.SetActive(false);
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
