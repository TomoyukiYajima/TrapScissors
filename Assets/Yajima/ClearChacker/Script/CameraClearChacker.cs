using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraClearChacker : ClearChacker {

    #region シリアライズ変数
    [SerializeField]
    private GameObject m_GameStart;     // ゲームスタートオブジェクト
    #endregion

    private GameObject m_Player;        // プレイヤー
    private GameObject m_MainCamera;    // メインカメラ
    private GameObject m_ClearChacker;  // クリアチェッカー

    private Camera m_Camera;            // カメラスクリプト

	// Use this for initialization
	public override void Start () {
        base.Start();

        m_Player = GameObject.FindGameObjectWithTag("Player");

        m_ClearChacker = GameObject.Find("ClearChacker");

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
        // テキストボックスの表示
        DrawText();
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(CameraClearChacker), true)]
    [CanEditMultipleObjects]
    public class CameraClearChackerEditor : ClearChackerEditor
    {
        SerializedProperty GameStart;

        protected override void OnChildEnable()
        {
            GameStart = serializedObject.FindProperty("m_GameStart");
        }

        protected override void OnChildInspectorGUI()
        {
            CameraClearChacker chacker = target as CameraClearChacker;

            EditorGUILayout.LabelField("〇カメラクリアチェッカーの設定");
            GameStart.objectReferenceValue = EditorGUILayout.ObjectField("ゲームスタートオブジェクト", chacker.m_GameStart, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
