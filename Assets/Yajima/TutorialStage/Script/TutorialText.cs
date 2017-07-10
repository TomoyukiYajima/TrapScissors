using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TutorialText : MonoBehaviour {

    [SerializeField]
    private int m_Speed = 10;                   // 追加速度
    [SerializeField]
    private string m_TextName;                  // テキストフォルダの名前
    [SerializeField]
    private GameObject[] m_ClearChackBoxes;     // クリアチェックボックス
    [SerializeField]
    private GameObject m_PressBottun;           // プリーズボタン
    [SerializeField]
    private TutorialDrawTexture m_DrawTexture;  // チュートリアル画像表示

    private int m_DrawTextNumber = 0;           // 現在表示しているテキスト番号
    private int m_DrawTextFileNumber = 1;       // 表示しているテキストデータ番号
    private Text m_Text;                        // UIテキスト

    private float m_AddTextTime = 0.0f;         // 文字追加時間
    private int m_Count = 0;                    // 現在の表示カウント
    private int m_CheckBoxNumber = 0;           // アクティブ状態にするチェックボックス番号
    private string m_InitTextName;              // 初期テキストファイルの名前
    private bool m_IsDrawEnd = false;           // テキスト表示が終了したか
    private GameManager.GameState m_GameState =
        GameManager.GameState.START;            // テキスト表示後のゲームの状態

    private List<string> m_DrawTexts = 
        new List<string>();                     // 表示する文字列リスト

    private TutorialTexture m_Texture;          // 表示するテクスチャ
    //private List<GameObject> m_DrawSprites =
    //    new List<GameObject>();                 // 表示画像配列
    //private Dic

    // Use this for initialization
    void Start () {
        m_Text = GetComponent<Text>();
        m_Text.text = "";
        // 初期のテキストファイルの名前を入れる
        m_InitTextName = m_TextName;
        // テキストファイルの読み込み
        ReadTextFile();
        // 表示画像があれば、画像の表示
        m_DrawTexture.SetTexture(m_DrawTextFileNumber, m_DrawTextNumber + 1);
    }
	
	// Update is called once per frame
	void Update () {
        // 決定ボタンが押されたら、次のテキストの表示
        if (Input.GetButtonDown("Submit")) DrawText();

        m_AddTextTime += Time.deltaTime * m_Speed;
        // 追加時間に達したら、表示文字を追加する
        if ((int)m_AddTextTime == m_Count - 1 || m_Count >= m_DrawTexts[m_DrawTextNumber].Length) return;
        m_Text.text += m_DrawTexts[m_DrawTextNumber].Substring(m_Count, 1);
        m_Count++;
        // プリーズボタンの表示
        if (m_Count >= m_DrawTexts[m_DrawTextNumber].Length)
            m_PressBottun.SetActive(true);
    }

    // テキストファイルの読み込みを行います
    private void ReadTextFile()
    {
        m_IsDrawEnd = false;
        var text = "";
        // テキストを空にする
        m_Count = 0;
        m_Text.text = "";
        // FileReadTest.txtファイルを読み込む
        // Application.dataPath Assetファイルまでのパスの取得
        var path = Application.dataPath;
        if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            var str = Application.productName + "_Data";
            path = path.Substring(0, path.Length - str.Length);
            //path = "Text";
        }
        FileInfo file = new FileInfo(path + "/Text/" + m_TextName + ".txt");
        try
        {
            // 一行毎読み込み
            // テキストフォルダをUnicodeに設定する
            //// 既定の状態だと、日本語は文字化けしてしまう
            //// ->エンコードの設定をデフォルトにする
            //var enc = Encoding.Default;
            using (StreamReader sr = new StreamReader(file.OpenRead(), Encoding.Unicode))
            {
                text = sr.ReadToEnd();
                // テキストリストに追加
                AddTextList(text);
            }
        }
        catch (Exception e)
        {
            // 改行コード
            text += SetDefaultText();
            m_Text.text = path;
        }

        // 読み込んだテキストを表示する
        //m_Text.text = m_Strings[0];
    }

    // テキストリストへの追加処理
    private void AddTextList(string text)
    {
        string str = "";
        string addStr = "";
        int count = 0;
        for (int i = 0; i != text.Length; i++)
        {
            str += text.Substring(i, 1);
            // 指定文字列が含まれていたらリストに追加
            // m_Strings
            if (str.IndexOf("\r\n") >= 0)
            {
                //m_Strings.Add(str);
                count++;
                addStr += str;
                str = "";
                // 改行が2個あった場合は、テキストリストに追加
                if (count >= 2)
                {
                    m_DrawTexts.Add(addStr);
                    count = 0;
                    addStr = "";
                }
            }
            // 最後の文字だった場合もリストに追加
            if (i == text.Length - 1)
            {
                addStr += str;
                m_DrawTexts.Add(addStr);
            }
        }
    }

    public void NextText(string text, int chackNumber, GameManager.GameState state)
    {
        m_TextName = m_InitTextName + text;
        m_CheckBoxNumber = chackNumber;
        m_DrawTextFileNumber++;
        // 表示画像があれば、画像の表示
        m_DrawTexture.SetTexture(m_DrawTextFileNumber, m_DrawTextNumber + 1);
        m_GameState = state;
        // 配列の初期化
        m_DrawTexts.Clear();
        // 再読み込み
        ReadTextFile();
    }

    private void DrawText()
    {
        // 表示中にボタンが押されたら、すべて表示する
        if (!(m_Count >= m_DrawTexts[m_DrawTextNumber].Length))
        {
            m_Text.text = m_DrawTexts[m_DrawTextNumber];
            m_Count = m_DrawTexts[m_DrawTextNumber].Length;
            m_PressBottun.SetActive(true);
            return;
        }

        // すべて表示していれば、次のテキストを表示
        m_DrawTextNumber++;
        m_AddTextTime = 0;
        m_Count = 0;
        m_PressBottun.SetActive(false);
        // テクスチャが表示されていれば、カウントを減らす
        if (m_Texture != null && m_Texture.gameObject.activeSelf) m_Texture.SubCount();
        m_DrawTexture.SetTexture(m_DrawTextFileNumber, m_DrawTextNumber + 1);
        SoundManger.Instance.PlaySE(0);
        // 表示するテキストがなければ、終了
        if (m_DrawTextNumber > m_DrawTexts.Count - 1)
        {
            m_DrawTextNumber = 0;
            m_IsDrawEnd = true;
            this.transform.parent.gameObject.SetActive(false);
            // 指定した状態に変更
            GameManager.gameManager.GameStateSet(m_GameState);
            // チェックボックスのアクティブ状態を変更
            var checkBox = m_ClearChackBoxes[m_CheckBoxNumber];
            if (!checkBox.activeSelf) checkBox.SetActive(true);
        }
        // テキストを空にする
        m_Text.text = "";
    }

    // テキスト表示が終了したかを返します
    public bool IsDrawEnd() { return m_IsDrawEnd; }

    // テキスト表示が終了したかを返します(テキスト番号の指定)
    public bool IsDrawEnd(int number) {
        // 指定したテキスト番号ならば、表示が完了したかを返す
        if (m_DrawTextFileNumber == number) return m_IsDrawEnd;
        // 指定したテキスト番号ではない
        return false;
    }

    // テキスト表示終了後のゲームの状態を設定します
    public void SetDrawEndGameState(GameManager.GameState state) { m_GameState = state; }

    // テクスチャを設定します
    public void SetTexture(TutorialTexture texture) { m_Texture = null; m_Texture = texture; }

    // 改行コード処理
    private string SetDefaultText() { return "C#あ\n"; }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(TutorialText), true)]
    [CanEditMultipleObjects]
    public class TutorialTextEditor : Editor
    {
        //SerializedProperty TextBox;
        SerializedProperty Speed;
        SerializedProperty TextName;
        SerializedProperty ClearChackBoxes;
        SerializedProperty PressBottun;
        SerializedProperty DrawTexture;

        public void OnEnable()
        {
            //TextBox = serializedObject.FindProperty("m_TextBox");
            Speed = serializedObject.FindProperty("m_Speed");
            TextName = serializedObject.FindProperty("m_TextName");
            ClearChackBoxes = serializedObject.FindProperty("m_ClearChackBoxes");
            PressBottun = serializedObject.FindProperty("m_PressBottun");
            DrawTexture = serializedObject.FindProperty("m_DrawTexture");
        }

        public override void OnInspectorGUI()
        {
            // 必ず書く
            serializedObject.Update();

            TutorialText tutorialText = target as TutorialText;

            EditorGUILayout.LabelField("〇テキスト表示の設定");
            // int
            Speed.intValue = EditorGUILayout.IntField("表示速度", tutorialText.m_Speed);
            // string
            TextName.stringValue = EditorGUILayout.TextField("表示するテキストファイル名", tutorialText.m_TextName);

            EditorGUILayout.Space();

            PressBottun.objectReferenceValue = EditorGUILayout.ObjectField("次のテキスト表示を知らせる画像", tutorialText.m_PressBottun, typeof(GameObject), true);
            DrawTexture.objectReferenceValue = EditorGUILayout.ObjectField("チュートリアル画像表示用オブジェクト", tutorialText.m_DrawTexture, typeof(TutorialDrawTexture), true);
            // ClearChackBox.objectReferenceValue = EditorGUILayout.ObjectField("クリア条件表示", tutorialText.m_ClearChackBox, typeof(GameObject), true);
            // 配列
            EditorGUILayout.PropertyField(ClearChackBoxes, new GUIContent("表示するチェックボックス"), true);
            //ClearChackBox.objectReferenceValue = EditorGUILayout.ObjectField("クリア条件表示", tutorialText.m_ClearChackBox, typeof(GameObject), true);
            //TextBox.objectReferenceValue = EditorGUILayout.ObjectField("テキストボックス", tutorialText.m, typeof(GameObject), true);

            // m_SpriteCounts

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
