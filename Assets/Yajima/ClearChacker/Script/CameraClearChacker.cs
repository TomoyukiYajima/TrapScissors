using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraClearChacker : ClearChacker {

    #region シリアライズ変数
    [SerializeField]
    private GameObject m_GameStart;     // ゲームスタートオブジェクト
    [SerializeField]
    private GameObject m_FreeLookUI;     // フリーロックUI
    [SerializeField]
    private GameObject m_CameraMapUI;     // カメラマップUI
    [SerializeField]
    private bool m_IsLookDraw;          // フリーロックUIを表示するか
    #endregion

    private GameObject m_Player;        // プレイヤー
    private GameObject m_MainCamera;    // メインカメラ
    private GameObject m_ClearChacker;  // クリアチェッカー

    private Camera m_Camera;            // カメラスクリプト

	// Use this for initialization
	public override void Start () {
        base.Start();

        if (m_GameStart == null) m_GameStart = GameObject.Find("GameStart");

        if (m_CameraMapUI == null) m_CameraMapUI = GameObject.Find("MainCameraPos");
        if (!TutorialMediator.GetInstance().IsTutorialAction(4))
            m_CameraMapUI.GetComponent<Image>().enabled = false;

        //if (m_FreeLookUI == null) GameObject.Find("FreeLook");
        if (!m_IsLookDraw) m_FreeLookUI.GetComponent<Image>().enabled = false;

        if (m_Player == null) m_Player = GameObject.FindGameObjectWithTag("Player");

        if (m_ClearChacker == null) m_ClearChacker = GameObject.Find("ClearChacker");

        m_MainCamera = GameObject.Find("Main Camera");
        if (m_MainCamera != null) m_Camera = m_MainCamera.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	public override void Update () {
        base.Update();

        CheckViewportPoint();
    }

    // ビューポート座標の計算を行います
    private void CheckViewportPoint()
    {
        // クリアチェッカーのワールド座標を、ビューポート座標に変換
        var viewportPoint = m_Camera.WorldToViewportPoint(m_ClearChacker.transform.position);
        var point = new Vector2(viewportPoint.x, viewportPoint.y);
        var distance = Vector2.Distance(Vector2.one * 0.5f, point);

        // 一定範囲内に入っていなかったら、返す
        if (distance > 0.1f) return;
        m_GameStart.SetActive(true);
        m_FreeLookUI.GetComponent<Image>().enabled = true;
        // テキストボックスの表示
        DrawText();
    }

    protected override void ChengeBox()
    {
        if (m_IsChengeBox) return;
        if (!m_IsChangeToggle) m_TutorialCheck.InMove();
        else
        {
            m_TutorialCheck.NextTutorial();
        }
        //m_TutorialCheck.NextTutorial();
        //m_TutorialCheck.InMoveAdd();
        m_IsChengeBox = true;
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(CameraClearChacker), true)]
    [CanEditMultipleObjects]
    public class CameraClearChackerEditor : ClearChackerEditor
    {
        SerializedProperty GameStart;
        SerializedProperty FreeLookUI;
        SerializedProperty IsLookDraw;

        protected override void OnChildEnable()
        {
            GameStart = serializedObject.FindProperty("m_GameStart");
            FreeLookUI = serializedObject.FindProperty("m_FreeLookUI");
            IsLookDraw = serializedObject.FindProperty("m_IsLookDraw");
        }

        protected override void OnChildInspectorGUI()
        {
            CameraClearChacker chacker = target as CameraClearChacker;

            EditorGUILayout.LabelField("〇カメラクリアチェッカーの設定");
            GameStart.objectReferenceValue = EditorGUILayout.ObjectField("ゲームスタートオブジェクト", chacker.m_GameStart, typeof(GameObject), true);
            FreeLookUI.objectReferenceValue = EditorGUILayout.ObjectField("フリーロックUI", chacker.m_FreeLookUI, typeof(GameObject), true);
            IsLookDraw.boolValue = EditorGUILayout.Toggle("フリーロックUIを表示するか", chacker.m_IsLookDraw);
        }
    }
#endif
    #endregion
}
