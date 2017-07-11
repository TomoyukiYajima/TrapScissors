using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraRay : MonoBehaviour
{
    [SerializeField]
    private GameObject _stageObj = null;
    [SerializeField]
    private GameObject _backUpObje = null;

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        RayCheck();

        if (_stageObj == null && _backUpObje == null) return;
        if (_backUpObje != _stageObj)
        {
            if(_backUpObje != null) MatChenge(_backUpObje, 1.0f);
            _backUpObje = _stageObj;
        }
    }

    void RayCheck()
    {
        RaycastHit hit;
        Ray ray;
        ray = new Ray(transform.position + new Vector3(0, 0, 0), transform.forward);
        if (Physics.SphereCast(ray, 2, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        
        if (hit.collider != null && hit.collider.tag == "StageObje")
        {
            _stageObj = hit.collider.gameObject;
            _backUpObje = _stageObj;
            MatChenge(_stageObj, 0.5f);
        }
        else _stageObj = null;
    }

    void MatChenge(GameObject obj, float alpha)
    {
        //List<Material> mat = new List<Material>();
        Material[] mat = obj.GetComponent<Renderer>().materials;
        Color matColor;
        for(int i = 0; i < mat.Length; i++)
        {
            matColor = mat[i].color;
            mat[i].color = new Color(matColor.r, matColor.g, matColor.b, alpha);
        }
    }
}
