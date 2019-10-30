using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カードデータそのものとその処理
public class CardModel : MonoBehaviour
{
    public Sprite CardSprite;   // カードの画像
    public int CardNumber;      // カード番号
    public string CardName;     // カードの名前
    public string CardText;     // カードの説明、効果文
    public int CardCost;        // カードのコスト
    public int CardScore;       // カードのスコア
    public int CardType;        // カードのタイプ
    public int CardRarity;      // カードのレアリティ
    public int CardMaximum;     // カードの最大枚数

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

        CardSprite = cardEntity.CardSpriteEntity;
        CardNumber = cardEntity.CardNumberEntity;
        CardName = cardEntity.CardNameEntity;
        CardText = cardEntity.CardTextEntity;
        CardCost = cardEntity.CardCostEntity;
        CardScore = cardEntity.CardScoreEntity;
        CardType = cardEntity.CardTypeEntity;
        CardRarity = cardEntity.CardRarityEntity;
        CardMaximum = cardEntity.CardMaximumEntity;
    }
}

