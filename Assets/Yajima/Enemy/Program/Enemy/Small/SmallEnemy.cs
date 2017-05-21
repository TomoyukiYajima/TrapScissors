﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SmallEnemy : Enemy3D {

    #region シリアライズ変数
    //[SerializeField]
    //protected GameObject m_Canvas = null;               // キャンバス
    //[SerializeField]
    //protected GameObject m_MeatUI = null;               // お肉UI
    #endregion

    #region private関数
    //private GameObject m_Frame;             // キャンバスのフレーム
    private List<Transform> m_BoxPoints =
        new List<Transform>();              // 移動用ポイントコンテナ
    private Dictionary<Transform, int> m_ResultPoints =
        new Dictionary<Transform, int>();   // 移動ポイントの評価
    #endregion

    #region 基盤関数
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //// キャンパスが設定されていなかったら取得
        //if (m_Canvas == null) m_Canvas = GameObject.Find("Canvas");
        //// キャンパスのフレームを取得
        //var frame = m_Canvas.transform.FindChild("Frame");
        //if (frame != null) m_Frame = frame.gameObject;
    }

    protected override void DiscoverPlayer(float deltaTime)
    {
        // 移動(通常の移動速度の数倍)
        Move(deltaTime, m_Speed * 2.0f);

        //base.DiscoverPlayer(deltaTime);]

        // 移動(通常の移動速度の数倍)
        //Move(deltaTime, m_Speed * 2.0f);
        //m_Agent.destination = m_Player.transform.position;
        //Camera.

        GameObject obj = null;
        if (!InPlayer(out obj, 20.0f, true))
        {
            // 待機状態に遷移
            ChangeState(State.Idel, AnimationNumber.ANIME_IDEL_NUMBER);
            m_Agent.Resume();
            m_Player = null;
            ChangeSpriteColor(Color.red);
            return;
        };
    }
    #endregion

    #region override関数
    protected override void ReturnMove(float deltaTime, float subSpeed = 1.0f)
    {
        base.ReturnMove(deltaTime, subSpeed);
        // 移動距離の加算
        //m_MoveLength += Mathf.Abs(m_TotalVelocity.x) + Mathf.Abs(m_TotalVelocity.y) + Mathf.Abs(m_TotalVelocity.z);
    }

    protected override void ChangePlayerHitMove(GameObject player)
    {
        base.ChangePlayerHitMove(player);
        PointRunaway(player.transform);
    }

    //protected override void DiscoverFoodMove(float deltaTime)
    //{
    //    // 二次元(x, z)の距離を求める
    //    var v1 = new Vector2(m_Agent.destination.x, m_Agent.destination.z);
    //    var v2 = new Vector2(m_MouthPoint.position.x, m_MouthPoint.position.z);
    //    var length = Vector2.Distance(v1, v2);

    //    // 一定距離内なら、持ち上げ状態に遷移
    //    if (length < 0.5f)
    //    {
    //        //// 持ち上げ状態に遷移
    //        //ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_Lift);
    //        // えさ食べ状態に遷移
    //        ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_Eat);
    //        ChangeSpriteColor(Color.yellow);
    //        m_Agent.Stop();
    //        return;
    //    }
    //}

    //protected override void TurnWall()
    //{
    //    // 一定距離移動したら、折り返す
    //    //if (m_MoveLength < m_TurnLength * 10) return;

    //    m_MoveLength = 0.0f;
    //    //base.TurnWall();
    //    // 角度の設定
    //    SetDegree();
    //}

    protected override void TrapReleaseAction()
    {
        CreateMeat(AnimalMeat.MeatNumber.SMALL_NUMBER);

        //// 肉UIの生成
        //// GameObject _foodObj = Instantiate(_food);
        //var m = Instantiate(m_MeatUI);
        //var meat = m.GetComponent<AnimalMeat>();
        //meat.SetMeat(AnimalMeat.MeatNumber.SMALL_NUMBER);
        //// カメラ
        //var camera = GameObject.Find("Main Camera");
        //if (camera == null) return;
        //var mainCamera = camera.GetComponent<Camera>();
        //// スプライトの位置に生成
        //meat.SetObjPosition(m_Sprite.transform.position, mainCamera);
        //meat.transform.localScale = Vector3.one;

        //// キャンパスのフレームに追加
        //Instantiate(m_MeatUI);
    }
    #endregion

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(SmallEnemy), true)]
    [CanEditMultipleObjects]
    public class SmallEditor : Enemy3DEditor
    {
        //SerializedProperty CanvasObj;
        //SerializedProperty MeatUI;

        protected override void OnChildEnable()
        {
            //CanvasObj = serializedObject.FindProperty("m_Canvas");
            //MeatUI = serializedObject.FindProperty("m_MeatUI");
        }

        protected override void OnChildInspectorGUI()
        {
            SmallEnemy enemy = target as SmallEnemy;

            //EditorGUILayout.LabelField("〇小動物固有のステータス");
            //// GameObject
            //MeatUI.objectReferenceValue = EditorGUILayout.ObjectField("お肉UIオブジェクト", enemy.m_MeatUI, typeof(GameObject), true);
            //CanvasObj.objectReferenceValue = EditorGUILayout.ObjectField("キャンパスオブジェクト", enemy.m_Canvas, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
