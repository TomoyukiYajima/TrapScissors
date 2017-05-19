using UnityEngine;
using System.Collections;

public class WolfEnemy : MiddleEnemy {

    // Use this for initialization
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
    // 音反応なし
    public override void SoundNotice(Transform point) { }
    #endregion
}
