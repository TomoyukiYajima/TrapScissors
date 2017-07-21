using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraRay : MonoBehaviour
{
    [SerializeField]
    private GameObject _stageObj = null;
    [SerializeField]
    private GameObject _backUpObj = null;
    private GameObject _player = null;
    private CameraMove _cameraMove = null;

    void Start()
    {
        _cameraMove = this.GetComponent<CameraMove>();
        _player = GameObject.Find("Player_2_Test");
    }

	// Update is called once per frame
	void Update ()
    {
        RayCheck();

        if (_stageObj == null && _backUpObj == null) return;
        if (_backUpObj != _stageObj)
        {
            if(_backUpObj != null) MatChenge(_backUpObj, 1.0f);
            _backUpObj = _stageObj;
        }
    }

    void RayCheck()
    {
        RaycastHit _hit;
        Ray _ray;
        Vector3 _pos = Vector3.zero;
        //プレイヤーがいない、もしくはカメラだけで動く状態なら、中心にRayを飛ばす
        if (_player == null || _cameraMove.LockCheck() == true)
        {
            _ray = new Ray(transform.position + new Vector3(0, 0, 0), transform.forward);
        }
        else
        {
            _pos =new Vector3(_player.transform.position.x,_player.transform.position.y + 1,_player.transform.position.z);
            _ray = new Ray(transform.position + new Vector3(0, 0, 0), _pos - this.transform.position);
        }

//#if UNITY_EDITOR
        if (Physics.SphereCast(_ray, 2, out _hit, 100))
        {
            Debug.DrawLine(_ray.origin, _hit.point, Color.red);
        }
//#endif

        if (_hit.collider != null && _hit.collider.tag == "StageObje")
        {
            _stageObj = _hit.collider.gameObject;
            MatChenge(_stageObj, 0.5f);
        }
        else _stageObj = null;
    }

    void MatChenge(GameObject obj, float alpha)
    {
        Material[] mat = obj.GetComponent<Renderer>().materials;
        Color matColor;
        for(int i = 0; i < mat.Length; i++)
        {
            matColor = mat[i].color;
            mat[i].color = new Color(matColor.r, matColor.g, matColor.b, alpha);
        }
    }
}
