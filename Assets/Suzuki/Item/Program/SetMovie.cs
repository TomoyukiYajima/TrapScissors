using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class SetMovie : MonoBehaviour
{
    public VideoPlayer _videoPlayer;
    [SerializeField]
    private string _nextStage;
    [SerializeField]
    private float _waitTime;
    void Start()
    {
        _videoPlayer.Play();
    }
    void Update()       
    {//ボタンを押して終了か、再生が終わって終了
        if (   Input.GetButtonDown("Trap") || Input.GetButtonDown("Whistle")//Xボタン,Aボタン
            || Input.GetButtonDown("Food") || Input.GetButtonDown("Pause")) //Yボタン,Startボタン
        {
            _videoPlayer.Stop();
            AnyButtonON();
        }       

        if (!_videoPlayer.isPlaying) //再生中でなければシーン移動
        {
            _videoPlayer.Stop();
            AnyButtonON();
        }
    }
    IEnumerator AnyButtonON()
    {
        yield return new WaitForSecondsRealtime(_waitTime);
        SceneManagerScript.sceneManager.AnyButtonOn(true, _nextStage);

    }
}
