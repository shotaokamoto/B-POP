using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if(!UIMgr.instance.isClientTurn)
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
        //Debug.Log("OnEndDrag");

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
            // カードをドロップした座標が使用されているかどうか
            if (!UIMgr.instance.isCheckPos)
            {
                Debug.Log("カードを使用しました。----------");
                this.gameObject.SetActive(false);   // カードを削除

                DropTime = StartTime;   // カードを使用した時間を取得
                
                // リストにクライアントのデータを加える
                // 使用したカードデータ
                UIMgr.instance.useCardData.CardDataProxy =
                    new Model.CardDataProxy(
                        this.gameObject.GetComponent<CardController>().modelData.CardNumberData,
                        this.gameObject.GetComponent<CardController>().modelData.CardNameData,
                        this.gameObject.GetComponent<CardController>().modelData.CardTextData,
                        this.gameObject.GetComponent<CardController>().modelData.CardCostData,
                        this.gameObject.GetComponent<CardController>().modelData.CardScoreData,
                        this.gameObject.GetComponent<CardController>().modelData.CardTypeData,
                        this.gameObject.GetComponent<CardController>().modelData.CardRarityData
                        );
                Debug.Log("使用したカードデータ　：" + UIMgr.instance.useCardData.CardDataProxy);

                // 使用したプレイヤー
                UIMgr.instance.useCardData.PlayerNum = 1;
                Debug.Log("カードを使用したプレイヤー　：" + UIMgr.instance.useCardData.PlayerNum);

                // 使用した時間
                UIMgr.instance.useCardData.CardPlayTime = DropTime;
                Debug.Log("カードを使用した時間　：" + UIMgr.instance.useCardData.CardPlayTime);
                
                // リストにクライアントのデータを加える
                UIMgr.instance.useCardData.FieldPos = UIMgr.instance.Obj.GetComponent<Ctrl.TileData_Maneger>().planePos;
                Debug.Log("カードの使用した座標　：" + UIMgr.instance.useCardData.FieldPos);
                // 使用されている座標リストに追加
                PlayerMgr.instance.CheckUsePosList.Add(UIMgr.instance.Obj.GetComponent<Ctrl.TileData_Maneger>().planePos);

                // リストに追加
                UIMgr.instance._UseCardData.Add(UIMgr.instance.useCardData);
                Debug.Log("使用したカードデータをリストに追加　：" + UIMgr.instance._UseCardData); 
               
            }
            UIMgr.instance.isCheckPos = false;
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
