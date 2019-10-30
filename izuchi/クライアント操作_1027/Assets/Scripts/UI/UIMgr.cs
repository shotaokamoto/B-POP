using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;

public class UIMgr : MonoBehaviour
{
    public DropPlace dropPlace;

    [SerializeField] public Transform handTransform;
    [SerializeField] public CardController cardPrefab;   
    [SerializeField] public Text timeCountText;
    [SerializeField] public Text nowCostText;
    [SerializeField] public Text nowTurnText;
    [SerializeField] public Text nowScoreText;

    public bool isClientTurn;   // ターンフラグ
    public bool isNowClick;     // クリックフラグ
    public bool isCheckPos;     // 使用されている座標フラグ
    public bool isCheckRay;

    public int nowNum;          // 現在の手札カード数 
    public int dropNum;         // ドロップしたカード数
    public int drawNum;         // ドローするカード数

    public float timeCount;     // 時間管理
    public int nowCost;         // 現在のコスト
    public int nowTurn;         // 現在のターン
    public int nowScore;        // 現在のスコア

    // シングルトン化(どこからでもアクセス可能)
    public static UIMgr instance;

    // 仮デッキ
    List<int> kariDeck = new List<int>() {
        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
    };

    // デッキ
    List<Model.CardDataProxy> Deck = new List<Model.CardDataProxy>();

    // クライアントが送るデータ
    public List<Model.ClientData> _UseCardData = new List<Model.ClientData>();
    public Model.ClientData useCardData;

    public GameObject Obj;
    public bool isDragEnd = false;

    private List<List<int>> _DrawDataList = new List<List<int>>();
    private GlobalParamenter _prm = new GlobalParamenter();
    public GameObject[][] _AllFieldObj;
    public GameObject Rock;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        // クライアントが送るデータの初期化
        useCardData = new Model.ClientData(null, 0, 0, new Vector2());
    }

    // 初期化処理
    void Start()
    {
        //_DrawDataList = Model.RecvCliantDataProxy._Instance.DrawDataList; // 岩描画での必要なデータ類を受け取る
        //PlayerMgr.instance.CheckUsePos(); // 岩が配置されている座標を取得

        // テスト
        List<int> testData = new List<int>();
        testData = new List<int>() { 0, 0 };
        _DrawDataList.Add(testData);
        testData = new List<int>() { 7, 7 };
        _DrawDataList.Add(testData);

        
        StartGame();

        //リスト新規作成
        _AllFieldObj = new GameObject[_prm.FieldSize.y][];
        for (int i = 0; i < _prm.FieldSize.y; i++)
        {
            _AllFieldObj[i] = new GameObject[(int)_prm.FieldSize.y];
        }

        //タイルをソート
        GameObject[] allFieldData = GameObject.FindGameObjectsWithTag("Stage");

        for (int i = 0; i < allFieldData.Length; i++)
        {
            _AllFieldObj[(int)allFieldData[i].GetComponent<Ctrl.TileData_Maneger>().planePos.y]
                        [(int)allFieldData[i].GetComponent<Ctrl.TileData_Maneger>().planePos.x] = allFieldData[i];
        }

        // 岩描画
        for (int i = 0; i < _DrawDataList.Count; i++)
        {
            CreateLock(_DrawDataList[i]);
        }


        // 右上が使われている(テスト)
        //Vector2 test = new Vector2(3, 3);
        //PlayerMgr.instance.CheckUsePosList.Add(test);
    }

    // ゲームスタート時の初期化処理
    void StartGame()
    {
        isClientTurn = true;
        isNowClick = false;
        isCheckPos = false;
        isCheckRay = false;
        nowNum = 0;
        dropNum = 0;
        drawNum = 0;
        nowCost = 3;   // マナコストの値
        nowTurn = 0;
        nowScore = 0;

        ShowNowScore();
        ShowNowTurn();
        ShowNowCost();
        SettingHand();
        TurnCalc();
    }

    // 現在のスコア表示
    void ShowNowScore()
    {
        nowScoreText.text = nowScore.ToString();
    }

    // 現在のターンの表示
    void ShowNowTurn()
    {
        nowTurnText.text = nowTurn.ToString();
    }

    // 制限時間の表示
    void ShowTimeCount()
    {
        timeCountText.text = timeCount.ToString();
    }

    // コストの表示
    void ShowNowCost()
    {
        nowCostText.text = nowCost.ToString();
    }

    // コストの消費
    public void ReduceNowCost(int cost, bool isCard)
    {
        if(isCard)
        {
            nowCost -= cost;
        }
        ShowNowCost();
    }

    //　初期手札の配布
    void SettingHand()
    {
        // カードを５枚配る
        for (int i = 0; i < 5; i++)
        {
            //GiveCardToHand(Deck, handTransform);
            GiveCardToHand(kariDeck, handTransform);
        }
    }
    
    // 手札の配布
    //void GiveCardToHand(List<Model.CardDataProxy> deck, Transform hand)
    //{
    //    if (deck.Count == 0)
    //    {
    //        return;
    //    }
    //    int cardID = deck[0];
    //    deck.RemoveAt(0);
    //    CreateCard(cardID, hand);
    //    nowNum++;
    //}

    void GiveCardToHand(List<int> Karideck, Transform hand)
    {
        if (Karideck.Count == 0)
        {
            return;
        }
        int cardID = Karideck[0];
        Karideck.RemoveAt(0);
        CreateCard(cardID, hand);
        nowNum++;
    }

    // カードの生成
    void CreateCard(int cardID, Transform hand)
    {
        // カードの生成とデータの受け渡し
        CardController card = Instantiate(cardPrefab, hand, false);
        card.Init(cardID); 
    }
    
    // ターン管理
    void TurnCalc()
    {
        //StopAllCoroutines();
        //StartCoroutine(CountDown());
        if (isClientTurn)
        {
            StopAllCoroutines();
            StartCoroutine(CountDown());
            ClientTurn();
        }
        else
        {
            ServerTurn();
        }
    }

    // カウントダウン
    IEnumerator CountDown()
    {
        
        timeCount = 10.0f; // 制限時間の値
        ShowTimeCount();

        while (timeCount > 0.0f)
        {
            
            yield return new WaitForSeconds(1.0f); // 1秒待機s
            timeCount--;
            ShowTimeCount();
        }

        if(isClientTurn)
            ChangeTurn();
    }

    // ターンチェンジ
    public void ChangeTurn()
    {
        isClientTurn = !isClientTurn;
        TurnCalc();
    }

    // クライアントのターン処理
    void ClientTurn()
    {
        Debug.Log("クライアントのターン----------------");

        // サーバから引いたカードのリストデータを受け取る
        //Deck = Model.RecvCliantDataProxy._Instance.DrawCardList;

        // 使用されている座標を取得
        //PlayerMgr.instance.CheckUsePos();

        // カードを配る
        //if (nowNum < 5) // 5枚以下
        {
            for (int i = 0; i < drawNum; i++)
            {
                //GiveCardToHand(Deck, handTransform);
                GiveCardToHand(kariDeck, handTransform);
            }
        }
        nowCost = 3;   // マナコストを戻す
        ShowNowCost();
        if (nowTurn < 10) // 最大ターン数
        {
            nowTurn++;      // ターンを増やす
        }

        // サーバから現在のターンデータを受け取る 
        //nowTurn = Model.RecvCliantDataProxy._Instance.NowTurn;

        ShowNowTurn();
    }

    // サーバのターン処理
    void ServerTurn()
    {
        Debug.Log("サーバのターン----------------------");

        // サーバに送った後に使用されている座標リストをクリア
        PlayerMgr.instance.CheckUsePosList.Clear();

        dropNum = dropPlace.DropCard;
        nowNum -= dropNum;
        drawNum = dropNum;
        // デッキのカードを追加
        for (int i = 0; i < drawNum; i++)
        {
            //Deck.Add();
        }

        Debug.Log("手札のカード数:" + nowNum);
        Debug.Log("ドロップしたカード数:" + dropNum);
        Debug.Log("ドローするカード数:" + drawNum);

       
    }

    // 岩描画
    void CreateLock(List<int> data)
    {
        Vector3 pos;
        pos = ConvertArrayToWorldPos(data[0], data[1]);
        pos.y += 0.5f;

        //オブジェクトを生成
        Instantiate(Rock, pos, transform.rotation, _AllFieldObj[data[0]][data[1]].transform);
    }

    #region　補助関数

    /// <summary>
    /// 配列からワールド座標に変換します
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Vector3 ConvertArrayToWorldPos(int x, int y)
    {
        Vector3 outpos = new Vector3();

        outpos = _AllFieldObj[y][x].transform.position;

        return outpos;
    }


    #endregion

    // 更新処理
    void Update()
    {
        // マウスボタンを押していたらフラグを立てる
        if (Input.GetMouseButton(0))
        {
            isNowClick = true;
        }
        else
        {
            isNowClick = false;
        }
    }
    
}