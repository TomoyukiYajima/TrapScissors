using UnityEngine;
using System.Collections;
//using System

public class AnimalMeat : MonoBehaviour {

    public Camera m_MainCamera;                         // メインカメラ
    //public Transform m_Obj;
    protected string m_MoveUIName = "MeatCount";        // 目的地のUIの名前
    protected GameObject m_UI;                          // 目的地のUIオブジェクト
    private int m_MyNumber;                             // UIの番号                         
    private MeatState m_State = MeatState.NULL;         // 状態

    //private GameManager m_GameManager = null;           // ゲームマネージャ

    public enum MeatState
    {
        NULL,
        SMALL_STATE,
        LARGE_STATE
    }

    public enum MeatNumber
    {
        SMALL_NUMBER = 0,
        LARGE_NUMBER = 1
    }

    // Use this for initialization
    void Start () {
        // メインカメラが設定されていなかった場合
        if (m_MainCamera == null)
        {
            var obj = GameObject.Find("Main Camera");
            if (obj == null) return;
            var camera = obj.GetComponent<Camera>();
            m_MainCamera = camera;
        }

        //// ゲームマネージャの取得
        //if (m_GameManager == null)
        //{
        //    var obj = GameObject.Find("GameManager");
        //    if (obj == null) return;
        //    var manager = obj.GetComponent<GameManager>();
        //    if (manager == null) return;
        //    m_GameManager = manager;
        //}

        if (m_State == MeatState.NULL) SetMeat(MeatNumber.SMALL_NUMBER);
    }
	
	// Update is called once per frame
	void Update () {
        //m_UI
        var speed = 4.0f;
        var dir = m_UI.transform.position - this.transform.position;
        this.transform.position += speed * dir.normalized * (Time.deltaTime * 60.0f);
        var length = Vector3.Distance(m_UI.transform.position, this.transform.position);
        if (length > speed) return;

        // 消滅処理
        // スコアの追加
        GameManager.gameManager.PointAdd(1);

        var score = m_UI.transform.parent.GetComponent<FoodUIMove>();
        if (score != null) score.FoodCountAdd(m_MyNumber);

        //if (m_GameManager != null) m_GameManager.PointAdd(1);
        //m_GameManager.PointAdd(1);
        Destroy(gameObject);
    }

    // 3Dオブジェクトの位置を設定します
    public void SetObjPosition(Vector3 position, Camera camera)
    {
        // オブジェクトのあった位置に移動
        var screenPos = camera.WorldToScreenPoint(position);
        gameObject.transform.position = screenPos;
    }

    public void SetMeat(MeatNumber num)
    {
        // 番号で状態の設定を行う
        if (num == MeatNumber.SMALL_NUMBER) SetMeat(MeatState.SMALL_STATE, "Small", 1);
        else SetMeat(MeatState.LARGE_STATE, "Large", 0);
        //// 位置の設定
        //SetObjPosition(position);
        // 目的地の決定
        var ui = GameObject.Find(m_MoveUIName);
        if (ui == null) return;
        m_UI = ui;
        var frame = m_UI.transform.parent;
        if (frame != null) this.transform.parent = frame;
    }

    private void SetMeat(MeatState state, string meatName, int number)
    {
        m_State = state;
        m_MoveUIName = meatName + m_MoveUIName;
        m_MyNumber = number;
    }
}
