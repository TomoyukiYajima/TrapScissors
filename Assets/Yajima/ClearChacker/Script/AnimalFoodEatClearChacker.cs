using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimalFoodEatClearChacker : ClearChacker
{
    #region シリアライズ変数
    [SerializeField]
    private GameObject m_Animal;    // 動物オブジェクト
    [SerializeField]
    private GameObject m_AnimalCreate;        // 動物スクリプト
    #endregion

    private Enemy3D m_Enemy;        // 動物スクリプト

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_Enemy = m_Animal.GetComponent<Enemy3D>();
    }

    // Update is called once per frame
    public override void Update()
    {
        // 動物がえさを食べたら、表示
        if(m_Enemy.IsEatFood())
        {
            // 追加テキストの表示
            DrawText();
            // 動物を表示
            if (m_AnimalCreate != null && !m_AnimalCreate.activeSelf)
                m_AnimalCreate.SetActive(true);
        }
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimalFoodEatClearChacker), true)]
    [CanEditMultipleObjects]
    public class AnimalFoodEatClearChackerEditor : ClearChackerEditor
    {
        //SerializedProperty IsChangeToggle;
        //SerializedProperty IsEnd;
        //SerializedProperty AddText;
        SerializedProperty Animal;
        SerializedProperty AnimalCreate;

        protected override void OnChildEnable()
        {
            //IsChangeToggle = serializedObject.FindProperty("m_IsChangeToggle");
            //IsEnd = serializedObject.FindProperty("m_IsEnd");
            //AddText = serializedObject.FindProperty("m_AddText");
            Animal = serializedObject.FindProperty("m_Animal");
            AnimalCreate = serializedObject.FindProperty("m_AnimalCreate");
        }

        protected override void OnChildInspectorGUI()
        {
            AnimalFoodEatClearChacker chacker = target as AnimalFoodEatClearChacker;

            EditorGUILayout.LabelField("〇動物クリアチェッカーの設定");
            //// bool
            //IsChangeToggle.boolValue = EditorGUILayout.Toggle("トグル変更をおこなうか", chacker.m_IsChangeToggle);
            //IsEnd.boolValue = EditorGUILayout.Toggle("終了処置をおこなうか", chacker.m_IsEnd);

            //AddText.stringValue = EditorGUILayout.TextField("表示する追加テキストファイル名", chacker.m_AddText);
            Animal.objectReferenceValue = EditorGUILayout.ObjectField("参照する動物", chacker.m_Animal, typeof(GameObject), true);
            AnimalCreate.objectReferenceValue = EditorGUILayout.ObjectField("生成する動物", chacker.m_AnimalCreate, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
