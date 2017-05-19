﻿using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DropShadow : MonoBehaviour {

    #region シリアライズ変数
    [SerializeField]
    private GameObject m_Object;            // 影の元のオブジェクト
    [SerializeField]
    private Transform m_AnimalObj;          // 動物オブジェクト
    [SerializeField]
    private Transform m_SpriteObj;          // 動物オブジェクト
    #endregion

    #region 基盤関数
    //// Use this for initialization
    void Start()
    {
        //m_Parent = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update () {
        // レイの更新
        RayUpdate();
    }
    #endregion

    #region レイ関連
    // レイの更新
    private void RayUpdate()
    {
        // 一回スケールを０にする
        //this.transform.localScale = Vector3.zero;
        // 元のオブジェクトがいない場合は返す
        if (m_Object == null) return;
        // オブジェクトの位置から地面までのレイを伸ばす
        var pos = m_Object.transform.position;
        Ray ray = new Ray(pos, pos - Vector3.up * 100.0f);
        RaycastHit hitInfo;
        var hit = Physics.Raycast(ray, out hitInfo);
        // 地面に当たらなかった場合は返す
        if (!hit || hitInfo.collider.gameObject.tag != "Ground") return;
        // 衝突地点の位置に変更する
        var point = hitInfo.point;
        this.transform.position = point + Vector3.up * 0.1f;
        // 距離に応じて、影の大きさを変える
        var length = Vector3.Distance(m_Object.transform. position, point);
        var scale = Mathf.Clamp(1.0f / (length / 3.0f), 0.1f, 1.2f);
        //this.transform.localScale = Vector3.one * scale;

        // 画像の角度を合わせる(Y軸)
        //var degree = (m_AnimalObj.rotation.y + m_SpriteObj.rotation.y) * 180.0f / Mathf.PI;
        //this.transform.rotation = Quaternion.Euler(0.0f, degree, 0.0f);
    }
    #endregion

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(DropShadow), true)]
    [CanEditMultipleObjects]
    public class DropShadowEditor : Editor
    {
        SerializedProperty Object;
        SerializedProperty AnimalObj;
        SerializedProperty SpriteObj;

        public void OnEnable()
        {
            Object = serializedObject.FindProperty("m_Object");
            AnimalObj = serializedObject.FindProperty("m_AnimalObj");
            SpriteObj = serializedObject.FindProperty("m_SpriteObj");
        }

        public override void OnInspectorGUI()
        {
            // 必ず書く
            serializedObject.Update();

            DropShadow dropShadow = target as DropShadow;

            // GameObject
            Object.objectReferenceValue = EditorGUILayout.ObjectField("影を出すオブジェクト", dropShadow.m_Object, typeof(GameObject), true);
            AnimalObj.objectReferenceValue = EditorGUILayout.ObjectField("動物オブジェクト", dropShadow.m_AnimalObj, typeof(Transform), true);
            SpriteObj.objectReferenceValue = EditorGUILayout.ObjectField("スプライトオブジェクト", dropShadow.m_SpriteObj, typeof(Transform), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
