using UnityEngine;
using System.Collections;

public class WolfEnemy : MiddleEnemy {

    // Use this for initialization

    private Food.Food_Kind m_EatFood;   // 食べたえさ

    protected override void Start()
    {
        base.Start();
        m_AnimalFeedName = "RabbitEnemy";
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    #region override関数
    protected override void EatFood()
    {
        // 臭い肉を食べた場合
        if(m_EatFood == Food.Food_Kind.Rabbit)
        {
            // プレイヤーを見つけていたら、追跡状態に遷移
            if(m_Player != null)
            {
                ChangeDiscoverState(DiscoverState.Discover_Player);
                ChangeSpriteColor(Color.blue);
                // えさの削除
                Destroy(m_FoodObj);
                m_FoodObj = null;
                m_Agent.Resume();
                return;
            }
        }

        base.EatFood();
    }

    // 音反応なし
    public override void SoundNotice(Transform point) { }

    protected override bool IsFoodCheck(Food.Food_Kind food)
    {
        return food == Food.Food_Kind.Tanuki || food == Food.Food_Kind.Rabbit;
    }

    protected override bool IsLikeFood(Food.Food_Kind food)
    {
        //return food == Food.Food_Kind.Tanuki;
        m_EatFood = food;
        return food == Food.Food_Kind.Tanuki || food == Food.Food_Kind.Rabbit;
    }
    #endregion
}
