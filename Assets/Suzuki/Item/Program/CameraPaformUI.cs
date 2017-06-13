using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraPaformUI : MonoBehaviour
{
    //点滅スピード調整
    [SerializeField]
    private float _speed = 0.03f;
    [SerializeField]
    private Image _TargetCamera;

    // Update is called once per frame
    void Update()
    {
        float _acolor = this.GetComponent<Image>().color.a;
        if (_acolor < 0 || _acolor > 1)
        {
            _speed = _speed * -1;
        }
        this.GetComponent<Image>().color = new Color(255, 255, 255, _acolor + _speed);
        if (Input.GetButtonDown("Trap"))
        {
            gameObject.SetActive(false);
           _TargetCamera.gameObject.SetActive(false);
           
            
        }
    }
}
