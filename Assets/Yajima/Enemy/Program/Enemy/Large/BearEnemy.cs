using UnityEngine;
using System.Collections;

public class BearEnemy : LargeEnemy {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    #region override関数
    public override void SoundNotice(Transform point)
    {
        // 眠っている場合は反応しない
        // プレイヤーを見つけた場合、音のなった位置に移動
        if (m_State == State.DiscoverMove) SoundMove(point);
    }

    protected override bool IsFoodCheck(Food.Food_Kind food)
    {
        return food == Food.Food_Kind.Tanuki || food == Food.Food_Kind.Rabbit;
    }
    #endregion
}
