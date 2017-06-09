using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadMove : MonoBehaviour
{
    public float _speed = 1.0f;
    private RectTransform _myrect;
    // Use this for initialization
    void Start()
    {
        _myrect = gameObject.GetComponent<RectTransform>();
        ToMovePosition();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ToMovePosition()
    {
        int _randX = Random.Range(-370, 370);
        if(_randX - this.transform.localPosition.x == 0)
        {
            ToMovePosition();
            return;
        }
        else if (_randX <= this.transform.localPosition.x)
        {
            _speed = (this.transform.localPosition.x - _randX) / 200.0f;
            ToRotate(180);
        }
        else
        {
            _speed = (_randX - this.transform.localPosition.x) / 200.0f;
            ToRotate(0);
        }
        LeanTween.move(_myrect, new Vector2(_randX, -195), _speed)
            .setOnComplete(() =>
            {
                ToMovePosition();
            });
        

    }
    public void ToRotate(float rotate)
    {
        transform.Rotate(0f, rotate, 0f);
    }
}
