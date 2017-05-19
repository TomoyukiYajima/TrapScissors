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

	// Use this for initialization
	void Start ()
    {
        _positionNumber = 0;
        _myRect = this.GetComponent<RectTransform>();
        //ButtonSelect();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RightMove();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            LeftMove();
        }
    }

    public void RightMove()
    {
        if (_positionNumber >= _toPosition.Length - 1) return;
        _positionNumber++;
        ArrowCollarChange(0);
        ButtonSelect();
        LeanTween.move(_myRect, new Vector2(_toPosition[_positionNumber], this.transform.localPosition.y), 0.2f);
    }

    public void LeftMove()
    {
        if (_positionNumber <= 0) return;
        _positionNumber--;
        ArrowCollarChange(1);
        ButtonSelect();
        LeanTween.move(_myRect, new Vector2(_toPosition[_positionNumber], this.transform.localPosition.y), 0.2f);
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
