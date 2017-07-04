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
    private GameObject m_CheckBox;              // チェックボックス
    [SerializeField]
    private int m_CheckBoxNum = 0;              // 表示するチェックボックス番号
    [SerializeField]
    protected float m_DrawTextTimer;            // テキストを表示するまでの時間
    [SerializeField]
    protected bool m_IsChangeToggle;            // トグルのチェックボックスを変更するか
    [SerializeField]
    protected bool m_IsEnd;                     // 終了するか
    [SerializeField]
    protected string m_AddText;                 // 追加表示するテキスト
    [SerializeField]
    private GameManager.GameState m_GameState
        = GameManager.GameState.PLAY;           // 変更するゲームの状態
    #endregion

    protected Toggle m_Toggle;              // トグル

    private float m_Timer;                  // テキスト表示時間
    private bool m_IsTextDraw = false;      // テキストを表示したか

    // Use this for initialization
    public virtual void Start()
    {
        // トグルの設定
        if (m_CheckBox != null) m_Toggle = m_CheckBox.GetComponent<Toggle>();
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
        if (m_Toggle) m_Toggle.isOn = true;
    }

    // 追加テキストの表示
    protected void DrawText()
    {
        // m_Timer += Time.deltaTime;
        //if (IsTextDrawTime()) return;

        //m_Timer += Time.deltaTime;

        // if (!m_IsTextDraw)
        if (!m_IsTextDraw)
        {
            // 表示時間に達したら、追加テキストの表示
            if (IsTextDrawTime())
            {
                // 追加テキストの表示
                // m_GameState
                //TutorialMediator.GetInstance().NextDrawText(m_AddText, m_CheckBoxNum);
                TutorialMediator.GetInstance().NextDrawText(m_AddText, m_CheckBoxNum, m_GameState);
                m_IsTextDraw = true;
                // チェックボックスの変更
                if (m_Toggle)
                {
                    m_Toggle.isOn = m_IsChangeToggle;
                    SoundManger.Instance.PlaySE(2);
                }
            }

            m_Timer += Time.deltaTime;
        }
        else DrawEnd();
    }

    // 表示終了処理
    protected void DrawEnd()
    {
        var state = GameManager.gameManager.GameStateCheck();
        if (!TutorialMediator.GetInstance().IsTextDrawEnd() ||
            state == GameManager.GameState.END) return;
        // 終了するならば、チュートリアルクリアの確認を行う
        if (m_IsEnd)
        {
            // テキスト表示が終了したならば、チュートリアルクリア
            if (TutorialMediator.GetInstance().IsTextDrawEnd())
                TutorialClear();
        }
    }

    // テキスト表示時間に達したかを返します
    protected bool IsTextDrawTime() { return m_Timer >= m_DrawTextTimer; }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(ClearChacker), true)]
    [CanEditMultipleObjects]
    public class ClearChackerEditor : Editor
    {
        SerializedProperty CheckBox;
        SerializedProperty CheckBoxNum;
        SerializedProperty DrawTextTimer;
        SerializedProperty IsChangeToggle;
        SerializedProperty IsEnd;
        SerializedProperty AddText;
        SerializedProperty GameState;

        public void OnEnable()
        {
            CheckBox = serializedObject.FindProperty("m_CheckBox");
            CheckBoxNum = serializedObject.FindProperty("m_CheckBoxNum");
            DrawTextTimer = serializedObject.FindProperty("m_DrawTextTimer");
            IsChangeToggle = serializedObject.FindProperty("m_IsChangeToggle");
            IsEnd = serializedObject.FindProperty("m_IsEnd");
            AddText = serializedObject.FindProperty("m_AddText");
            GameState = serializedObject.FindProperty("m_GameState");

            OnChildEnable();
        }

        protected virtual void OnChildEnable() { }

        public override void OnInspectorGUI()
        {
            // 必ず書く
            serializedObject.Update();

            ClearChacker chacker = target as ClearChacker;

            EditorGUILayout.LabelField("〇クリアチェッカーの設定");
            CheckBox.objectReferenceValue = EditorGUILayout.ObjectField("チェックボックス", chacker.m_CheckBox, typeof(GameObject), true);
            // int
            CheckBoxNum.intValue = EditorGUILayout.IntField("表示するチェックボックス番号", chacker.m_CheckBoxNum);
            // float
            DrawTextTimer.floatValue = EditorGUILayout.FloatField("テキスト表示までの時間", chacker.m_DrawTextTimer);
            // bool
            IsChangeToggle.boolValue = EditorGUILayout.Toggle("チェックボックスの変更をおこなうか", chacker.m_IsChangeToggle);
            //// チェックボックスの変更をする場合
            //if (chacker.m_IsChangeToggle) CheckBoxNum.intValue = EditorGUILayout.IntField("変更するチェックボックス番号", chacker.m_CheckBoxNum);
            IsEnd.boolValue = EditorGUILayout.Toggle("終了処置をおこなうか", chacker.m_IsEnd);

            AddText.stringValue = EditorGUILayout.TextField("表示する追加テキストファイル名", chacker.m_AddText);

            // 列挙クラスの表示
            EditorGUILayout.PropertyField(GameState, new GUIContent("テキスト表示後のゲームの状態"));

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
