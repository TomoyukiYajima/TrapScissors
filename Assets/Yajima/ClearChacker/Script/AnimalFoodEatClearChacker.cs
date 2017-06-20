using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimalFoodEatClearChacker : ClearChacker
{
    #region シリアライズ変数
    //[SerializeField]
    //private bool m_IsChangeToggle;
    //[SerializeField]
    //private bool m_IsEnd;
    //[SerializeField]
    //private string m_AddText;        // 動物スクリプト
    [SerializeField]
    private GameObject m_Animal;    // 動物オブジェクト
    [SerializeField]
    private GameObject m_AnimalCreate;        // 動物スクリプト
    #endregion

    private Enemy3D m_Enemy;        // 動物スクリプト
    private bool m_Eat = false;

    private bool m_IsDraw = false;
    private bool m_IsDrawEnd = false;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_Enemy = m_Animal.GetComponent<Enemy3D>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (m_Eat)
        {
            if (m_IsDrawEnd) return;
            var mediator = GameObject.Find("TutorialMediator");
            if (mediator == null) return;

            if (TutorialMediator.GetInstance().IsTextDrawEnd())
            {
                if (m_IsEnd) {
                    TutorialClear();
                    m_IsDrawEnd = true;
                }
            }
        }

        if (m_IsDraw) return;

        // 動物がえさを食べたら、表示
        //if (m_Enemy.IsTrapHit()) TutorialClear();
        if(m_Enemy.IsEatFood())
        {
            TutorialMediator.GetInstance().NextDrawText(m_AddText);
            m_IsDraw = true;
            // チェックボックスをオンにする
            if (m_IsChangeToggle) m_Toggle.isOn = true;

            m_Eat = true;
            // オオカミを表示
            if (m_AnimalCreate != null) m_AnimalCreate.SetActive(true);
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
