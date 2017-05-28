using UnityEngine;
using System.Collections;

public class RunawayPoint : MonoBehaviour {

    private Vector3 m_AddPosition;      // 追加位置
    private Vector3 m_InitAddPosition;  // 初期の追加位置
    private float m_Length;             // 追加位置との長さ

    // Use this for initialization
    void Start () {
        // 追加位置を入れる
        // ローカル座標を入れる
        m_AddPosition = new Vector3(this.transform.localPosition.x, 0.0f, this.transform.localPosition.z);
        m_InitAddPosition = m_AddPosition;
        m_Length = Vector3.Distance(Vector3.zero, m_AddPosition);
	}
	
	// Update is called once per frame
	void Update () {

	}

    // 位置の設定を行います
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position + m_AddPosition;
    }

    // 追加位置の変更を行います
    public void ChangeAddPosition(float degree)
    {
        var vec = new Vector3(
            Mathf.Cos(degree * Mathf.Deg2Rad), 0.0f, 
            Mathf.Sin(degree * Mathf.Deg2Rad)
            );
        // 単位ベクトルに長さを掛けて渡す
        m_AddPosition = vec * m_Length;
    }

    // 追加位置の初期化を行います
    public void InitAddPosition()
    {
        m_AddPosition = m_InitAddPosition;
    }
}
