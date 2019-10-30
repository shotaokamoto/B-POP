using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDropHandler
{
    public Transform defaultParent;
    public bool isDraggable;    // ドラッグ可能フラグ
    public float StartTime;         // 開始時間
    public float DropTime;          // カードを使用した時間
    public GameObject ParentName;   // 親の名前

    private void Awake()
    {
        isDraggable = false;
        StartTime = 0.0f;
        DropTime = 0.0f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        

        // カードのコストとマナコストを比較
        CardController card = GetComponent<CardController>();
        if (card.model._CardCost <= UIMgr.instance.nowCost)
        {
            isDraggable = true;
        }
        else
        {
            isDraggable = false;
        }

        if(!isDraggable)
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
            this.gameObject.SetActive(false);
                
            DropTime = StartTime;   // カードを使用した時間を取得
            Debug.Log("カードを出した時間:" + DropTime);

            // リストにクライアントのデータを加える
            //UIMgr.instance.useCardData.CardDataProxy = ;
            //UIMgr.instance.useCardData.PlayerNum = ;
            UIMgr.instance.useCardData.CardPlayTime = DropTime;
            //UIMgr.instance.useCardData.FieldPos = ;
            UIMgr.instance._UseCardData.Add(UIMgr.instance.useCardData);
            Debug.Log(UIMgr.instance._UseCardData);

        }

        UIMgr.instance.isDragEnd = true;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!UIMgr.instance.isNowClick)
        {
            this.gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
            //this.gameObject.transform.position += new Vector3(0, 50, 0);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (!isNowDrag)
        {
            this.gameObject.transform.localScale = new Vector2(1, 1);
            //this.gameObject.transform.position += new Vector3(0, -50, 0);
        }
    }
    public void OnPointerDown()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public void OnDrop(PointerEventData eventData)
    {
        
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
