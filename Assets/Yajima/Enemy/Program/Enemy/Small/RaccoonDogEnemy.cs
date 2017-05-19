using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaccoonDogEnemy : SmallEnemy {

    private int m_PrevPointNum = 0;     // 前回の移動ポイント
    private List<Transform> m_Points =
        new List<Transform>();          // ポイント配列
    private float m_Radius = 7.0f;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        // 配列ポイント
        var parent = GameObject.Find("MovePoints");
        // 子オブジェクト
        foreach (Transform child in parent.transform)
        {
            var length = Vector3.Distance(
                child.position, this.transform.position
                );
            // 一定距離外なら返す
            if (length > m_Radius) continue;
            // 一定距離内なら、ポインタに追加する
            m_Points.Add(child);
        }
    }

    //// Update is called once per frame
    //void Update () {

    //}

    protected override void ChangeMovePoint()
    {
        //base.ChangeMovePoint();
        // 次の移動ポイントに変更
        var num = Random.Range(0, m_Points.Count);
        if (num == 0) return;
        // 前回の番号と同一なら、変更する
        if (num == m_PrevPointNum) num = (num + 1) % m_Points.Count;
        m_PrevPointNum = num;
        var pos = m_Points[num].position;
        ChangeMovePoint(pos);
    }

    protected override void DrawGizmos()
    {
        base.DrawGizmos();
        // 円の描画
        var color = Color.green;
        color.a = 0.2f;
        Gizmos.color = color;
        //DrawObjLine(m_MovePoints[m_CurrentMovePoint].name);
        Gizmos.DrawSphere(transform.parent.position, m_Radius);
    }
}
