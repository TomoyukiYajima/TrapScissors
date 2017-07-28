using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BearEnemy : LargeEnemy {

    #region 変数
    #region シリアライズ変数
    [SerializeField]
    protected GameObject m_RemovePoint = null;      // 逃げるポイント
    [SerializeField]
    protected GameObject m_SleepCollider = null;    // 睡眠時の衝突判定
    #endregion

    #region private変数
    private RunawayPoint m_RunawayPoint;    // 逃げ用ポイント
    private SleepCollider m_SCollider;  // 睡眠時用衝突判定スクリプト
    #endregion
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        m_MiddleSENumber = 23;
        m_SENormalTime = 0.2f;
        m_RunawayPoint = m_RemovePoint.GetComponent<RunawayPoint>();
        // 睡眠状態に変更
        m_State = AnimalState.Sleep;
        m_MotionNumber = (int)AnimalAnimatorNumber.ANIMATOR_TRAP_HIT_NUMBER;
        m_Agent.isStopped = true;
        m_SCollider = m_SleepCollider.GetComponent<SleepCollider>();
    }

    protected override void Update()
    {
        base.Update();

        m_RunawayPoint.SetPosition(this.transform.position);

        // 睡眠判定オブジェクトが衝突したら、起きるようにする
        if (m_SCollider.IsHit() && m_State == AnimalState.Sleep) WakeUp();
    }
    #endregion

    #region override関数
    public override void SoundNotice(Transform point)
    {
        // 眠っている場合は起きる
        // プレイヤーを見つけた場合、音のなった位置に移動
        if (m_State == AnimalState.Sleep) WakeUp();
        else base.SoundNotice(point);
    }

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

    protected override void AnimalHit(GameObject animal)
    {
        base.AnimalHit(animal);
        // 逃げポイントの追加位置の変更
        // 前方ベクトルから、角度の取得
        var vec = animal.transform.position - this.transform.position;
        var angle = Mathf.Atan2(vec.z, vec.x);
        m_Mark.ExclamationMark();
        m_RunawayPoint.ChangeAddPosition(angle * Mathf.Rad2Deg - 180);
    }

    protected override bool IsFoodCheck(Food.Food_Kind food)
    {
        // return food == Food.Food_Kind.Tanuki || food == Food.Food_Kind.Rabbit;
        return food != Food.Food_Kind.NULL;
    }

    protected override bool IsLikeFood(Food.Food_Kind food)
    {
        return food == Food.Food_Kind.Carrot || food == Food.Food_Kind.Meat;
    }

    //protected override void TriggerEnterObject(Collider other)
    //{
    //    base.TriggerEnterObject(other);

    //    // プレイヤーに衝突したら、起こすようにする
    //    if (m_State != State.Sleep) return;
    //    ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
    //    this.tag = "LargeEnemy";
    //}
    #endregion

    #region public関数
    public void CheckFood()
    {
        // 睡眠状態でなければ返す
        if (m_State != AnimalState.Sleep) return;

        // えさ判定で、trueならば、起こす(待機状態に遷移)
        int value1 = Random.Range(1, 100 + 1);
        // 個数によって、判定用の値を変える
        // ゲームマネージャから、えさの個数を取得する
        int count = GameManager.gameManager.FoodCountCheck();
        int maxCount = 5;
        int value2 = count * (100 / maxCount);
        // 乱数値と比較して、大きかったら起こす
        // (乱数値 > 反応するまでの値)
        //print("最大値 " + value2.ToString() + " を超えたら返す : " + value1.ToString());
        if (value1 > value2) return;

        ChangeState(AnimalState.Idel, AnimalAnimatorNumber.ANIMATOR_IDEL_NUMBER);
        
        // 視界の描画をONにする
        if (!m_RayPoint.gameObject.activeSelf)
            m_RayPoint.gameObject.SetActive(true);
        m_Agent.isStopped = false;
    }
    #endregion

    #region private関数
    private void WakeUp()
    {
        ChangeState(AnimalState.Idel, AnimalAnimatorNumber.ANIMATOR_IDEL_NUMBER);
        // 視界の描画をONにする
        if (!m_RayPoint.gameObject.activeSelf)
            m_RayPoint.gameObject.SetActive(true);
        m_Agent.isStopped = false;
    }
    #endregion
    #endregion

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(BearEnemy), true)]
    [CanEditMultipleObjects]
    public class BearEditor : LargeEnemyEditor
    {
        SerializedProperty RemovePoint;
        SerializedProperty SleepCollider;

        protected override void OnChildEnable()
        {
            base.OnChildEnable();

            RemovePoint = serializedObject.FindProperty("m_RemovePoint");
            SleepCollider = serializedObject.FindProperty("m_SleepCollider");
        }

        protected override void OnChildInspectorGUI()
        {
            base.OnChildInspectorGUI();

            EditorGUILayout.Space();

            BearEnemy enemy = target as BearEnemy;

            EditorGUILayout.LabelField("〇クマ固有のステータス");
            RemovePoint.objectReferenceValue = EditorGUILayout.ObjectField("逃げポイント", enemy.m_RemovePoint, typeof(GameObject), true);
            SleepCollider.objectReferenceValue = EditorGUILayout.ObjectField("睡眠時の衝突判定", enemy.m_SleepCollider, typeof(GameObject), true);
        }
    }
#endif
    #endregion
}
