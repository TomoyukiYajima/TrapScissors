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

        //// キャンパスが設定されていなかったら取得
        //if (m_Canvas == null) m_Canvas = GameObject.Find("Canvas");
        //// キャンパスのフレームを取得
        //var frame = m_Canvas.transform.FindChild("Frame");
        //if (frame != null) m_Frame = frame.gameObject;

        //// 壁を発見したとき
        //GameObject wall = null;
        //if (InWall(out wall, 20))
        //{
        //    // ベクトルを求める
        //    var cross = Vector3.Cross(wall.transform.up, this.transform.forward);
        //    m_RemovePoint.transform.localPosition = cross;
        //}
    }

    protected override void Update()
    {
        base.Update();
        m_RunawayPoint.SetPosition(this.transform.position);
        //var rotate = wall.transform.rotation.eulerAngles;
    }

    protected override void DiscoverPlayer(float deltaTime)
    {
        // 移動(通常の移動速度の数倍)
        Move(deltaTime, m_DiscoverSpeed);
        m_MoveLength += m_DiscoverSpeed * deltaTime;
        //base.DiscoverPlayer(deltaTime);]
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
            //var rotate = wall.transform.rotation.eulerAngles;
            m_RunawayPoint.ChangeAddPosition(rotate);
            //print(rotate.y.ToString());
        }

        // 移動(通常の移動速度の数倍)
        //Move(deltaTime, m_Speed * 2.0f);
        //m_Agent.destination = m_Player.transform.position;
        //Camera.

        // 一定距離移動したら、待機状態に遷移
        //GameObject obj = null;
        // if (!InPlayer(out obj, 20.0f, true) || m_MoveLength > 20)
        if (m_MoveLength > 20)
        {
            // 待機状態に遷移
            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            m_Agent.Resume();
            m_Player = null;
            // 移動速度を変える
            m_Agent.speed = m_Speed;
            m_MoveLength = 0.0f;
            ChangeSpriteColor(Color.red);
            return;
        };
    }

    protected override void DiscoverAnimal(float deltaTime)
    {
        // 逃げる
        ChangeMovePoint(m_RunawayPoint.gameObject.transform.position);
        m_MoveLength += m_DiscoverSpeed * deltaTime;
        ChangeSpriteColor(Color.white);

        // 壁を発見したとき
        GameObject wall = null;
        Vector3 point = Vector3.zero;
        if (InWall(out wall, out point, 2))
        {
            var up = wall.transform.up;
            //var vec = Vector3.zero;
            //var cross = Vector3.zero;
            var degree = 0.0f;

            var wallCollider = wall.GetComponent<BoxCollider>();
            // 壁がボックスコライダーの場合の計算
            //if(wallCollider != null)
            //{
            //    // 対象とのベクトル
            //    //var objDir = point - wall.transform.position;
            //    // ポイントの左ベクトルを求める
            //    //var left = //point.

            //    // 対象の方向を向いた単位ベクトル
            //    //var oneDir = DirectionOne(point);
            //    // 壁 - point
            //    //var vec1 = new Vector2(point.x, point.z) - new Vector2(wall.transform.position.x, wall.transform.position.z);
            //    // ベクトル間の角度を計算(二次元)
            //    //var vec1 = new Vector2(objDir.x, objDir.z);
            //    //var vec2 = new Vector2(oneDir.x, oneDir.z);
            //    //var angle = Vector2.Angle(vec1, vec2);
            //    //var cross = Vector2.
            //    //degree = Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;
            //}
            //else
            //{
            //    var vec = point - wall.transform.position;
            //    // 壁と衝突点との外積を求めて、角度を決める
            //    var cross = Vector3.Cross(vec, up);
            //    degree = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;
            //}

            var vec = point - wall.transform.position;
            // 壁と衝突点との外積を求めて、角度を決める
            var cross = Vector3.Cross(vec, up);
            degree = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;

            //print(degree);

            // 壁に沿うように逃げる
            //var rotate = wall.transform.rotation.eulerAngles;
            m_RunawayPoint.ChangeAddPosition(degree);

            //print(rotate.y.ToString());
        }

        // 一定距離移動したら、待機状態に遷移
        if (m_MoveLength > 20)
        {
            // 待機状態に遷移
            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            m_MoveLength = 0.0f;
            // 移動速度を変える
            m_Agent.speed = m_Speed;
            m_Agent.Resume();
            ChangeSpriteColor(Color.red);
        }
    }
    #endregion

    #region override関数
    protected override void ReturnMove(float deltaTime, float subSpeed = 1.0f)
    {
        base.ReturnMove(deltaTime, subSpeed);
        // 移動距離の加算
        //m_MoveLength += Mathf.Abs(m_TotalVelocity.x) + Mathf.Abs(m_TotalVelocity.y) + Mathf.Abs(m_TotalVelocity.z);
    }

    protected override void ChangePlayerHitMove(GameObject player)
    {
        base.ChangePlayerHitMove(player);
        // 逃げポイントの追加位置の変更
        // 前方ベクトルから、角度の取得
        var vec = player.transform.position - this.transform.position;
        var angle = Mathf.Atan2(vec.z, vec.x);
        m_RunawayPoint.ChangeAddPosition(angle * Mathf.Rad2Deg - 180);
        //ChangeMovePoint(m_RunawayPoint.transform.position);
        //PointRunaway(player.transform);
    }

    protected override void SetAnimator()
    {
        base.SetAnimator();
        // 逃げアニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_CHASE_NUMBER] = "Run";
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

    //protected override void DiscoverFoodMove(float deltaTime)
    //{
    //    // 二次元(x, z)の距離を求める
    //    var v1 = new Vector2(m_Agent.destination.x, m_Agent.destination.z);
    //    var v2 = new Vector2(m_MouthPoint.position.x, m_MouthPoint.position.z);
    //    var length = Vector2.Distance(v1, v2);

    //    // 一定距離内なら、持ち上げ状態に遷移
    //    if (length < 0.5f)
    //    {
    //        //// 持ち上げ状態に遷移
    //        //ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_Lift);
    //        // えさ食べ状態に遷移
    //        ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_Eat);
    //        ChangeSpriteColor(Color.yellow);
    //        m_Agent.Stop();
    //        return;
    //    }
    //}

    //protected override void TurnWall()
    //{
    //    // 一定距離移動したら、折り返す
    //    //if (m_MoveLength < m_TurnLength * 10) return;

    //    m_MoveLength = 0.0f;
    //    //base.TurnWall();
    //    // 角度の設定
    //    SetDegree();
    //}

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
