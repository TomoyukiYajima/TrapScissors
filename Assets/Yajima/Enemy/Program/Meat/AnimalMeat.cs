using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimalMeat : MonoBehaviour {

    public Camera m_MainCamera;                     // メインカメラ
    public Sprite[] m_Sprites;                      // スプライト配列
    public float m_Speed = 10.0f;                   // 移動速度

    protected string m_MoveUIName = "MeatCount";    // 目的地のUIの名前
    protected GameObject m_UI;                      // 目的地のUIオブジェクト

    private int m_MyNumber;                         // UIの番号                         
    private MeatState m_State = MeatState.NULL;     // 状態
    private Image m_Image;                          // イメージ

    private enum MeatState
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

        if (m_Image == null) m_Image = gameObject.GetComponent<Image>();

        if (m_State == MeatState.NULL) SetMeat(MeatNumber.SMALL_NUMBER);
    }
	
	// Update is called once per frame
	void Update () {
        //m_UI
        // 相手の場所が変わることがあるので、毎回移動方向の更新を行う
        var dir = m_UI.transform.position - this.transform.position;
        this.transform.position += m_Speed * dir.normalized * (Time.deltaTime * 60.0f);
        // 相手との距離が移動速度未満でなかったら返す
        var length = Vector3.Distance(m_UI.transform.position, this.transform.position);
        if (length > m_Speed) return;

        // 消滅処理
        // スコアの追加
        GameManager.gameManager.PointAdd(1);
        var score = m_UI.transform.parent.GetComponent<FoodUIMove>();
        if (score != null) score.FoodCountAdd(m_MyNumber);
        // 自身の削除
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
        else SetMeat(MeatState.LARGE_STATE, "Large", 2);
        // 目的地の決定
        var ui = GameObject.Find(m_MoveUIName);
        if (ui == null) return;
        m_UI = ui;
        var frame = m_UI.transform.parent;
        //if (frame != null) this.transform.parent = frame;
        // UI の場合は、上の処理だと警告文が表示される 
        if (frame != null) this.transform.SetParent(frame);
    }

    private void SetMeat(MeatState state, string meatName, int number)
    {
        m_State = state;
        if (number == 1) m_MoveUIName = "SmallMeatCount";
        else m_MoveUIName = "Food (2)";
        // スプライトの変更
        if (m_Image == null) m_Image = gameObject.GetComponent<Image>();
        var sprite = m_Sprites[number - 1];
        if (sprite != null) m_Image.sprite = sprite;
        m_MyNumber = number;
    }
}
