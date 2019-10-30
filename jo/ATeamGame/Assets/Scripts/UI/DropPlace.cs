using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    public int DropCard;        // フィールドに出したカード数
    public float StartTime;
    public float LapTime;

    // カードの場所
    public enum TYPE
    {
        HAND,   // 手札
        FIELD   // フィールド
    }
    public TYPE type;   // カードの場所

    // 初期化処理
    public void Start()
    {
        DropCard = 0;
        StartTime = 0.0f;
        LapTime = 0.0f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!UIMgr.instance.isClientTurn)
        {
            return;
        }
        if(type == TYPE.HAND)
        {
            return;
        }
        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if (card != null)
        {
            if (!card.movement.isDraggable)
            {
                return;
            }
            card.movement.defaultParent = this.transform;
            UIMgr.instance.ReduceNowCost(card.model._CardCost, true);
        }
        DropCard++;
        LapTime = StartTime;
    }

    private void Update()
    {
        if(!UIMgr.instance.isClientTurn)
        {
            DropCard = 0;
            StartTime = 0.0f;
        }

        if(UIMgr.instance.isClientTurn)
        {
            StartTime += Time.deltaTime;
        }

    }

}
