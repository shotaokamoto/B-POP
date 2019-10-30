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

        CardNumberText.text = cardModel.CardNumber.ToString();
        CardNameText.text = cardModel.CardName.ToString();
        CardTextText.text = cardModel.CardText.ToString();
        CardCostText.text = cardModel.CardCost.ToString();
        CardScoreText.text = cardModel.CardScore.ToString();
        CardTypeText.text = cardModel.CardType.ToString();
    }

    private void Update()
    {
        // ドラッグしていたらカードを半透明にする
        if(GetComponent<CardController>().movement.isNowDrag)
        {
            this.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            CardImage.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            CardImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
