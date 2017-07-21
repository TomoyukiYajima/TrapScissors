using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SetMovie : MonoBehaviour
{
    public VideoPlayer _videoPlayer;
    [SerializeField]
    private string _nextStage;
    [SerializeField]
    private float _time;

    void Update()
    {
        _time += Time.deltaTime;

        if ((int)_time == 5) SoundManger.Instance.PlayBGM(3); //タイトルロゴ表示後
        else if ((int)_time == 84) SceneMovement();                     //PVが終了後にシーン移動

        if (Input.GetButtonDown("Trap") || Input.GetButtonDown("Whistle") ||//Xボタン,Aボタン
             Input.GetButtonDown("Food") || Input.GetButtonDown("Pause"))   //Yボタン,Startボタン 
        {
            SceneMovement();
        }
        //再生中でなければシーン移動
        if (!_videoPlayer.isPlaying) SceneMovement();
    }
    void SceneMovement()
    {
        _videoPlayer.Stop();
        SceneManagerScript.sceneManager.FadeOut(_nextStage);
    }
}
