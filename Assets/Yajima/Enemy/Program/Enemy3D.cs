using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy3D : MonoBehaviour
{
    #region 変数
    #region シリアライズ変数
    [SerializeField]
    protected int m_RotateDegree = 180;                 // 振り向き時の角度
    [SerializeField]
    protected float m_Speed = 1.0f;                     // 移動速度
    [SerializeField]
    protected float m_DiscoverSpeed = 4.0f;             // 発見時の移動速度
    [SerializeField]
    protected float m_TrapHitSpeed = 3.0f;              // 移動速度
    [SerializeField]
    protected float m_RageTime = 10.0f;                 // 暴れる時間
    [SerializeField]
    protected float m_ReRageTime = 5.0f;                // 再度暴れる時間
    [SerializeField]
    protected float m_ViewLength = 10.0f;               // プレイヤーが見える距離
    [SerializeField]
    protected float m_ViewAngle = 30.0f;                // プレイヤーが見える角度
    //[SerializeField]
    //protected Transform m_GroundPoint = null;           // 接地ポイント
    [SerializeField]
    protected Transform m_RayPoint = null;              // レイポイント
    [SerializeField]
    protected Transform m_MouthPoint = null;            // 口ポイント
    [SerializeField]
    protected Transform[] m_MovePoints = null;          // 移動用ポイント配列
    //[SerializeField]
    //protected GameObject m_Sprite = null;               // スプライト
    [SerializeField]
    protected GameObject m_Model = null;                // モデル
    [SerializeField]
    protected CameraMove m_MainCamera = null;           // メインカメラ
    [SerializeField]
    protected WallChackPoint m_WChackPoint = null;      // 壁捜索ポイント
    [SerializeField]
    protected GameObject m_Canvas = null;               // キャンバス
    [SerializeField]
    protected GameObject m_Meat = null;                 // お肉
    [SerializeField]
    protected GameObject m_MeatUI = null;               // お肉UI
    [SerializeField]
    protected GameObject m_DiscoverUI = null;           // 発見時のUI
    [SerializeField]
    protected Animator m_Animator;                      // アニメーター
    #endregion

    #region protected変数
    protected int m_Size = 1;                           // 動物の大きさ(内部数値)
    protected float m_StateTimer = 0.0f;                // 状態の時間
    //protected string m_LineObjName = "Player";
    protected string m_FeedName = "Food(Clone)";        // 反応するえさの名前
    // えさStateの追加
    //protected Food.Food_Kind m_FoodState =
    //    Food.Food_Kind.NULL;                            // 反応するえさの状態
    protected string m_AnimalFeedName = "";             // 反応するトラバサミにかかった動物の名前
    protected Enemy3D m_TakeInAnimal;                   // 持ち帰る動物オブジェクト
    protected bool m_IsRendered = false;                // カメラに映っているか
    protected Vector3 m_TotalVelocity = Vector3.zero;   // 合計の移動量
    protected Vector3 m_Velocity = Vector3.right;       // 移動量
    protected Vector3 m_MovePointPosition;              // 移動ポイントの位置
    protected Transform m_DiscoverPlayer;               // プレイヤーを発見
    //protected Transform m_InitPoint;
    protected GameObject m_Player = null;               // 当たったプレイヤー
    protected GameObject m_FoodObj;                     // えさオブジェクト
    protected GameObject m_TargetAnimal;                // 追跡・逃亡用動物
    protected DiscoverMark m_Mark;                      // 発見時のマーク
    protected Trap_Small m_SmallTrap = null;
    protected Rigidbody m_Rigidbody;
    protected Collider m_Collider;                      // 自身のコライダー
    protected DSNumber m_DSNumber =
        DSNumber.DISCOVERED_CHASE_NUMBER;               // 追跡状態の番号 
    protected State m_State = State.Idel;               // 状態
    protected DiscoverState m_DState =
        DiscoverState.Discover_None;                    // 発見状態
    protected DiscoverFoodState m_DFState =
        DiscoverFoodState.DiscoverFood_Move;            // えさ発見状態  
    protected NavMeshAgent m_Agent;                     // ナビメッシュエージェント  

    protected GameManager.GameState m_GameState =
        GameManager.GameState.START;

    // モーション番号
    protected int m_MotionNumber = (int)AnimatorNumber.ANIMATOR_IDEL_NUMBER;
    // モーション配列
    protected Dictionary<int, string> m_AnimatorStates =
        new Dictionary<int, string>();
    #endregion

    #region private変数
    //private bool m_IsPravGround;                    // 前回の接地判定
    //private int m_PointCount = 0;                   // 移動ポイント数
    private int m_CurrentMovePoint = -1;            // 現在の移動ポイント
    private float m_MoveStartTime = 0.5f;           // 移動開始時間
    private bool m_IsTrapHit = false;               // トラバサミに挟まったか
    private GameObject m_TrapsObj = null;           // トラバサミの親クラス
    private Transform m_OtherCreateBox;             // 持ち帰る動物の元の親オブジェクト
    private TrapHitState m_THState =
        TrapHitState.TrapHit_Change;                  // トラップヒット状態
    private State m_PrevState = State.Idel;         // 前回の行動
    //private GameObject m_Frame;             // キャンバスのフレーム                                     
    //private List<State>
    //    m_DiscoveredStates = new List<State>();     // 発見後の行動

    private const int MULT_SPEED = 10;               // 速度の倍率(調整しやすくさせるため)
    private const string PLAYER_TAG = "Player";      // プレイヤータグ
    private const string TRAP_NAME = "sample1(Clone)";
    #endregion
    #endregion

    #region 列挙クラス
    // 状態クラス 
    [System.Flags]
    public enum State
    {
        Idel = 0,           // 待機状態
        Discover = 1,       // 発見状態
        DiscoverMove = 2,   // 発見後の移動状態
        Attack = 4,         // 攻撃状態
        Faint = 8,          // 気絶状態
        Sleep = 16,          // 睡眠状態
        TrapHit = 32,        // トラバサミに挟まれている状態
        Meat = 64,           // お肉状態
        DeadIdel = 128,       // 死亡待機状態
        Runaway = 256,        // 逃亡状態
    }

    // 発見状態
    protected enum DiscoverState
    {
        Discover_None,      // なにも見つけていない状態
        Discover_Player,    // プレイヤーを発見状態
        Discover_Animal,    // 動物発見状態
        Discover_Food,      // えさ発見状態
        Discover_Trap,      // トラバサミ発見状態
        Discover_Lost,      // 見失う状態
        Discover_Lost_Stop  // 見失い停止状態
    }
    // えさ発見状態
    protected enum DiscoverFoodState
    {
        DiscoverFood_Move,      // 発見移動
        DiscoverFood_AnimalMove,    // 動物発見移動
        DiscoverFood_Eat,       // えさ食べ状態
        DiscoverFood_Lift,      // 持ち上げ状態
        DiscoverFood_TakeAway,  // 持ち帰り状態
        //DiscoverFood_Runaway    // えさ逃げ状態
    }
    // トラバサミに挟まれた時の状態クラス
    protected enum TrapHitState
    {
        TrapHit_Change, // トラップ化状態
        TrapHit_TakeIn, // トラバサミに飲み込まれた状態
        TrapHit_Runaway // トラバサミ逃げ状態
        //TrapHit_Rage,   // トラバサミに挟まれている(暴れている)状態
        //TrapHit_Touch,  // トラバサミに挟まれ終わった状態
    }

    // アニメーター番号
    protected enum AnimatorNumber
    {
        ANIMATOR_NULL = 0,
        ANIMATOR_IDEL_NUMBER = 1,
        ANIMATOR_DISCOVER_NUMBER = 2,
        ANIMATOR_CHASE_NUMBER = 3,
        ANIMATOR_ATTACK_NUMBER = 4,
        ANIMATOR_LOST_NUMBER = 5,
        ANIMATOR_TRAP_HIT_NUMBER = 6,
        ANIMATOR_WALL_HIT_NUMBER = 7,
        ANIMATOR_FAINT_NUMBER = 8,
        ANIMATOR_DEAD_NUMBER = 9,
        ANIMATOR_SLEEP_NUMBER = 10
    };

    // DiscoveredStateNumber
    protected enum DSNumber
    {
        DISCOVERED_CHASE_NUMBER = 0,
        DISCOVERED_RUNAWAY_NUMBER = 1,
        DISCOVERED_ANIMAL_TAKEOUT_NUMBER = 2,
        DISCOVERED_FEED_NUMBER = 3
    }

    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    protected virtual void Start()
    {
        // アニメーションリストにリソースを追加
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_Agent = GetComponent<NavMeshAgent>();
        // オブジェクトの確認
        CheckObject();
        // ナビメッシュエージェントの設定
        SetAgentStatus();
        // アニメーターの設定
        SetAnimator();
        //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_IDEL_NUMBER], 0.1f, -1, 0.1f);
        // 移動配列に何も入っていなかった場合は通常移動
        //if(m_MovePoints.Length == 0)

        //m_DiscoveredStates.Add(State.Chase);
        //m_DiscoveredStates.Add(State.Runaway);
        m_Mark = m_DiscoverUI.GetComponent<DiscoverMark>();

        // スプライトカラーの変更
        //ChangeSpriteColor(Color.red);

        // キャンパスが設定されていなかったら取得
        if (m_Canvas == null) m_Canvas = GameObject.Find("Canvas");
        //// キャンパスのフレームを取得
        //var frame = m_Canvas.transform.FindChild("Frame");
        //if (frame != null) m_Frame = frame.gameObject;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        ////ビルボード
        //Vector3 p = m_MainCamera.transform.localPosition;
        ////transform.LookAt(p);
        //m_Sprite.transform.LookAt(p);

        // 衝突時の移動量を消す
        m_Rigidbody.velocity = Vector3.zero;

        //var layer = m_Animator.GetLayerIndex("Base Layer");
        //var state = m_Animator.GetCurrentAnimatorStateInfo(layer);
        //var nTime = state.normalizedTime % 1.0f;

        // ゲームマネージャの状態が変更された場合
        if (m_GameState != GameManager.gameManager.GameStateCheck())
        {
            m_GameState = GameManager.gameManager.GameStateCheck();

            // ゲームプレイでない場合
            if (GameManager.gameManager.GameStateCheck() != GameManager.GameState.PLAY &&
                GameManager.gameManager.GameStateCheck() != GameManager.GameState.START)
            {
                // 自身の衝突判定をオフにする
                //var collider = gameObject.GetComponent<Collider>();
                //if (collider != null) collider.isTrigger = true;
                //if (collider.enabled) collider.enabled = false;
                m_Collider.enabled = false;
                // エージェントの停止
                m_Agent.velocity = Vector3.zero;
                // ゲームマネージャの状態が「PLAY」以外だったら動かない
                if (m_Agent.enabled) m_Agent.Stop();
                m_Animator.enabled = false;
                //m_Animator.Stop();
                //gameObject.SetActive(false);
                return;
            }
            else
            {
                m_Animator.enabled = true;
                if (m_Agent.enabled && m_State != State.Sleep) m_Agent.Resume();
                m_Collider.enabled = true;
                //m_Animator.Play();
            }            
        }

        if (GameManager.gameManager.GameStateCheck() != GameManager.GameState.PLAY &&
            GameManager.gameManager.GameStateCheck() != GameManager.GameState.START) return;

        // 状態の更新
        UpdateState(Time.deltaTime);

        m_IsRendered = false;
    }
    #endregion

    #region 状態関数
    // 状態の更新
    private void UpdateState(float deltaTime)
    {
        // 移動開始時間が 0 になったら移動
        m_MoveStartTime = Mathf.Max(m_MoveStartTime - deltaTime, 0.0f);
        if (m_MoveStartTime > 0.0f) return;

        // 状態の変更
        switch (m_State)
        {
            case State.Idel: Idel(deltaTime); break;
            case State.Discover: Discover(deltaTime); break;
            //case State.DiscoverMove: DiscoverMove(deltaTime); break;
            //case State.Chase: Chase(deltaTime); break;
            //case State.AnimalTakeOut: AnimalTakeOut(deltaTime); break;
            //case State.FeedEat: FeedEat(deltaTime); break;
            case State.Attack: Attack(deltaTime); break;
            case State.Faint: Faint(deltaTime); break;
            case State.Sleep: Sleep(deltaTime); break;
            case State.TrapHit: TrapHit(deltaTime); break;
            case State.Meat: MeatIdel(deltaTime); break;
            case State.DeadIdel: DeadIdel(deltaTime); break;
            case State.Runaway: Runaway(deltaTime); break;
        };

        // 状態の時間加算
        m_StateTimer += deltaTime;

        // 位置ベクトルを代入
        //MoveVelocity();

        //Vector2 newVelocity = m_Rigidbody.velocity;
        //Vector2 gravity = Vector2.up * m_Rigidbody.velocity.y;
        //newVelocity = m_Velocity * m_Speed + gravity;
        //m_Rigidbody.velocity = newVelocity;

        //m_IsPravGround = IsGround();
    }

    // 状態の変更
    protected void ChangeState(State state, AnimatorNumber motion)
    {
        if (m_State == state) return;
        // 前回の状態を入れる
        m_PrevState = m_State;
        // 状態の更新を行う
        m_State = state;
        m_StateTimer = 0.0f;
        // アニメーションの変更
        ChangeAnimation(motion);
        // ナビメッシュエージェントの移動停止
        //m_Agent.Stop();
    }

    // トラップヒット状態の変更
    protected void ChangeTrapHitState(
        TrapHitState thState, AnimatorNumber motion = AnimatorNumber.ANIMATOR_TRAP_HIT_NUMBER)
    {
        // 状態の変更
        ChangeState(State.TrapHit, motion);
        // 同じトラップヒット状態なら返す
        if (m_THState == thState) return;
        m_THState = thState;
        m_StateTimer = 0.0f;
    }

    // アニメーションの変更
    protected void ChangeAnimation(AnimatorNumber motion)
    {
        if (motion == AnimatorNumber.ANIMATOR_NULL || (int)motion == m_MotionNumber) return;
        m_Animator.CrossFade(m_AnimatorStates[(int)motion], 0.1f, 0);
        m_MotionNumber = (int)motion;
    }

    // 待機状態
    protected virtual void Idel(float deltaTime)
    {
        // オブジェクトの捜索
        SearchObject();
        // 移動
        PointMove(deltaTime);
    }

    #region 発見状態
    // 発見状態
    protected virtual void Discover(float deltaTime)
    {
        //print(m_DState.ToString());

        switch (m_DState)
        {
            case DiscoverState.Discover_Player: DiscoverPlayer(deltaTime); break;
            case DiscoverState.Discover_Animal: DiscoverAnimal(deltaTime); break;
            case DiscoverState.Discover_Food: DiscoverFood(deltaTime); break;
            case DiscoverState.Discover_Trap: DiscoverTrap(deltaTime); break;
            case DiscoverState.Discover_Lost: DiscoverLost(deltaTime); break;
            case DiscoverState.Discover_Lost_Stop: DiscoverLostStop(deltaTime); break;
        }
    }
    // 発見状態の変更
    protected void ChangeDiscoverState(DiscoverState state)
    {
        var motion = AnimatorNumber.ANIMATOR_DISCOVER_NUMBER;
        if (state == DiscoverState.Discover_Food) motion = AnimatorNumber.ANIMATOR_NULL;
        ChangeState(State.Discover, motion);
        // 同じ行動なら返す
        if (m_DState == state) return;
        m_DState = state;
        m_StateTimer = 0.0f;
    }
    protected virtual void DiscoverPlayer(float deltaTime)
    {
        //// 発見アニメーションの場合
        //if (m_MotionNumber == (int)AnimatorNumber.ANIMATOR_DISCOVER_NUMBER)
        //{
        //    var layer = m_Animator.GetLayerIndex("Base Layer");
        //    var state = m_Animator.GetCurrentAnimatorStateInfo(layer);
        //    var nTime = state.normalizedTime % 1.0f;
        //    // アニメーションの再生が完了したら、次のアニメーションに変更
        //    if (nTime < 0.9f)
        //    {
        //        m_Agent.Stop();
        //        return;
        //    }
        //    else
        //    {
        //        // アニメーションの変更
        //        ChangeAnimation(AnimatorNumber.ANIMATOR_CHASE_NUMBER);
        //        m_Agent.Resume();
        //    }
        //}

        // 一定時間経過したら、次のアニメーションを再生
        if (!IsEndTimeAnimation(0.9f)) m_Agent.Stop();
        else
        {
            // アニメーションの変更
            ChangeAnimation(AnimatorNumber.ANIMATOR_CHASE_NUMBER);
            //m_Agent.Resume();
        }

        GameObject obj = null;
        if (!InPlayer(out obj, 2.0f, true))
        {
            // 見失う状態に遷移
            ChangeDiscoverState(DiscoverState.Discover_Lost);
            // アニメーションの変更
            ChangeAnimation(AnimatorNumber.ANIMATOR_LOST_NUMBER);
            m_Agent.Resume();
        };
    }

    // 動物発見状態
    protected virtual void DiscoverAnimal(float deltaTime)
    {
    }

    #region えさ発見状態
    // えさ発見状態
    protected virtual void DiscoverFood(float deltaTime)
    {
        switch (m_DFState)
        {
            case DiscoverFoodState.DiscoverFood_Move: DiscoverFoodMove(deltaTime); break;
            case DiscoverFoodState.DiscoverFood_AnimalMove: DiscoverFoodAnimalMove(deltaTime); break;
            case DiscoverFoodState.DiscoverFood_Eat: DiscoverFoodEat(deltaTime); break;
            case DiscoverFoodState.DiscoverFood_Lift: DiscoverFoodLift(deltaTime); break;
            case DiscoverFoodState.DiscoverFood_TakeAway: DiscoverFoodTakeOut(deltaTime); break;
                //case DiscoverFoodState.DiscoverFood_Runaway: DiscoverFeedTakeOut(deltaTime); break;
        }
    }
    // えさ発見状態の状態変更
    protected void ChangeDiscoverFoodState(DiscoverFoodState state)
    {
        ChangeDiscoverState(DiscoverState.Discover_Food);
        // 同じ行動なら返す
        if (m_DFState == state) return;
        m_DFState = state;
        m_StateTimer = 0.0f;
    }
    // えさ発見移動
    protected virtual void DiscoverFoodMove(float deltaTime)
    {
        // 二次元(x, z)の距離を求める
        var v1 = new Vector2(m_Agent.destination.x, m_Agent.destination.z);
        //var v2 = new Vector2(m_MouthPoint.position.x, m_MouthPoint.position.z);
        var v2 = new Vector2(transform.position.x, transform.position.z);
        var length = Vector2.Distance(v1, v2);

        // 一定距離内なら、えさ食べ状態に遷移
        if (length < 1.2f)
        {
            // えさ食べ状態に遷移
            ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_Eat);
            SoundManger.Instance.PlaySE(11);
            m_Agent.Stop();
            return;
        }

        // えさが無くなっていたら、待機状態に遷移
        if (m_FoodObj == null)
        {
            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            ChangeSpriteColor(Color.red);
        }
    }
    // 動物発見状態
    protected virtual void DiscoverFoodAnimalMove(float deltaTime)
    {
        // 口ポインタとの距離を計算
        //var length = Vector3.Distance(
        //    m_Agent.destination, m_MouthPoint.position
        //    );
        // 二次元(x, z)の距離を求める
        //var v1 = new Vector2(m_Agent.destination.x, m_Agent.destination.z);
        //var v2 = new Vector2(m_MouthPoint.position.x, m_MouthPoint.position.z);
        //var length = Vector2.Distance(v1, v2);

        var v1 = new Vector2(m_Agent.destination.x, m_Agent.destination.z);
        var v2 = new Vector2(transform.position.x, transform.position.z);
        var length = Vector2.Distance(v1, v2);

        // 一定距離内なら、持ち上げ状態に遷移
        if (length < 0.8f)
        {
            // 持ち上げ状態に遷移
            ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_Lift);
            ChangeSpriteColor(Color.yellow);
            return;
        }

        // えさがなかったら、待機状態に遷移
        if (m_SmallTrap == null)
        {
            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            ChangeMovePoint();
            m_Agent.Resume();
        }
    }
    // えさ食べ状態
    protected void DiscoverFoodEat(float deltaTime)
    {
        // えさが無くなっていたら、待機状態に遷移
        if (m_FoodObj == null)
        {
            var mediator = GameObject.Find("TutorialMediator");
            if (mediator != null)
            {
                // チュートリアルなら動かないようにする
                if (TutorialMediator.GetInstance() != null) return;
            }

            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            // アニメーションの変更
            //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_IDEL_NUMBER], 0.1f, -1);
            //ChangeSpriteColor(Color.red);
            m_Agent.Resume();
        }

        if (m_StateTimer <= 3.0f) return;
        // えさを食べた時の処理
        EatFood();
    }

    // えさ持ち上げ状態
    protected virtual void DiscoverFoodLift(float deltaTime)
    {
        // 仮
        if (m_StateTimer < 1.0f) return;

        // 持ち上げ終了したら、持ち帰り状態に遷移
        ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_TakeAway);

        // 動物を持ち上げる
        var animal = m_SmallTrap.GetAnimal();
        var animalParent = animal.transform.parent;
        // 相手側にかかっているトラバサミの削除
        var animalScript = animal.GetComponent<Enemy3D>();
        //animalScript.ChangeTakeIn();
        animalScript.DeleteTrap();
        // 口オブジェクトの子オブジェクトに変更
        var agent = animal.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        m_OtherCreateBox = animalParent.transform.parent;
        animalParent.transform.parent = m_MouthPoint;
        animalParent.transform.localPosition = Vector3.zero;
        animalParent.transform.localRotation = new Quaternion();
        //var sprite = m_Sprite.GetComponent<EnemySprite>();
        //animalParent.transform.localScale =
        //    new Vector3(
        //        animal.transform.localScale.x / sprite.GetSpriteScale().x,
        //        animal.transform.localScale.y / sprite.GetSpriteScale().y,
        //        animal.transform.localScale.z / sprite.GetSpriteScale().z
        //        );
        animalParent.transform.localScale = animal.transform.localScale;
        animal.transform.localPosition = Vector3.zero;
        animal.transform.localRotation = new Quaternion();

        //var animalScript = animal.GetComponent<Enemy3D>();
        // 持ち上げる動物を入れる
        if (animalScript != null) m_TakeInAnimal = animalScript;
        // 生成ボックスの移動ポイント取得
        var nest = GameObject.Find("WolfNest");
        m_SmallTrap.Null();
        m_SmallTrap = null;
        // 初期位置に移動
        ChangeMovePoint(nest.transform.position);
        m_Agent.Resume();
    }
    // えさ持ち帰り状態
    protected virtual void DiscoverFoodTakeOut(float deltaTime)
    {
        var length = Vector3.Distance(
            m_Agent.destination, this.transform.position
            );
        // 移動ポイントに到達したら消える
        if (length > m_Speed) return;
        ChangeState(State.DeadIdel, AnimatorNumber.ANIMATOR_DEAD_NUMBER);

        // 持ち上げた動物の初期化
        //// 動物を持ち上げる
        //var animal = m_Trap.GetAnimal();
        //var animalParent = animal.transform.parent;
        //// 口オブジェクトの子オブジェクトに変更
        //m_OtherCreateBox = animalParent.transform.parent;
        var otherParent = m_TakeInAnimal.gameObject.transform.parent;
        otherParent.gameObject.transform.parent = m_OtherCreateBox;
        //animalParent.transform.parent = m_MouthPoint;
        m_TakeInAnimal.transform.localPosition = Vector3.zero;
        m_TakeInAnimal.transform.localRotation = new Quaternion();
        m_TakeInAnimal.InitState();
        //var sprite = m_Sprite.GetComponent<EnemySprite>();
        //m_TakeInAnimal.transform.localScale =
        //    new Vector3(
        //        animal.transform.localScale.x / sprite.GetSpriteScale().x,
        //        animal.transform.localScale.y / sprite.GetSpriteScale().y,
        //        animal.transform.localScale.z / sprite.GetSpriteScale().z
        //        );
        //animal.transform.localPosition = Vector3.zero;
        //animal.transform.localRotation = new Quaternion();

        m_TakeInAnimal = null;
        // ステータスの初期化
        InitState();
    }
    #endregion

    // トラバサミ発見状態
    protected virtual void DiscoverTrap(float deltaTime)
    {
        GameObject obj = null;
        // トラバサミを見つけたか
        if (!InObject(TRAP_NAME, out obj))
        {
            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            m_Agent.Resume();
            ChangeSpriteColor(Color.red);
        }
    }
    // 見失う状態
    protected void DiscoverLost(float deltaTime)
    {
        // プレイヤーが見えたら、再度追跡
        GameObject player = null;
        if (InPlayer(out player, 2.0f, true))
        {
            // 追跡状態に遷移
            ChangeDiscoverState(DiscoverState.Discover_Player);
            SoundManger.Instance.PlaySE(13);
            // アニメーションの変更
            //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_CHASE_NUMBER], 0.1f, -1);
            ChangeAnimation(AnimatorNumber.ANIMATOR_CHASE_NUMBER);
            //m_Mark.ExclamationMark();
            //ChangeSpriteColor(Color.blue);
            return;
        }

        // 二次元(x, z)の距離を求める
        var v1 = new Vector2(m_Agent.destination.x, m_Agent.destination.z);
        var v2 = new Vector2(m_MouthPoint.position.x, m_MouthPoint.position.z);
        var length = Vector2.Distance(v1, v2);
        // 見失ったポイントに移動してもいなかったら、待機状態に遷移
        if (length < 2.0f)
        {
            ChangeDiscoverState(DiscoverState.Discover_Lost_Stop);
            // アニメーションの変更
            ChangeAnimation(AnimatorNumber.ANIMATOR_LOST_NUMBER);
            m_Agent.Stop();
        }
    }
    // 見失い停止状態
    protected void DiscoverLostStop(float deltaTime)
    {
        // 見失ったポイントに移動してもいなかったら、待機状態に遷移
        if (IsEndTimeAnimation(0.9f))
        {
            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            m_Agent.Resume();
            // 移動速度を変える
            m_Agent.speed = m_Speed;
            m_Player = null;
            //ChangeSpriteColor(Color.red);
        }
    }
    #endregion

    // 攻撃状態
    protected virtual void Attack(float deltaTime)
    {
        if (m_StateTimer < 2.0f) return;
        // 待機状態に遷移
        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        //ChangeSpriteColor(Color.red);
        //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_IDEL_NUMBER], 0.1f, -1);
        m_Agent.Resume();
    }

    // 気絶状態
    protected void Faint(float deltaTime)
    {
        // 一定時間経過まで動かない
        if (m_StateTimer < 3.0f) return;
        // 待機状態に遷移
        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        ChangeMovePoint();
        m_Agent.Resume();
        ChangeSpriteColor(Color.red);
    }

    // 睡眠状態
    protected void Sleep(float deltaTime)
    {
        //// えさ判定で、trueならば、起こす(待機状態に遷移)
        //int value1 = Random.Range(1, 100 + 1);
        //// 個数によって、判定用の値を変える
        //int count = 6;
        //int value2 = Mathf.Min(count, 10) * 6;
        //print((value1 > value2).ToString());

        // 
        //ChangeState(State.Idel, AnimationNumber.ANIME_IDEL_NUMBER);
        //ChangeSpriteColor(Color.red);
        //m_Agent.Resume();
    }

    #region トラバサミヒット状態
    // トラバサミに挟まれている状態
    protected void TrapHit(float deltaTime)
    {
        switch (m_THState)
        {
            case TrapHitState.TrapHit_Change: TrapHitChange(deltaTime); break;
            case TrapHitState.TrapHit_TakeIn: TrapHitTakeIn(deltaTime); break;
            case TrapHitState.TrapHit_Runaway: TrapHitRunaway(deltaTime); break;
        };
    }

    // 飲み込まれ状態
    protected void TrapHitTakeIn(float deltaTime)
    {
    }

    // トラバサミ逃げ状態
    protected void TrapHitRunaway(float deltaTime)
    {
        // トラバサミの方向に移動するように、ポイントの更新をする
        ChangeMovePoint(m_SmallTrap.transform.position);

        // 壁に衝突したら、気絶状態に遷移
        if (m_WChackPoint.IsWallHit())
        {
            // 気絶状態に遷移
            ChangeState(State.Faint, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            // 付属しているトラバサミの削除
            DeleteTrap();
            ChangeSpriteColor(Color.gray);
            m_Agent.Stop();
            return;
        }

        // オブジェクトの位置から地面までのレイを伸ばす
        var pos = this.transform.position;
        Ray ray = new Ray(pos, pos - Vector3.up * 100.0f);
        RaycastHit hitInfo;
        var hit = Physics.Raycast(ray, out hitInfo);
        // 地面に当たらなかった場合は返す
        if (!hit || hitInfo.collider.gameObject.tag != "Ground") return;

        // ステージ外の床に接地していたら、逃げ状態に遷移
        if (hitInfo.transform.name.IndexOf("StageOutPlane") < 0) return;

        // 逃げ状態に遷移
        ChangeState(State.Runaway, AnimatorNumber.ANIMATOR_FAINT_NUMBER);
        ChangeSpriteColor(Color.green);
        // 自身の衝突判定のトリガーをオンにする
        //var collider = gameObject.GetComponent<Collider>();
        //collider.isTrigger = true;
        m_Collider.isTrigger = true;

    }

    // 罠状態
    protected void TrapHitChange(float deltaTime)
    {
        // トラップから解除されたら
        //if (m_TrapObj != null) return;
        if (m_SmallTrap != null) return;

        m_IsTrapHit = false;

        //// 死亡待機状態に遷移
        //ChangeState(State.DeadIdel, AnimationNumber.ANIME_DEAD_NUMBER);
        //// トラバサミが解放されたときの行動
        //TrapReleaseAction();
        //// ステータスの初期化
        //InitState();

        // トラバサミが解放されたときの行動
        TrapReleaseAction();
        // 動物の消去
        DeadAnimal();
    }
    #endregion

    // 肉待機状態
    protected void MeatIdel(float deltaTime)
    {
        // 持っている肉が無かったら、死亡待機状態に遷移
        var meat = this.transform.FindChild("Food(Clone)");
        if (meat != null) return;
        ChangeState(State.DeadIdel, AnimatorNumber.ANIMATOR_DEAD_NUMBER);
        // ステータスの初期化
        InitState();
    }

    // 死亡待機状態
    protected void DeadIdel(float deltaTime)
    {
        // 外部からアクティブ状態に変更されたら、待機状態に遷移
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(true);
        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        // 最初のポイントに移動
        ChangeMovePoint(m_MovePointPosition);
        m_Agent.Resume();
        //if (!m_EnemySprite.gameObject.activeSelf) m_EnemySprite.gameObject.SetActive(true);
        ChangeSpriteColor(Color.red);
    }
    // 逃げ状態
    protected void Runaway(float deltaTime)
    {
        // トラバサミの方向に移動するように、ポイントの更新をする
        ChangeMovePoint(m_SmallTrap.transform.position);
        // 画面に映っている間は返す
        if (IsRendered()) return;
        // 自身の衝突判定のトリガーをオンにする
        //var collider = gameObject.GetComponent<Collider>();
        //collider.isTrigger = false;
        m_Collider.isTrigger = false;
        // 付属しているトラバサミの削除
        DeleteTrap();
        // 動物の消去
        DeadAnimal();
    }
    #endregion

    #region virtual関数
    // 移動ベクトルを代入します
    //protected virtual void MoveVelocity()
    //{
    //    // 移動量の加算
    //    //m_Rigidbody.velocity = m_TotalVelocity;
    //    // 移動量の初期化
    //    m_TotalVelocity = Vector3.zero;
    //}

    // 壁に衝突したときに、折り返します
    protected virtual void TurnWall()
    {
        // 壁に当たった、崖があった場合は折り返す
        //if (m_WChackPoint != null)
        //{
        //    if (m_WChackPoint.IsWallHit())
        //    {
        //    }
        //}

        // 壁に当たらなかった場合は折り返す
        if (m_WChackPoint == null || !m_WChackPoint.IsWallHit()) return;
        // 角度の設定
        SetDegree();
    }

    // 角度の設定
    protected void SetDegree()
    {
        // 移動量の変更
        m_Velocity *= -1;
        //var wall = m_WChackPoint.GetHitWallObj();
        // 角度の取得
        //var rotate = wall.transform.rotation.eulerAngles;
        // オブジェクトの回転
        //transform.Rotate(transform.up, m_RotateDegree);
        //// スプライトの回転
        //m_Sprite.transform.Rotate(Vector3.up, m_RotateDegree);
        // 壁捜索ポイントの方向変更
        m_WChackPoint.ChangeDirection();
    }
    //// トラバサミに当たった時の行動です
    //protected virtual void TrapHitAction()
    //{
    //    // トラップ化状態に遷移
    //    ChangeTrapHitState(
    //        TrapHitState.TrapHit_Change,
    //        AnimationNumber.ANIME_TRAP_NUMBER
    //        );
    //    // 自身の衝突判定をオフにする
    //    var collider = gameObject.GetComponent<Collider>();
    //    //if (collider != null) collider.isTrigger = true;
    //    collider.enabled = false;
    //    ChangeSpriteColor(Color.green);
    //    m_Agent.Stop();
    //    m_Agent.enabled = false;
    //}

    // 小さいトラバサミに衝突した時の行動です
    protected virtual void SmallTrapHitAction()
    {
        // トラップ化状態に遷移
        ChangeTrapHitState(
            TrapHitState.TrapHit_Change,
            AnimatorNumber.ANIMATOR_DEAD_NUMBER
            );
        m_IsTrapHit = true;
        // 自身の衝突判定をオフにする
        //var collider = gameObject.GetComponent<Collider>();
        //if (collider != null) collider.isTrigger = true;
        //collider.enabled = false;
        //m_Collider.enabled = false;
        m_Collider.isTrigger = true;
        m_Collider.enabled = false;
        //ChangeSpriteColor(Color.green);
        // エージェントを停止させる
        m_Agent.Stop();
        m_Agent.enabled = false;
    }

    // 大きいトラバサミに衝突した時の行動です
    protected virtual void BigTrapHitAction() { }

    // トラバサミを解除された時の行動です
    protected virtual void TrapReleaseAction() { }

    // 反応するえさかどうかを判定します
    protected virtual bool IsFoodCheck(Food.Food_Kind food)
    {
        //if (food == Food.Food_Kind.Goat) return true;
        //return false;
        return food == Food.Food_Kind.Carrot;
    }

    // えさの判断を行います
    protected void ChangeFoodMove(Food food)
    {
        // 好きなえさだったら寄り付く　嫌いなえさだったら逃げる
        if (IsLikeFood(food.food_Kind))
        {
            ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_Move);
            ChangeMovePoint(food.transform.position);
            ChangeSpriteColor(Color.magenta);
        }
        else {
            // 逃げる
            //ChangeState(State.Runaway, AnimationNumber.ANIME_RUNAWAY_NUMBER);
            PointRunaway(food.transform);
            //ChangeSpriteColor(Color.gray);
        }
    }

    // えさを食べた時の行動
    protected virtual void EatFood()
    {
        // えさを食べ終わったら、待機状態に遷移
        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        ChangeSpriteColor(Color.red);
        m_Agent.Resume();
        // えさの削除
        Destroy(m_FoodObj);
        m_FoodObj = null;
        // ゲームマネージャ側の減算処理を呼ぶ
        GameManager.gameManager.FoodCountSub();
        //// アニメーションの変更
        //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_IDEL_NUMBER], 0.1f, -1);
        ChangeAnimation(AnimatorNumber.ANIMATOR_IDEL_NUMBER);
    }

    // 好きなえさ
    protected virtual bool IsLikeFood(Food.Food_Kind food)
    {
        if (food == Food.Food_Kind.Carrot) return true;
        return false;
    }

    // 特定の状態で動かないようにします(衝突判定時)
    protected virtual bool IsNotChangeState()
    {
        return ((m_State & (State.TrapHit | State.Meat | State.Faint | State.Attack)) != 0);
    }
    #endregion

    #region 判定用関数
    // プレイヤーが見えているか
    protected bool InPlayer(out GameObject player, float addLength = 1.0f, bool isDiscover = false)
    {
        return InObject("PlayerSprite", out player, addLength, isDiscover, 8);
    }

    // タグの付いたオブジェクトを捜します
    protected bool InTagObject(string tag, out GameObject hitObj)
    {
        hitObj = null;
        //var isPlayer = false;
        var objName = name;
        var obj = GameObject.Find(objName);
        // オブジェクトがいない場合は返す
        if (obj == null) return false;
        // レイポイントからオブジェクトの位置までのレイを伸ばす
        var playerDir = obj.transform.position - m_RayPoint.position;
        Ray ray = new Ray(
            m_RayPoint.position,
            playerDir
            );
        RaycastHit hitInfo;
        var hit = Physics.Raycast(ray, out hitInfo);
        // プレイヤーに当たらなかった場合、
        // プレイヤー以外に当たった場合は返す
        //print("見えているか調査");
        if (!hit || hitInfo.collider.name != objName) return false;
        // 当たったオブジェクト
        hitObj = hitInfo.collider.gameObject;
        // プレイヤーとの距離を求める
        var length = Vector3.Distance(
            m_RayPoint.position,
            obj.transform.position
            );
        // 可視距離から離れていれば返す
        if (length > m_ViewLength) return false;
        var dir = obj.transform.position - m_RayPoint.position;
        var angle = Vector3.Angle(m_RayPoint.forward, dir);
        if (Mathf.Abs(angle) > m_ViewAngle) return false;
        // プレイヤーを見つけた
        return true;
        //return false;
    }

    protected bool InObject(
        string name, out GameObject hitObj,
        float addLength = 1.0f, bool isDiscover = false,
        int layerMask = -1)
    {
        //var isPlayer = false;
        hitObj = null;
        var objName = name;
        var obj = GameObject.Find(objName);
        // オブジェクトがいない場合は返す
        if (obj == null) return false;
        // レイポイントからオブジェクトの位置までのレイを伸ばす
        var playerDir = obj.transform.position - m_RayPoint.position;
        Ray ray = new Ray(
            m_RayPoint.position,
            playerDir
            );
        RaycastHit hitInfo;
        var hit = Physics.Raycast(ray, out hitInfo);
        // 指定レイヤーと衝突する場合
        //if (layerMask != -1)
        //    hit = Physics.Raycast(ray, out hitInfo, layerMask);
        // プレイヤーに当たらなかった場合、
        // プレイヤー以外に当たった場合は返す
        //print("見えているか調査");
        if (!hit || hitInfo.collider.name != objName)
        {
            //print(hitInfo.collider.name);
            return false;
        }
        // 当たったオブジェクト
        hitObj = hitInfo.collider.gameObject;
        // プレイヤーとの距離を求める
        //var length = Vector3.Distance(m_RayPoint.position, obj.transform.position);
        var length = Vector2.Distance(
            new Vector2(m_RayPoint.position.x, m_RayPoint.position.z),
            new Vector2(obj.transform.position.x, obj.transform.position.z)
            );
        // 可視距離から離れていれば返す
        //print("距離調査");
        if (length >= m_ViewLength * addLength) return false;
        // 発見状態でなかったら、角度の計算を行う
        if (!isDiscover)
        {
            // 視野角の外ならば返す
            var dir = obj.transform.position - m_RayPoint.position;
            var angle = Vector3.Angle(m_RayPoint.forward, dir);
            if (Mathf.Abs(angle) > m_ViewAngle) return false;
        }
        //print("角度調査");

        // プレイヤーを見つけた
        //print("見つけた");
        return true;
    }

    // オブジェクト用
    protected bool InObject(GameObject obj = null)
    {
        // オブジェクトがいない場合は返す
        if (obj == null) return false;
        // オブジェクトがアクティブ状態でなければ返す
        if (!obj.activeSelf) return false;
        // レイポイントからオブジェクトの位置までのレイを伸ばす
        var dir = obj.transform.position - m_RayPoint.position;
        Ray ray = new Ray(m_RayPoint.position, dir);
        RaycastHit hitInfo;
        var hit = Physics.Raycast(ray, out hitInfo);
        if (!hit) return false;
        // 距離を求める
        var length = Vector3.Distance(
            m_RayPoint.position,
            obj.transform.position
            );
        // 可視距離から離れていれば返す
        if (length >= m_ViewLength) return false;
        // 視野角の外ならば返す
        //var dir = obj.transform.position - m_RayPoint.position;
        var angle = Vector3.Angle(m_RayPoint.forward, dir);
        if (Mathf.Abs(angle) > m_ViewAngle) return false;
        return true;
    }

    // 前方に壁があるかを調べます
    protected bool InWall(out GameObject hitObj, out Vector3 hitPoint, float length = 2.0f)
    {
        hitObj = null;
        hitPoint = Vector3.zero;
        // レイポイントからオブジェクトの位置までのレイを伸ばす
        var point = m_RayPoint.position + this.transform.forward * length;
        var dir = point - m_RayPoint.position;
        //var playerDir = obj.transform.position - m_RayPoint.position;
        Ray ray = new Ray(m_RayPoint.position, dir);
        RaycastHit hitInfo;
        var hit = Physics.Raycast(ray, out hitInfo);
        //print("見えているか調査");
        if (!hit || hitInfo.collider.tag != "Wall") return false;
        // 指定距離より長かったら返す
        if (hitInfo.distance > length) return false;
        // 当たった壁
        hitObj = hitInfo.collider.gameObject;
        hitPoint = hitInfo.point;
        return true;
    }

    // レイチェックを行います
    //private bool ChackRay(Vector3 position, GameObject obj, out GameObject hitObj)
    //{
    //    // レイポイントからオブジェクトの位置までのレイを伸ばす
    //    var playerDir = obj.transform.position - m_RayPoint.position;
    //    Ray ray = new Ray(
    //        m_RayPoint.position,
    //        playerDir
    //        );
    //    RaycastHit hitInfo;
    //    var hit = Physics.Raycast(ray, out hitInfo);
    //    // プレイヤーに当たらなかった場合、
    //    // プレイヤー以外に当たった場合は返す
    //    //print("見えているか調査");
    //    if (!hit || hitInfo.collider.name != objName) return false;
    //    // 当たったオブジェクト
    //    hitObj = hitInfo.collider.gameObject;
    //    // プレイヤーとの距離を求める
    //    var length = Vector3.Distance(
    //        m_RayPoint.position,
    //        obj.transform.position
    //        );
    //    // 可視距離から離れていれば返す
    //    //print("距離調査");
    //    if (length > m_ViewLength) return false;
    //    // 視野角の外ならば返す
    //    var dir = obj.transform.position - m_RayPoint.position;
    //    var angle = Vector3.Angle(m_RayPoint.forward, dir);
    //    //print("角度調査");
    //    if (Mathf.Abs(angle) > m_ViewAngle) return false;
    //    // プレイヤーを見つけた
    //    //print("見つけた");
    //    return true;
    //}

    // 罠の動物が見えているか
    protected bool IsInTrapAnimal(out GameObject hitObj)
    {
        hitObj = null;
        // 
        //var animal = GameObject.Find("trapAnimal");
        ////// プレイヤーがいない場合は返す
        ////if (m_Player == null) return false;
        //// プレイヤーが空の場合、プレイヤーをセットする
        ////SetPlayer();
        //// プレイヤーがトラップ状態でなかったら、返す
        //// トラップ状態でも、ターゲットがいなければ返す
        ////if (m_Player.GetState() != Player.State.TRAP ||
        ////    m_Player._target == null) return false;

        //Ray ray = new Ray(
        //    m_RayPoint.transform.position,
        //    m_Player.transform.position
        //    );
        //RaycastHit hitInfo;
        //var hit = Physics.Raycast(ray, out hitInfo);
        //print("見えているか調査");
        //// 間にオブジェクトがある場合は返す
        //if (hitInfo.collider != null) return false;
        //// 罠になっている動物を入れる
        //hitObj = null; //m_Player._target;
        //// 罠の動物との距離を求める
        //var length = Vector3.Distance(
        //    m_RayPoint.transform.position,
        //    hitObj.transform.position
        //    );
        //// 可視距離から離れていれば返す
        //print("距離調査");
        //if (length > m_ViewLength) return false;
        //// 視野角の外ならば返す
        //var dir = hitObj.transform.position - m_RayPoint.position;
        //var angle = Vector3.Angle(this.transform.forward, dir);
        //print("角度調査");
        //if (Mathf.Abs(angle) < m_ViewAngle) return false;
        //// プレイヤーを見つけた
        //print("見つけた");
        //return true;
        return false;
    }

    // 接地しているか
    //protected bool IsGround()
    //{
    //    int layerMask = LayerMask.GetMask(new string[] { "Ground" });
    //    Collider2D hit =
    //        Physics2D.OverlapPoint(m_GroundPoint.position, layerMask);
    //    return hit != null;
    //}
    #endregion

    #region その他関数
    // ステータスの初期化を行います
    protected virtual void InitState()
    {
        // 待機状態に変更
        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        ChangeSpriteColor(Color.red);
        // トラバサミを空っぽにする
        //m_TrapObj = null;
        m_SmallTrap = null;
        // ナビメッシュエージェント関連の初期化
        m_Agent.enabled = true;
        m_CurrentMovePoint = 0;
        if(m_MovePoints.Length > 0)
            m_MovePointPosition = m_MovePoints[0].position;

        // 自身の衝突判定をオンにする
        //var collider = gameObject.GetComponent<Collider>();
        //if (collider != null) collider.isTrigger = true;
        //collider.enabled = true;
        m_Collider.enabled = true;
        m_Model.SetActive(true);

        // 初期位置に変更
        this.transform.localPosition = Vector3.zero;
        // 親オブジェクトを非アクティブに変更
        this.transform.parent.gameObject.SetActive(false);
    }

    // 動物を消去処理を行います
    protected void DeadAnimal()
    {
        // 死亡待機状態に遷移
        ChangeState(State.DeadIdel, AnimatorNumber.ANIMATOR_DEAD_NUMBER);
        // ステータスの初期化
        InitState();
    }

    // 移動関数
    protected virtual void ReturnMove(float deltaTime, float subSpeed = 1.0f)
    {
        // 壁に衝突したときに、折り返す
        TurnWall();

        //this.transform.position += m_Velocity.normalized / 10;
        //return;

        //velocity = Vector3.forward;
        // 移動
        ///var cameraR = Vector3.Scale(m_MainCamera.transform.right, Vector3.one);
        //var v = (Vector3.forward - Vector3.right) * m_Velocity.z +
        //    cameraR * m_Velocity.x;

        var v = m_Velocity;
        v.y = 0;
        //var v = (Vector3.forward - Vector3.right) * Input.GetAxis("Vertical") + cameraR * Input.GetAxis("Horizontal");

        m_TotalVelocity =
            (m_Speed * subSpeed) * MULT_SPEED *
            v.normalized * deltaTime;
    }
    // 移動関数
    protected virtual void Move(float deltaTime, float subSpeed = 1.0f)
    {
        if (m_MovePoints.Length > 0) PointMove(deltaTime);
        else ReturnMove(deltaTime, subSpeed);
    }
    // ナビメッシュの移動を行います(指定位置ベクトルに移動)
    private void PointMove(float deltaTime)
    {
        // エージェントの移動
        //m_Agent.destination = m_MovePoints[m_CurrentMovePoint % m_MovePoints.Length].position;
        // ベクトルとの角度によって、角度を変更します
        // 目的地に到着したら、目的地のポイントを変える
        //var length = Vector3.Distance(

        //    );
        var vec1 = new Vector2(m_Agent.destination.x, m_Agent.destination.z);
        var vec2 = new Vector2(this.transform.position.x, this.transform.position.z);
        var length = Vector2.Distance(vec1, vec2);
        if (length < 2.0f) ChangeMovePoint();
    }
    // 移動ポイントの変更を行います
    protected virtual void ChangeMovePoint()
    {
        // 次の移動ポイントに変更
        m_CurrentMovePoint++;
        var pos =
            m_MovePoints[m_CurrentMovePoint % m_MovePoints.Length].position;
        ChangeMovePoint(pos);
    }
    // 移動ポイントの変更を行います(位置ベクトル指定)
    protected void ChangeMovePoint(Vector3 position)
    {
        // 次の移動ポイントに変更
        m_MovePointPosition = position;
        //m_Agent.Stop();
        // エージェントの移動
        if (!m_Agent.enabled) m_Agent.enabled = true;
        if (m_Agent.enabled) m_Agent.destination = m_MovePointPosition;
        //m_Agent.Resume();
        //ChangeSpriteAngle();
    }
    // プレイヤーを見つけた時の処理です
    protected virtual void ChangePlayerHitMove(GameObject player)
    {
        // 発見状態に遷移
        m_DSNumber = DSNumber.DISCOVERED_CHASE_NUMBER;
        //ChangeState(State.Discover, AnimationNumber.ANIME_IDEL_NUMBER);
        ChangeDiscoverState(DiscoverState.Discover_Player);
        SoundManger.Instance.PlaySE(13);
        m_Mark.ExclamationMark();
        // 移動速度を変える
        m_Agent.speed = m_DiscoverSpeed;
        //var player = obj.transform.parent.GetComponent<Player>();
        //if (player != null)
        //    m_Player = player;
        m_Player = player;
        m_DiscoverPlayer = player.transform;
        m_DSNumber = DSNumber.DISCOVERED_RUNAWAY_NUMBER;
        // 移動ポイントの変更
        m_Agent.Resume();
        //SoundNotice(m_DiscoverPlayer);
        //ChangeSpriteColor(Color.blue);
    }
    //スプライトの角度の変更を行います
    //private void ChangeSpriteAngle()
    //{
    //    // スプライトの角度変更
    //    var v = new Vector2(-1.0f, 1.0f).normalized;
    //    var pointV = m_MovePointPosition - this.transform.position;
    //    var dir = new Vector2(pointV.x, pointV.z).normalized;
    //    var angle = Vector2.Angle(v, dir);
    //    // 一定角度なら画像を変える
    //    var cAnagle = 30.0f;
    //    var num = 1;
    //    if (Mathf.Abs(angle) < cAnagle) num = 2;
    //    else if (Mathf.Abs(angle) < 180.0f - cAnagle) num = 0;
    //    //else m_EnemySprite.ChangeSprite(1);
    //    // 右ベクトルとの角度を求める
    //    // クォータービュー上に左右を分断する線を引いて、
    //    // 移動ベクトルとの角度を求めるイメージ
    //    var rightV = new Vector2(1.0f, 1.0f).normalized;
    //    var x = 1.0f;
    //    if (Vector2.Angle(rightV, dir) >= 90.0f) x = -1;
    //    m_EnemySprite.ChangeSprite(num, x);
    //}
    // オブジェクトの確認を行います
    private void CheckObject()
    {
        //// モデルの確認
        CheckModel();
        // 生成ボックスの確認
        CheckCreateBox();
        // メインカメラの確認
        CheckMainCamera();
        // トラバサミの親オブジェクトの確認
        // 付属しているトラバサミの削除
        var traps = GameObject.Find("Traps");
        if (traps != null) m_TrapsObj = traps;
    }
    // 生成ボックスの確認を行います
    private void CheckCreateBox()
    {
        // 生成ボックスの移動ポイント取得
        var box = this.transform.parent.GetComponentInParent<EnemyCreateBox>();
        //if (box != null)
        //{
        //}
        if (box == null) return;

        var count = 0;
        var size = box.GetMovePointsSize();
        // 移動ポイント配列のサイズ変更
        ResizeMovePoints(size);
        for (int i = 0; i != size; i++)
        {
            var point = box.GetMovePoint(i);
            // 移動ポイントが空でなかったら、追加する
            if (point != null)
            {
                m_MovePoints[count] = point;
                count++;
            }
        }
        // 移動ポイントの変更
        if (count != 0) ChangeMovePoint();
    }

    // 指定位置から逃げるようにします
    protected void PointRunaway(Transform point)
    {
        //var pointBox = GameObject.Find("MovePoints");
        // 移動ポイントコンテナがない場合は、
        // 自分の持っているポイントで移動する
        var length = 0.0f;
        var pointPos = point.position;
        var setPos = Vector3.zero;
        // 持っているポイントで、音の位置との最長距離を求める
        for (int i = 0; i != m_MovePoints.Length; i++)
        {
            var pos = m_MovePoints[i].position;
            var pointLength = Vector3.Distance(pointPos, pos);
            //var degree = Vector3.Angle(pointPos, pos);
            // 前回のポイントとの位置より長かったら,
            // 角度が一定角度より大きければ更新する
            //  && Mathf.Abs(degree) > 20.0f
            if (length < pointLength)
            {
                length = pointLength;
                setPos = pos;
            }
        }
        ChangeMovePoint(setPos);

        //if (pointBox == null)
        //{
        //    var length = 0.0f;
        //    var pointPos = point.position;
        //    // 持っているポイントで、音の位置との最長距離を求める
        //    for (int i = 0; i != m_MovePoints.Length; i++)
        //    {
        //        var pos = m_MovePoints[i].position;
        //        var pointLength = Vector3.Distance(pointPos, pos);
        //        var degree = Vector3.Angle(pointPos, pos);
        //        // 前回のポイントとの位置より長かったら,
        //        // 角度が一定角度より大きければ更新する
        //        if (length < pointLength && Mathf.Abs(degree) > 20.0f)
        //        {
        //            length = pointLength;
        //            m_MovePointPosition = pos;
        //        }
        //    }
        //}
        //else
        //{
        //    // 移動ポイントコンテナがある場合は、全ポイントを調べて
        //    // 移動ポイントを決める
        //    //pointBox.child
        //    int count = 0;
        //    foreach(Transform child in pointBox.transform)
        //    {
        //        m_BoxPoints.Add(child);
        //        m_ResultPoints[m_BoxPoints[count]] = count;
        //        count++;
        //    }
        //    // 取得したポイント全部との最長距離を取る
        //    //var length = 0.0f;
        //    for (int i = 0; i != m_BoxPoints.Count; i++)
        //    {
        //        var pos = m_BoxPoints[i].position;
        //        var pointLength = Vector3.Distance(point.position, pos);
        //        //// 前回のポイントとの位置より長かったら、更新する
        //        //if (length < pointLength)
        //        //{
        //        //    length = pointLength;
        //        //    m_MovePointPosition = pos;
        //        //}

        //        // 移動ポイントの評価

        //    }
        //}
    }

    // 指定した時間でアニメーションが終了したかを返します
    protected bool IsEndTimeAnimation(float time = 1.0f)
    {
        var layer = m_Animator.GetLayerIndex("Base Layer");
        var state = m_Animator.GetCurrentAnimatorStateInfo(layer);
        var nTime = state.normalizedTime % 1.0f;
        // 正規化された時間内なら、falseを返す
        if (nTime < time) return false;
        return true;
    }

    // メインカメラの確認を行います
    private void CheckMainCamera()
    {
        // メインカメラが設定されていなかった場合
        if (m_MainCamera == null)
        {
            var camera = GameObject.Find("Main Camera");
            if (camera == null) return;
            var cMove = camera.GetComponent<CameraMove>();
            m_MainCamera = cMove;
            //ビルボード
            Vector3 p = m_MainCamera.transform.localPosition;
            //transform.LookAt(p);
            //m_Sprite.transform.LookAt(p);
        }
    }
    // モデルの確認を行います
    private void CheckModel()
    {
        // スプライトがなかった場合
        if (m_Model == null)
        {
            var obj = this.transform.FindChild("Model");
            if (obj == null) return;
            m_Model = obj.gameObject;
        }
        //// エネミースプライトの取得
        //m_EnemySprite = m_Sprite.GetComponent<EnemySprite>();
        //m_EnemySprite.ChackRender();
    }

    // エージェントの設定を行います
    protected void SetAgentStatus()
    {
        // ステータス
        m_Agent.speed = m_Speed;
        m_Agent.acceleration = m_Speed * 10;
        m_Agent.autoBraking = false;
    }

    // アニメーターの設定を行います
    protected virtual void SetAnimator()
    {
        // 待機アニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_IDEL_NUMBER] = "Wait";
        // 発見アニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_DISCOVER_NUMBER] = "Discover";
        //// 走るアニメーション
        //m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_CHASE_NUMBER] = "Chase";
        // 見失いアニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_LOST_NUMBER] = "Lost";
        // 死亡アニメーション
        m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_DEAD_NUMBER] = "Death";
    }

    // 敵のスプライトカラーの変更
    protected void ChangeSpriteColor(Color color)
    {
        var child = gameObject.transform.FindChild("EnemySprite");
        if (child == null) return;
        var child2 = child.gameObject.transform.FindChild("Sprite");
        var sprite = child2.GetComponent<SpriteRenderer>();
        if (sprite == null) return;
        sprite.color = color;
    }

    // プレイヤーとの向きを返します(単位ベクトル)
    protected Vector3 PlayerDirection()
    {
        var player = GameObject.Find("Player_2");

        return ObjectDirectionOne(player);
        //// プレイヤーがいなければ、ゼロベクトルを返す
        //if (player != null) return Vector3.zero;
        //var direction = Vector3.one;
        //var dir = player.transform.position - this.transform.position;
        //if (dir.x < 0.0f) direction.x = -1.0f;
        //if (dir.y < 0.0f) direction.y = -1.0f;
        //if (dir.z < 0.0f) direction.z = -1.0f;
        //return direction;
    }

    // 対象との向きを取得します
    protected Vector3 ObjectDirection(GameObject obj, Vector3 addPosition = new Vector3())
    {
        var direction = Vector3.zero;
        // オブジェクトが空だったら、ゼロベクトルを返す
        if (obj == null) return direction;
        direction = (obj.transform.position + addPosition) - this.transform.position;
        return direction.normalized;
    }

    // 対象との向きを取得します(単位ベクトル)
    protected Vector3 ObjectDirectionOne(GameObject obj)
    {
        if (obj == null) return Vector3.zero;
        return DirectionOne(obj.transform.position);
        //var direction = Vector3.one;
        //var dir = obj.transform.position - this.transform.position;
        //if (dir.x < 0.0f) direction.x = -1.0f;
        //if (dir.y < 0.0f) direction.y = -1.0f;
        //if (dir.z < 0.0f) direction.z = -1.0f;
        //return direction;
    }

    protected Vector3 DirectionOne(Vector3 position)
    {
        var direction = Vector3.one;
        var dir = position - this.transform.position;
        if (dir.x < 0.0f) direction.x = -1.0f;
        if (dir.y < 0.0f) direction.y = -1.0f;
        if (dir.z < 0.0f) direction.z = -1.0f;
        return direction;
    }

    // 対象との距離を取得します
    //protected float ObjectLength(GameObject obj)
    //{
    //    var length = 0.0f;
    //}

    #region 捜索関連
    // 捜索関数
    protected virtual void SearchObject()
    {
        // トラバサミの捜索
        SearchTrap();
        // 反応する動物の捜索
        SearchAnimal();
        // プレイヤーの捜索
        SearchPlayer();
    }
    // プレイヤーの捜索
    protected void SearchPlayer()
    {
        GameObject obj = null;
        // プレイヤーを見つけた場合
        if (InPlayer(out obj))
        {
            var mediator = GameObject.Find("TutorialMediator");
            // チュートリアルステージの123以外 反応
            // if (mediator == null || !TutorialMediator.GetInstance().IsTutorialAction(1, 2, 3))
            if (mediator == null || TutorialMediator.GetInstance().IsTutorialAction(5))
            {
                // プレイヤーを見つけた時の処理
                ChangePlayerHitMove(obj);
                return;
            }
            else
            {
                // チュートリアルステージの4 初期化
                if (TutorialMediator.GetInstance().IsTutorialAction(4))
                {
                    // チュートリアルシーンだった場合
                    // チュートリアルシーンの初期化処理
                    TutorialMediator.GetInstance().TutorialInit();
                }
            }
        };
    }

    // トラバサミの捜索
    protected void SearchTrap()
    {
        //var traps = GameObject.Find("Traps");
        //foreach (Transform child in traps.transform)
        //{
        //    // ターゲットの取得

        //    // えさとかかっている動物の名前が同一なら、状態遷移
        //}

        var traps = GameObject.Find(TRAP_NAME);

        if (traps == null) return;
        GameObject obj = null;
        // トラバサミを見つけたか
        if (InObject(TRAP_NAME, out obj))
        {
            var trap = traps.GetComponent<Trap_Small>();
            if (trap == null) return;
            // もし、反応するえさがある場合は、えさ発見移動状態に遷移
            m_SmallTrap = trap;
            //var name = "RabbitEnemy";
            var animal = trap.GetAnimal();
            if (animal != null && animal.name == m_AnimalFeedName)
            {
                //ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_Move);
                ChangeDiscoverFoodState(DiscoverFoodState.DiscoverFood_AnimalMove);
                //var enemy = m_DiscoverObj.GetComponent<Enemy3D>();
                //if (enemy == null) return;
                //m_Animal = enemy;
                // 移動ポイントを動物に変更
                ChangeMovePoint(animal.transform.position);
                //ChangeSpriteColor(Color.magenta);
                return;
            }

            // トラバサミ発見状態に遷移
            ChangeDiscoverState(DiscoverState.Discover_Trap);
            var box = this.transform.parent.GetComponentInParent<EnemyCreateBox>();
            //// 移動ポイントを一つ前に変更
            //ChangeMovePoint(box.GetMovePoint(Mathf.Max(0, (m_CurrentMovePoint - 1)) % m_MovePoints.Length).position);
            // 次のポイントに変更
            ChangeMovePoint();
            //ChangeSpriteColor(Color.cyan);
            return;
        }
    }

    // 反応する動物を捜索します
    protected virtual void SearchAnimal()
    {
        SearchAnimal("LargeEnemy");
    }

    protected void SearchAnimal(string animalTag)
    {
        // 反応する動物のタグを取得する
        var animals = GameObject.FindGameObjectsWithTag(animalTag);
        // 該当するタグを持つ動物がいなかったら返す
        if (animals.Length == 0) return;
        foreach (GameObject animal in animals)
        {
            // 同種類の動物だったら、次の動物に移動
            if (animal.name == this.name) continue;
            // 動物が見えていたら、動物発見状態に遷移
            if (InObject(animal))
            {
                AnimalHit(animal);
                break;
            }
        }
    }

    // 動物を見つけた時の処理です
    protected virtual void AnimalHit(GameObject animal)
    {
        ChangeDiscoverState(DiscoverState.Discover_Animal);
        // アニメーションの変更
        //m_Animator.CrossFade(m_AnimatorStates[(int)AnimatorNumber.ANIMATOR_CHASE_NUMBER], 0.1f, -1);
        ChangeAnimation(AnimatorNumber.ANIMATOR_CHASE_NUMBER);
        m_TargetAnimal = animal;
    }
    #endregion

    // プレイヤーとの衝突判定処理
    protected void OnCollidePlayer(Collider collision)
    {
        var tag = collision.gameObject.tag;
        if (tag == PLAYER_TAG)
        {
            ////// 当たったプレイヤーを子供に追加
            ////collision.gameObject.transform.parent = gameObject.transform;
            //var player = collision.GetComponent<Player>();
            //if (player == null) return;
            //// プレイヤーがはさんだ状態なら、トラップヒット状態に遷移
            //if (player.GetState() == Player.State.ENDURE)
            //{
            //    // 暴れ状態に遷移
            //    ChangeTrapHitState(
            //        TrapHitState.TrapHit_Rage,
            //        AnimationNumber.ANIME_TRAP_HIT_NUMBER
            //        );
            //    // 暴れる時間を入れる
            //    player.SetFall(m_RageTime);
            //    m_Player = player;
            //    // カメラの追尾を解除
            //    //var camera = m_MainCamera.GetComponent<CameraMove>();
            //    //camera.MoveChenge(false);
            //    m_MainCamera.MoveChenge(false);
            //    // 敵を揺らす
            //    var speed = 0.5f;
            //    // .Add("easetype", "easeInOutBack")
            //    var moveHash = iTween.Hash(
            //        "x", speed, "y", speed / 10, "delay", 0.5f, "time", 0.5f
            //        );
            //    moveHash.Add("easetype", iTween.EaseType.easeInCubic);
            //    //moveHash.Add("loopType", "ioop");
            //    // iTween.Hash("x", speed, "y", speed, "time", m_RageTime)
            //    iTween.ShakePosition(
            //        gameObject,
            //        moveHash
            //        );
            //    ChangeSpriteColor(Color.yellow);
            //}
        }
    }

    // プレイヤーの設定します
    protected void SetPlayer()
    {
        //if (m_Player != null) return;
        //var obj = GameObject.Find("Player");
        //if (obj == null) return;
        //var player = obj.GetComponent<Player>();
        //if (player == null) return;
        //m_Player = player;
    }
    // 移動ポイント配列のサイズ変更
    protected void ResizeMovePoints(int size)
    {
        for (int i = 0; i != m_MovePoints.Length; i++)
        {
            m_MovePoints[i] = null;
        }
        // サイズの変更
        m_MovePoints = new Transform[size];
    }

    // ポイント配列の変更を行います
    protected void ChangeMovePoints(int size)
    {

    }

    // お肉UIを生成します
    protected void CreateMeat(AnimalMeat.MeatNumber number)
    {
        // 肉UIの生成
        // GameObject _foodObj = Instantiate(_food);
        var m = Instantiate(m_MeatUI);
        var meat = m.GetComponent<AnimalMeat>();
        //meat.SetMeat(AnimalMeat.MeatNumber.SMALL_NUMBER);
        meat.SetMeat(number);
        SoundManger.Instance.PlaySE(12);
        // カメラ
        var camera = GameObject.Find("Main Camera");
        if (camera == null) return;
        var mainCamera = camera.GetComponent<Camera>();
        // スプライトの位置に生成
        meat.SetObjPosition(this.transform.position, mainCamera);
        meat.transform.localScale = Vector3.one;
    }

    // 付属しているトラバサミの削除
    protected void DeleteTrap()
    {
        // 付属しているトラバサミの削除
        // 付属しているトラバサミの削除
        // トラバサミの取得
        foreach (Transform child in m_TrapsObj.transform)
        {
            var trap = child.GetComponent<Trap_Small>();
            // 自身に付属しているトラバサミなら削除する
            if (trap.GetAnimal() == gameObject)
            {
                trap.Null();
                Destroy(child.gameObject);
            }
        }
        m_SmallTrap = null;
        //m_TrapObj = null;
    }
    #endregion

    #region public関数
    // プレイヤーの方向を向きます(単位ベクトル)
    public void ChasePlayer()
    {
        var player = GameObject.Find("Player");
        // プレイヤーがいなければ、返す
        //if (player != null) return;
        if (player != null)
        {
            ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
            return;
        }
        // 移動ポイントの変更
        ChangeMovePoint(player.transform.position);
        m_Agent.Resume();
        //var direction = Vector2.right;
        //var dir = player.transform.position - this.transform.position;
        //var length = 2.0f;
        //// 近づきすぎたら、移動しない
        //if (Mathf.Abs(dir.x) < length) direction.x = 0.0f;
        //// 方向転換
        //if (dir.x < 0.0f) direction.x = -1.0f;
        //// 移動量に代入
        //m_TotalVelocity = m_Speed * direction * Time.deltaTime;
    }

    // 敵がはさまれた時の、暴れる時間を返します(秒数)
    //public float RageTime() { return m_RageTime; }

    // 敵を待機状態にさせます
    public void ChangeWait()
    {
        ChangeState(State.Idel, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        ChangeSpriteColor(Color.red);
    }

    // 敵を飲み込まれ状態にさせます
    //public void ChangeTakeIn()
    //{
    //    // 暴れ状態に遷移
    //    ChangeTrapHitState(
    //        TrapHitState.TrapHit_TakeIn,
    //        AnimatorNumber.ANIMATOR_TRAP_HIT_NUMBER
    //        );
    //}

    // 敵をトラップ化させます
    public void ChangeTrap(GameObject obj = null)
    {
        // 相手が空なら返す
        if (obj == null) return;
        // 挟まったトラバサミを入れる
        var smallTrap = obj.GetComponent<Trap_Small>();
        // 小さいトラバサミに衝突した時の処理
        if (smallTrap != null)
        {
            // すでにはさんでいる場合は、返す
            if (smallTrap._state == Trap_Small.TrapState.CAPTURE_TRAP) return;
            m_SmallTrap = smallTrap;
            // トラバサミに挟まった時のアクション
            SmallTrapHitAction();
            return;
        }

        var bigTrap = obj.GetComponent<BigTrap>();
        // 大きいトラバサミに当たった場合
        if (bigTrap != null)
        {
            // すでにはさんでいる場合は、返す
            if (bigTrap._state == BigTrap.TrapState.CAPTURE) return;
            // トラバサミに挟まった時のアクション
            BigTrapHitAction();
            m_IsTrapHit = true;
            return;
        }
    }

    // 動物をお肉に変更します
    public void ChangeMeat()
    {
        if (m_State == State.Meat) return;
        ChangeState(State.Meat, AnimatorNumber.ANIMATOR_IDEL_NUMBER);
        // お肉の作成
        var meat = GameObject.Instantiate(m_Meat);
        meat.GetComponent<Food>().SelectFood(1);
        meat.transform.parent = this.transform;
        meat.transform.localPosition = Vector3.zero;
        // 自身の衝突判定をオフにする
        m_Collider.enabled = false;
        // エージェントの停止
        m_Agent.Stop();
        m_Agent.enabled = false;
        // 画像を表示しない
        m_Model.SetActive(false);

        // モデルの表示をオフにする

    }

    // 動物の状態を取得します
    public State GetState() { return m_State; }

    // トラバサミに挟まったかを返します
    public bool IsTrapHit() { return m_IsTrapHit; }

    // えさ食べ状態かを返します
    public bool IsEatFood() { return m_DFState == DiscoverFoodState.DiscoverFood_Eat; }

    // カメラに映っているかを返します
    public bool IsRendered() { return m_IsRendered; }

    // 音に気付きます
    public virtual void SoundNotice(Transform point)
    {
        SoundMove(point);
    }

    // 音の位置に近づきます
    protected void SoundMove(Transform point)
    {
        //　音の位置をポイントに変更します
        ChangeMovePoint(point.position);
        m_Agent.Resume();
    }
    // 視野のステータスを取得します
    public Vector2 GetRayStatus()
    {
        return new Vector2(m_ViewLength, m_ViewAngle);
    }
    #endregion

    #region Unity関数
    #region 衝突判定関数
    // トリガー用
    public void OnTriggerEnter(Collider other)
    {
        // 特定の状態なら返す
        if (IsNotChangeState()) return;
        // トリガーとの衝突判定
        TriggerEnterObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        // 特定の状態なら返す
        if (IsNotChangeState()) return;
        // トリガーとの衝突判定
        TriggerStayObject(other);
    }

    // トリガーとの衝突判定(衝突時)
    protected virtual void TriggerEnterObject(Collider other)
    {
        TriggerStayObject(other);
    }

    // トリガーとの衝突判定(衝突中)
    protected virtual void TriggerStayObject(Collider other)
    {
        var objName = other.name;
        // えさの衝突判定に当たった場合の処理
        if (objName == "FoodCollide")
        {
            // すでにえさを発見している場合は、返す
            if (m_State == State.Discover && m_DState == DiscoverState.Discover_Food) return;
            // 親である餌の取得
            var obj = other.transform.parent;
            var food = obj.GetComponent<Food>();
            // 反応するえさでなければ返す
            if (!IsFoodCheck(food.CheckFoodKind())) return;
            // えさ発見移動状態に遷移
            ChangeFoodMove(food);
            m_FoodObj = obj.gameObject;
            return;
        }
    }

    #endregion

    #region ギズモ関数
    // ギズモの描画
    //public void OnDrawGizmos()
    //{
    //    DrawGizmos();

    //    //DrawObjLine("PlayerSprite");
    //    ////// 視野角の右端の描画
    //    ////Gizmos.DrawRay(m_RayPoint.position, Quaternion.Euler(0, m_ViewAngle, 0) * forward);
    //    ////// 視野角の左端の描画
    //    ////Gizmos.DrawRay(m_RayPoint.position, Quaternion.Euler(0, -m_ViewAngle, 0) * forward);
    //    //var color = Color.green;
    //    //color.a = 0.2f;
    //    //Gizmos.color = color;
    //    ////DrawObjLine(m_MovePoints[m_CurrentMovePoint].name);
    //    //var pos = m_SmallTrap.transform.position;
    //    //Gizmos.DrawSphere(pos, 1.0f);

    //    //// 移動の描画
    //    //for(int i = 0; i != m_MovePoints.Length; i++)
    //    //{
    //    //    Gizmos.DrawLine(
    //    //        m_MovePoints[i].transform.position, 
    //    //        m_MovePoints[(i + 1) % m_MovePoints.Length].transform.position
    //    //        );
    //    //}
    //}

    protected virtual void DrawGizmos()
    {
        // レイの描画
        DrawObjLine("PlayerSprite");
    }

    // 対象との線分を描画します
    protected void DrawObjLine(string name)
    {
        var obj = GameObject.Find(name);
        if (obj == null) return;
        var length = Vector3.Distance(obj.transform.position, m_RayPoint.position);
        if (length > m_ViewLength) Gizmos.color = Color.blue;
        else Gizmos.color = Color.red;
        // レイの描画
        Gizmos.DrawLine(m_RayPoint.position, obj.transform.position);
    }
    #endregion

    #region カメラ判定関数
    // カメラの描画範囲に入っている間に、実行される
    public void OnWillRenderObject()
    {
        if (m_MainCamera.tag != "MainCamera") return;
        // 見えている
        m_IsRendered = true;
    }
    #endregion
    #endregion
    #endregion

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(Enemy3D), true)]
    [CanEditMultipleObjects]
    public class Enemy3DEditor : Editor
    {
        SerializedProperty Speed;
        SerializedProperty DiscoverSpeed;
        SerializedProperty TrapHitSpeed;
        SerializedProperty RageTime;
        SerializedProperty ReRageTime;
        SerializedProperty ViewLength;
        SerializedProperty ViewAngle;
        //SerializedProperty GroundPoint;
        SerializedProperty RayPoint;
        SerializedProperty MouthPoint;
        SerializedProperty MovePoints;
        //SerializedProperty Sprite;
        SerializedProperty Model;
        SerializedProperty MainCamera;
        SerializedProperty WChackPoint;
        SerializedProperty RotateDegree;
        SerializedProperty CanvasObj;
        SerializedProperty Meat;
        SerializedProperty MeatUI;
        SerializedProperty DiscoverUI;
        SerializedProperty AnimalAnimator;

        protected List<SerializedProperty> m_Serializes = new List<SerializedProperty>();
        protected List<string> m_SerializeNames = new List<string>();

        public void OnEnable()
        {
            //for(var i = 0; i != m_SerializeNames.Count; i++)
            //{
            //    SetSerialize(m_Serializes[i], m_SerializeNames[i]);
            //}

            Speed = serializedObject.FindProperty("m_Speed");
            DiscoverSpeed = serializedObject.FindProperty("m_DiscoverSpeed");
            TrapHitSpeed = serializedObject.FindProperty("m_TrapHitSpeed");
            RageTime = serializedObject.FindProperty("m_RageTime");
            ReRageTime = serializedObject.FindProperty("m_ReRageTime");
            ViewLength = serializedObject.FindProperty("m_ViewLength");
            ViewAngle = serializedObject.FindProperty("m_ViewAngle");
            //GroundPoint = serializedObject.FindProperty("m_GroundPoint");
            RayPoint = serializedObject.FindProperty("m_RayPoint");
            MovePoints = serializedObject.FindProperty("m_MovePoints");
            MouthPoint = serializedObject.FindProperty("m_MouthPoint");
            //Sprite = serializedObject.FindProperty("m_Sprite");
            Model = serializedObject.FindProperty("m_Model");
            MainCamera = serializedObject.FindProperty("m_MainCamera");
            WChackPoint = serializedObject.FindProperty("m_WChackPoint");
            RotateDegree = serializedObject.FindProperty("m_RotateDegree");
            CanvasObj = serializedObject.FindProperty("m_Canvas");
            Meat = serializedObject.FindProperty("m_Meat");
            MeatUI = serializedObject.FindProperty("m_MeatUI");
            DiscoverUI = serializedObject.FindProperty("m_DiscoverUI");
            AnimalAnimator = serializedObject.FindProperty("m_Animator");

            OnChildEnable();
        }

        private void AddSerialize()
        {
            m_Serializes.Add(Speed);
        }

        public void SetSerialize(SerializedProperty serialize, string name)
        {
            serialize = serializedObject.FindProperty(name);
        }

        protected virtual void OnChildEnable() { }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            // 必ず書く
            serializedObject.Update();

            Enemy3D enemy = target as Enemy3D;

            EditorGUILayout.LabelField("〇動物共通のステータス");
            // int
            RotateDegree.intValue = EditorGUILayout.IntField("折り返し時の角度(度数法)", enemy.m_RotateDegree);

            // float
            Speed.floatValue = EditorGUILayout.FloatField("移動速度(m/s)", enemy.m_Speed);
            DiscoverSpeed.floatValue = EditorGUILayout.FloatField("発見時の移動速度(m/s)", enemy.m_DiscoverSpeed);
            TrapHitSpeed.floatValue = EditorGUILayout.FloatField("はさまれた時の速度(m/s)", enemy.m_TrapHitSpeed);
            RageTime.floatValue = EditorGUILayout.FloatField("暴れる時間(秒)", enemy.m_RageTime);
            ReRageTime.floatValue = EditorGUILayout.FloatField("再度暴れる時間(秒)", enemy.m_ReRageTime);
            ViewLength.floatValue = EditorGUILayout.FloatField("視野距離(m)", enemy.m_ViewLength);
            ViewAngle.floatValue = EditorGUILayout.FloatField("視野角度(度数法)", enemy.m_ViewAngle);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("〇各種オブジェクトの位置ベクトル");
            // Transform
            //GroundPoint.objectReferenceValue = EditorGUILayout.ObjectField("接地ポイント", enemy.m_GroundPoint, typeof(Transform), true);
            RayPoint.objectReferenceValue = EditorGUILayout.ObjectField("レイポイント", enemy.m_RayPoint, typeof(Transform), true);
            MouthPoint.objectReferenceValue = EditorGUILayout.ObjectField("口ポイント", enemy.m_MouthPoint, typeof(Transform), true);
            // 配列
            EditorGUILayout.PropertyField(MovePoints, new GUIContent("徘徊ポイント"), true);
            // EditorGUILayout.PropertyField( prop , new GUIContent( “array1” ), true );

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("〇各種オブジェクトのオブジェクト設定");
            Model.objectReferenceValue = EditorGUILayout.ObjectField("モデル", enemy.m_Model, typeof(GameObject), true);
            //Sprite.objectReferenceValue = EditorGUILayout.ObjectField("敵の画像", enemy.m_Sprite, typeof(GameObject), true);
            MainCamera.objectReferenceValue = EditorGUILayout.ObjectField("メインカメラ", enemy.m_MainCamera, typeof(CameraMove), true);
            WChackPoint.objectReferenceValue = EditorGUILayout.ObjectField("壁捜索ポイント", enemy.m_WChackPoint, typeof(WallChackPoint), true);
            // GameObject
            Meat.objectReferenceValue = EditorGUILayout.ObjectField("お肉オブジェクト", enemy.m_Meat, typeof(GameObject), true);
            MeatUI.objectReferenceValue = EditorGUILayout.ObjectField("お肉UIオブジェクト", enemy.m_MeatUI, typeof(GameObject), true);
            DiscoverUI.objectReferenceValue = EditorGUILayout.ObjectField("発見UIオブジェクト", enemy.m_DiscoverUI, typeof(GameObject), true);
            CanvasObj.objectReferenceValue = EditorGUILayout.ObjectField("キャンパスオブジェクト", enemy.m_Canvas, typeof(GameObject), true);
            // m_Animator
            AnimalAnimator.objectReferenceValue = EditorGUILayout.ObjectField("アニメーター", enemy.m_Animator, typeof(Animator), true);

            EditorGUILayout.Space();

            OnChildInspectorGUI();

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnChildInspectorGUI() { }
    }

    [MenuItem("Tools/SelectGameObject")]
    static void SelectGameObject()
    {

        //var obj = FindObjectOfType<GameObject>();
        //if (obj == null)
        //{
        //    Debug.Log("何も選択されていない");
        //    return;
        //}

        //Selection.activeGameObject = obj.gameObject;
        //Debug.Log(Selection.activeGameObject.name);
        var boxes = new List<GameObject>();

        //Debug.Log(Selection.gameObjects.Length);

        foreach (GameObject obj in Selection.gameObjects)
        {
            //boxes
            if (obj.name == "CreateBox")
            {
                boxes.Add(obj);
                Debug.Log(obj.name);
            }
        }
    }

#endif
    #endregion
}
