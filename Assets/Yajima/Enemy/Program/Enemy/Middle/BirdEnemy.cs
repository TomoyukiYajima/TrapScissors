using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BirdEnemy : MiddleEnemy {

    #region シリアライズ
    [SerializeField]
    private float m_RotateTime = 5.0f;      // 一周当たりの時間
    [SerializeField]
    private float m_RotateLength = 5.0f;    // 回転の半径
    #endregion

    //#region private変数
    //private float m_MoveDegree = 0.0f;      // 移動角度
    //private GameObject m_DiscoverObj;       // 発見したオブジェクト
    //private Enemy3D m_Animal = null;        // 発見した動物
    //#endregion

    #region 基盤関数
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        m_LineObjName = "RabbitEnemy";
    }

    //// Update is called once per frame
    //void Update () {

    //}
    #endregion

    #region 発見状態派生関数
    //protected override void DiscoverMove(float deltaTime)
    //{
    //    // オブジェクトとの方向を計算
    //    var addPos = Vector3.up * (m_DiscoverObj.transform.localScale.y * 0.8f);
    //    var dir = ObjectDirection(m_DiscoverObj, addPos);
    //    // 発見したオブジェクトに対して移動
    //    //m_Velocity = -Vector3.up;
    //    // v.yを消している
    //    //Move(deltaTime);

    //    var length = Vector3.Distance(
    //        this.transform.position,
    //        m_DiscoverObj.transform.position + addPos
    //        );
    //    var speed = m_Speed * 10;
    //    //if (length < speed) return;
    //    if (length < 0.1f)
    //    {
    //        // 持ち上げ状態に遷移
    //        ChangeDiscoverState(DiscoverState.Discover_Lift);
    //        return;
    //    }
    //    m_TotalVelocity = speed * dir.normalized * deltaTime;
    //    //this.transform.position += m_Velocity * 0.1f;

    //    ChangeSpriteColor(Color.blue);
    //}

    //protected override void DiscoverTakeAway(float deltaTime)
    //{
    //    // 移動
    //    var v = new Vector3(1.0f, 1.0f, 0.0f).normalized;
    //    //m_TotalVelocity = m_Speed * 10 * v * deltaTime;
    //    // プログラムで親子関係にして、リジッドボディ側に移動量を入れると、
    //    // おかしくなる
    //    gameObject.transform.position += m_Speed / 10 * v * deltaTime;
    //    //m_DiscoverObj.transform.parent.position = Vector3.zero;

    //    // 両方カメラ外に出たら、自身と持っているオブジェクトを削除する
    //    if (!m_IsRendered && !m_Animal.IsRendered())
    //    {
    //        // 相手を削除
    //        Destroy(m_DiscoverObj);
    //        m_DiscoverObj = null;
    //        // 自身を削除
    //        var parent = gameObject.transform.parent.gameObject;
    //        Destroy(parent);
    //    }
    //}
    #endregion

    #region その他関数
    // 回転移動
    private void RotateMove(float deltaTime)
    {
        var addD = 360.0f / m_RotateTime * deltaTime;
        m_MoveDegree += addD;
        //m_Degree = Mathf.Min(m_Degree + addD, 360.0f);
        //m_Degree = Mathf.Clamp()
        // 回転
        this.transform.Rotate(Vector3.up, addD);
        // ラジアンに変換
        var radius = m_MoveDegree / 180 * Mathf.PI;
        // 回転用の移動ベクトルを代入
        m_Velocity = new Vector3(Mathf.Cos(radius), 0.0f, Mathf.Sin(radius));
        this.transform.localPosition = m_Velocity * m_RotateLength;
    }
    #endregion

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(BirdEnemy), true)]
    [CanEditMultipleObjects]
    public class BirdEnemyEditor : MiddleEnemyEditor
    {
        SerializedProperty RotateTime;
        SerializedProperty RotateLength;

        protected override void OnChildEnable()
        {
            base.OnChildEnable();
            RotateTime = serializedObject.FindProperty("m_RotateTime");
            RotateLength = serializedObject.FindProperty("m_RotateLength");
        }

        protected override void OnChildInspectorGUI()
        {
            base.OnChildInspectorGUI();

            BirdEnemy birdEnemy = target as BirdEnemy;

            EditorGUILayout.LabelField("〇トリ固有のステータス");
            // float
            RotateTime.floatValue = EditorGUILayout.FloatField("一周当たりの時間(秒)", birdEnemy.m_RotateTime);
            RotateLength.floatValue = EditorGUILayout.FloatField("半径(m)", birdEnemy.m_RotateLength);
        }
    }
#endif
    #endregion
}
