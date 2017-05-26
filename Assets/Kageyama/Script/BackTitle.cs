using UnityEngine;
using System.Collections;

public class BackTitle : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Trap"))
        {
            SceneManagerScript.sceneManager.FadeOut("Title");
        }
    }
}
