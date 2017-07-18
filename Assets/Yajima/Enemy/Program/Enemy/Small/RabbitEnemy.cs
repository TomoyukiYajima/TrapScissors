using UnityEngine;
using System.Collections;

public class RabbitEnemy : SmallEnemy {
    #region override関数
    public override void SoundNotice(Transform point)
    {
        // 音のなった位置から離れます
        PointRunaway(point);
    }
    #endregion
}
