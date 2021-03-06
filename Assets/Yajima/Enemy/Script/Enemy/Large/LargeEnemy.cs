﻿using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LargeEnemy : MiddleEnemy {

    #region 関数
    #region override関数
    protected override bool SearchAnimal()
    {
        if (m_DState == AnimalState_DiscoverState.Discover_Animal) return false;

        // 自分以外の動物を襲うようにする
        if (SearchAnimal("SmallEnemy")) return true;
        else if (SearchAnimal("LargeEnemy")) return true;

        return false;
    }

    protected override void SetAnimator()
    {
        base.SetAnimator();
        // 睡眠アニメーション
        m_AnimatorStates[(int)AnimalAnimatorNumber.ANIMATOR_SLEEP_NUMBER] = "Sleep";
    }
    #endregion
    #endregion

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(LargeEnemy), true)]
    [CanEditMultipleObjects]
    public class LargeEnemyEditor : Enemy3DEditor
    {
        SerializedProperty AttackCollider;

        protected override void OnChildEnable()
        {
            AttackCollider = serializedObject.FindProperty("m_AttackCollider");
        }

        protected override void OnChildInspectorGUI()
        {
            LargeEnemy largeEnemy = target as LargeEnemy;

            EditorGUILayout.LabelField("〇大型動物のステータス");
            AttackCollider.objectReferenceValue = EditorGUILayout.ObjectField("攻撃判定", largeEnemy.m_AttackCollider, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
