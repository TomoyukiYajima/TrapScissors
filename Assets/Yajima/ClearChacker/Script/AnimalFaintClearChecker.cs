using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AnimalFaintClearChecker : AnimalClearChacker {

    #region 関数
    // Use this for initialization
    public override void Start () {
        base.Start();

        m_Enemy = m_Animal.GetComponent<Enemy3D>();
	}

    // Update is called once per frame
    public override void Update () {
        if (m_Enemy.GetState() == AnimalState.Faint) DrawText();
	}
    #endregion
}
