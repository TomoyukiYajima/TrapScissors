using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AnimalFaintClearChecker : AnimalClearChacker {

	// Use this for initialization
	public override void Start () {
        base.Start();

        m_Enemy = m_Animal.GetComponent<Enemy3D>();
	}

    // Update is called once per frame
    public override void Update () {
        if (m_Enemy.GetState() == AnimalState.Faint) DrawText();
	}

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimalFaintClearChecker), true)]
    [CanEditMultipleObjects]
    public class AnimalFaintClearCheckerEditor : ClearChackerEditor
    {
        SerializedProperty Animal;

        protected override void OnChildEnable()
        {
            Animal = serializedObject.FindProperty("m_Animal");
        }

        protected override void OnChildInspectorGUI()
        {
            AnimalFaintClearChecker chacker = target as AnimalFaintClearChecker;

            EditorGUILayout.LabelField("〇動物壁衝突クリアチェッカーの設定");
            Animal.objectReferenceValue = EditorGUILayout.ObjectField("動物オブジェクト", chacker.m_Animal, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
