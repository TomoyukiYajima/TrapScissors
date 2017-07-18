﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonFlash : MonoBehaviour
{
    private RectTransform _myRect;

	// Use this for initialization
	void Start ()
    {
        _myRect = this.GetComponent<RectTransform>();
        Scale();
        Flash();
    }

    public void Scale()
    {
        LeanTween.scale(_myRect, new Vector3(1.0f, 1.0f, 1.0f), 2)
            .setLoopPingPong()
            .setEase(LeanTweenType.easeOutQuart);
    }
	
    public void Flash()
    {
        LeanTween.alpha(_myRect, 1.0f, 2)
            .setLoopPingPong()
            .setEase(LeanTweenType.easeOutQuart);
    }
}
