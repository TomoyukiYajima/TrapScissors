using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BigTrap : MonoBehaviour
{
    public enum TrapState
    {
        WAIT,       //待機状態
        CAPTURE,    //捕獲状態
    }
    public TrapState _state;
    public bool _istutorial = false;
    public int _stagenum = 0;
    public GameObject _traphit;                  //エフェクトの表示
    [SerializeField, TooltipAttribute("Resultで表示されるオブジェクト")]
    private GameObject _result;
    [SerializeField, TooltipAttribute("捕獲時の動物")]
    private GameObject _targetAnimal;
    private bool _flg;                           //捕獲状態事に判定
    private Animator _animator;

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
            _animator.SetBool("Close", true); _flg = true;
            SoundManger.Instance.PlaySE(10);
            Instantiate(_traphit, this.transform.position, Quaternion.identity);
            transform.localScale = new Vector3(10,10,10); //縮小対策
            _animator.CrossFade("Hit", 0.1f, -1, 0);
            GameManager.gameManager.HuntCountAdd();
            if (_istutorial == false)
            {
                _result.SetActive(true);
                GameManager.gameManager.GameStateSet(GameManager.GameState.END);
            }
            if (_stagenum == null) //次ステージへ移行できるようにする
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
