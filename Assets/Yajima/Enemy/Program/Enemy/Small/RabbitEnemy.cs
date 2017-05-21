using UnityEngine;
using System.Collections;

public class RabbitEnemy : SmallEnemy {
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //m_FoodState = Food.Food_Kind.Rabbit;
    }

    //// Update is called once per frame
    //void Update () {

    //}

    #region override関数
    public override void SoundNotice(Transform point)
    {
        // 音のなった位置から離れます
        PointRunaway(point);
    }
    #endregion
}
