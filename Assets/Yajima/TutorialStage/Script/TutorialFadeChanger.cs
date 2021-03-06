﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFadeChanger : MonoBehaviour {

    [SerializeField]
    public GameObject[] m_ActiveAnimal;     // アクティブ状態の動物
    [SerializeField]
    public GameObject[] m_NotActiveAnimal;  // 非アクティブ状態の動物

    public GameObject m_Player;             // プレイヤー
    public int m_FadeNumber = 1;            // フェード番号

    private bool m_IsFadeOut = false;
    private bool m_IsFadeIn = false;
    private float m_Timer;

    private float m_ChangeTimer;

    private Vector3 m_InitPosition;         // プレイヤーの初期位置

	// Use this for initialization
	void Start () {
        // プレイヤー
        m_InitPosition = m_Player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (TutorialMediator.GetInstance().IsTextDrawEnd(m_FadeNumber)) Fade();

        if (m_IsFadeIn) m_ChangeTimer = Mathf.Min(m_ChangeTimer + Time.deltaTime, 1.0f);
    }

    private void Fade()
    {
        if (!m_IsFadeOut)
        {
            SceneManagerScript.sceneManager.FadeBlack(1.0f);
            m_IsFadeOut = true;
        }
        else
        {
            if (m_IsFadeIn) return;
            m_Timer += 1.0f / 30.0f;
            if (m_Timer < 1.0f) return;

            SceneManagerScript.sceneManager.FadeWhite();
            m_IsFadeIn = true;

            //プレイヤーを初期位置に戻す
            m_Player.transform.position = m_InitPosition;
            // えさの削除
            var food = GameObject.Find("Food(Clone)");
            GameObject.Destroy(food);
            // えさの追加
            var foodUI = GameObject.Find("FoodParent");
            var foodUIMove = foodUI.GetComponent<FoodUIMove>();
            foodUIMove.FoodCountAdd(0);
            foodUIMove.FoodCountSub(1);

            for (int i = 0; i != m_ActiveAnimal.Length; ++i)
            {
                m_ActiveAnimal[i].SetActive(false);
            }

            for (int i = 0; i != m_NotActiveAnimal.Length; ++i)
            {
                m_NotActiveAnimal[i].SetActive(true);
            }
        }
    }

    public bool IsFadeEnd()
    {
        return m_ChangeTimer >= 1.0f;
    }
}
