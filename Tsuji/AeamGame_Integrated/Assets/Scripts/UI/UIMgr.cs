using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int nowNum;          // 現在の手札カード数 
    public int dropNum;         // ドロップしたカード数
    public int drawNum;         // ドローするカード数

    public float timeCount;       // 時間管理
    public int nowCost;         // 現在のコスト
    public int nowTurn;         // 現在のターン
    public int nowScore;        // 現在のスコア

    // シングルトン化(どこからでもアクセス可能)
    public static UIMgr instance;

    // 仮デッキ
    List<int> Deck = new List<int>() {
        1, 2, 3, 4, 5 ,6,
        1, 2, 3, 4, 5 ,6,
        1, 2, 3, 4, 5 ,6,
        1, 2, 3, 4, 5 ,6,
        1, 2, 3, 4, 5 ,6,
    };

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // 初期化処理
    void Start()
    {
        StartGame();
    }

    // ゲームスタート時の初期化処理
    void StartGame()
    {
        isClientTurn = true;
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
            GiveCardToHand(Deck, handTransform);
        }
    }
    
    // 手札の配布
    void GiveCardToHand(List<int> deck, Transform hand)
    {
        if (deck.Count == 0)
        {
            return;
        }
        int cardID = deck[0];
        deck.RemoveAt(0);
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
        StopAllCoroutines();
        StartCoroutine(CountDown());
        if (isClientTurn)
        {
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
        timeCount = 20.0f; // 制限時間の値
        ShowTimeCount();

        while (timeCount > 0.0f)
        {
            yield return new WaitForSeconds(1.0f); // 1秒待機s
            timeCount--;
            ShowTimeCount();
        }
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
        // カードを配る
        //if (nowNum < 5) // 5枚以下
        {
            for (int i = 0; i < drawNum; i++)
            {
                GiveCardToHand(Deck, handTransform);
            }
        }
        nowCost = 3;   // マナコストを戻す
        ShowNowCost();
        // 最大ターン数
        if (nowTurn < 10)
        {
            nowTurn++;      // ターンを増やす
        }
        ShowNowTurn();
    }

    // サーバのターン処理
    void ServerTurn()
    {
        Debug.Log("サーバのターン----------------------");
        dropNum = dropPlace.DropCard;
        nowNum -= dropNum;
        drawNum = dropNum;
        // デッキのカードを追加
        for (int i = 0; i < drawNum; i++)
        {
            //Deck.Add(1);
        }

        Debug.Log("手札のカード数:" + nowNum);
        Debug.Log("ドロップしたカード数:" + dropNum);
        Debug.Log("ドローするカード数:" + drawNum);
    }

}