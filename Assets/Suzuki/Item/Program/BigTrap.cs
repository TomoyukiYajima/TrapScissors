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
    public bool _istutorial = false;
    public int _stagenum=0;
    public GameObject _traphit;
    [SerializeField, TooltipAttribute("Resultで表示されるオブジェクト")]
    private GameObject _result;
    private bool _flg;
    [SerializeField]
    private GameObject _targetAnimal;
    private Animator _animator;


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
            SoundManger.Instance.PlaySE(10);
            Enemy3D animal = col.gameObject.GetComponent<Enemy3D>();
            animal.ChangeTrap(gameObject);
            ChengeState(TrapState.CAPTURE);
            _targetAnimal = col.gameObject;
            Instantiate(_traphit, this.transform.position, Quaternion.identity);
            _animator.SetBool("Close", true);
            _flg = true;
            _animator.CrossFade("Hit", 0.1f, -1);
            //this.gameObject.transform.localScale = new Vector3(5,5,5);
            GameManager.gameManager.HuntCountAdd();
            if (_istutorial == false)
            {
                _result.SetActive(true);
                GameManager.gameManager.GameStateSet(GameManager.GameState.END);
            }
            if (_stagenum == null)
            {
                GameObject _openstage = GameObject.Find("OpenStage");
                _openstage.GetComponent<OpenStage>().StageSet(_stagenum);
            }
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
//効果音素材：ポケットサウンド – http://pocket-se.info/
