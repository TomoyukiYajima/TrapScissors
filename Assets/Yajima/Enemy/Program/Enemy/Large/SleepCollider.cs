using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCollider : MonoBehaviour
{

    #region 変数
    private bool m_IsHit = false;   // 衝突したか
    private Collider m_Collider;    // 衝突判定
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    void Start()
    {
        m_Collider = this.GetComponent<Collider>();
    }
    #endregion

    #region public関数
    // 衝突したかを返します
    public bool IsHit() { return m_IsHit; }

    public void OnCollisionEnter(Collision collision)
    {
        // プレイヤーに衝突したら、自身の衝突判定を消す
        if (m_Collider.enabled) m_Collider.enabled = false;
        m_IsHit = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        m_IsHit = false;
    }
    #endregion
    #endregion
}
