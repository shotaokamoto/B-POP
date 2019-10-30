using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// EventSystem利用



public class TileBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public GameObject obj;
   
    private Color _default_color;  // 初期化カラー
    private Color _select_color;    // 選択時カラー
    protected Material _material;
    public Vector2 planePos;

    public GameObject childObj; 
    public GameObject topObj;
    public GameObject downObj;
    public GameObject leftObj;
    public GameObject rightObj;

    public int myRank = 0;              //建っている建物のランク　　０の時は建てられていない
    public int myCost = 0;              //建っている建物のコスト　　同一のコストであればランクアップ可能
    public bool isLinkBuilding = false; //繋がってるかどうか

    public MakeObject mObj;
    /// <summary>
    /// RankUp
    /// </summary>
    //private void SearchRankUp()
    //{
    //    //左右優先
    //    //左を探す
    //    if (leftObj != null && leftObj.GetComponent<TileBase>().childObj != null)
    //    {
    //        if (!leftObj.GetComponent<TileBase>().isLinkBuilding && myCost == leftObj.GetComponent<TileBase>().myCost)
    //        {
    //            leftObj.GetComponent<TileBase>().isLinkBuilding = true;
    //            isLinkBuilding = true;

    //            leftObj.GetComponent<TileBase>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
    //            this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

    //            leftObj.GetComponent<TileBase>().childObj.GetComponent<CubeData>().myRank = 2;
    //            childObj.GetComponent<CubeData>().myRank = 2;

    //            MakeObject.Instance.makeColider(0, this.childObj.transform.position, this.childObj);
              
    //            return;
    //        }
    //    }
    //    //右を探す
    //    if (rightObj != null && rightObj.GetComponent<TileBase>().childObj != null)
    //    {
    //        if (!rightObj.GetComponent<TileBase>().isLinkBuilding && myCost == rightObj.GetComponent<TileBase>().myCost)
    //        {
    //            rightObj.GetComponent<TileBase>().isLinkBuilding = true;
    //            isLinkBuilding = true;

    //            rightObj.GetComponent<TileBase>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
    //            this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

    //            rightObj.GetComponent<TileBase>().childObj.GetComponent<CubeData>().myRank = 2;
    //            childObj.GetComponent<CubeData>().myRank = 2;

    //            MakeObject.Instance.makeColider(1, this.childObj.transform.position,this.childObj);
               
    //            return;
    //        }
    //    }
    //    //上を探す
    //    if (topObj != null && topObj.GetComponent<TileBase>().childObj != null)
    //    {
    //        if (!topObj.GetComponent<TileBase>().isLinkBuilding && myCost == topObj.GetComponent<TileBase>().myCost)
    //        {
    //            topObj.GetComponent<TileBase>().isLinkBuilding = true;
    //            isLinkBuilding = true;

    //            topObj.GetComponent<TileBase>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
    //            this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

    //            topObj.GetComponent<TileBase>().childObj.GetComponent<CubeData>().myRank = 2;
    //            childObj.GetComponent<CubeData>().myRank = 2;

    //            MakeObject.Instance.makeColider(2, this.childObj.transform.position,this.childObj);
    //            return;
    //        }
    //    }
    //    //下を探す
    //    if (downObj != null && downObj.GetComponent<TileBase>().childObj != null)
    //    {
    //        if (!downObj.GetComponent<TileBase>().isLinkBuilding && myCost == downObj.GetComponent<TileBase>().myCost)
    //        {
    //            downObj.GetComponent<TileBase>().isLinkBuilding = true;
    //            isLinkBuilding = true;

    //            downObj.GetComponent<TileBase>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
    //            this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

    //            downObj.GetComponent<TileBase>().childObj.GetComponent<CubeData>().myRank = 2;
    //            childObj.GetComponent<CubeData>().myRank = 2;

    //            MakeObject.Instance.makeColider(3, this.childObj.transform.position, this.childObj);
    //            return;
    //        }
    //    }
        
    //}

    void Start()
    {
        // このクラスが付属しているマテリアルを取得 
        _material = this.gameObject.GetComponent<Renderer>().material;
   
        // 選択時と非選択時のカラーを保持 
        _default_color = _material.color;
        _select_color = Color.cyan;
    }

    //ポインターがオブジェクト上に入った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        _material.color = _select_color;
    }

    //ポインターがオブジェクトから出た時

    public void OnPointerExit(PointerEventData ped)
    { 
        _material.color = _default_color;
    }

    //クリックされた時

    public void OnPointerDown(PointerEventData _data)
    {
        //Debug.Log("clicked");
        if(myRank == 0)
        {
            MakeObject.Instance.makeObject(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
            myRank = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Stage")
        {
           childObj = collision.gameObject;
           collision.transform.parent = this.gameObject.transform;
           //myRank = childObj.GetComponent<CubeData>().myRank;
           //myCost = childObj.GetComponent<CubeData>().myCost;
           ////Debug.Log(myCost);
           //SearchRankUp();
        }
    }

}

