﻿using UnityEngine;
using System.Collections;

public class WallChackPoint : MonoBehaviour
{
    #region 変数
    private bool m_IsWallHit = false;   // 壁に衝突したか
    private GameObject m_HitWallObj;        // 当たったオブジェクト
    #endregion

    //void Update()
    //{
        
    //}

    // 壁に衝突したか
    public bool IsWallHit() { return m_IsWallHit; }

    // 衝突した壁を取得します
    public GameObject GetHitWallObj() { return m_HitWallObj; }

    // 方向を変えます
    public void ChangeDirection()
    {
        //m_Direction *= -1;
        Vector3 dir = Vector3.one * -1;
        ChangeDirection(dir);
        //var pos = gameObject.transform.localPosition;
        //var newPos = new Vector3(
        //    pos.x * dir.x,
        //    pos.y * dir.y,
        //    0.0f
        //    );
        //gameObject.transform.localPosition = newPos;
        m_IsWallHit = false;
    }

    public void ChangeDirection(Vector3 dir)
    {
        var pos = gameObject.transform.localPosition;
        var newPos = new Vector3(
            pos.x * dir.x,
            pos.y * dir.y,
            pos.z * dir.z
            );
        gameObject.transform.localPosition = newPos;
        m_IsWallHit = false;
    }

    private void CheckWall(Collider col)
    {
        m_IsWallHit = false;
        if (col.gameObject.tag != "Wall") return;
        m_IsWallHit = true;
        m_HitWallObj = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        CheckWall(other);
    }

    public void OnTriggerStay(Collider other)
    {
        CheckWall(other);
    }

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //if (collision.gameObject.tag != "Wall" && 
    //    //    collision.gameObject.name != "Wall") return;
    //    if (collision.gameObject.tag != "Ground") return;
    //    m_IsWallHit = true;
    //}
}
