using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointClearChacker : ClearChacker
{
    [SerializeField]
    private GameObject m_Effect; // 生成するエフェクト

    // Use this for initialization
    //public override void Start () {

    //}

    //// Update is called once per frame
    //public override void Update () {

    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerSprite")
        {
            // チュートリアルのクリア
            TutorialClear();
            Instantiate(m_Effect, this.transform.position, Quaternion.identity,this.transform);
        }
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(PointClearChacker), true)]
    [CanEditMultipleObjects]
    public class PointClearChackerrEditor : ClearChackerEditor
    {
        SerializedProperty Effect;

        protected override void OnChildEnable()
        {
            Effect = serializedObject.FindProperty("m_Effect");
        }

        protected override void OnChildInspectorGUI()
        {
            PointClearChacker chacker = target as PointClearChacker;

            EditorGUILayout.LabelField("〇ポイントクリアチェッカーの設定");

            Effect.objectReferenceValue = EditorGUILayout.ObjectField("生成するエフェクト", chacker.m_Effect, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
