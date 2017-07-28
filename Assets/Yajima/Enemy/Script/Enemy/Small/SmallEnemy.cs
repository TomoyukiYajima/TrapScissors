using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SmallEnemy : Enemy3D
{
    #region 変数
    #region シリアライズ変数
    //[SerializeField]
    //protected GameObject m_Canvas = null;               // キャンバス
    //[SerializeField]
    //protected GameObject m_MeatUI = null;               // お肉UI
    [SerializeField]
    protected GameObject m_RemovePoint = null;  // 逃げるポイント
    #endregion

    #region private関数
    //private GameObject m_Frame;             // キャンバスのフレーム
    private RunawayPoint m_RunawayPoint;    // 逃げ用ポイント
    private float m_MoveLength = 0.0f;      // 移動距離
    private List<Transform> m_BoxPoints =
        new List<Transform>();              // 移動用ポイントコンテナ
    private Dictionary<Transform, int> m_ResultPoints =
        new Dictionary<Transform, int>();   // 移動ポイントの評価
    #endregion
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        m_RunawayPoint = m_RemovePoint.GetComponent<RunawayPoint>();
    }

    protected override void Update()
    {
        base.Update();
        m_RunawayPoint.SetPosition(this.transform.position);
    }

    protected override int DiscoverPlayer(float deltaTime)
    {
        // 移動(通常の移動速度の数倍)
        Move(deltaTime, m_DiscoverSpeed);
        m_MoveLength += m_DiscoverSpeed * deltaTime;
        ChangeMovePoint(m_RunawayPoint.transform.position);
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

        if (m_MoveLength > 20)
        {
            // 待機状態に遷移
            ChangeState(AnimalState.Idel, AnimalAnimatorNumber.ANIMATOR_IDEL_NUMBER);
            m_Agent.isStopped = false;
            m_Player = null;
            // 移動速度を変える
            m_Agent.speed = m_Speed;
            m_MoveLength = 0.0f;
            ChangeSpriteColor(Color.red);
            return 0;
        };
        return 0;
    }

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
            var up = wall.transform.up;
            var degree = 0.0f;
            var wallCollider = wall.GetComponent<BoxCollider>();
            var vec = point - wall.transform.position;
            // 壁と衝突点との外積を求めて、角度を決める
            var cross = Vector3.Cross(vec, up);
            degree = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;
            // 壁に沿うように逃げる
            m_RunawayPoint.ChangeAddPosition(degree);
        }

        // 一定距離移動したら、待機状態に遷移
        if (m_MoveLength > 20)
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
    #endregion

    #region override関数
    //protected override void SearchObject()
    //{
    //    // トラバサミの捜索
    //    SearchTrap();
    //    // 反応する動物の捜索
    //    SearchAnimal();
    //    // プレイヤーの捜索
    //    SearchPlayer();
    //}

    protected override void ChangePlayerHitMove(GameObject player)
    {
        base.ChangePlayerHitMove(player);
        // 逃げポイントの追加位置の変更
        // 前方ベクトルから、角度の取得
        var vec = player.transform.position - this.transform.position;
        var angle = Mathf.Atan2(vec.z, vec.x);
        m_RunawayPoint.ChangeAddPosition(angle * Mathf.Rad2Deg - 180);
    }

    protected override void SetAnimator()
    {
        base.SetAnimator();
        // 逃げアニメーション
        m_AnimatorStates[(int)AnimalAnimatorNumber.ANIMATOR_CHASE_NUMBER] = "Run";
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
        if (objName == "AttackCollider")
            ChangeMeat();
    }

    protected override void TriggerStayObject(Collider other)
    {
        base.TriggerStayObject(other);

        var objName = other.name;
        // 攻撃判定との衝突判定
        // お肉状態に遷移
        if (objName == "AttackCollider")
            ChangeMeat();
    }

    protected override void TrapReleaseAction()
    {
        CreateMeat(AnimalMeat.MeatNumber.SMALL_NUMBER);
    }
    #endregion
    #endregion

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(SmallEnemy), true)]
    [CanEditMultipleObjects]
    public class SmallEditor : Enemy3DEditor
    {
        //SerializedProperty CanvasObj;
        //SerializedProperty MeatUI;
        SerializedProperty RemovePoint;

        protected override void OnChildEnable()
        {
            //CanvasObj = serializedObject.FindProperty("m_Canvas");
            //MeatUI = serializedObject.FindProperty("m_MeatUI");
            RemovePoint = serializedObject.FindProperty("m_RemovePoint");
        }

        protected override void OnChildInspectorGUI()
        {
            SmallEnemy enemy = target as SmallEnemy;

            EditorGUILayout.LabelField("〇小動物固有のステータス");
            //// GameObject
            //MeatUI.objectReferenceValue = EditorGUILayout.ObjectField("お肉UIオブジェクト", enemy.m_MeatUI, typeof(GameObject), true);
            //CanvasObj.objectReferenceValue = EditorGUILayout.ObjectField("キャンパスオブジェクト", enemy.m_Canvas, typeof(GameObject), true);
            RemovePoint.objectReferenceValue = EditorGUILayout.ObjectField("逃げポイント", enemy.m_RemovePoint, typeof(GameObject), true);

        }
    }
#endif
    #endregion
}
