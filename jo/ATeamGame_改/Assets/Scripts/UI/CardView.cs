using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour
{
    [SerializeField] public Image CardImage;       // カードの画像
    [SerializeField] public Text CardNumberText;   // カード番号
    [SerializeField] public Text CardNameText;     // カードの名前
    [SerializeField] public Text CardTextText;     // カードの説明、効果文
    [SerializeField] public Text CardCostText;     // カードのコスト
    [SerializeField] public Text CardScoreText;    // カードのスコア
    [SerializeField] public Text CardTypeText;     // カードのタイプ

    // カードの情報表示
    public void Show(CardModel cardModel)
    {
        CardImage.sprite = cardModel.CardSprite;

        CardNumberText.text = cardModel._CardNumber.ToString();
        CardNameText.text = cardModel._CardName.ToString();
        CardTextText.text = cardModel._CardText.ToString();
        CardCostText.text = cardModel._CardCost.ToString();
        CardScoreText.text = cardModel._CardScore.ToString();
        CardTypeText.text = cardModel._CardType.ToString();
    }

}
