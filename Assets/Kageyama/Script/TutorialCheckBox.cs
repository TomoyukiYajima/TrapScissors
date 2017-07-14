using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheckBox : MonoBehaviour
{
    #region 移動に関係する変数
    [SerializeField, TooltipAttribute("チェックボックス")]
    private RectTransform[] _checkBox;
    //移動させる対象の配列の番号
    private int _num = 0;
    [SerializeField,TooltipAttribute("画面に出てくるときの移動先")]
    private Vector2 _inPosition;
    [SerializeField, TooltipAttribute("画面外に出ていくときの移動先")]
    private Vector2 _outPosition;
    [SerializeField, TooltipAttribute("出てくるときの移動時間")]
    private float _moveTime;
    #endregion

    // Use this for initialization
    void Start ()
    {
    }

    /// <summary>
    /// 項目に掛かれている行動が完了したら、表示外に移動させ、次の項目を画面内に移動させる
    /// </summary>
    #region NextTutorial
    public void NextTutorial()
    {
        LeanTween.move(_checkBox[_num], _outPosition, _moveTime)
            .setOnComplete(() => {
                InMove();
            });
    }
    #endregion

    /// <summary>
    /// 項目に掛かれている行動が完了したら、表示外に移動させ、次の項目を画面内に移動させる
    /// </summary>
    /// <param name="delayTime">待機時間</param>
    #region NextTutorial(Delay)
    public void NextTutorial(float delayTime)
    {
        LeanTween.move(_checkBox[_num], _outPosition, _moveTime)
            .setDelay(delayTime)
            .setOnComplete(()=>{
                InMove();
            });
    }
    #endregion

    /// <summary>
    /// 画面外から入ってくる(最初の1回はこれを呼ぶ)
    /// </summary>
    #region InMove
    public void InMove()
    {
        LeanTween.move(_checkBox[_num], _inPosition, _moveTime);
    }
    #endregion

    /// <summary>
    /// 画面外から入り、次をセットする
    /// </summary>
    #region InMoveAdd
    void InMoveAdd()
    {
        if (_num < _checkBox.Length - 1)
        {
            _num++;
            LeanTween.move(_checkBox[_num], _inPosition, _moveTime);
        }
    }
    #endregion

}
