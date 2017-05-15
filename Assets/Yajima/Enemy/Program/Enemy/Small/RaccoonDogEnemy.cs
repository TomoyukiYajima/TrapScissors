using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RacoonDogEnemy : SmallEnemy {

    private List<Transform> points =
        new List<Transform>();          // ポイント配列

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //// 配列ポイント
        //var parent = GameObject.Find("MovePoints");
        ////var children = parent.transform.getChil
        //foreach(Transform child in parent.transform)
        //{

        //}
    }

    //// Update is called once per frame
    //void Update () {

    //}
}
