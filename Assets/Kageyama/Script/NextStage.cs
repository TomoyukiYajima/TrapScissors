using UnityEngine;
using System.Collections;

public class NextStage : MonoBehaviour
{
    [SerializeField]
    private GameObject _NextStageImage; //シーン移動可能になった時に出す画像
    [SerializeField]
    private string _nextStage;  //次のシーンの名前
    [SerializeField]
    private float _waitTime;    //移動可能になるまでの時間
    private bool _sceneOutFlag; //移動可能かどうか
    // Use this for initialization
    void Start()
    {
        _sceneOutFlag = false;
        StartCoroutine(AnyButtonON());
    }

    void Update()
    {
        if(Input.anyKeyDown && _sceneOutFlag == true)
        {
            ResultManager.resultManager.ClearSetActive(false);
        }
    }

    //何のボタンを押しても移動できるようにする
    IEnumerator AnyButtonON()
    {
        yield return new WaitForSecondsRealtime(_waitTime);
        _NextStageImage.SetActive(true);
        //移動可能状態にする
        _sceneOutFlag = true;
        //どのボタンでも反応するようにする
        SceneManagerScript.sceneManager.AnyButtonOn(true, _nextStage);

    }
}
