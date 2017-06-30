﻿using UnityEngine;
using System.Collections;

public class OpenStage : MonoBehaviour
{
    //どのシーンまでクリアしたか保存する値
    private string _openStage = "OpenStage";

    void Awake()
    {
        //他に同じオブジェクトがあれば、自分を削除する
        if (GameObject.Find("OpenStage") != this.gameObject) Destroy(gameObject);
        //クリアしたステージが0以下もしくは数字ではなけらば、0を保存する
        if (PlayerPrefs.GetInt(_openStage) >= 0) PlayerPrefs.SetInt(_openStage, 0);
    }

    void Start()
    {
        //このオブジェクトはシーン移行時には削除しない
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// どのステージまで解放したか保存する
    /// </summary>
    /// <param name="number">保尊する整数</param>
    public void StageSet(int number)
    {
        if (number < 0) return;
        PlayerPrefs.SetInt(_openStage, number);
    }

    /// <summary>
    /// どこまで解放したか確認する
    /// </summary>
    /// <returns></returns>
    public int OpenStageCheck()
    {
        return PlayerPrefs.GetInt(_openStage);
    }
}
