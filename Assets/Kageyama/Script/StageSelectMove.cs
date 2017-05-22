using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelectMove : MonoBehaviour
{
    [SerializeField]
    private float[] _toPosition;
    [SerializeField]
    private GameObject[] _chilButton;
    [SerializeField]
    private GameObject[] _arrowButton;
    private int _positionNumber;
    private RectTransform _myRect;
    private bool _moveflag;

	// Use this for initialization
	void Start ()
    {
        _positionNumber = 0;
        _myRect = this.GetComponent<RectTransform>();
        //ButtonSelect();
        _moveflag = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    RightMove();
        //}
        //else if (Input.GetKeyDown(KeyCode.J))
        //{
        //    LeftMove();
        //}

        if(Input.GetAxis("Horizontal") >= 0.5f)
        {
            RightMove();
        }
        else if (Input.GetAxis("Horizontal") <= -0.5f)
        {
            LeftMove();
        }
    }

    public void RightMove()
    {
        if (_positionNumber >= _toPosition.Length - 1 || _moveflag == true) return;
        _moveflag = true;
        _positionNumber++;
        ArrowCollarChange(0);
        ButtonSelect();
        LeanTween.move(_myRect, new Vector2(_toPosition[_positionNumber], this.transform.localPosition.y), 0.2f)
            .setOnComplete(() => { _moveflag = false; });
    }

    public void LeftMove()
    {
        if (_positionNumber <= 0 || _moveflag == true) return;
        _moveflag = true;
        _positionNumber--;
        ArrowCollarChange(1);
        ButtonSelect();
        LeanTween.move(_myRect, new Vector2(_toPosition[_positionNumber], this.transform.localPosition.y), 0.2f)
            .setOnComplete(()=> { _moveflag = false; });
    }

    void ButtonSelect()
    {
        _chilButton[_positionNumber].GetComponent<Button>().Select();
    }

    void ArrowCollarChange(int num)
    {
        LeanTween.color(_arrowButton[num].GetComponent<RectTransform>(), new Color(1.0f, 0.0f, 0.0f, 1.0f), 0.2f)
            .setOnComplete(() => {
                LeanTween.color(_arrowButton[num].GetComponent<RectTransform>(), new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.2f);
            });
    }
}
