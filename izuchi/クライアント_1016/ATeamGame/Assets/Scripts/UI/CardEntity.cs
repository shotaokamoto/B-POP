using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CardEntity", menuName = "CreateEntity")]

// カードデータそのもの
public class CardEntity : ScriptableObject
{
    public Sprite CardSpriteEntity; // カードの画像
    public int CardNumberEntity;    // カード番号
    public string CardNameEntity;   // カードの名前
    public string CardTextEntity;   // カードの説明、効果文
    public int CardCostEntity;      // カードのコスト
    public int CardScoreEntity;     // カードのスコア
    public int CardTypeEntity;      // カードのタイプ
    public int CardRarityEntity;    // カードのレアリティ
    public int CardMaximumEntity;   // カードの最大枚数
}
