using UnityEngine;
using System.Collections;

public class LargeEnemy : MiddleEnemy {

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    #region override関数
    public override void SoundNotice(Transform point)
    {
        base.SoundNotice(point);
    }
    #endregion
}
