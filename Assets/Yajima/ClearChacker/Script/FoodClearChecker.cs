using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FoodClearChecker : ClearChacker
{

    public GameObject m_Food;           // えさオブジェクト

    private bool m_IsGetFood = false;   // えさを取得したか

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (m_Food == null)
        {
            var obj = this.transform.GetChild(0);
            m_Food = obj.gameObject;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (m_Food == null)
        {
            if (m_IsGetFood) return;
            DrawText();
            TutorialMediator.GetInstance().SetTutorialAction(false);
            m_IsGetFood = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag != "Player" || m_IsGetFood) return;
        TutorialMediator.GetInstance().SetTutorialAction(true);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.tag != "Player" || m_IsGetFood) return;
        TutorialMediator.GetInstance().SetTutorialAction(false);
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(FoodClearChecker), true)]
    [CanEditMultipleObjects]
    public class FoodClearCheckerEditor : ClearChackerEditor
    {
        SerializedProperty Food;

        protected override void OnChildEnable()
        {
            Food = serializedObject.FindProperty("m_Food");
        }

        protected override void OnChildInspectorGUI()
        {
            FoodClearChecker chacker = target as FoodClearChecker;

            EditorGUILayout.LabelField("〇フードクリアチェッカーの設定");
            Food.objectReferenceValue = EditorGUILayout.ObjectField("えさオブジェクト", chacker.m_Food, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
