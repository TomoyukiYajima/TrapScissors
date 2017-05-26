using UnityEngine;
using System.Collections;

public class AnyButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _AnyButtonImage;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(AnyButtonON());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator AnyButtonON()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        _AnyButtonImage.SetActive(true);
        SceneManagerScript.sceneManager.AnyButtonOn(true, "StageSelect");

    }
}
