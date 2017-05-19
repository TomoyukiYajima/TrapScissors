using UnityEngine;
using System.Collections;

public class BoarEnemy : MiddleEnemy {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    #region override関数
    public override void SoundNotice(Transform point)
    {
        // プレイヤーを見つけた場合、音のなった位置に移動
        // 見つけていない場合、音のなった位置から離れます
        if (m_State == State.DiscoverMove) SoundMove(point);
        else PointRunaway(point);
    }
    #endregion
}
