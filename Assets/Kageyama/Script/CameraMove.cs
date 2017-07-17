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
    private float _size = 0;                //カメラの拡大縮小のサイズ
    private float _sizeMin = 7;
    private float _sizeMax = 20;
    #endregion

    private Vector3 newPosition;    //移動時に座標を計算するために一時的に座標を入れる変数
    [SerializeField]
    private GameObject _cameraMap;  //ミニマップのカメラUI

    void Start()
    {
        _size = this.GetComponent<Camera>().orthographicSize;
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
        OpningMove(0);

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

            //サイズが範囲内のときのみ拡大縮小を行う
            if (_sizeMin <= _size && _size <= _sizeMax && GameManager.gameManager.GameStateCheck() == GameManager.GameState.PLAY)
            {
                //サイズの拡大縮小をする
                _size += Input.GetAxis("RightStick");
                if (_size < _sizeMin) _size = _sizeMin;
                else if (_size > _sizeMax) _size = _sizeMax;
                this.GetComponent<Camera>().orthographicSize = _size;
            }
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
            _size = _sizeMin;
            this.GetComponent<Camera>().orthographicSize = _size;
        }
        //ロックを外す
        else if(Input.GetAxis("Lock") < 0.5f && _playerMoveLock == true)
        {
            transform.position = _playerReturnPos;
            _cameraMap.SetActive(false);
            _playerMoveLock = false;
            _size = _sizeMin;
            this.GetComponent<Camera>().orthographicSize = _size;
        }
    }

    /// <summary>
    /// ゲーム開始時に拡大していく
    /// </summary>
    #region OpningExpansion
    IEnumerator OpningExpansion()
    {
        while (_size > _sizeMin && _modeSkipFlag == false)
        {
            _size -= 1;
            this.GetComponent<Camera>().orthographicSize = _size;
            yield return null;
        }
        if(_size != _sizeMin)
        {
            _size = _sizeMin;
            this.GetComponent<Camera>().orthographicSize = _size;
        }
        _openingMoveflag = false;

    }
    #endregion

    /// <summary>
    /// ゲーム開始時に動く
    /// </summary>
    /// <param name="num">どこの座標まで動くか</param>
    #region OpningMove
    void OpningMove(int num)
    {
        Skip();
        if (_openingMoveflag != true) return;
        LeanTween.move(gameObject, _movePoint[num], _moveTime[num])
            .setOnComplete(() =>
            {
                num++;
                if (num >= _movePoint.Count)
                {
                    num = 0;
                    StartCoroutine(OpningExpansion());
                    return;
                }
                OpningMove(num);
            });
    }
    #endregion

    /// <summary>
    /// 最初の演出をスキップする
    /// </summary>
    #region Skip
    public void Skip()
    {
        if (_modeSkipFlag == true)
        {
            _openingMoveflag = false;
            StartCoroutine(OpningExpansion());
            return;
        }
    }

    #endregion

    /// <summary>
    /// スムーズに追いかけるときはtrue、ピッタリくっつくときはfalseにする
    /// </summary>
    /// <param name="frag">スムーズにするかどうか</param>
    #region LerpFragChenge
    public void LerpFragChenge(bool frag)
    {
        _lerpFrag = frag;
    }
    #endregion

    /// <summary>
    /// プレイヤーに追尾しないで、カメラ単独で動ける状態かどうか調べる
    /// </summary>
    /// <returns>単独で動けるかどうか</returns>
    #region LockCheck
    public bool LockCheck()
    {
        return _playerMoveLock;
    }
    #endregion

    /// <summary>
    /// カメラが単独で動くときの座標の取得
    /// </summary>
    /// <returns>単独で動いているときの座標値</returns>
    #region OffsetCheck
    public Vector3 OffsetCheck()
    {
        return _offset_Move;
    }
    #endregion
}
