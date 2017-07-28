using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiscoverMark : MonoBehaviour {

    public GameObject m_MainCamera;     // メインカメラ

    private Vector3 m_InitPosition;     // 初期座標
    private Image m_Image;              // イメージスクリプト

	// Use this for initialization
	void Start () {
        m_InitPosition = this.transform.localPosition;
        // メインカメラの設定
        if(m_MainCamera == null)
        {
            var camera = GameObject.Find("Main Camera");
            if (camera != null) m_MainCamera = camera;
        }
        // イメージの取得
        m_Image = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        //ビルボード
        Vector3 p = m_MainCamera.transform.localPosition;
        this.transform.parent.LookAt(p);
    }

    // 感嘆符の表示
    public void ExclamationMark()
    {
        m_Image.enabled = true;
        // ローカル座標との違いで、思うように動かない？
        //LeanTween.move(gameObject, this.transform.position + Vector3.up * 1.5f, 1.0f)
        //.setEase(LeanTweenType.easeOutExpo);
        //.setOnComplete(() => { this.transform.localPosition = m_InitPosition; m_Image.enabled = false; });

        // リーンツインで値の設定を行う
        LeanTween.value(gameObject, -2.0f, 0.0f, 1.0f)
        .setOnUpdate((float val) => { this.transform.localPosition = Vector3.up * val; })
        .setEase(LeanTweenType.easeOutExpo)
        .setOnComplete(() => { this.transform.localPosition = m_InitPosition; m_Image.enabled = false; });
    }

    // 疑問符の表示
    public void QuestionMark()
    {

    }
}
