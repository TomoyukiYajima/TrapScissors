﻿using UnityEngine;
using System.Collections;

public class TutorialMediator : MonoBehaviour {

    public TutorialStage m_TutorialStage;
    public TutorialText m_TutorialText;

    private int m_ControllCount = 0;            // コントロールカウント

    private static TutorialMediator instance;   // インスタンス

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // インスタンスの取得を行います
    public static TutorialMediator GetInstance()
    {
        // インスタンスが無かった生成する
        if (instance == null)
        {
            instance = (TutorialMediator)FindObjectOfType(typeof(TutorialMediator));
            //// インスタンスが無かった場合、ログの表示
            //if (instance == null) Debug.LogError("TutorialMediator Instance Error");
        }

        return instance;
    }

    // コントロールカウントの取得・変更を行います
    public int ControllCount
    {
        set { m_ControllCount = value; }
        get { return this.m_ControllCount; }
    }

    // コントロールカウントの加算を行います
    public void AddControllCount()
    {
        m_ControllCount++;
    }

    public void NextDrawText(string text)
    {
        // 一旦停止する
        GameManager.gameManager.GameStateSet(GameManager.GameState.PAUSE);
        // テキストボックスの表示
        m_TutorialText.NextText(text);
        m_TutorialStage.DrawTextBox();
    }

    public bool IsTextDrawEnd() { return m_TutorialText.IsDrawEnd(); }
}
