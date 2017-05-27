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
    private bool _endPopUp;     //ポップアップが終わったら次の演出をするかかどうか
    [SerializeField]
    private bool _gameClear;     //ゲームクリアかどうか

    private float _drawTime;    //文字を消すまでの時間
    private bool _end;          //演出が終わったかどうか

    // Use this for initialization
    void Start ()
    {
        _end = false;
        StartCoroutine(PopUp());
        if(_gameClear == false)
        {
            SceneManagerScript.sceneManager.Black();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //終わったら次の演出に移動しない場合は時間計測をしない
        if (_endPopUp == false) return;
        _drawTime += Time.deltaTime;
        if(_drawTime >= (_delayTime +_sizeTime + _waitTime * _word.Length) + 2.0f && _end == false)
        {
            _end = true;
            //ゲームクリアの場合
            if (_gameClear == true)
            {
                ResultManager.resultManager.ClearPopUpActiveEnd();
                return;
            }

            else if(_gameClear == false)
            {
                LeanTween.move(this.GetComponent<RectTransform>(), new Vector2(0, 120), 0.5f)
                    .setOnComplete(()=> {
                        StartCoroutine(GameOverActiveWait());
                    });
            }
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

    IEnumerator GameOverActiveWait()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        ResultManager.resultManager.GameOverPopActiveEnd();
    }
}
