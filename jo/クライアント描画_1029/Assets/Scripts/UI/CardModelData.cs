using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class CardModelData : MonoBehaviour
{
    public Sprite CardSpriteData;   // カードの画像
    public int CardNumberData;      // カード番号
    public string CardNameData;     // カードの名前
    public string CardTextData;     // カードの説明、効果文
    public int CardCostData;        // カードのコスト
    public int CardScoreData;       // カードのスコア
    public int CardTypeData;        // カードのタイプ
    public int CardRarityData;      // カードのレアリティ
    public int CardMaximumData;     // カードの最大枚数

    // カードのデータ
    public void Put(CardModel cardModel)
    {
        CardSpriteData = cardModel.CardSprite;
        CardNumberData = cardModel.CardNumber;
        CardNameData = cardModel.CardName;
        CardTextData = cardModel.CardText;
        CardCostData = cardModel.CardCost;
        CardScoreData = cardModel.CardScore;
        CardTypeData = cardModel.CardType;
        CardRarityData = cardModel.CardRarity;
        CardMaximumData = cardModel.CardMaximum;
    }

}
