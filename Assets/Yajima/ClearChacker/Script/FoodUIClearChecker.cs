using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FoodUIClearChecker : ClearChacker {

    private FoodUIMove m_FoodUI;

    private bool m_IsMove = false;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        var foodParent = GameObject.Find("FoodParent");
        m_FoodUI = foodParent.GetComponent<FoodUIMove>();
    }

    // Update is called once per frame
    public override void Update () {
        // えさUIの移動
        MoveFoodUI();

        CheckFoodUI();
    }

    // えさUIの移動を行います
    private void MoveFoodUI()
    {
        if (!m_IsMove)
        {
            m_FoodUI.LeftRotation();
            m_IsMove = true;
        }
    }

    private void CheckFoodUI()
    {
        var number = m_FoodUI.SelectFoodNumber();
        if (number == 0 && m_FoodUI.FoodCountCheck(0) >= 1) DrawText();
    }
}
