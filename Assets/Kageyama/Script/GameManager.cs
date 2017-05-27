using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        START,
        PLAY,
        END,
        PAUSE
    }

    protected static GameManager _gameManager;

    #region ステージの環境設定
    //ゲームの状態
    [SerializeField]
    private GameState _gameState;
    //トラップの最大数
    [SerializeField]
    private int _trapNumber;
    [SerializeField]
    private float _clampX_max;
    [SerializeField]
    private float _clampX_min;
    [SerializeField]
    private float _clampZ_max;
    [SerializeField]
    private float _clampZ_min;
    #endregion
    #region 時間設定
    //制限時間があるかどうか
    [SerializeField]
    private bool _timeCheck;
    //ゲームの時間
    [SerializeField]
    private float _gameTime;
    //現在の時間
    private float _timeCount;
    #endregion
    #region ゲーム進行に関する値
    //現在のポイント
    private int _point;
    //何番目に何の動物を捕まえたか
    [SerializeField]
    private List<int> _getAnimal = new List<int>();
    #endregion
    #region その他
    //ゲームを止めた時に出すUI
    [SerializeField]
    private GameObject _pauseUI;
    public int _foodCount;
    private int _huntcount;
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
        else if(_gameState == GameState.PAUSE)
        {
            GameStop();
        }
    }

    /// <summary>
    /// ゲームプレイ中にする処理
    /// </summary>
    void GamePlay()
    {
        //ゲームを止める
        if (Input.GetButtonDown("Pause"))
        {
            SceneManagerScript.sceneManager.FadeBlack();
            _gameState = GameState.PAUSE;
            _pauseUI.SetActive(true);
            _pauseUI.transform.FindChild("Restart").gameObject.GetComponent<Button>().Select();
        }

        //制限時間を設けない、または制限時間が0秒以下ならば時間を計測しない
        if (_timeCheck == true && _gameTime > 0)
        {
            _timeCount += Time.deltaTime;
            //if (_timeCount >= _gameTime)
            //{
            //    _gameState = GameState.END;
            //    SceneManagerScript.sceneManager.FadeBlack();
            //}
        }
    }

    void GameStop()
    {
        //ゲームを再開する
        if (Input.GetButtonDown("Pause"))
        {
            Restart();
        }
    }

    /// <summary>
    /// ゲームを再開する
    /// </summary>
    public void Restart()
    {
        _pauseUI.SetActive(false);
        SceneManagerScript.sceneManager.FadeWhite();
        _gameState = GameState.PLAY;
    }

    /// <summary>
    /// ゲームを止めているときに出すUIを非表示にする
    /// </summary>
    public void PauseUIFalse(GameObject falseObje)
    {
        falseObje.SetActive(false);
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

    /// <summary>
    /// ステージに置かれている餌の数を足す
    /// </summary>
    public void FoodCountAdd()
    {
        _foodCount++;
    }

    /// <summary>
    /// ステージに置かれている餌の数を減らす
    /// </summary>
    public void FoodCountSub()
    {
        _foodCount--;
    }

    /// <summary>
    /// ステージに置かれている餌の数
    /// </summary>
    /// <returns></returns>
    public int FoodCountCheck()
    {
        return _foodCount;
    }

    /// <summary>
    /// 移動できるX座標の最大の値
    /// </summary>
    /// <returns></returns>
    public float ClampX_MAX()
    {
        return _clampX_max;
    }
    /// <summary>
    /// 移動できるX座標の最小の値
    /// </summary>
    /// <returns></returns>
    public float ClampX_MIN()
    {
        return _clampX_min;
    }

    /// <summary>
    /// 移動できるZ座標の最大の値
    /// </summary>
    /// <returns></returns>
    public float ClampZ_MAX()
    {
        return _clampZ_max;
    }

    /// <summary>
    /// 移動できるZ座標の最小の値
    /// </summary>
    /// <returns></returns>
    public float ClampZ_MIN()
    {
        return _clampZ_min;
    }

    public GameState GameStateCheck()
    {
        return _gameState;
    }

    public void GameStateSet(GameState state)
    {
        _gameState = state;
    }

    public void HuntCountAdd()
    {
        _huntcount++;
    }

    public int HuntCountCheck()
    {
        _huntcount = 10;
        return _huntcount;
    }

    public int GameTime()
    {
        return (int)_timeCount;
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
        SerializedProperty PauseUI;
        SerializedProperty ClampX_MAX;
        SerializedProperty ClampX_MIN;
        SerializedProperty ClampZ_MAX;
        SerializedProperty ClampZ_MIN;

        public void OnEnable()
        {
            State = serializedObject.FindProperty("_gameState");
            TrapNumber = serializedObject.FindProperty("_trapNumber");
            TimeCheck = serializedObject.FindProperty("_timeCheck");
            GameTime = serializedObject.FindProperty("_gameTime");
            GetAnimal = serializedObject.FindProperty("_getAnimal");
            PauseUI = serializedObject.FindProperty("_pauseUI");
            ClampX_MAX = serializedObject.FindProperty("_clampX_max");
            ClampX_MIN = serializedObject.FindProperty("_clampX_min");
            ClampZ_MAX = serializedObject.FindProperty("_clampZ_max");
            ClampZ_MIN = serializedObject.FindProperty("_clampZ_min");
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
            EditorGUILayout.Space();
            PauseUI.objectReferenceValue = EditorGUILayout.ObjectField("ポーズ中に出すオブジェクト", manager._pauseUI, typeof(GameObject), true) as GameObject;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("X座標の移動可能範囲( MIN / MAX )");
            EditorGUILayout.BeginHorizontal();
            ClampX_MIN.floatValue = EditorGUILayout.FloatField(manager._clampX_min, GUILayout.Width(32));
            ClampX_MAX.floatValue = EditorGUILayout.FloatField(manager._clampX_max, GUILayout.Width(32));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Z座標の移動可能範囲( MIN / MAX )");
            EditorGUILayout.BeginHorizontal();
            ClampZ_MIN.floatValue = EditorGUILayout.FloatField(manager._clampZ_min, GUILayout.Width(32));
            ClampZ_MAX.floatValue = EditorGUILayout.FloatField(manager._clampZ_max, GUILayout.Width(32));
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
}
