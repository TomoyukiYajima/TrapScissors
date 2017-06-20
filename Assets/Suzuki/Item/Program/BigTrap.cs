using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BigTrap : MonoBehaviour
{
    public enum TrapState
    {
        WAIT,       //動物が掛かるのを待っている状態
        CAPTURE,    //動物を捕まえている状態
    }
    public TrapState _state;
    [SerializeField, TooltipAttribute("Resultで表示されるオブジェクト")]
    private GameObject _result;
    private bool _flg;
    #region 挟むときに必要な変数
    //鋏んでいるオブジェクトを入れる
    [SerializeField]
    private GameObject _targetAnimal;
    #endregion

    #region 火花が散るための変数
    [SerializeField, TooltipAttribute("火花")]
    public GameObject _traphit;
    //[SerializeField, TooltipAttribute("火花の位置")]
    //public GameObject _HibanaIti;
    #endregion

    // Use this for initialization
    void Start()
    {
        _targetAnimal = null;
        _state = TrapState.WAIT;
        _flg = false;
        
    }
    //当たっている最中も取得する当たり判定
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "LargeEnemy")
        {
            Enemy3D animal = col.gameObject.GetComponent<Enemy3D>();
            animal.ChangeTrap(gameObject);
            ChengeState(TrapState.CAPTURE);
            _targetAnimal = col.gameObject;

            Instantiate(_traphit, this.transform.position, Quaternion.identity);
            _flg = true;
            _result.SetActive(true);
            GameManager.gameManager.HuntCountAdd();
            GameManager.gameManager.GameStateSet(GameManager.GameState.END);
        }
    }

    public void ChengeState(TrapState state)
    {
        _state = state;
    }
    public bool FlgTarget()
    {
        return _flg;
    }

}
