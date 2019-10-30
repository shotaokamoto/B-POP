using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カードデータそのものとその処理
public class CardModel
{
    public Sprite CardSprite;   // カードの画像

    // ホストからもらう情報
    public int _CardNumber;     //カード番号
    public string _CardName;    //カードの名前
    public string _CardText;    //カードの説明、効果文
    public int _CardCost;       //カードのコスト
    public int _CardScore;      //カードのスコア
    public int _CardType;       //カードのタイプ

    public static CardModel instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public CardModel(int cardID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        CardSprite = cardEntity.CardSprite;

        // ホストからもらう情報
        _CardNumber = cardEntity._CardNumber;
        _CardName = cardEntity._CardName;
        _CardText = cardEntity._CardText;
        _CardCost = cardEntity._CardCost;
        _CardScore = cardEntity._CardScore;
        _CardType = cardEntity._CardType;
    }
}
