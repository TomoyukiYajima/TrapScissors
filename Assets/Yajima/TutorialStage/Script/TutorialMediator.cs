using UnityEngine;
using System.Collections;

public class TutorialMediator : MonoBehaviour {

    public TutorialStage m_TutorialStage;       // チュートリアルステージ
    public TutorialText m_TutorialText;         // チュートリアルテキスト
    public int m_StageNumber;                   // ステージ番号

    private GameObject m_Player;                // プレイヤー
    private Vector3 m_InitPlayerPosition;       // プレイヤーの初期位置
    private int m_ActionCount = 0;              // プレイヤーのアクションカウント
    private bool m_IsAction = false;            // プレイヤーのアクションができるか

    private static TutorialMediator instance;   // 自身のインスタンス

    // Use this for initialization
    void Start () {
        // プレイヤーの初期位置を入れる
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            m_Player = player;
            m_InitPlayerPosition = player.transform.position;
        }

        ActionCount = m_StageNumber;
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
            // インスタンスが無かった場合、ログの表示
            if (instance == null) Debug.LogError("TutorialMediator Instance Error");
        }

        return instance;
    }

    // コントロールカウントの取得・変更を行います
    public int ActionCount
    {
        set { m_ActionCount = value; }
        get { return this.m_ActionCount; }
    }

    // コントロールカウントの加算を行います
    public void AddActionCount() { m_ActionCount++; }

    // 
    public void NextDrawText(string text, int chackNumber, GameManager.GameState state)
    {
        // 一旦停止する
        GameManager.gameManager.GameStateSet(GameManager.GameState.PAUSE);
        // テキストボックスの表示
        m_TutorialText.NextText("_" + text, chackNumber, state);
        m_TutorialStage.DrawTextBox();
    }

    // 
    public void TutorialInit()
    {
        // 一旦停止する
        //GameManager.gameManager.GameStateSet(GameManager.GameState.PAUSE);
        //SceneManagerScript.sceneManager.FadeBlack();

        // フェード中は移動させない
        m_Player.transform.position = m_InitPlayerPosition;
        // フェードインする
        SceneManagerScript.sceneManager.FadeWhite();
    }

    // チュートリアルアクションを行えるかを返します
    // 引数　指定したチュートリアル番号(複数指定可能)
    public bool IsTutorialAction(params int[] numbers)
    {
        for(int i = 0; i != numbers.Length; ++i)
        {
            // 同一番号であるならば、true
            var num = numbers[i];
            if (m_StageNumber == numbers[i] && !m_IsAction)
                return true;
        }
        // 該当するチュートリアルではない
        return false;
    }

    // チュートリアルアクションを行えるかを設定します
    public void SetTutorialAction(bool isAction) { m_IsAction = isAction; }

    // テキスト表示が終了したかを返します
    public bool IsTextDrawEnd() { return m_TutorialText.IsDrawEnd(); }

    // テキスト表示終了後のゲームの状態を設定します
    public void SetDrawEndGameState(GameManager.GameState state) { m_TutorialText.SetDrawEndGameState(state); }
}
