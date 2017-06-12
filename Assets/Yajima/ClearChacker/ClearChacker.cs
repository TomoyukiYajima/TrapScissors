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

    private Toggle m_Toggle;            // トグル

    // Use this for initialization
    void Start()
    {
        // トグルの設定
        m_Toggle = m_ChackBox.GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        // collision.transform.tag == "Player"
        if (collision.transform.name == "PlayerSprite")
        {
            // ゲームクリア
            GameManager.gameManager.GameStateSet(GameManager.GameState.END);
            ResultManager.resultManager.ClearPopUpActiveEnd();
            // チェックボックスをオンにする
            m_Toggle.isOn = true;
            //ResultManager.resultManager.ClearSetActive(true);
            //_gameOver.SetActive(true);
        }
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
        }

        public override void OnInspectorGUI()
        {
            // 必ず書く
            serializedObject.Update();

            ClearChacker chacker = target as ClearChacker;

            EditorGUILayout.LabelField("〇クリアチェッカーの設定");
            ChackBox.objectReferenceValue = EditorGUILayout.ObjectField("チェックボックス", chacker.m_ChackBox, typeof(GameObject), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
