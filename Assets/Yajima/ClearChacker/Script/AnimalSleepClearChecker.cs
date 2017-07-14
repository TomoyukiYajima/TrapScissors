using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimalSleepClearChecker : ClearChacker {

    [SerializeField]
    private GameObject m_Animal;

    private Enemy3D m_AnimalScript;

    // Use this for initialization
    public override void Start () {
        base.Start();

        m_AnimalScript = m_Animal.GetComponent<Enemy3D>();
	}

    // Update is called once per frame
    public override void Update () {
        if (m_AnimalScript.GetState() == Enemy3D.State.Sleep) return;

        if (m_Toggle)
        {
            if(!m_Toggle.isOn)
            {
                m_Toggle.isOn = true;
                ChengeBox();
            }
        }
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimalSleepClearChecker), true)]
    [CanEditMultipleObjects]
    public class AnimalSleepClearCheckerEditor : ClearChackerEditor
    {
        //SerializedProperty IsChangeToggle;
        //SerializedProperty IsEnd;
        //SerializedProperty AddText;
        SerializedProperty Animal;

        protected override void OnChildEnable()
        {
            //IsChangeToggle = serializedObject.FindProperty("m_IsChangeToggle");
            //IsEnd = serializedObject.FindProperty("m_IsEnd");
            //AddText = serializedObject.FindProperty("m_AddText");
            Animal = serializedObject.FindProperty("m_Animal");
        }

        protected override void OnChildInspectorGUI()
        {
            AnimalSleepClearChecker checker = target as AnimalSleepClearChecker;

            EditorGUILayout.LabelField("〇動物クリアチェッカーの設定");
            //// bool
            //IsChangeToggle.boolValue = EditorGUILayout.Toggle("トグル変更をおこなうか", chacker.m_IsChangeToggle);
            //IsEnd.boolValue = EditorGUILayout.Toggle("終了処置をおこなうか", chacker.m_IsEnd);

            //AddText.stringValue = EditorGUILayout.TextField("表示する追加テキストファイル名", chacker.m_AddText);
            Animal.objectReferenceValue = EditorGUILayout.ObjectField("参照する動物", checker.m_Animal, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
