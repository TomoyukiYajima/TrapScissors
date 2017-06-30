using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DescriptionCancel : MonoBehaviour
{
    [SerializeField]
    private GameObject _eventSystem;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.anyKeyDown)
        {
            _eventSystem.GetComponent<EventSystem>().sendNavigationEvents = true; 
            this.gameObject.SetActive(false);
        }
	}
}
