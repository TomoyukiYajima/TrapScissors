using UnityEngine;
using System.Collections;

public class RabbitEnemy : SmallEnemy {
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        m_FoodState = Food.Food_Kind.Rabbit;
    }

    //// Update is called once per frame
    //void Update () {

    //}
}
