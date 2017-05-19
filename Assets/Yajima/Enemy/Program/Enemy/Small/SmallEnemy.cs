using UnityEngine;
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

        base.DiscoverPlayer(deltaTime);
    }
    #endregion

    #region override関数
    protected override void ReturnMove(float deltaTime, float subSpeed = 1.0f)
    {
        base.ReturnMove(deltaTime, subSpeed);
        // 移動距離の加算
        //m_MoveLength += Mathf.Abs(m_TotalVelocity.x) + Mathf.Abs(m_TotalVelocity.y) + Mathf.Abs(m_TotalVelocity.z);
    }

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

    public override void SoundNotice(Transform point)
    {
        //var pointBox = GameObject.Find("MovePoints");
        // 移動ポイントコンテナがない場合は、
        // 自分の持っているポイントで移動する
        var length = 0.0f;
        var pointPos = point.position;
        var setPos = Vector3.zero;
        // 持っているポイントで、音の位置との最長距離を求める
        for (int i = 0; i != m_MovePoints.Length; i++)
        {
            var pos = m_MovePoints[i].position;
            var pointLength = Vector3.Distance(pointPos, pos);
            //var degree = Vector3.Angle(pointPos, pos);
            // 前回のポイントとの位置より長かったら,
            // 角度が一定角度より大きければ更新する
            //  && Mathf.Abs(degree) > 20.0f
            if (length < pointLength)
            {
                length = pointLength;
                setPos = pos;
            }
        }
        ChangeMovePoint(setPos);

        //if (pointBox == null)
        //{
        //    var length = 0.0f;
        //    var pointPos = point.position;
        //    // 持っているポイントで、音の位置との最長距離を求める
        //    for (int i = 0; i != m_MovePoints.Length; i++)
        //    {
        //        var pos = m_MovePoints[i].position;
        //        var pointLength = Vector3.Distance(pointPos, pos);
        //        var degree = Vector3.Angle(pointPos, pos);
        //        // 前回のポイントとの位置より長かったら,
        //        // 角度が一定角度より大きければ更新する
        //        if (length < pointLength && Mathf.Abs(degree) > 20.0f)
        //        {
        //            length = pointLength;
        //            m_MovePointPosition = pos;
        //        }
        //    }
        //}
        //else
        //{
        //    // 移動ポイントコンテナがある場合は、全ポイントを調べて
        //    // 移動ポイントを決める
        //    //pointBox.child
        //    int count = 0;
        //    foreach(Transform child in pointBox.transform)
        //    {
        //        m_BoxPoints.Add(child);
        //        m_ResultPoints[m_BoxPoints[count]] = count;
        //        count++;
        //    }
        //    // 取得したポイント全部との最長距離を取る
        //    //var length = 0.0f;
        //    for (int i = 0; i != m_BoxPoints.Count; i++)
        //    {
        //        var pos = m_BoxPoints[i].position;
        //        var pointLength = Vector3.Distance(point.position, pos);
        //        //// 前回のポイントとの位置より長かったら、更新する
        //        //if (length < pointLength)
        //        //{
        //        //    length = pointLength;
        //        //    m_MovePointPosition = pos;
        //        //}

        //        // 移動ポイントの評価

        //    }
        //}
        m_BoxPoints.Clear();
        m_ResultPoints.Clear();
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
