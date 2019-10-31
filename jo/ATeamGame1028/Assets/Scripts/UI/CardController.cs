using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;           // 見かけ(view)に関することを操作
    public CardModel model;         // データ(Model)に関することを操作
    public CardMovement movement;   // 移動(movement)に関することを操作
    public CardModelData modelData; 

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
        modelData = GetComponent<CardModelData>();
    }


    // カードの作成
    public void Init(int cardID)
    {
        model = new CardModel(cardID);
        view.Show(model);
        modelData.Put(model);
    }

}
