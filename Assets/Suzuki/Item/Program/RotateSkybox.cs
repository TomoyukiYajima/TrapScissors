﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField, TooltipAttribute("朝方")]
    private Material Dawn;
    [SerializeField, TooltipAttribute("朝")]
    private Material Morning;
    [SerializeField, TooltipAttribute("夕焼け")]
    private Material Evening;
    [SerializeField, TooltipAttribute("夜")]
    private Material Night;
    [SerializeField, TooltipAttribute("曇り")]
    private Material Cloudy;
    public float _anglePerFrame = 0.1f;    // 1フレームに何度回すか
    public float _rot = 0f;//0.03fが基本
    public GameObject _light;
    // Use this for initialization
    void Start(){
        RenderSettings.skybox = Morning;
    }
    // Update is called once per frame
    void Update()
    {
        _rot += _anglePerFrame;
        if (_rot >= 360.0f)
        {
            RenderSettings.skybox = Morning;
            _rot -= 360.0f;//1周(360°)すると0°になり、再度加算される
        }
        else if (_rot >= 330.0f) RenderSettings.skybox = Dawn;
        else if (_rot >= 210.0f) RenderSettings.skybox = Night;
        else if (_rot >= 180.0f) RenderSettings.skybox = Evening;

 RenderSettings.skybox.SetFloat("_Rotation", _rot);    //_rotが °の役割になり回転する
    }
}
