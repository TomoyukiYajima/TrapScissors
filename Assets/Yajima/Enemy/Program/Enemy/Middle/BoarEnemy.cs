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

    protected override bool IsFoodCheck(Food.Food_Kind food)
    {

        //if (food == Food.Food_Kind.Goat || food == Food.Food_Kind.Rabbit) return true;
        //return base.IsFoodCheck(food);
        return food == Food.Food_Kind.Goat || food == Food.Food_Kind.Rabbit;
    }

    protected override bool IsLikeFood(Food.Food_Kind food)
    {
        return food == Food.Food_Kind.Tanuki;
    }
    #endregion
}
