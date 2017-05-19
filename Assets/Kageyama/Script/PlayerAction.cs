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
    [TooltipAttribute("回収できるかどうか")]
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
    #endregion
    #region 指笛に必要な変数
    [SerializeField]
    private GameObject _whistle;
    #endregion

    [SerializeField]
    private GameObject _bigTrap;
    private BigTrap _trap;
    // Use this for initialization
    void Start ()
    {
        _onTrapFlag = false;
        _foodNumber = 0;
        _trapMax = GameManager.gameManager.TrapNumber();
        _foodUIMove = _foodUI.GetComponent<FoodUIMove>();
        //今選んでいる餌を調べる
        _foodNumber = _foodUIMove.SelectFoodNumber();
        _trap = _bigTrap.GetComponent<BigTrap>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_trap.FlgTarget() == true) return;
        Action();
	}

    //プレイヤーのボタン操作
    void Action()
    {
        //トラップの設置、回収
        if (Input.GetButtonDown("Trap"))
        {
            if (_onTrapFlag == false && _trapCount < _trapMax)
            {
                // 生成カウントに加算
                GameObject traps = GameObject.Find("Traps");
                if (traps != null)
                {
                    Vector3 pos = new Vector3(this.transform.position.x,
                                                       this.transform.position.y - 1.5f,
                                                       this.transform.position.z);
                    Instantiate(
                        _trapObje, pos,
                        traps.transform.rotation, traps.transform
                        );
                    _trapCount++;
                }
            }
            else if (_onTrapFlag == true)
            {
                Destroy(_recovery);
                _onTrapFlag = false;
                _trapCount--;
            }
        }


        //餌をまく
        if (Input.GetButtonDown("Food"))
        {
            FoodCheck();
        }

        //音を鳴らす
        if(Input.GetButtonDown("Whistle"))
        {
            SoundManger.Instance.PlaySE(0);
            StartCoroutine(WhistleActive());
        }

    }

    /// <summary>
    /// 設置する餌を切り替える
    /// </summary>
    /// <param name="num">右側に切りかえるなら正の数、左側なら負の数</param>
    public void FoodCheck()
    {
        //今選んでいる餌を調べる
        _foodNumber = _foodUIMove.SelectFoodNumber();
        //選んでいる餌が所持数0以下なら、何もしない
        if (_foodUIMove.FoodCountCheck(_foodNumber) <= 0) return;
        _foodUIMove.FoodCountSub(_foodNumber);

        Vector3 pos = new Vector3(this.transform.position.x,
                                                       this.transform.position.y - 1.5f,
                                                       this.transform.position.z);
        //餌を生成
        GameObject _foodObj = Instantiate(_food);
        _foodObj.transform.localPosition = pos;
        _foodObj.GetComponent<Food>().SelectFood(_foodNumber);
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
    }

    void OnTriggerExit(Collider col)
    {
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
}
