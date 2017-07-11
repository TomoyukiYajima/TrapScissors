using System.Collections;
using UnityEngine;

public class FoodUICountClearChecker : ClearChacker {

    private FoodUIMove m_FoodUI;

    // Use this for initialization
    public override void Start () {
        var foodParent = GameObject.Find("FoodParent");
        m_FoodUI = foodParent.GetComponent<FoodUIMove>();
    }
	
	// Update is called once per frame
	public override void Update () {
        if (m_FoodUI.FoodCountCheck(1) >= 1) DrawText();
	}
}
