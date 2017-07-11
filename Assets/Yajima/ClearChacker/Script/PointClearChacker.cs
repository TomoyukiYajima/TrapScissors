using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointClearChacker : ClearChacker
{
    [SerializeField]
    private GameObject m_Effect;        // 生成するエフェクト
    [SerializeField]
    private GameObject m_ParticleObj;   // パーティクルオブジェクト

    private ParticleSystem m_Particle;  // パーティクルシステム

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        // パーティクルが空だったら、子オブジェクトから取得する
        if (m_ParticleObj == null) m_ParticleObj = this.transform.Find("TutorialPoint").gameObject;
        m_Particle = m_ParticleObj.GetComponent<ParticleSystem>();
    }

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
            m_Particle.Stop();
        }
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(PointClearChacker), true)]
    [CanEditMultipleObjects]
    public class PointClearChackerrEditor : ClearChackerEditor
    {
        SerializedProperty Effect;
        SerializedProperty ParticleObj;

        protected override void OnChildEnable()
        {
            Effect = serializedObject.FindProperty("m_Effect");
            ParticleObj = serializedObject.FindProperty("m_ParticleObj");
        }

        protected override void OnChildInspectorGUI()
        {
            PointClearChacker chacker = target as PointClearChacker;

            EditorGUILayout.LabelField("〇ポイントクリアチェッカーの設定");

            Effect.objectReferenceValue = EditorGUILayout.ObjectField("生成するエフェクト", chacker.m_Effect, typeof(GameObject), true);

            ParticleObj.objectReferenceValue = EditorGUILayout.ObjectField("出し続けているエフェクト", chacker.m_ParticleObj, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
