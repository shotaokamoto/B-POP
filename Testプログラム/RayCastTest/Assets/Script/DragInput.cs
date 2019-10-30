using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragInput : MonoBehaviour,IDragHandler,IEndDragHandler
{

    public void OnDrag(PointerEventData pointerEventData)
    {
        this.transform.position = pointerEventData.position;
    }
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        RayCastTest.Instance.isDragEnd = true;
    }

}
