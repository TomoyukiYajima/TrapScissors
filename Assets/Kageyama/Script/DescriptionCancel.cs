﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DescriptionCancel : MonoBehaviour
{
    [SerializeField]
    private GameObject _eventSystem;
	// Use this for initialization
	void Start ()
    {
	    if(_eventSystem == null || _eventSystem.name != "EventSystem")
        {
            _eventSystem = GameObject.Find("EventSystem");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.anyKeyDown)
        {
            SoundManger.Instance.PlaySE(1);
            _eventSystem.GetComponent<EventSystem>().sendNavigationEvents = true; 
            this.gameObject.SetActive(false);
        }
	}
}
