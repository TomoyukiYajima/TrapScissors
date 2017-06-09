using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour
{
    #region オブジェクトと座標の宣言
    //プレイヤー
    private GameObject _findObj;
    //プレイヤーの座標
    private Vector3 _objPos;
    //ミニマップのプレイヤーのUI
    private GameObject _mapUI;
    //ミニマップのプレイヤーのUIの座標
    private Vector2 _uiPos;
    #endregion

    #region 移動に必要な変数
    //GameManagerクラス
    private GameManager _gameManager;
    //プレイヤーの移動できる範囲を取得
    private float _moveX, _moveZ;
    #endregion

    [SerializeField]
    private string _targetTagName;
    [SerializeField]
    private bool _mainCameraFlag;

    // Use this for initialization
    void Start ()
    {
        //ゲームマネージャーの取得
        _gameManager = GameManager.gameManager;
        //ゲームマネージャーからどの位移動できるのか取得
        _moveX = (_gameManager.ClampX_MAX() - _gameManager.ClampX_MIN()) / 2;
        _moveZ = (_gameManager.ClampZ_MAX() - _gameManager.ClampZ_MIN()) / 2;

        StartPosCheck(_targetTagName);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
    }

    public void StartPosCheck(string objName)
    {
        //プレイヤーの探して取得
        _findObj = GameObject.FindWithTag(objName);
        //プレイヤーの場所を写すUIの取得
        _mapUI = this.gameObject;
    }

    void Move()
    {
        if (_mainCameraFlag == true)
        {
            _objPos = new Vector3(_findObj.transform.localPosition.x - (_gameManager.ClampX_MAX() - _moveX) - _findObj.GetComponent<CameraMove>().OffsetCheck().x,
                               _findObj.transform.localPosition.y,
                               _findObj.transform.localPosition.z - (_gameManager.ClampZ_MAX() - _moveZ) - _findObj.GetComponent<CameraMove>().OffsetCheck().z);

        }
        else
        {
            //プレイヤーから座標を取得
            //プレイヤーから座標を取得
            _objPos = new Vector3(_findObj.transform.localPosition.x - (_gameManager.ClampX_MAX() - _moveX),
                                    _findObj.transform.localPosition.y,
                                    _findObj.transform.localPosition.z - (_gameManager.ClampZ_MAX() - _moveZ));
        }
        var scaleX = _objPos.x / _moveX;
        var scaleZ = _objPos.z / _moveZ;

        _uiPos.x = 50.0f * scaleX;
        _uiPos.y = 50.0f * scaleZ;

        //取得したUIに座標を与える
        _mapUI.GetComponent<RectTransform>().localPosition = _uiPos;
    }
}
