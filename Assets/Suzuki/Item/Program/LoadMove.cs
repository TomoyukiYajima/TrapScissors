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
        //transform.Rotate(0f, 180f, 0f);
        _myrect = gameObject.GetComponent<RectTransform>();
        ToMovePosition();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ToMovePosition()
    {
        LeanTween.move(_myrect, new Vector2(30, 30), _speed)
            .setOnComplete(() =>
            {
                ToRotate();
                LeanTween.move(_myrect, new Vector2(770, 30), _speed)
                .setOnComplete(() =>
                {
                    ToRotate();
                })
            .setLoopPingPong();
            })
            ;
        

    }
    public void ToRotate()
    {
        transform.Rotate(0f, 180f, 0f);
        //LeanTween.rotate(_myrect, 180f, 1f);
    }
}
