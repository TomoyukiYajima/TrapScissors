using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RaccoonDogEnemy : SmallEnemy {

    #region 変数
    #region シリアライズ変数
    // 移動半径
    [SerializeField]
    private float m_MoveRadius = 7.0f;      // 移動範囲
    [SerializeField]
    private bool m_IsRandomMove = true;     // ランダム移動するか
    #endregion

    #region private変数
    private int m_PrevPointNum = 0;     // 前回の移動ポイント
    private List<Transform> m_Points =
        new List<Transform>();          // 付近のポイント配列
    #endregion
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        // ランダム移動する場合は、ポイントの再設定を行う
        if (m_IsRandomMove) SetRandomMovePoint();
    }
    #endregion

    #region override関数
    // ランダム移動する場合は ChangeMovePoint() をオーバーライドする
#if m_IsRandomMove
    protected override void ChangeMovePoint()
    {
        // 次の移動ポイントに変更
        var num = Random.Range(0, m_MovePoints.Length);
        // 前回の番号と同一なら、変更する
        if (num == m_PrevPointNum) num = (num + 1) % m_MovePoints.Length;
        m_PrevPointNum = num;
        var pos = m_MovePoints[num].position;
        ChangeMovePoint(pos);
    }
#endif

    #endregion

    #region private関数
    private void SetRandomMovePoint()
    {
        // 配列ポイント
        var points = GameObject.Find("MovePoints");
        // 子オブジェクト
        foreach (Transform child in points.transform)
        {
            // ポイントの取得
            var vec1 = new Vector2(child.position.x, child.position.z);
            var vec2 = new Vector2(this.transform.position.x, this.transform.position.z);
            var length = Vector2.Distance(vec1, vec2);
            // 一定距離外なら返す
            if (length > m_MoveRadius) continue;
            // 一定距離内なら、ポインタに追加する
            m_Points.Add(child);
        }
        // 付近のポイントを取得したら、親のポイントに入れる
        ResizeMovePoints(m_Points.Count);
        for (int i = 0; i != m_Points.Count; i++)
        {
            m_MovePoints[i] = m_Points[i];
        }
        // ローカルのポイント配列の中身を削除する
        m_Points.Clear();
        ChangeMovePoint();
    }
    #endregion

    #region Unity関数
    public void OnDrawGizmos()
    {
        //base.DrawGizmos();
        if (!m_IsRandomMove) return;
        // 円の描画
        var color = Color.green;
        color.a = 0.15f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.parent.position, m_MoveRadius);
    }
    #endregion

    #endregion

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(RaccoonDogEnemy), true)]
    [CanEditMultipleObjects]
    public class RaccoonDogEnemyEditor : SmallEditor
    {
        SerializedProperty MoveRadius;
        SerializedProperty IsRandomMove;

        protected override void OnChildEnable()
        {
            base.OnChildEnable();

            MoveRadius = serializedObject.FindProperty("m_MoveRadius");
            IsRandomMove = serializedObject.FindProperty("m_IsRandomMove");
        }

        protected override void OnChildInspectorGUI()
        {
            base.OnChildInspectorGUI();

            RaccoonDogEnemy enemy = target as RaccoonDogEnemy;

            EditorGUILayout.LabelField("〇タヌキ固有のステータス");
            // float
            MoveRadius.floatValue = EditorGUILayout.FloatField("移動範囲", enemy.m_MoveRadius);
            // bool 
            IsRandomMove.boolValue = EditorGUILayout.Toggle("ランダム移動を行うか", enemy.m_IsRandomMove);
        }
    }
#endif
#endregion
}
