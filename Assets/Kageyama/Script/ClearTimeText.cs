using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClearTimeText : MonoBehaviour
{
    private int _clearTime;
    private int _clearTimeCount;
    private int _clearTimemin;
    private Text _myText;
    private bool _skipFlag;
    string _clearCountText = "";
    string _clearMinText = "";
    [SerializeField]
    private GameObject _huntCountText;

    void Awake()
    {
        _myText = this.GetComponent<Text>();
        _myText.text = "00min00sec";
        _skipFlag = false;
    }
    
    // Use this for initialization
    void Start()
    {
        _clearTime = GameManager.gameManager.GameTime();
        StartCoroutine(CountAddStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && _skipFlag == false)
        {
            _skipFlag = true;
            _clearTimemin = _clearTime / 60;
            _clearTimeCount = _clearTime % 60;

            TextWrite();
        }
    }
    
    IEnumerator CountAddStart()
    {
        _clearCountText = "";
        _clearMinText = "";
        for (int i = 0; i < _clearTime; i++)
        {
            if (_skipFlag == true)
            {
                break;
            }
            else if (_skipFlag == false)
            {
                _clearTimeCount++;
                SoundManger.Instance.PlaySE(15);
                if (_clearTimeCount >= 60)
                {
                    _clearTimeCount = 0;
                    _clearTimemin++;
                }
                TextWrite();
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
        _huntCountText.GetComponent<HuntCountText>().CountStart();
    }

    void TextWrite()
    {
        if (_clearTimeCount <= 9) _clearCountText = "0" + _clearTimeCount.ToString();
        else if (_clearTimeCount > 9) _clearCountText = _clearTimeCount.ToString();

        if (_clearTimemin <= 9) _clearMinText = "0" + _clearTimemin.ToString();
        else if (_clearTimemin > 9) _clearMinText = _clearTimemin.ToString();

        _myText.text = _clearMinText + "min " + _clearCountText + "sec";
    }

}
