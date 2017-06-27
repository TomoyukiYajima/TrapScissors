using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMove : MonoBehaviour
{
    #region プレイヤー追尾に必要な変数
    private GameObject _player = null;
    private Vector3 _offset = Vector3.zero;
    private Vector3 _offset_Move = Vector3.zero;
    [SerializeField, TooltipAttribute("スムーズに追いかけるかどうか")]
    private bool _lerpFrag;
    #endregion
    #region ゲーム開始時にステージを見渡すときに使用する変数
    [System.NonSerialized]
    public bool _openingMoveflag;
    [SerializeField]
    private Vector3 _cameraPosition;
    [SerializeField]
    private List<Vector3> _movePoint = new List<Vector3>();
    [SerializeField]
    private List<float> _moveTime = new List<float>();
    [System.NonSerialized]
    public bool _modeSkipFlag;
    #endregion
    #region カメラだけで動くときに使用する変数
    [SerializeField]
    private bool _playerMoveLock;   //動ける状態かどうかのフラグ
    private Vector3 _playerReturnPos;
    [SerializeField, TooltipAttribute("カメラの移動速度")]
    private float _speed;
    private Vector3 _moveDirection = Vector3.zero;
    private float _clampX_max, _clampX_min; //X座標のクランプ
    private float _clampZ_max, _clampZ_min; //Z座標のクランプ
    #endregion
    private Vector3 newPosition;
    [SerializeField]
    private GameObject _cameraMap;  //ミニマップのカメラUI

    void Start()
    {
        _modeSkipFlag = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        _offset = _cameraPosition - _player.transform.position;
        _offset_Move = _cameraPosition - _player.transform.localPosition;
        //独立して動くときの移動できる範囲を計算
        _clampX_max = GameManager.gameManager.ClampX_MAX() + _offset_Move.x;
        _clampX_min = GameManager.gameManager.ClampX_MIN() + _offset_Move.x;
        _clampZ_max = GameManager.gameManager.ClampZ_MAX() + _offset_Move.z;
        _clampZ_min = GameManager.gameManager.ClampZ_MIN() + _offset_Move.z;
        
        _playerMoveLock = false;
        _openingMoveflag = true;
        StartCoroutine(OpningExpansion());

    }

    void LateUpdate()
    {
        newPosition = transform.position;
        //プレイヤーと一緒に動く
        if (_playerMoveLock == false && _openingMoveflag == false)
        {
            newPosition.x = _player.transform.position.x + _offset.x;
            newPosition.y = _player.transform.position.y + _offset.y;
            newPosition.z = _player.transform.position.z + _offset.z;
            //ピッタリと追いかける
            if (_lerpFrag == false) transform.position = newPosition;
            //スムーズに追いかける
            else transform.position = Vector3.Lerp(transform.position, newPosition, 5.0f * Time.deltaTime);
        }
    }

    void Update()
    {
        //ポーズ中は、カメラは何も変わらないようにする
        if (GameManager.gameManager.GameStateCheck() == GameManager.GameState.PAUSE) return;
        //ロック中はカメラだけで自由に移動できるようにする
        if (_playerMoveLock == true)
        {
            newPosition = transform.position;
            _moveDirection = (Vector3.forward - Vector3.right) * Input.GetAxis("Vertical") + (Vector3.forward + Vector3.right) * Input.GetAxis("Horizontal");
            newPosition += _moveDirection * Time.deltaTime * _speed;
            transform.position = new Vector3(Mathf.Clamp(newPosition.x, _clampX_min, _clampX_max),
                                         newPosition.y,
                                         Mathf.Clamp(newPosition.z, _clampZ_min, _clampZ_max));
        }

        //スタート時は強制的にロックする
        if (GameManager.gameManager.GameStateCheck() == GameManager.GameState.START)
        {
            _playerMoveLock = true;
            return;
        }

        //ロックする
        if (Input.GetAxis("Lock") >= 0.5f && _playerMoveLock == false)
        {
            _playerReturnPos = transform.position;
            _cameraMap.SetActive(true);
            _playerMoveLock = true;
        }
        //ロックを外す
        else if(Input.GetAxis("Lock") < 0.5f && _playerMoveLock == true)
        {
            print(_playerReturnPos);
            transform.position = _playerReturnPos;
            _cameraMap.SetActive(false);
            _playerMoveLock = false;
        }
    }

    /// <summary>
    /// ゲーム開始時に拡大していく
    /// </summary>
    IEnumerator OpningExpansion()
    {
        float _size = this.GetComponent<Camera>().orthographicSize;
        while (_size > 7 && _modeSkipFlag == false)
        {
            _size -= 4;
            this.GetComponent<Camera>().orthographicSize = _size;
            yield return null;
        }
        if(_size != 7)
        {
            _size = 7;
            this.GetComponent<Camera>().orthographicSize = _size;
        }
        OpningMove(0);

    }

    /// <summary>
    /// ゲーム開始時に動く
    /// </summary>
    /// <param name="num"></param>
    void OpningMove(int num)
    {
        if(_modeSkipFlag == true)
        {
            _openingMoveflag = false;
            return;
        }
        if (_openingMoveflag != true) return;
        LeanTween.move(gameObject, _movePoint[num], _moveTime[num])
            .setOnComplete(() =>
            {
                num++;
                if (num >= _movePoint.Count)
                {
                    num = 0;
                    _openingMoveflag = false;
                    return;
                }
                OpningMove(num);
            });
    }

    /// <summary>
    /// スムーズに追いかけるときはtrue、ピッタリくっつくときはfalseにする
    /// </summary>
    /// <param name="frag">スムーズにするかどうか</param>
    public void LerpFragChenge(bool frag)
    {
        _lerpFrag = frag;
    }
    
    /// <summary>
    /// プレイヤーに追尾しないで、カメラ単独で動ける状態かどうか調べる
    /// </summary>
    /// <returns></returns>
    public bool LockCheck()
    {
        return _playerMoveLock;
    }

    /// <summary>
    /// カメラが単独で動くときの座標の取得
    /// </summary>
    /// <returns></returns>
    public Vector3 OffsetCheck()
    {
        return _offset_Move;
    }
}
