using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MeatScore : MonoBehaviour {

    private int m_Score;    // スコア

    public Text m_Text;

	// Use this for initialization
	void Start () {
        m_Score = 0;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void AddScore(int score)
    {
        //m_Score += score;

        // テキストに入れる
        var text = GameManager.gameManager.PointCheck().ToString();
        m_Text.text = text;
    }
}
