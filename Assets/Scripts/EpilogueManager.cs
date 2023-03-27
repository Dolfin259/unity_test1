using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EpilogueManager : MonoBehaviour
{
   
   [SerializeField]
   private Text mainText;

   [SerializeField]
    private Text nameText;
    private string _text = 
    "「ーー森の奥の墓場を抜けると自分と瓜二つの”なにか”に襲われた」&「あれは自分の幻だったのだろうか,,,」&「辺りを見渡すと森は消え、　もといた場所に立っていた。彼は自分の幻に打ち勝ったのだ」&「誰に話しても信じてもらえるはずもない」&「とんだ目にあったと嘆いたが、男の目はかつての輝きを少しだけ取り戻していた」&「自分にはまだ　出来ることがあるかもしれない」&「ーー孤独な騎士は歩き始めた」&「GAMECLEAR」";

    [SerializeField]
    private float captionSpeed = 0.2f; //文字を送るスピード
    private const char SEPARATE_MAIN_START = '「';
    private const char SEPARATE_MAIN_END = '」';
    private const char SEPARATE_PAGE = '&';
    private Queue<string> _pageQueue;
    private Queue<char> _charQueue;

    [SerializeField]
    private GameObject nextPageIcon;
   
    //文を1文字ごとに区切り、キューに格納したものを返す
    private Queue<char> SeparateString(string str)
    {

        // 文字列をchar型の配列にする = 1文字ごとに区切る
        char[] chars = str.ToCharArray();
        Queue<char> charQueue = new Queue<char>();

        // foreach文で配列charsに格納された文字を全て取り出し
        // キューに加える
        foreach (char c in chars) charQueue.Enqueue(c);
        return charQueue;
    }

        //文字列を指定した区切り文字ごとに区切り、キューに格納したものを返す
    private Queue<string> SeparateString(string str, char sep)
    {
        string[] strs = str.Split(sep);
        Queue<string> queue = new Queue<string>();
        foreach (string l in strs) queue.Enqueue(l);
        return queue;
    }

    private void Init()
    {
        _pageQueue = SeparateString(_text, SEPARATE_PAGE);
        ShowNextPage();
    }
    //次のページを表示する
    private bool ShowNextPage()
    {
        if (_pageQueue.Count <= 0) return false;
            // オブジェクトの表示/非表示を設定する
            nextPageIcon.SetActive(false);
            ReadLine(_pageQueue.Dequeue());
            return true;
    }

    //1文字を出力する
    private bool OutputChar()
    {
        if (_charQueue.Count <= 0)
        {
            nextPageIcon.SetActive(true);
            return false;
        }
        mainText.text += _charQueue.Dequeue();
        return true;
    }

    private void OutputAllChar()
    {
        StopCoroutine(ShowChars(captionSpeed));
        while (OutputChar()) ;
        nextPageIcon.SetActive(true);
    }

    private void OnClick()
    {
        if (_charQueue.Count > 0) OutputAllChar();
        else
        {
            if (!ShowNextPage())
                // UnityエディタのPlayモードを終了する
                UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private IEnumerator ShowChars(float wait)
        {
        // OutputCharメソッドがfalseを返す(=キューが空になる)までループする
        while (OutputChar())
            // wait秒だけ待機
            yield return new WaitForSeconds(wait);
        // コルーチンを抜け出す
        yield break;
        }

    
    private void ReadLine(string text)
        {
            string[] ts = text.Split(SEPARATE_MAIN_START);
            string name = ts[0];
            string main = ts[1].Remove(ts[1].LastIndexOf(SEPARATE_MAIN_END));
            nameText.text = name;
            mainText.text = "";
            _charQueue = SeparateString(main);
            // コルーチンを呼び出す
            StartCoroutine(ShowChars(captionSpeed));
        }

    private void Start()
        {
            Init();
        }

    private void Update()
        {
            // 左(=0)クリックされたらOnClickメソッドを呼び出し
            if (Input.GetMouseButtonDown(0)) OnClick();
        }
}