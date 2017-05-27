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
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClearPopUpActiveEnd()
    {
        _clearPopUpParent.SetActive(false);
        SceneManagerScript.sceneManager.Black();
        _gameClear.SetActive(true);
    }

    public void GameOverPopActiveEnd()
    {
        _gameOver.SetActive(true);
        _gameOver.transform.FindChild("ContinueButton").GetComponent<Button>().Select();
    }

    public void ClearSetActive(bool flag)
    {
        _gameClear.SetActive(flag);
    }
}
