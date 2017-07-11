using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FadeClearChacker : ClearChacker {

    [SerializeField]
    private TutorialFadeChanger m_FadeChanger;

	// Use this for initialization
	public override void Start () {
        base.Start();
	}

    // Update is called once per frame
    public override void Update () {
        if (m_FadeChanger.IsFadeEnd()) DrawText();
	}

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(FadeClearChacker), true)]
    [CanEditMultipleObjects]
    public class FadeClearChackerEditor : ClearChackerEditor
    {
        SerializedProperty FadeChanger;

        protected override void OnChildEnable()
        {
            FadeChanger = serializedObject.FindProperty("m_FadeChanger");
        }

        protected override void OnChildInspectorGUI()
        {
            FadeClearChacker chacker = target as FadeClearChacker;

            EditorGUILayout.LabelField("〇フードクリアチェッカーの設定");
            FadeChanger.objectReferenceValue = EditorGUILayout.ObjectField("フェードチェンジャー", chacker.m_FadeChanger, typeof(TutorialFadeChanger), true);
        }
    }
#endif
    #endregion
}
