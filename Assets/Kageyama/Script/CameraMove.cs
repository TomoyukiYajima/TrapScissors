using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    private GameObject _player = null;
    private Vector3 _offset = Vector3.zero;
    [SerializeField, TooltipAttribute("スムーズに追いかけるかどうか")]
    private bool _lerpFrag;
    [SerializeField, TooltipAttribute("追尾させるかどうか")]
    private bool _moveFrag;

    //プレイヤーの移動を固定して、カメラで全体を見渡せるようにする
    [SerializeField]
    private bool _playerMoveLock;
    private Vector3 _playerReturnPos;
    [SerializeField, TooltipAttribute("カメラの移動速度")]
    private float _speed;
    private Vector3 _moveDirection = Vector3.zero;
    private float _clampX;
    private float _clampZ;
    private Vector3 newPosition;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _offset = transform.position - _player.transform.position;
        _clampX = GameManager.gameManager.ClampX();
        _clampZ = GameManager.gameManager.ClampZ();
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
        if (_playerMoveLock == true)
        {
            newPosition = transform.position;
            _moveDirection = (Vector3.forward - Vector3.right) * Input.GetAxis("Vertical") + (Vector3.forward + Vector3.right) * Input.GetAxis("Horizontal");
            newPosition += _moveDirection * Time.deltaTime * _speed;
            transform.position = new Vector3(Mathf.Clamp(newPosition.x, -100.0f, _clampX),
                                         newPosition.y,
                                         Mathf.Clamp(newPosition.z, -30.0f, _clampZ));
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_playerMoveLock == true)
            {
                transform.position = _playerReturnPos;
                _playerMoveLock = false;
            }
            else
            {
                _playerReturnPos = transform.position;
                _playerMoveLock = true;
            }
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
}
