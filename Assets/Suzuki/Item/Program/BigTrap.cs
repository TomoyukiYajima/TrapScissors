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
    public bool _istutorial=false;
    public int _stagenum;
    [SerializeField, TooltipAttribute("Resultで表示されるオブジェクト")]
    private GameObject _result;
    private bool _flg;
    [SerializeField]
    private GameObject _targetAnimal;
    private Animator _animator;

    public GameObject _traphit;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _targetAnimal = null;
        _state = TrapState.WAIT;
        _flg = false;
        _animator.SetBool("Close", false);

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
            Instantiate(_traphit,this.transform.position, Quaternion.identity);
            _animator.SetBool("Close", true);

            _flg = true;

            GameManager.gameManager.HuntCountAdd();
            if (_istutorial == false)
            {
                
                _result.SetActive(true);
                GameManager.gameManager.GameStateSet(GameManager.GameState.END);
            }
            GameObject _openstage = GameObject.Find("OpenStage");
            _openstage.GetComponent<OpenStage>().StageSet(_stagenum);
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
