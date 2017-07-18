using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField]
    private GameObject _playStartImage;

    // Use this for initialization
    void Start ()
    {
        if (_mainCamera == null) _mainCamera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.gameManager.GameStateCheck() == GameManager.GameState.START)
        {
            if (Input.GetButtonDown("Submit"))
            {
                GameManager.gameManager.GameStateSet(GameManager.GameState.PLAY);
                _mainCamera.GetComponent<CameraMove>()._modeSkipFlag = true;
                LeanTween.cancel(_mainCamera);
                _playStartImage.SetActive(false);
                _mainCamera.GetComponent<CameraMove>().Skip();
            }
        }
	}
}
