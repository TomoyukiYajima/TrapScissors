using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyCreateBox : MonoBehaviour {

    #region 変数
    #region シリアライズ変数
    [SerializeField]
    private Transform[] m_MovePoints = null;    // 移動用ポイント配列
    [SerializeField]
    private GameObject m_CreateEnemy;           // 生成用オブジェクト
    [SerializeField]
    private GameObject m_MainCamera;            // メインカメラ
    [SerializeField]
    private bool m_IsStartCreate = false;       // ゲーム開始時に生成するか
    #endregion

    #region private変数
    private bool m_IsRendered = true;           // カメラ内か
    #endregion
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    void Start () {
        // メインカメラがなかった場合は、探す
        if (m_MainCamera == null)
        {
            var camera = GameObject.Find("Main Camera");
            if (camera == null) return;
            m_MainCamera = camera;
        }
        // 動物の設定を行います
        var child = gameObject.transform.GetChild(0);
        m_CreateEnemy = child.gameObject;
        // 開始時に生成しない場合は返す
        if (!m_IsStartCreate) return;
    }
	
	// Update is called once per frame
	void Update () {
        // カメラから映っていなかったら、アクティブ状態に変更
        if (!m_IsRendered)
        {
            // 子オブジェクトが非アクティブ状態なら、アクティブ状態に変更
            if (m_CreateEnemy.activeSelf) return;
            m_CreateEnemy.SetActive(true);
        }

        m_IsRendered = false;
    }
    #endregion

    #region public関数
    // 移動ポイント配列のポイントを取得します
    public Transform GetMovePoint(int num) { return m_MovePoints[num]; }

    // 移動ポイント配列のサイズを取得します
    public int GetMovePointsSize() { return m_MovePoints.Length; }
    #endregion

    #region Unity関数
    public void OnWillRenderObject()
    {
        if (m_MainCamera.tag != "MainCamera") return;
        // 見えている
        m_IsRendered = true;
    }
    #endregion
    #endregion

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyCreateBox), true)]
    [CanEditMultipleObjects]
    public class EnemyCreateBoxEditor : Editor
    {
        //SerializedProperty CreateCount;
        //SerializedProperty CreateTime;
        //SerializedProperty CreateLength;
        SerializedProperty IsStartCreate;
        SerializedProperty CreateEnemy;
        SerializedProperty MainCamera;
        SerializedProperty MovePoints;

        public void OnEnable()
        {
            //CreateCount = serializedObject.FindProperty("m_CreateCount");
            //CreateTime = serializedObject.FindProperty("m_CreateTime");
            //CreateLength = serializedObject.FindProperty("m_CreateLength");
            IsStartCreate = serializedObject.FindProperty("m_IsStartCreate");
            CreateEnemy = serializedObject.FindProperty("m_CreateEnemy");
            MainCamera = serializedObject.FindProperty("m_MainCamera");
            MovePoints = serializedObject.FindProperty("m_MovePoints");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            // 必ず書く
            serializedObject.Update();

            EnemyCreateBox createBox = target as EnemyCreateBox;

            //// int
            //CreateCount.intValue = EditorGUILayout.IntField("敵の最大生成数", createBox.m_CreateCount);

            //// float
            //CreateTime.floatValue = EditorGUILayout.FloatField("敵の生成間隔(秒)", createBox.m_CreateTime);
            //CreateLength.floatValue = EditorGUILayout.FloatField("敵の生成距離(m)", createBox.m_CreateLength);
            // bool
            IsStartCreate.boolValue = EditorGUILayout.Toggle("ゲーム開始時に生成", createBox.m_IsStartCreate);

            EditorGUILayout.Space();
            // Transform
            CreateEnemy.objectReferenceValue = EditorGUILayout.ObjectField("生成する敵の種類", createBox.m_CreateEnemy, typeof(GameObject), true);
            MainCamera.objectReferenceValue = EditorGUILayout.ObjectField("メインカメラ", createBox.m_MainCamera, typeof(GameObject), true);
            // 配列
            EditorGUILayout.PropertyField(MovePoints, new GUIContent("動物の徘徊ポイント"), true);
            //EditorGUILayout.Space();
            //// WallChackPoint
            //WChackPoint.objectReferenceValue = EditorGUILayout.ObjectField("壁捜索ポイント", enemy.m_WChackPoint, typeof(WallChackPoint), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
