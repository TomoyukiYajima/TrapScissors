using UnityEngine;
using System.Collections;

public class SetTrapClearChacker : ClearChacker{

    [SerializeField]
    private GameObject m_TrapParent;        // トラバサミの親オブジェクト

    //private float m_Timer;                  // 経過時間

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        // トラバサミの親オブジェクトが無かったら捜します
        if (m_TrapParent == null) m_TrapParent = GameObject.Find("Traps");
    }

    // Update is called once per frame
    public override void Update () {
        // トラバサミが設置されていない・テキストを表示していたら返す
        //  m_Timer >= m_DrawTextTimer
        //if (m_TrapParent.transform.childCount == 0 || IsTextDrawTime()) return;

        if (m_TrapParent.transform.childCount == 0) return;

        // 時間の加算
        //m_Timer += Time.deltaTime;

        // (m_Timer < m_DrawTextTimer) return;
        //if (!IsTextDrawTime()) return;

        // テキストの表示
        DrawText();

        //TutorialMediator.GetInstance().NextDrawText(m_AddText);
        //m_Toggle.isOn = m_IsChangeToggle;
    }


}
