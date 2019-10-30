using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/********************************
 * 
 * OnEndDragの中を変更してます
 * 
 * ****************************/

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform defaultParent;
    public bool isDraggable;        // ドラッグ可能フラグ
    public float StartTime;         // 開始時間
    public float DropTime;          // カードを使用した時間
    public GameObject ParentName;   // 親の名前

    private void Start()
    {
        isDraggable = false;
        StartTime = 0.0f;
        DropTime = 0.0f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!UIMgr.instance.isClientTurn)
        {
            return;
        }

        // カードのコストとマナコストを比較
        CardController card = GetComponent<CardController>();
        if (card.model.CardCost <= UIMgr.instance.nowCost)
        {
            isDraggable = true;
        }
        else
        {
            isDraggable = false;
        }

        if (!isDraggable)
        {
            return;
        }

        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }

        transform.SetParent(defaultParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        ParentName = GameObject.Find("Field");  // 親の名前が[Field]に設定
        // 親の名前が[Field]だったらカードを使用した判定
        if (transform.parent.gameObject == ParentName)
        {
            Debug.Log("カードを使用----------------------------");

            // カードをドロップした座標が使用されているかどうか
            if (!PlayerMgr.instance.CheckUsePosList.Contains(UIMgr.instance.Obj.GetComponent<Ctrl.TileData_Maneger>().planePos))
            {
                //card情報追加
                DropTime = StartTime;   // カードを使用した時間を取得

                Debug.Log("カードを出した時間:" + DropTime);

                UIMgr.instance.useCardData.CardDataProxy =
                 new Model.CardDataProxy(
                           gameObject.GetComponent<CardController>().modelData.CardNumberData,
                           gameObject.GetComponent<CardController>().modelData.CardNameData,
                           gameObject.GetComponent<CardController>().modelData.CardTextData,
                           gameObject.GetComponent<CardController>().modelData.CardCostData,
                           gameObject.GetComponent<CardController>().modelData.CardScoreData,
                           gameObject.GetComponent<CardController>().modelData.CardTypeData,
                           gameObject.GetComponent<CardController>().modelData.CardRarityData
                        );
                //UIMgr.instance._UseCardData.Add(UIMgr.instance.useCardData);
                this.gameObject.SetActive(false);   // カードを削除
                // 使用した時間
                UIMgr.instance.useCardData.CardPlayTime = DropTime;
            }
            UIMgr.instance.isDragEnd = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!UIMgr.instance.isNowClick)
        {
            this.gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.transform.localScale = new Vector2(1, 1);
    }

    private void Update()
    {
        if (!UIMgr.instance.isClientTurn)
        {
            StartTime = 0.0f;
        }
        else
        {
            StartTime += Time.deltaTime;
        }
    }

}
