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

        public void OnEnable()
        {
            ChackBox = serializedObject.FindProperty("m_ChackBox");

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
