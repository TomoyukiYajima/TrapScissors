using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TutorialStage : MonoBehaviour {

    #region シリアライズ変数
    [SerializeField]
    private GameObject m_TextBox;               // テキストボックス
    [SerializeField]
    private GameManager.GameState m_GameState = 
        GameManager.GameState.PAUSE;            // 最初のテキスト表示後のゲームの状態
    [SerializeField]
    private bool m_IsMoveCamera = false;
    #endregion

    private bool m_IsMove = false;

    // Use this for initialization
    void Start () {
        // テキストボックスを表示する
        m_TextBox.SetActive(true);
        // 一旦停止する
        GameManager.gameManager.GameStateSet(GameManager.GameState.PAUSE);
        TutorialMediator.GetInstance().SetDrawEndGameState(m_GameState);
    }
	
	// Update is called once per frame
	void Update () {
        // カメラ移動
        if (m_IsMove || !m_IsMoveCamera) return;

        if (TutorialMediator.GetInstance().IsTextDrawEnd())
        {
            var camera = GameObject.Find("Main Camera").GetComponent<CameraMove>();
            camera.enabled = true;
            m_IsMove = true;
        }
    }

    public void DrawTextBox()
    {
        // テキストボックスを表示する
        m_TextBox.SetActive(true);
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(TutorialStage), true)]
    [CanEditMultipleObjects]
    public class TutorialStageEditor : Editor
    {
        SerializedProperty TextBox;
        SerializedProperty GameState;
        SerializedProperty IsMoveCamera;

        public void OnEnable()
        {
            TextBox = serializedObject.FindProperty("m_TextBox");
            GameState = serializedObject.FindProperty("m_GameState");
            IsMoveCamera = serializedObject.FindProperty("m_IsMoveCamera");
        }

        public override void OnInspectorGUI()
        {
            // 必ず書く
            serializedObject.Update();

            TutorialStage tutorial = target as TutorialStage;

            EditorGUILayout.LabelField("〇チュートリアルステージの設定");
            TextBox.objectReferenceValue = EditorGUILayout.ObjectField("テキストボックス", tutorial.m_TextBox, typeof(GameObject), true);

            // 列挙クラスの表示
            EditorGUILayout.PropertyField(GameState, new GUIContent("テキスト表示後のゲームの状態"));

            // カメラ移動させるか
            IsMoveCamera.boolValue = EditorGUILayout.Toggle("表示後にカメラ移動させるか", tutorial.m_IsMoveCamera);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
