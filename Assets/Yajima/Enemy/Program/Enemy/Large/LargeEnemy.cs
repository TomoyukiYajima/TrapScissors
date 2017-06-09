using UnityEngine;
using System.Collections;

public class LargeEnemy : MiddleEnemy {

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    protected override void SetAnimator()
    {
        base.SetAnimator();
        // 睡眠アニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_SLEEP_NUMBER] = "Sleep";
    }
}
