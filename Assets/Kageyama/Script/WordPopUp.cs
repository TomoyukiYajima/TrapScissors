using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordPopUp : MonoBehaviour
{
    public RectTransform[] _word;
    [SerializeField]
    private float _delayTime;   //アニメーションを始めるまでの時間
    [SerializeField]
    private float _sizeTime;    //大きくするのにかける時間
    [SerializeField]
    private float _waitTime;    //一つ一つのアニメーションを始めるために待つ時間
    [SerializeField]
    private bool _success;      //Successの文字かどうか

    private float _drawTime;    //文字を消すまでの時間

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(PopUp());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_success == false) return;
        _drawTime += Time.deltaTime;
        if(_drawTime >= (_delayTime +_sizeTime + _waitTime * _word.Length) + 2.0f)
        {
            this.transform.parent.gameObject.SetActive(false);
            SceneManagerScript.sceneManager.FadeBlack();
            return;
        }

    }

    IEnumerator PopUp()
    {
        yield return new WaitForSecondsRealtime(_delayTime);
        for (int i = 0; i < _word.Length; i++)
        {
            LeanTween.scale(_word[i], new Vector2(1, 1), _sizeTime);
            yield return new WaitForSecondsRealtime(_waitTime);
        }
    }
}
