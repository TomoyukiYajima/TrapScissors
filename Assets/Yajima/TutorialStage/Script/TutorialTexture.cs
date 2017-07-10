using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TutorialTexture : MonoBehaviour
{
    //public GameObject m_Texture;    // 表示する画像
    [SerializeField]
    private int m_DateNumber;       // 表示するテキストデータ番号
    [SerializeField]
    private int m_Point;            // 表示位置
    [SerializeField]
    private int m_Count;            // 表示カウント

    // 表示カウントを減算します
    public void SubCount()
    {
        m_Count--;
        // 表示カウントが0以下になったら、非アクティブ状態にする
        if (m_Count <= 0) gameObject.SetActive(false);
    }

    // 画像を表示するタイミングかを返します
    public bool IsCheckDraw(int number, int point)
    {
        if (m_DateNumber != number || m_Point != point) return false;
        return true;
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(TutorialTexture), true)]
    [CanEditMultipleObjects]
    public class TutorialTextureEditor : Editor
    {
        SerializedProperty DateNumber;
        SerializedProperty Point;
        SerializedProperty Count;

        public void OnEnable()
        {
            DateNumber = serializedObject.FindProperty("m_DateNumber");
            Point = serializedObject.FindProperty("m_Point");
            Count = serializedObject.FindProperty("m_Count");
        }

        public override void OnInspectorGUI()
        {
            // 必ず書く
            serializedObject.Update();

            TutorialTexture texture = target as TutorialTexture;

            EditorGUILayout.LabelField("〇チュートリアル画像の設定");
            // int
            DateNumber.intValue = EditorGUILayout.IntField("表示をするテキスト番号", texture.m_DateNumber);
            Point.intValue = EditorGUILayout.IntField("表示位置", texture.m_Point);
            Count.intValue = EditorGUILayout.IntField("表示を消すまでのカウント", texture.m_Count);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
