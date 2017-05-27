using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonFlash : MonoBehaviour
{
    private RectTransform _myRect;

	// Use this for initialization
	void Start ()
    {
        _myRect = this.GetComponent<RectTransform>();
        LeanTween.scale(_myRect, new Vector3(1.0f, 1.0f, 1.0f), 2)
            .setLoopPingPong()
            .setEase(LeanTweenType.easeOutQuart);
        LeanTween.alpha(_myRect, 1.0f, 2)
            .setLoopPingPong()
            .setEase(LeanTweenType.easeOutQuart);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
