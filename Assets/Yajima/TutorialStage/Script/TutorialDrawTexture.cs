using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TutorialDrawTexture : MonoBehaviour {

    #region 変数
    [SerializeField]
    private TutorialTexture[] m_Textures;      // 表示するテクスチャ

    private int m_DrawCount;            // 表示カウント
    #endregion

    #region 関数
    public void SetTexture(int number, int point)
    {
        // 登録している画像全体で調べる
        if (m_Textures.Length == 0) return;
        for(int i = 0; i != m_Textures.Length; ++i)
        {
            if (m_Textures[i] == null) continue;
            // 表示位置に達していれば、画像を表示する
            if(m_Textures[i].IsCheckDraw(number, point))
            {
                // メディエーターに画像の登録を行う
                m_Textures[i].gameObject.SetActive(true);
                TutorialMediator.GetInstance().SetTexture(m_Textures[i]);
                // フェード画像もアクティブ状態にする

                break;
            }
        }
    }
    #endregion

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(TutorialDrawTexture), true)]
    [CanEditMultipleObjects]
    public class TutorialDrawTextureEditor : Editor
    {
        SerializedProperty Textures;

        public void OnEnable()
        {
            Textures = serializedObject.FindProperty("m_Textures");
        }

        public override void OnInspectorGUI()
        {
            // 必ず書く
            serializedObject.Update();

            TutorialDrawTexture drawTexture = target as TutorialDrawTexture;

            EditorGUILayout.LabelField("〇画像表示の設定");
            // 配列
            EditorGUILayout.PropertyField(Textures, new GUIContent("表示する画像"), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
