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
        m_MiddleSENumber = 19;
        m_SENormalTime = 0.3f;

        // イノシシの追加処理
        var enemies = GameObject.Find("Enemies");
        foreach (Transform child in enemies.transform)
        {
            // 指定文字列が無かったら、次のオブジェクトを確かめる
            if (child.name.IndexOf("BoarCreateBox") < 0) continue;
            var boar = child.Find("Boar");
            var boarAnimal = boar.Find("BoarEnemy");
            if (boarAnimal == null) continue;
            // イノシシの追加
            m_Boars.Add(boarAnimal);
        }
    }
    #endregion

    #region override関数
    //protected override void Attack(float deltaTime)
    //{
    //    // 攻撃判定をアクティブ状態に変更
    //    if (!m_AttackCollider.activeSelf)
    //        m_AttackCollider.SetActive(true);
    //    if (m_StateTimer < 2.0f) return;
    //    // 待機状態に遷移
    //    ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
    //    m_DState = DiscoverState.Discover_None;
    //    m_Agent.isStopped = false;
    //    // 攻撃判定を非アクティブ状態に変更
    //    m_AttackCollider.SetActive(false);
    //}

    protected override void EatFood()
    {
        // 臭い肉を食べた場合
        if(m_EatFood == Food.Food_Kind.SmellMeat)
        {
            // プレイヤーを見つけていたら、追跡状態に遷移
            if(m_Player != null)
            {
                ChangeDiscoverState(AnimalState_DiscoverState.Discover_Player);
                // えさの削除
                Destroy(m_FoodObj);
                m_FoodObj = null;
                m_Agent.isStopped = false;
                // ゲームマネージャ側の減算処理を呼ぶ
                ////GameManager.gameManager.FoodCountSub();
                // アニメーションの変更
                ChangeAnimation(AnimalAnimatorNumber.ANIMATOR_CHASE_NUMBER);
                return;
            }
        }

        base.EatFood();
    }

    // 動物発見時の行動
    protected override int DiscoverAnimal(float deltaTime)
    {
        // 移動ポイントを見つけた動物の位置にする
        m_Agent.destination = m_TargetAnimal.transform.position;
        // 一定距離内なら、攻撃状態に遷移
        var length = Vector3.Distance(this.transform.position, m_TargetAnimal.transform.position);
        var otherCol = m_TargetAnimal.GetComponent<BoxCollider>();
        var scale = m_TargetAnimal.transform.localScale.z * otherCol.size.z;
        if (length > scale + 1.0f) return 0;
        ChangeState(AnimalState.Attack, AnimalAnimatorNumber.ANIMATOR_ATTACK_NUMBER);
        // アニメーションの変更
        m_Agent.isStopped = true;
        return 0;
    }

    protected override bool SearchAnimal()
    {
        var mediator = GameObject.Find("TutorialMediator");
        // チュートリアルステージの2なら、動物に反応しない
        if (mediator != null && !TutorialMediator.GetInstance().IsTutorialAction(2)) return false;
        // 動物発見状態ならfalseを返す
        if (m_DState == AnimalState_DiscoverState.Discover_Animal) return false;
        // 小さい動物を捜す
        if (SearchAnimal("SmallEnemy")) return true;
        // イノシシが見えているかを確かめる
        for (int i = 0; i != m_Boars.Count; i++)
        {
            if (!InObject(m_Boars[i].gameObject)) continue;
            if (!m_Boars[i].gameObject.transform.parent.parent.gameObject.activeSelf) continue;
            // 相手が特定の状態だったら返す
            var boar = m_Boars[i].GetComponent<Enemy3D>();
            if (boar.GetState() == AnimalState.Meat) return false;
            // 動物発見状態に遷移
            ChangeDiscoverState(AnimalState_DiscoverState.Discover_Animal);
            // アニメーションの変更
            ChangeAnimation(AnimalAnimatorNumber.ANIMATOR_CHASE_NUMBER);
            m_TargetAnimal = m_Boars[i].gameObject;
            return true;
            //break;
        }
        // 見つからなかった
        return false;
    }

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

    protected override void TriggerEnterObject(Collider other)
    {
        base.TriggerEnterObject(other);

        var objName = other.name;
        // 攻撃判定との衝突判定
        // お肉状態に遷移
        if (objName == "AttackCollider")
            ChangeMeat();
    }

    protected override void TriggerStayObject(Collider other)
    {
        base.TriggerStayObject(other);

        var objName = other.name;
        // 攻撃判定との衝突判定
        // お肉状態に遷移
        if (objName == "AttackCollider")
            ChangeMeat();
    }
    #endregion
    #endregion
}