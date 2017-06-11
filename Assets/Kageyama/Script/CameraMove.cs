using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    #region プレイヤー追尾に必要な変数
    private GameObject _player = null;
    private Vector3 _offset = Vector3.zero;
    private Vector3 _offset_Move = Vector3.zero;
    [SerializeField, TooltipAttribute("スムーズに追いかけるかどうか")]
    private bool _lerpFrag;
    [SerializeField, TooltipAttribute("追尾させるかどうか")]
    private bool _moveFrag;
    #endregion
    #region カメラだけで動くときに使用する変数
    //プレイヤーの移動を固定して、カメラで全体を見渡せるようにする
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
        _player = GameObject.FindGameObjectWithTag("Player");
        _offset = transform.position - _player.transform.position;
        _offset_Move = transform.localPosition - _player.transform.localPosition;
        _clampX_max = GameManager.gameManager.ClampX_MAX() + _offset_Move.x;
        _clampX_min = GameManager.gameManager.ClampX_MIN() + _offset_Move.x;
        _clampZ_max = GameManager.gameManager.ClampZ_MAX() + _offset_Move.z;
        _clampZ_min = GameManager.gameManager.ClampZ_MIN() + _offset_Move.z;
        _playerMoveLock = false;
    }

    void LateUpdate()
    {
        newPosition = transform.position;
        //プレイヤーと一緒に動く
        if (_playerMoveLock == false)
        {
            if (_moveFrag == false) return;

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

        if (_playerMoveLock == true || GameManager.gameManager.GameStateCheck() == GameManager.GameState.START)
        {
            newPosition = transform.position;
            _moveDirection = (Vector3.forward - Vector3.right) * Input.GetAxis("Vertical") + (Vector3.forward + Vector3.right) * Input.GetAxis("Horizontal");
            newPosition += _moveDirection * Time.deltaTime * _speed;
            transform.position = new Vector3(Mathf.Clamp(newPosition.x, _clampX_min, _clampX_max),
                                         newPosition.y,
                                         Mathf.Clamp(newPosition.z, _clampZ_min, _clampZ_max));
        }

        if (GameManager.gameManager.GameStateCheck() == GameManager.GameState.START)
        {
            _playerMoveLock = true;
            return;
        }

        if (Input.GetAxis("Lock") >= 0.5f && _playerMoveLock == false)
        {
            _playerReturnPos = transform.position;
            _cameraMap.SetActive(true);
            _playerMoveLock = true;
        }
        else if(Input.GetAxis("Lock") < 0.5f && _playerMoveLock == true)
        {
            transform.position = _playerReturnPos;
            _cameraMap.SetActive(false);
            _playerMoveLock = false;
        }
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
    /// カメラを追尾させるかどうか(trueなら追尾させる、falseなら追尾させない)
    /// </summary>
    /// <param name="frag">追尾させるかどうか</param>
    public void MoveChenge(bool frag)
    {
        _moveFrag = frag;
    }

    public bool LockCheck()
    {
        return _playerMoveLock;
    }

    public Vector3 OffsetCheck()
    {
        return _offset_Move;
    }
}
