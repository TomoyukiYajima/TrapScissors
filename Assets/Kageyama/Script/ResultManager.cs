using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    protected static ResultManager _resultManager;

    [SerializeField]
    private GameObject _clearPopUpParent;
    [SerializeField]
    private GameObject _overPopUpParent;
    [SerializeField]
    private GameObject _gameClear;
    [SerializeField]
    private GameObject _gameOver;
    [SerializeField]
    private GameObject _timeText;

    public static ResultManager resultManager
    {
        get
        {
            if (_resultManager == null)
            {
                _resultManager = (ResultManager)FindObjectOfType(typeof(ResultManager));
                if (_resultManager == null)
                {
                    Debug.LogError("SceneChange Instance Error");
                }
            }

            return _resultManager;
        }
    }

    /// <summary>
    /// クリアした時に文字を表示させる
    /// </summary>
    public void ClearPopUpActiveEnd()
    {
        //_clearPopUpParent.SetActive(false);
        SceneManagerScript.sceneManager.Black();
        _gameClear.SetActive(true);
    }

    /// <summary>
    /// ゲームオーバーの時に文字を表示させる
    /// </summary>
    public void GameOverPopActiveEnd()
    {
        _gameOver.SetActive(true);
        _gameOver.transform.Find("ContinueButton").GetComponent<Button>().Select();
    }

    /// <summary>
    /// ゲームクリアのUI表示を設定する
    /// </summary>
    /// <param name="flag"></param>
    public void ClearSetActive(bool flag)
    {
        _gameClear.SetActive(flag);
    }
}
