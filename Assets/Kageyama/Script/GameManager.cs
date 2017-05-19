﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        START,
        PLAY,
        END
    }

    protected static GameManager _gameManager;

    #region ステージの環境設定
    [SerializeField]
    private GameState _gameState;
    [SerializeField]
    private int _trapNumber;
    #endregion
    #region 時間設定
    [SerializeField]
    private bool _timeCheck;
    [SerializeField]
    private float _gameTime;
    private float _timeCount;
    #endregion
    #region ゲーム進行に関する値
    private int _point;
    [SerializeField]
    private List<int> _getAnimal = new List<int>();
    #endregion

    //どこでも参照可
    public static GameManager gameManager
    {
        get
        {
            if (_gameManager == null)
            {
                _gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
                if (_gameManager == null)
                {
                    Debug.LogError("SceneChange Instance Error");
                }
            }

            return _gameManager;
        }
    }

    void Start()
    {
        _timeCount = 0;
        _point = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            GetAnimalAdd(1);
        }
        //ゲーム開始時にする処理
        if (_gameState == GameState.START)
        {
        }

        //ゲームプレ中にする処理
        else if (_gameState == GameState.PLAY)
        {
            GamePlay();   
        }

        //ゲームが終わっているときにする処理
        else if (_gameState == GameState.END)
        {
        }
    }

    /// <summary>
    /// ゲームプレイ中にする処理
    /// </summary>
    void GamePlay()
    {
        //制限時間を設けない、または制限時間が0秒以下ならば時間を計測しない
        if (_timeCheck == true && _gameTime > 0)
        {
            _timeCount += Time.deltaTime;
            print(_timeCount);
            if (_timeCount >= _gameTime)
            {
                _gameState = GameState.END;
                SceneManagerScript.sceneManager.FadeBlack();
            }
        }
    }

    /// <summary>
    /// ポイントを増やす
    /// </summary>
    /// <param name="add">増やす値</param>
    public void PointAdd(int add)
    {
        _point += add;
    }

    /// <summary>
    /// ポイントを減らす
    /// </summary>
    /// <param name="sum">減らす値</param>
    public void PointSum(int sum)
    {
        _point -= sum;
    }

    /// <summary>
    /// 現在のポイントの値を確認する
    /// </summary>
    /// <returns></returns>
    public int PointCheck()
    {
        return _point;
    }

    /// <summary>
    /// 捕まえた動物を入れる
    /// </summary>
    /// <param name="animalnum">動物の番号</param>
    public void GetAnimalAdd(int animalnum)
    {
        _getAnimal.Add(animalnum);
    }

    /// <summary>
    /// 何の動物を捕まえたか調べる
    /// </summary>
    /// <param name="num">何番目に捕まえたか</param>
    /// <returns></returns>
    public int GetAnimalCheck(int num)
    {
        return _getAnimal[num];
    }

    /// <summary>
    /// トラップを置ける最大数を確認する
    /// </summary>
    /// <returns></returns>
    public int TrapNumber()
    {
        return _trapNumber;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameManager))]
    public class SceneManagerEditor : Editor
    {
        SerializedProperty State;
        SerializedProperty TrapNumber;
        SerializedProperty TimeCheck;
        SerializedProperty GameTime;
        SerializedProperty GetAnimal;

        public void OnEnable()
        {
            State = serializedObject.FindProperty("_gameState");
            TrapNumber = serializedObject.FindProperty("_trapNumber");
            TimeCheck = serializedObject.FindProperty("_timeCheck");
            GameTime = serializedObject.FindProperty("_gameTime");
            GetAnimal = serializedObject.FindProperty("_getAnimal");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GameManager manager = target as GameManager;

            EditorGUILayout.PropertyField(State, new GUIContent("ゲームの状況"));
            TrapNumber.intValue = EditorGUILayout.IntField("仕掛けられる罠の最大数", manager._trapNumber);
            TimeCheck.boolValue = EditorGUILayout.Toggle("制限時間をつける", manager._timeCheck);
            if (manager._timeCheck == true)
            {
                GameTime.floatValue = EditorGUILayout.FloatField("制限時間", manager._gameTime);
            }
            EditorGUILayout.PropertyField(GetAnimal, true);
            serializedObject.ApplyModifiedProperties();
            
        }
    }
#endif
}
