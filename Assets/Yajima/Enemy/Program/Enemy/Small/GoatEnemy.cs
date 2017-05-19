using UnityEngine;
using System.Collections;

public class GoatEnemy : SmallEnemy {

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        m_FoodState = Food.Food_Kind.Goat;
    }

    //// Update is called once per frame
    //void Update () {

    //}
}
