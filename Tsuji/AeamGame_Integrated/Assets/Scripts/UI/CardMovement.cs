using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDropHandler
{
    public Transform defaultParent;
    public bool isDraggable;    // ドラッグ可能フラグ
    public bool isNowDrag;      // フラグ

    private void Awake()
    {
        isDraggable = false;
        isNowDrag = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isNowDrag)
        {
            isNowDrag = true;
        }
        else
        {
            isNowDrag = false;
        }

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

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (!isNowDrag)
        {
            this.gameObject.transform.localScale = new Vector2(2, 2);
            this.gameObject.transform.position += new Vector3(0, 50, 0);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (!isNowDrag)
        {
            this.gameObject.transform.localScale = new Vector2(1, 1);
            this.gameObject.transform.position += new Vector3(0, -50, 0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    private void Update()
    {
        
    }
}
