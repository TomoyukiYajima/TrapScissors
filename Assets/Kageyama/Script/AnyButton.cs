using UnityEngine;
using System.Collections;

public class AnyButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _AnyButtonImage;
    [SerializeField]
    private string _nextStage;
    [SerializeField]
    private float _waitTime;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(AnyButtonON());
	}

    IEnumerator AnyButtonON()
    {
        yield return new WaitForSecondsRealtime(_waitTime);
        _AnyButtonImage.SetActive(true);
        SceneManagerScript.sceneManager.AnyButtonOn(true, _nextStage);

    }
}
