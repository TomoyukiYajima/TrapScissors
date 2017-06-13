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
    private int m_Speed = 10;           // 追加速度
    [SerializeField]
    private string m_TextName;          // テキストフォルダの名前
    [SerializeField]
    private GameObject m_ClearChackBox; // クリアチェックボックス
    [SerializeField]
    private GameObject m_PressBottun;   // プリーズボタン

    private int m_DrawTextNumber = 0;   // 現在表示しているテキスト番号
    private Text m_Text;                // UIテキスト

    private float m_AddTextTime = 0.0f; // 文字追加時間
    private int m_Count = 0;            // 現在の表示カウント
    private string m_InitTextName;
    private bool m_IsDrawEnd = false;

    private List<string> m_DrawTexts = 
        new List<string>();             // 表示する文字列リスト

	// Use this for initialization
	void Start () {
        m_Text = GetComponent<Text>();
        m_Text.text = "";

        m_InitTextName = m_TextName;

        ReadTextFile();
	}
	
	// Update is called once per frame
	void Update () {
        // 決定ボタンが押されたら、次のテキストの表示
        if (Input.GetButtonDown("Submit"))
        {
            // 表示中にボタンが押されたら、すべて表示する
            if(!(m_Count >= m_DrawTexts[m_DrawTextNumber].Length))
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
            // 表示するテキストがなければ、終了
            if (m_DrawTextNumber > m_DrawTexts.Count - 1)
            {
                m_DrawTextNumber = 0;
                m_IsDrawEnd = true;
                this.transform.parent.gameObject.SetActive(false);
                GameManager.gameManager.GameStateSet(GameManager.GameState.PLAY);
                m_ClearChackBox.SetActive(true);
                // テキストを空にする
                //m_Text.text = "";
                //return;
            }
            // テキストを空にする
            m_Text.text = "";
        }

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

    public void NextText(string text)
    {
        m_TextName = m_InitTextName + text;
        // 配列の初期化
        m_DrawTexts.Clear();
        // 再読み込み
        ReadTextFile();
    }

    public bool IsDrawEnd() { return m_IsDrawEnd; }

    // 改行コード処理
    private string SetDefaultText()
    {
        return "C#あ\n";
    }

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(TutorialText), true)]
    [CanEditMultipleObjects]
    public class TutorialTextEditor : Editor
    {
        //SerializedProperty TextBox;
        SerializedProperty Speed;
        SerializedProperty TextName;
        SerializedProperty ClearChackBox;
        SerializedProperty PressBottun;

        public void OnEnable()
        {
            //TextBox = serializedObject.FindProperty("m_TextBox");
            Speed = serializedObject.FindProperty("m_Speed");
            TextName = serializedObject.FindProperty("m_TextName");
            ClearChackBox = serializedObject.FindProperty("m_ClearChackBox");
            PressBottun = serializedObject.FindProperty("m_PressBottun");
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
            ClearChackBox.objectReferenceValue = EditorGUILayout.ObjectField("クリア条件表示", tutorialText.m_ClearChackBox, typeof(GameObject), true);
            //TextBox.objectReferenceValue = EditorGUILayout.ObjectField("テキストボックス", tutorialText.m, typeof(GameObject), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
