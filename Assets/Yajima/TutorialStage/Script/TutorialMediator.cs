using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialMediator : MonoBehaviour {

    public TutorialStage m_TutorialStage;       // チュートリアルステージ
    public TutorialText m_TutorialText;         // チュートリアルテキスト
    public int m_StageNumber;                   // ステージ番号

    private GameObject m_Player;                // プレイヤー
    private Vector3 m_InitPlayerPosition;       // プレイヤーの初期位置
    private int m_ActionCount = 0;              // プレイヤーのアクションカウント
    private bool m_IsAction = false;            // プレイヤーのアクションができるか

    private bool m_Init = false;
    private float m_Timer = 0.0f;

    private Dictionary<int, int> m_Numbers =
       new Dictionary<int, int>();              // ステージ番号(ビット管理用)

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

        for (int i = 0; i != 5; ++i) m_Numbers[i + 1] = 1 << i;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Init) FadeInit();
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
    public void TutorialInit() { m_Init = true; }

    private void FadeInit()
    {
        // 一旦停止する
        //var state = GameManager.gameManager.GameStateCheck();
        //if (state == GameManager.GameState.END || state == GameManager.GameState.PAUSE) return;
        GameManager.gameManager.GameStateSet(GameManager.GameState.PAUSE);
        if (m_Timer == 0.0f) SceneManagerScript.sceneManager.Black(1.0f);
        if (m_Timer >= 1.0f)
        {
            // フェードインする
            SceneManagerScript.sceneManager.FadeWhite();
            GameManager.gameManager.GameStateSet(GameManager.GameState.PLAY);
            m_Player.transform.position = m_InitPlayerPosition;
            m_Timer = 0.0f;
            m_Init = false;
        }
        m_Timer += 1.0f / 60.0f;
    }

    // チュートリアルでのアクションが行えるかを返します
    // true ならアクションを行えます
    // 引数2　指定したチュートリアル番号(複数指定可能)
    public bool IsTutorialAction(bool isAction = true, params int[] numbers)
    {
        // 完全にアクションを行わないなら、falseを返す
        if (!isAction) return false;
        // アクションができる場所でなければ、falseを返す
        for (int i = 0; i != numbers.Length; ++i)
        {
            if (m_StageNumber == numbers[i] && !m_IsAction)
                return false;
        }
        // アクション制限をするチュートリアルではない
        return true;
    }

    public bool IsTutorialAction(int[] actionNumbers, params int[] numbers)
    {
        // 完全にアクションを行わないなら、falseを返す
        // !TutorialMediator.GetInstance().IsCheckAction(1, 2, 4)
        if (TutorialMediator.GetInstance().IsCheckAction(actionNumbers)) return false;

        //if (!isAction) return false;
        // アクションができる場所でなければ、falseを返す
        for (int i = 0; i != numbers.Length; ++i)
        {
            if (m_StageNumber == numbers[i] && !m_IsAction)
                return false;
        }
        // アクション制限をするチュートリアルではない
        return true;
    }

    public bool IsTutorialAction(int number)
    {
        // アクションができる場所でなければ、falseを返す
        if (m_StageNumber == number && !m_IsAction) return false;
        // アクション制限をするチュートリアルではない
        return true;
    }

    // チュートリアルでのアクションが行えるかの確認を行います
    public bool IsCheckAction(int[] numbers)
    {
        int num = 0;
        for (int i = 0; i != numbers.Length; ++i)
        {
            num = num | m_Numbers[numbers[i]];
        }
        // 0でなければ、true になる
        return (m_Numbers[m_StageNumber] & num) != 0;
    }

    // チュートリアルでのアクションが行えるかを返します
    // true ならアクションを行えます
    //public bool IsTutorialAction(int number, bool isAction = true)
    //{
    //    // 完全にアクションを行わないなら、falseを返す
    //    if (!isAction) return false;
    //    // アクションができる場所でなければ、falseを返す
    //    if (m_StageNumber == number && !m_IsAction) return false;
    //    //if (m_StageNumber == number && !m_IsAction) return true;
    //    return true;
    //}

    // チュートリアルアクションを行えるかを設定します
    public void SetTutorialAction(bool isAction) { m_IsAction = isAction; }

    public int GetChackBoxCount() { return m_TutorialText.GetChackBoxCount(); }

    // テキスト表示が終了したかを返します
    public bool IsTextDrawEnd() { return m_TutorialText.IsDrawEnd(); }

    // テキスト表示が終了したかを返します(テキスト番号の指定)
    public bool IsTextDrawEnd(int number) { return m_TutorialText.IsDrawEnd(number); }

    // テキスト表示終了後のゲームの状態を設定します
    public void SetDrawEndGameState(GameManager.GameState state) { m_TutorialText.SetDrawEndGameState(state); }

    // テクスチャを設定します
    public void SetTexture(TutorialTexture texture) { m_TutorialText.SetTexture(texture); }
}
