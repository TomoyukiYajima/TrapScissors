using UnityEngine;
using System.Collections;

public class BearEnemy : LargeEnemy {

    #region 関数
    #region 基盤関数
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        // 睡眠状態に変更
        m_State = State.Sleep;
        // スプライトカラーの変更
        ChangeSpriteColor(new Color(1.0f, 0.0f, 1.0f, 1.0f));
        m_Agent.Stop();
    }
    #endregion

    #region override関数
    public override void SoundNotice(Transform point)
    {
        // 眠っている場合は反応しない
        // プレイヤーを見つけた場合、音のなった位置に移動
        if (m_State == State.DiscoverMove) SoundMove(point);
    }

    protected override bool IsFoodCheck(Food.Food_Kind food)
    {
        // return food == Food.Food_Kind.Tanuki || food == Food.Food_Kind.Rabbit;
        return food != Food.Food_Kind.NULL;
    }

    protected override bool IsLikeFood(Food.Food_Kind food)
    {
        return food == Food.Food_Kind.Goat || food == Food.Food_Kind.Tanuki;
    }
    #endregion

    #region public関数
    public void CheckFood()
    {
        // えさ判定で、trueならば、起こす(待機状態に遷移)
        int value1 = Random.Range(1, 100 + 1);
        // 個数によって、判定用の値を変える
        // ゲームマネージャから、えさの個数を取得する
        int count = GameManager.gameManager.FoodCountCheck();
        int value2 = Mathf.Min(count, 10) * 3;
        // 乱数値と比較して、大きかったら起こす
        //if (value1 < value2)
        // (乱数値 >= 反応するまでの値)
        if (value1 > value2) return;

        ChangeState(State.Idel, AnimationNumber.ANIME_IDEL_NUMBER);
        // 視界の描画をONにする
        if (!m_RayPoint.gameObject.activeSelf)
            m_RayPoint.gameObject.SetActive(true);
        ChangeSpriteColor(Color.red);
        m_Agent.Resume();
    }
    #endregion
    #endregion
}
