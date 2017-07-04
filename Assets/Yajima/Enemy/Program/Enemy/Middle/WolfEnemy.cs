using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WolfEnemy : MiddleEnemy {

    #region 変数
    private Food.Food_Kind m_EatFood;   // 食べたえさ
    private List<Transform> m_Boars = 
        new List<Transform>();          // イノシシの配列
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        m_AnimalFeedName = "RabbitEnemy";

        // イノシシの追加処理
        var enemies = GameObject.Find("Enemies");
        foreach (Transform child in enemies.transform)
        {
            // 指定文字列が無かったら、次のオブジェクトを確かめる
            if (child.name.IndexOf("BoarCreateBox") < 0) continue;
            var boar = child.FindChild("Boar");
            var boarAnimal = boar.FindChild("BoarEnemy");
            if (boarAnimal == null) continue;
            // イノシシの追加
            m_Boars.Add(boarAnimal);
        }
    }
    #endregion

    #region override関数
    protected override void Attack(float deltaTime)
    {
        // 攻撃判定をアクティブ状態に変更
        if (!m_AttackCollider.activeSelf)
            m_AttackCollider.SetActive(true);
        //base.Attack(deltaTime);
        if (m_StateTimer < 2.0f) return;
        // 待機状態に遷移
        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        //ChangeSpriteColor(Color.red);
        m_Agent.Resume();
        // 攻撃判定を非アクティブ状態に変更
        m_AttackCollider.SetActive(false);
    }

    protected override void EatFood()
    {
        // 臭い肉を食べた場合
        if(m_EatFood == Food.Food_Kind.SmellMeat)
        {
            // プレイヤーを見つけていたら、追跡状態に遷移
            if(m_Player != null)
            {
                ChangeDiscoverState(DiscoverState.Discover_Player);
                //m_Mark.ExclamationMark();
                ChangeSpriteColor(Color.blue);
                // えさの削除
                Destroy(m_FoodObj);
                m_FoodObj = null;
                m_Agent.Resume();
                // アニメーションの変更
                //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_CHASE_NUMBER], 0.1f, -1);
                ChangeAnimation(AnimatorNumber.ANIMATOR_CHASE_NUMBER);
                return;
            }
        }

        base.EatFood();
    }

    // 動物発見時の行動
    protected override void DiscoverAnimal(float deltaTime)
    {
        // 移動ポイントを見つけた動物の位置にする
        m_Agent.destination = m_TargetAnimal.transform.position;
        //base.DiscoverAnimal(deltaTime);
        //GameObject target = null;
        // 一定距離内なら、攻撃状態に遷移
        var length = Vector3.Distance(this.transform.position, m_TargetAnimal.transform.position);
        var otherCol = m_TargetAnimal.GetComponent<BoxCollider>();
        var scale = m_TargetAnimal.transform.localScale.z * otherCol.size.z;
        if (length > scale + 1.0f) return;
        ChangeState(State.Attack, AnimatorNumber.ANIMATOR_ATTACK_NUMBER);
        // アニメーションの変更
        //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_ATTACK_NUMBER], 0.1f, -1);
        m_Agent.Stop();
    }

    protected override void SearchAnimal()
    {
        // 小さい動物を捜す
        SearchAnimal("SmallEnemy");
        // イノシシが見えているかを確かめる
        for (int i = 0; i != m_Boars.Count; i++)
        {
            if (!InObject(m_Boars[i].gameObject)) continue;
            if (!m_Boars[i].gameObject.transform.parent.parent.gameObject.activeSelf) continue;
            // 動物発見状態に遷移
            ChangeDiscoverState(DiscoverState.Discover_Animal);
            // アニメーションの変更
            //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_CHASE_NUMBER], 0.1f, -1);
            ChangeAnimation(AnimatorNumber.ANIMATOR_CHASE_NUMBER);
            //m_Animal = m_Boars[i].GetComponent<Enemy3D>();
            m_TargetAnimal = m_Boars[i].gameObject;
            break;
        }
    }

    // 音反応なし
    public override void SoundNotice(Transform point) { }

    protected override bool IsFoodCheck(Food.Food_Kind food)
    {
        return food == Food.Food_Kind.Meat || food == Food.Food_Kind.SmellMeat;
    }

    protected override bool IsLikeFood(Food.Food_Kind food)
    {
        //return food == Food.Food_Kind.Tanuki;
        m_EatFood = food;
        return food == Food.Food_Kind.Meat || food == Food.Food_Kind.SmellMeat;
    }
    #endregion
    #endregion
}