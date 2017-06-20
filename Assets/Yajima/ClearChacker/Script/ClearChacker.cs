using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ClearChacker : MonoBehaviour
{
    #region シリアライズ変数
    [SerializeField]
    private GameObject m_ChackBox;      // チェックボックス
    [SerializeField]
    protected bool m_IsChangeToggle;
    [SerializeField]
    protected bool m_IsEnd;
    [SerializeField]
    protected string m_AddText;        // 動物スクリプト
    #endregion

    protected Toggle m_Toggle;            // トグル

    // Use this for initialization
    public virtual void Start()
    {
        // トグルの設定
        m_Toggle = m_ChackBox.GetComponent<Toggle>();
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public void TutorialClear()
    {
        // ゲームクリア
        GameManager.gameManager.GameStateSet(GameManager.GameState.END);
        ResultManager.resultManager.ClearPopUpActiveEnd();
        // チェックボックスをオンにする
        m_Toggle.isOn = true;
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(ClearChacker), true)]
    [CanEditMultipleObjects]
    public class ClearChackerEditor : Editor
    {
        SerializedProperty ChackBox;
        SerializedProperty IsChangeToggle;
        SerializedProperty IsEnd;
        SerializedProperty AddText;

        public void OnEnable()
        {
            ChackBox = serializedObject.FindProperty("m_ChackBox");
            IsChangeToggle = serializedObject.FindProperty("m_IsChangeToggle");
            IsEnd = serializedObject.FindProperty("m_IsEnd");
            AddText = serializedObject.FindProperty("m_AddText");

            OnChildEnable();
        }

        protected virtual void OnChildEnable() { }

        public override void OnInspectorGUI()
        {
            // 必ず書く
            serializedObject.Update();

            ClearChacker chacker = target as ClearChacker;

            EditorGUILayout.LabelField("〇クリアチェッカーの設定");
            ChackBox.objectReferenceValue = EditorGUILayout.ObjectField("チェックボックス", chacker.m_ChackBox, typeof(GameObject), true);
            // bool
            IsChangeToggle.boolValue = EditorGUILayout.Toggle("トグル変更をおこなうか", chacker.m_IsChangeToggle);
            IsEnd.boolValue = EditorGUILayout.Toggle("終了処置をおこなうか", chacker.m_IsEnd);

            AddText.stringValue = EditorGUILayout.TextField("表示する追加テキストファイル名", chacker.m_AddText);

            EditorGUILayout.Space();

            OnChildInspectorGUI();

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnChildInspectorGUI() { }
    }
#endif
    #endregion
}
