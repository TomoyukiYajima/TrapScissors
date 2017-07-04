/*******************************************
制　作　者：影山清晃
最終編集者：影山清晃

内　　　容：プレイヤーのアクション
最終更新日：2017/05/15
*******************************************/
using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    #region 罠を設置、回収に必要な変数
    [SerializeField, TooltipAttribute("設置する罠")]
    private GameObject _trapObje;
    [TooltipAttribute("回収する罠")]
    private GameObject _recovery;
    [TooltipAttribute("罠を回収できるかどうか")]
    private bool _onTrapFlag;
    public int _trapMax;
    private int _trapCount;
    #endregion
    #region 餌設置に必要な変数
    [SerializeField]
    private GameObject _food;
    private int _foodNumber;
    [SerializeField]
    private GameObject _foodUI;
    private FoodUIMove _foodUIMove;
    [TooltipAttribute("餌を回収できるかどうか")]
    private bool _onFoodFlag;
    private int _foodMax;
    private GameObject _foodRecovery;
    #endregion
    #region 指笛に必要な変数
    [SerializeField]
    private GameObject _whistle;
    #endregion
    #region その他
    [SerializeField]
    private GameObject _bear;
    #endregion


    Animator m_Animator;
    NavMeshPlayer m_NavMeshPlayer;
    private float setCount;
    private float foodCount;
    public float setTime = 1f;
    private bool setTrap = false;
    // アクション制限
    //private bool isTutorialAction = false;

    [SerializeField]
    private GameObject _bigTrap;
    private BigTrap _trap;
    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_NavMeshPlayer = transform.parent.GetComponent<NavMeshPlayer>();

        _onTrapFlag = false;
        _trapMax = GameManager.gameManager.TrapNumber();
        _foodMax = GameManager.gameManager.FoodNumber();
        _foodUIMove = _foodUI.GetComponent<FoodUIMove>();
        //今選んでいる餌を調べる
        _foodNumber = _foodUIMove.SelectFoodNumber();
        _trap = _bigTrap.GetComponent<BigTrap>();
    }

    // Update is called once per frame
    void Update()
    {
        //大型トラップが何かを捕まえた、もしくはゲームが終了していたらアクションをできないようにする
        if (_trap.FlgTarget() == true || GameManager.gameManager.GameStateCheck() == GameManager.GameState.END) return;

        //print("set" + setCount);
        //print("food" + foodCount);

        if (m_NavMeshPlayer._AState == NavMeshPlayer.AnimationState.Set)
        {
            //m_NavMeshPlayer.move
            setCount += 1 * Time.deltaTime;
        }
        if (m_NavMeshPlayer._AState == NavMeshPlayer.AnimationState.Food)
        {
            //m_NavMeshPlayer.move
            foodCount += 1 * Time.deltaTime;
        }

        if (GameManager.gameManager.GameStateCheck() == GameManager.GameState.PLAY && setCount == 0.0f && foodCount == 0.0f)
        {
            Action();
        }

        if (setCount >= 1.3f)
        {
            m_NavMeshPlayer._AState = NavMeshPlayer.AnimationState.Idle;
            setCount = 0;
        }
        if (foodCount >= 1.3f)
        {
            m_NavMeshPlayer._AState = NavMeshPlayer.AnimationState.Idle;
            foodCount = 0;
        }
    }

    //プレイヤーのボタン操作
    void Action()
    {

        //トラップの設置、回収
        if (Input.GetButtonDown("Trap") && m_NavMeshPlayer._AState != NavMeshPlayer.AnimationState.Set && m_NavMeshPlayer._AState != NavMeshPlayer.AnimationState.Food)
        {
            // チュートリアルシーンでの特定のフィールド内で無ければ、アクションを行わない
            if (TutorialMediator.GetInstance().IsTutorialAction(1, 2, 3, 4)) return;
            if (setTrap != true)
            {
                setTrap = true;
            }
            if (m_NavMeshPlayer._AState != NavMeshPlayer.AnimationState.Set && _trapCount < _trapMax)
            {
                StartCoroutine(TrapIns(setTime));
                SoundManger.Instance.PlaySE(9);
                m_NavMeshPlayer._AState = NavMeshPlayer.AnimationState.Set;
                m_Animator.CrossFade("Set", 0.1f, -1);
            }
        }

        //餌をまく
        if (Input.GetButtonDown("Food") && m_NavMeshPlayer._AState != NavMeshPlayer.AnimationState.Set && m_NavMeshPlayer._AState != NavMeshPlayer.AnimationState.Food)
        {
            // チュートリアルシーンでの特定のフィールド内で無ければ、アクションを行わない
            if (TutorialMediator.GetInstance().IsTutorialAction(1, 2, 4)) return;
            //今選んでいる餌を調べる
            _foodNumber = _foodUIMove.SelectFoodNumber();
            FoodCheck();
            if (_foodUIMove.FoodCountCheck(_foodNumber) > 0)
            {
                m_Animator.CrossFade("Set", 0.1f, -1);
                m_NavMeshPlayer._AState = NavMeshPlayer.AnimationState.Food;
            }
        }

        //音を鳴らす
        if (Input.GetButtonDown("Whistle"))
        {
            // チュートリアルシーンでの特定のフィールド内で無ければ、アクションを行わない
            if (TutorialMediator.GetInstance().IsTutorialAction(1, 2, 3)) return;
            SoundManger.Instance.PlaySE(18);
            StartCoroutine(WhistleActive());
        }

    }

    /// <summary>
    /// 設置する餌を切り替える
    /// </summary>
    /// <param name="num">右側に切りかえるなら正の数、左側なら負の数</param>
    public void FoodCheck()
    {
        if (_onFoodFlag == false)
        {
            //選んでいる餌が所持数0以下なら、何もしない
            if (_foodUIMove.FoodCountCheck(_foodNumber) <= 0) return;

            StartCoroutine(FoodIns(setTime));

        }
        else if (_onFoodFlag == true)
        {
            if (_foodUIMove.FoodCountCheck(_foodNumber) >= 5) return;
            StartCoroutine(FoodDestroy(setTime));
        }
    }

    /// <summary>
    /// 指笛の当たり判定のOn,Off
    /// </summary>
    /// <returns></returns>
    public IEnumerator WhistleActive()
    {
        _whistle.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _whistle.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        //罠と衝突したら回収できるようにする
        TrapFlagChenge(col, true);
        //餌と衝突したら回収できるようにする
        FoodFlagChenge(col, true);
    }

    void OnTriggerExit(Collider col)
    {
        //餌から離れたら、Trapを置けるようにする
        FoodFlagChenge(col, false);

        if (col.tag != "Trap") return;
        //罠から離れたら、Trapを置けるようにする
        TrapFlagChenge(col, false);
    }

    /// <summary>
    /// 罠が置ける状態か、回収する状態か切り替える
    /// </summary>
    /// <param name="col">衝突している相手</param>
    /// <param name="flag">回収できるか否か</param>
    public void TrapFlagChenge(Collider col, bool flag)
    {
        if (col.tag == "Trap")
        {
            _onTrapFlag = flag;
            if (_onTrapFlag == true) _recovery = col.gameObject;
            else if (_onTrapFlag == false) _recovery = null;
        }
    }

    public void FoodFlagChenge(Collider col, bool flag)
    {
        if (col.tag == "Food")
        {
            _onFoodFlag = flag;
            if (_onFoodFlag == true) _foodRecovery = col.gameObject;
            else if (_onFoodFlag == false) _recovery = null;
        }
    }

    IEnumerator TrapIns(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        GameObject traps = GameObject.Find("Traps");

        if (_onTrapFlag == false && _trapCount < _trapMax)
        {
            Vector3 pos = new Vector3(this.transform.position.x,
           this.transform.position.y,
           this.transform.position.z);

            Instantiate(
                    _trapObje, pos,
                    traps.transform.rotation, traps.transform
                    );

            _trapCount++;
        }
        else if (_onTrapFlag == true)
        {
            Destroy(_recovery);
            _onTrapFlag = false;

            if (m_NavMeshPlayer._AState != NavMeshPlayer.AnimationState.Set)
            {
                m_NavMeshPlayer._AState = NavMeshPlayer.AnimationState.Set;
                m_Animator.CrossFade("Set", 0.1f, -1);
            }

            _trapCount--;
        }

    }


    IEnumerator FoodIns(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        _foodUIMove.FoodCountSub(_foodNumber);

        Vector3 pos = new Vector3(this.transform.position.x,
                                 this.transform.position.y,
                                 this.transform.position.z);
        //餌を生成
        GameObject _foodObj = Instantiate(_food);
        _foodObj.transform.localPosition = pos;
        _foodObj.GetComponent<Food>().SelectFood(_foodNumber);
        SoundManger.Instance.PlaySE(8);
        //撒かれた餌のカウント
        GameManager.gameManager.FoodCountAdd();
        _bear.GetComponent<BearEnemy>().CheckFood();
        GameManager.gameManager.PutFoodAdd(_foodObj);
    }
    IEnumerator FoodDestroy(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(_foodRecovery);
        _onFoodFlag = false;
        _foodUIMove.FoodCountAdd(_foodNumber);
        GameManager.gameManager.FoodCountSub();
    }
}
