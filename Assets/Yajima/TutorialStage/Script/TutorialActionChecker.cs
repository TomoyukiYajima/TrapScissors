using UnityEngine;
using System.Collections;

public class TutorialActionChecker : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        TutorialMediator.GetInstance().SetTutorialAction(true);
    }

    public void OnTriggerExit(Collider other)
    {
        TutorialMediator.GetInstance().SetTutorialAction(false);
    }
}
