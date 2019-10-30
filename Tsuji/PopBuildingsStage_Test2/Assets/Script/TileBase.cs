using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// EventSystem利用

public class TileBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public GameObject obj;
   
    private Color default_color;  // 初期化カラー
    private Color select_color;    // 選択時カラー
    public Vector2 planePos;
    protected Material _material;

    public GameObject childObj; 
    public GameObject TopObj;
    public GameObject DownObj;
    public GameObject LeftObj;
    public GameObject RightObj;

    public bool isBuilding = false;
    public int myCost = 0;              //建っている建物のコスト　　同一のコストであればランクアップ可能
    public bool isLinkBuilding = false; //繋がってるかどうか
    /// <summary>
    /// RankUpできるかどうかを探索する関数
    /// </summary>
    private void SearchRankUp()
    {
        //左右優先
        //左を探す
        if (LeftObj != null && LeftObj.GetComponent<TileBase>().childObj != null)
        {
            if (!LeftObj.GetComponent<TileBase>().isLinkBuilding && myCost == LeftObj.GetComponent<TileBase>().myCost)
            {
                LeftObj.GetComponent<TileBase>().isLinkBuilding = true;
                isLinkBuilding = true;

                LeftObj.GetComponent<TileBase>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
                this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

                LeftObj.GetComponent<TileBase>().childObj.GetComponent<CubeData>().myRank = 2;
                childObj.GetComponent<CubeData>().myRank = 2;

                MakeObject.Instance.makeColider(0, this.childObj.transform.position, this.childObj);
                return;
            }
        }
        //右を探す
        if (RightObj != null && RightObj.GetComponent<TileBase>().childObj != null)
        {
            if (!RightObj.GetComponent<TileBase>().isLinkBuilding && myCost == RightObj.GetComponent<TileBase>().myCost)
            {
                RightObj.GetComponent<TileBase>().isLinkBuilding = true;
                isLinkBuilding = true;

                RightObj.GetComponent<TileBase>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
                this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

                RightObj.GetComponent<TileBase>().childObj.GetComponent<CubeData>().myRank = 2;
                childObj.GetComponent<CubeData>().myRank = 2;

                MakeObject.Instance.makeColider(1, this.childObj.transform.position,this.childObj);
                return;
            }
        }
        //上を探す
        if (TopObj != null && TopObj.GetComponent<TileBase>().childObj != null)
        {
            if (!TopObj.GetComponent<TileBase>().isLinkBuilding && myCost == TopObj.GetComponent<TileBase>().myCost)
            {
                TopObj.GetComponent<TileBase>().isLinkBuilding = true;
                isLinkBuilding = true;

                TopObj.GetComponent<TileBase>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
                this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

                TopObj.GetComponent<TileBase>().childObj.GetComponent<CubeData>().myRank = 2;
                childObj.GetComponent<CubeData>().myRank = 2;

                MakeObject.Instance.makeColider(2, this.childObj.transform.position,this.childObj);
                return;
            }
        }
        //下を探す
        if (DownObj != null && DownObj.GetComponent<TileBase>().childObj != null)
        {
            if (!DownObj.GetComponent<TileBase>().isLinkBuilding && myCost == DownObj.GetComponent<TileBase>().myCost)
            {
                DownObj.GetComponent<TileBase>().isLinkBuilding = true;
                isLinkBuilding = true;

                DownObj.GetComponent<TileBase>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
                this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

                DownObj.GetComponent<TileBase>().childObj.GetComponent<CubeData>().myRank = 2;
                childObj.GetComponent<CubeData>().myRank = 2;

                MakeObject.Instance.makeColider(3, this.childObj.transform.position, this.childObj);
                return;
            }
        }
    }

    void Start()
    {
        // このクラスが付属しているマテリアルを取得 
        _material = this.gameObject.GetComponent<Renderer>().material;
   
        // 選択時と非選択時のカラーを保持 
        default_color = _material.color;
        select_color = Color.cyan;
    }

    //ポインターがオブジェクト上に入った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        _material.color = select_color;
        if (!isBuilding)
        {
            Debug.Log("建ってないよ");
        }
    }

    //ポインターがオブジェクトから出た時

    public void OnPointerExit(PointerEventData ped)
    { 
        _material.color = default_color;
    }

    //クリックされた時

    public void OnPointerDown(PointerEventData _data)
    {
        //Debug.Log("clicked");
        if(isBuilding == false)
        {
            if (MakeObject.Instance.Cobj != null)
            {
                MakeObject.Instance.coliderOff();
            }
            MakeObject.Instance.SetPosition(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
            isBuilding = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Stage")
        {
           childObj = collision.gameObject;
           collision.transform.parent = this.gameObject.transform;
           myCost = childObj.GetComponent<CubeData>().myCost;
           //Debug.Log(myCost);
           SearchRankUp();
        }
    }

}

