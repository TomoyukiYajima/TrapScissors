using UnityEngine;
using System.Collections;

public class TutorialActionChecker : MonoBehaviour
{
    public GameObject m_Obj;            // パーティクルオブジェクト

    private ParticleSystem m_Particle;  // パーティクルシステム

    // Use this for initialization
    void Start()
    {
        // パーティクルオブジェクトが空だった場合は、子オブジェクトから探す
        if (m_Obj == null) m_Obj = this.transform.Find("TutorialPoint").gameObject;
        m_Particle = m_Obj.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag != "Player") return;

        TutorialMediator.GetInstance().SetTutorialAction(true);
        // パーティクルを停止させる
        m_Particle.Stop(true);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.tag != "Player") return;
        TutorialMediator.GetInstance().SetTutorialAction(false);
    }
}
