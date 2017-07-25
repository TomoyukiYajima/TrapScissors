using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BearEnemy : LargeEnemy {

    #region 変数
    #region シリアライズ変数
    [SerializeField]
    protected GameObject m_RemovePoint = null;  // 逃げるポイント
    #endregion

    #region private変数
    private RunawayPoint m_RunawayPoint;    // 逃げ用ポイント
    private float m_MoveLength = 0.0f;      // 移動距離
    #endregion
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        m_RunawayPoint = m_RemovePoint.GetComponent<RunawayPoint>();
        // 睡眠状態に変更
        m_State = State.Sleep;
        m_MotionNumber = (int)AnimatorNumber.ANIMATOR_TRAP_HIT_NUMBER;
        m_Agent.isStopped = true;
    }

    protected override void Update()
    {
        base.Update();

        m_RunawayPoint.SetPosition(this.transform.position);
    }
    #endregion

    #region override関数
    public override void SoundNotice(Transform point)
    {
        // 眠っている場合は起きる
        // プレイヤーを見つけた場合、音のなった位置に移動
        if (m_State == State.Sleep)
        {
            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            // 視界の描画をONにする
            if (!m_RayPoint.gameObject.activeSelf)
                m_RayPoint.gameObject.SetActive(true);
            m_Agent.isStopped = false;
        }
        else base.SoundNotice(point);
    }

    //protected override void DiscoverAnimal(float deltaTime)
    //{
    //    // 逃げる
    //    ChangeMovePoint(m_RunawayPoint.gameObject.transform.position);
    //    m_MoveLength += m_DiscoverSpeed * deltaTime;

    //    // 壁を発見したとき
    //    GameObject wall = null;
    //    Vector3 point = Vector3.zero;
    //    if (InWall(out wall, out point, 2))
    //    {
    //        // 壁と衝突点との外積を求めて、角度を決める
    //        var up = wall.transform.up;
    //        var vec = point - wall.transform.position;
    //        var cross = Vector3.Cross(up, vec);
    //        var rotate = Mathf.Atan2(vec.z, vec.x);

    //        // 壁に沿うように逃げる
    //        //var rotate = wall.transform.rotation.eulerAngles;
    //        m_RunawayPoint.ChangeAddPosition(rotate);

    //        // 壁に沿うように逃げる
    //        //var rotate = wall.transform.rotation.eulerAngles;
    //        //m_RunawayPoint.ChangeAddPosition(rotate.y);
    //        //print(rotate.y.ToString());
    //    }

    //    // 一定距離移動したら、待機状態に遷移
    //    if (m_MoveLength > 20)
    //    {
    //        // 待機状態に遷移
    //        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
    //        m_MoveLength = 0.0f;
    //        // 移動速度を変える
    //        m_Agent.speed = m_Speed;
    //        m_Agent.isStopped = false;
    //    }
    //}

    protected override void DiscoverAnimal(float deltaTime)
    {
        // 移動ポイントを見つけた動物の位置にする
        m_Agent.destination = m_TargetAnimal.transform.position;
        // 一定距離内なら、攻撃状態に遷移
        var length = Vector3.Distance(this.transform.position, m_TargetAnimal.transform.position);
        var otherCol = m_TargetAnimal.GetComponent<BoxCollider>();
        var scale = m_TargetAnimal.transform.localScale.z * otherCol.size.z;
        if (length > scale + 1.0f) return;
        ChangeState(State.Attack, AnimatorNumber.ANIMATOR_ATTACK_NUMBER);
        // アニメーションの変更
        m_Agent.isStopped = true;
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
    #endregion

    #region public関数
    public void CheckFood()
    {
        // 睡眠状態でなければ返す
        if (m_State != State.Sleep) return;

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

        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        
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

        protected override void OnChildEnable()
        {
            base.OnChildEnable();

            RemovePoint = serializedObject.FindProperty("m_RemovePoint");
        }

        protected override void OnChildInspectorGUI()
        {
            base.OnChildInspectorGUI();

            EditorGUILayout.Space();

            BearEnemy enemy = target as BearEnemy;

            EditorGUILayout.LabelField("〇クマ固有のステータス");
            RemovePoint.objectReferenceValue = EditorGUILayout.ObjectField("逃げポイント", enemy.m_RemovePoint, typeof(GameObject), true);

        }
    }
#endif
    #endregion
}
