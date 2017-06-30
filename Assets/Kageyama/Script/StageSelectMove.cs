using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelectMove : MonoBehaviour
{
    [SerializeField]
    private float[] _toPosition;        //移動座標
    [SerializeField]
    private GameObject[] _chilButton;   //移動させるボタン
    [SerializeField]
    private GameObject[] _arrowButton;  //矢印のボタン
    private int _positionNumber;        //今の座標番号
    private RectTransform _myRect;      //自分のRectTransform
    private bool _moveflag;             //動ける状態か

	// Use this for initialization
	void Start ()
    {
        GameObject _stageCheck = GameObject.Find("OpenStage");
        int _opennumber = _stageCheck.GetComponent<OpenStage>().OpenStageCheck();
        if(_stageCheck != null)
        {
            for(int i = 0; i < _chilButton.Length; i++)
            {
                if(i > _opennumber) _chilButton[i].GetComponent<Button>().interactable = false;
                else _chilButton[i].GetComponent<Button>().interactable = true;
            }
        }

        _positionNumber = 0;
        _myRect = this.GetComponent<RectTransform>();
        //ButtonSelect();
        _moveflag = false;
        ArrowActive();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetAxis("Horizontal") >= 0.5f)
        {
            RightMove();
        }
        else if (Input.GetAxis("Horizontal") <= -0.5f)
        {
            LeftMove();
        }
    }

    /// <summary>
    /// 選択しているボタンを右に移動する
    /// </summary>
    public void RightMove()
    {
        if (_positionNumber >= _toPosition.Length - 1 || _moveflag == true) return;
        _moveflag = true;
        SoundManger.Instance.PlaySE(6);
        _positionNumber++;
        ArrowCollarChange(0);
        ButtonSelect();
        LeanTween.move(_myRect, new Vector2(_toPosition[_positionNumber], this.transform.localPosition.y), 0.2f)
            .setOnComplete(() => {
                _moveflag = false;
                ArrowActive();
            });
    }
    /// <summary>
    /// 選択しているボタンを左に移動する
    /// </summary>
    public void LeftMove()
    {
        if (_positionNumber <= 0 || _moveflag == true) return;
        _moveflag = true;
        SoundManger.Instance.PlaySE(6);
        _positionNumber--;
        ArrowCollarChange(1);
        ButtonSelect();
        LeanTween.move(_myRect, new Vector2(_toPosition[_positionNumber], this.transform.localPosition.y), 0.2f)
            .setOnComplete(()=> {
                _moveflag = false;
                ArrowActive();
            });
    }

    /// <summary>
    /// ボタンの選択する
    /// </summary>
    void ButtonSelect()
    {
        _chilButton[_positionNumber].GetComponent<Button>().Select();
    }

    /// <summary>
    /// 移動させる方向の矢印を点滅させる
    /// </summary>
    /// <param name="num">右なら0、左なら1</param>
    void ArrowCollarChange(int num)
    {
        LeanTween.color(_arrowButton[num].GetComponent<RectTransform>(), new Color(1.0f, 0.0f, 0.0f, 1.0f), 0.2f)
            .setOnComplete(() => {
                LeanTween.color(_arrowButton[num].GetComponent<RectTransform>(), new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.2f);
            });
    }

    void ArrowActive()
    {
        if (_positionNumber == 0) _arrowButton[1].SetActive(false);
        else if (_positionNumber == _toPosition.Length - 1) _arrowButton[0].SetActive(false);
        else
        {
            _arrowButton[0].SetActive(true);
            _arrowButton[1].SetActive(true);
        }
    }
}
