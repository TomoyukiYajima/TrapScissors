using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimalClearChacker : ClearChacker
{
    #region シリアライズ変数
    [SerializeField]
    private GameObject m_Animal;    // 動物オブジェクト
    #endregion

    private Enemy3D m_Enemy;        // 動物スクリプト

    // Use this for initialization
    public override void Start () {
        base.Start();

        m_Enemy = m_Animal.GetComponent<Enemy3D>();
    }

    // Update is called once per frame
    public override void Update()
    {
        // 動物がトラバサミにかかったら、チュートリアルクリア
        if (m_Enemy.IsTrapHit()) TutorialClear();
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimalClearChacker), true)]
    [CanEditMultipleObjects]
    public class ClearAnimalChackerEditor : ClearChackerEditor
    {
        SerializedProperty Animal;

        protected override void OnChildEnable()
        {
            Animal = serializedObject.FindProperty("m_Animal");
        }

        protected override void OnChildInspectorGUI()
        {
            AnimalClearChacker chacker = target as AnimalClearChacker;

            EditorGUILayout.LabelField("〇動物クリアチェッカーの設定");
            Animal.objectReferenceValue = EditorGUILayout.ObjectField("動物オブジェクト", chacker.m_Animal, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
