using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateSkybox : MonoBehaviour {
    public float _anglePerFrame = 0.1f;    // 1フレームに何度回すか
    private float _rot = 0.0f;
    public static Material Skybox;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _rot += _anglePerFrame;
        if (_rot >= 360.0f)
        {    
            _rot -= 360.0f;
        }
        RenderSettings.skybox.SetFloat("_Rotation", _rot);    //回転する

        
    }
}
