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
        //Debug.Log("OnDrop");

        if (!UIMgr.instance.isClientTurn)
        {
            return;
        }
        
        if (type == TYPE.HAND)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            UIMgr.instance.isCheckRay = true;
            UIMgr.instance.Obj = hit.collider.gameObject;
            Ctrl.BldgData_Maneger.Instance.SetPosition(new Vector3(UIMgr.instance.Obj.transform.position.x, UIMgr.instance.Obj.transform.position.y + 0.5f, UIMgr.instance.Obj.transform.position.z));
            UIMgr.instance.Obj.GetComponent<Ctrl.TileData_Maneger>().childObj = UIMgr.instance.Obj;

            // 小数点以下切り捨て
            int x, y;
            x = Mathf.RoundToInt(UIMgr.instance.Obj.transform.position.x);
            y = Mathf.RoundToInt(UIMgr.instance.Obj.transform.position.z);

            // カードの座標を取得
            UIMgr.instance.Obj.GetComponent<Ctrl.TileData_Maneger>().planePos = new Vector2(x, y);

            // そのマスが使われているか
            if (PlayerMgr.instance.CheckUsePosList.Contains(UIMgr.instance.Obj.GetComponent<Ctrl.TileData_Maneger>().planePos))
            {
                //Debug.Log("そのマスは使われています。");
                UIMgr.instance.isCheckPos = true;
            }
            else
            {
                //Debug.Log("そのマスは使われていませんでした。");
                UIMgr.instance.isCheckPos = false;
            }
        }
        else
        {
            UIMgr.instance.isCheckRay = false;
        }

        if(UIMgr.instance.isCheckPos)
        {
            return;
        }
        if(!UIMgr.instance.isCheckRay)
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
            UIMgr.instance.ReduceNowCost(card.model.CardCost, true);
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
