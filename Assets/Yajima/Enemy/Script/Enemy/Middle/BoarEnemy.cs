using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoarEnemy : MiddleEnemy {

    #region 変数
    #region シリアライズ変数
    [SerializeField]
    protected GameObject m_RemovePoint = null;  // 逃げるポイント
    #endregion

    #region private変数
    private RunawayPoint m_RunawayPoint;    // 逃げ用ポイント
    private float m_MoveLength = 0.0f;      // 移動距離
    #endregion
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        m_MiddleSENumber = 21;
        m_SENormalTime = 0.2f;
        m_RunawayPoint = m_RemovePoint.GetComponent<RunawayPoint>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        m_RunawayPoint.SetPosition(this.transform.position);
    }
    #endregion

    #region override関数
    protected override int DiscoverAnimal(float deltaTime)
    {
        // 逃げる
        ChangeMovePoint(m_RunawayPoint.gameObject.transform.position);
        m_MoveLength += m_DiscoverSpeed * deltaTime;

        // 壁を発見したとき
        GameObject wall = null;
        Vector3 point = Vector3.zero;
        if (InWall(out wall, out point, 2))
        {
            // 壁と衝突点との外積を求めて、角度を決める
            var up = wall.transform.up;
            var vec = point - wall.transform.position;
            var cross = Vector3.Cross(up, vec);
            var rotate = Mathf.Atan2(vec.z, vec.x);
            // 壁に沿うように逃げる
            m_RunawayPoint.ChangeAddPosition(rotate);
        }

        // 一定距離移動したら、待機状態に遷移
        if (m_MoveLength > 30)
        {
            // 待機状態に遷移
            ChangeState(AnimalState.Idel, AnimalAnimatorNumber.ANIMATOR_IDEL_NUMBER);
            m_MoveLength = 0.0f;
            // 移動速度を変える
            m_Agent.speed = m_Speed;
            m_Agent.isStopped = false;
        }
        return 0;
    }

    protected override void AnimalHit(GameObject animal)
    {
        base.AnimalHit(animal);
        // 逃げポイントの追加位置の変更
        // 前方ベクトルから、角度の取得
        var vec = animal.transform.position - this.transform.position;
        var angle = Mathf.Atan2(vec.z, vec.x);
        m_Mark.ExclamationMark();
        m_RunawayPoint.ChangeAddPosition(angle * Mathf.Rad2Deg - 180);
    }

    protected override void TriggerEnterObject(Collider other)
    {
        base.TriggerEnterObject(other);

        var objName = other.name;
        // 攻撃判定との衝突判定
        // お肉状態に遷移
        if (objName == "AttackCollider") ChangeMeat();
    }

    //protected override void TriggerStayObject(Collider other)
    //{
    //    base.TriggerStayObject(other);

    //    var objName = other.name;
    //    // 攻撃判定との衝突判定
    //    // お肉状態に遷移
    //    if (objName == "AttackCollider") ChangeMeat();
    //}

    protected override bool IsFoodCheck(Food.Food_Kind food)
    {
        return food == Food.Food_Kind.Carrot || food == Food.Food_Kind.SmellMeat;
    }

    protected override bool IsLikeFood(Food.Food_Kind food)
    {
        return food == Food.Food_Kind.Carrot;
    }
    #endregion
    #endregion

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(BoarEnemy), true)]
    [CanEditMultipleObjects]
    public class BoarEditor : MiddleEnemyEditor
    {
        SerializedProperty RemovePoint;

        protected override void OnChildEnable()
        {
            RemovePoint = serializedObject.FindProperty("m_RemovePoint");
        }

        protected override void OnChildInspectorGUI()
        {
            BoarEnemy enemy = target as BoarEnemy;

            EditorGUILayout.LabelField("〇イノシシ固有のステータス");
            // GameObject
            RemovePoint.objectReferenceValue = EditorGUILayout.ObjectField("逃げポイント", enemy.m_RemovePoint, typeof(GameObject), true);

        }
    }
#endif
    #endregion
}
