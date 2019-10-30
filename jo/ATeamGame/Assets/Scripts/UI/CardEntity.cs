using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CardEntity", menuName = "CreateEntity")]

// カードデータそのもの
public class CardEntity : ScriptableObject
{
    public Sprite CardSprite;   // カードの画像

    public int _CardNumber;     //カード番号
    public string _CardName;    //カードの名前
    public string _CardText;    //カードの説明、効果文
    public int _CardCost;       //カードのコスト
    public int _CardScore;      //カードのスコア
    public int _CardType;       //カードのタイプ
}
