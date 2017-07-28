using UnityEngine;
using System.Collections;

public class WallChackPoint : MonoBehaviour
{
    #region 変数
    private bool m_IsWallHit = false;           // 壁に衝突したか
    private bool m_IsStageClampHit = false;    // ステージクランプに衝突したか
    //private GameObject m_HitWallObj;            // 当たったオブジェクト
    #endregion

    #region 関数
    // 壁に衝突したかを返します
    public bool IsWallHit() { return m_IsWallHit; }

    // ステージクランプに衝突したかを返します
    public bool IsStageClampHit() { return m_IsStageClampHit; }

    // 方向を変えます
    public void ChangeDirection(Vector3 dir)
    {
        var pos = gameObject.transform.localPosition;
        var newPos = new Vector3(
            pos.x * dir.x,
            pos.y * dir.y,
            pos.z * dir.z
            );
        gameObject.transform.localPosition = newPos;
    }

    // 衝突したものを確認します
    private void CheckCollider(Collider col)
    {
        if (col.gameObject.tag == "Wall") m_IsWallHit = true;
        else if (col.gameObject.tag == "StageClamp") m_IsStageClampHit = true;
    }

    #region Unity関数
    public void OnTriggerEnter(Collider other)
    {
        CheckCollider(other);
    }

    public void OnTriggerStay(Collider other)
    {
        CheckCollider(other);
    }

    public void OnTriggerExit(Collider other)
    {
        m_IsWallHit = false;
        m_IsStageClampHit = false;
    }
    #endregion
    #endregion
}
