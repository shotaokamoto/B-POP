using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{

    protected int _sceneTask;
    private int cont = 0;
    public int width = 8;
    public int hight = 8;
    public GameObject maikeTile;
    public GameObject parentObj;
    public List<GameObject> objList;

    /// <summary>
    /// 最初の列 
    /// </summary>
    /// <param name="column"></param>
    private void FastColumn(int column) 
    {
        objList[column].GetComponent<TileBase>().TopObj = null;
        objList[column].GetComponent<TileBase>().DownObj = objList[column + hight].gameObject;

        if(column + 1 > width-1) objList[column].GetComponent<TileBase>().RightObj = null;
        else                   objList[column].GetComponent<TileBase>().RightObj = objList[column + 1].gameObject;

        if (column - 1 < 0) objList[column].GetComponent<TileBase>().LeftObj = null;
        else                objList[column].GetComponent<TileBase>().LeftObj = objList[column - 1].gameObject;
    }
    /// <summary>
    /// 最後の列
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    private void LastColumn(int column, int line)
    {
        objList[line * hight + column].GetComponent<TileBase>().TopObj = objList[(line * hight + column) - hight].gameObject;
        objList[line * hight + column].GetComponent<TileBase>().DownObj = null;

        if (column == width -1) objList[line * hight + column].GetComponent<TileBase>().RightObj = null;
        else                    objList[line * hight + column].GetComponent<TileBase>().RightObj = objList[(line * hight + column) + 1].gameObject;

        if (column == 0) objList[line * hight + column].GetComponent<TileBase>().LeftObj = null;
        else             objList[line * hight + column].GetComponent<TileBase>().LeftObj = objList[(line * hight + column) - 1].gameObject;
    }
    /// <summary>
    /// 最初の行
    /// </summary>
    private void FastLine(int line)
    {
        if (line == 0) objList[line * hight].GetComponent<TileBase>().TopObj = null;
        else           objList[line * hight].GetComponent<TileBase>().TopObj = objList[(line - 1) * hight];

        if (line == hight -1) objList[line * hight].GetComponent<TileBase>().DownObj = null;
        else                  objList[line * hight].GetComponent<TileBase>().DownObj = objList[(line + 1) * hight];

        objList[line * hight].GetComponent<TileBase>().RightObj = objList[line * hight + 1].gameObject;
        objList[line * hight].GetComponent<TileBase>().LeftObj = null;
    }
    /// <summary>
    /// 最後の行
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    private void LastLine(int column, int line)
    {
        if (line == 0) objList[line * hight + column].GetComponent<TileBase>().TopObj = null;
        else           objList[line * hight + column].GetComponent<TileBase>().TopObj = objList[(line - 1) * hight + column];

        if (line == width -1) objList[line * hight + column].GetComponent<TileBase>().DownObj = null;
        else                  objList[line * hight + column].GetComponent<TileBase>().DownObj = objList[(line + 1) * hight + column];

        objList[line * hight + column].GetComponent<TileBase>().RightObj = null;
        objList[line * hight + column].GetComponent<TileBase>().LeftObj = objList[line * hight + (column - 1)].gameObject;
    }
    /// <summary>
    /// それ以外
    /// </summary>
    private void SquareField(int column,int line)
    {
        objList[line * hight + column].GetComponent<TileBase>().TopObj   = objList[(line - 1) * hight + column];
        objList[line * hight + column].GetComponent<TileBase>().DownObj  = objList[(line + 1) * hight + column];
        objList[line * hight + column].GetComponent<TileBase>().RightObj = objList[(line * hight + column) + 1].gameObject;
        objList[line * hight + column].GetComponent<TileBase>().LeftObj  = objList[(line * hight + column) - 1].gameObject;

    }
    void Awake()
    {
        objList = new List<GameObject>();
    }

    // Use this for initialization
    void Start()
    {
        // 配置するプレハブの読み込み 
        GameObject prefab = maikeTile;
        // 配置元のオブジェクト指定 
        GameObject stageObject = GameObject.FindWithTag("Stage");
        // タイル配置
        for (int i = -hight / 2; i < hight / 2; i++)
        {
            
            for (int j = -width / 2; j < width / 2; j++)
            {

                Vector3 tile_pos = new Vector3(
                    0 + prefab.transform.localScale.x * i,
                    0,
                    0 + prefab.transform.localScale.z * j
                  );

                if (prefab != null)
                {
                    cont++;
                    // プレハブの複製 
                    GameObject instant_object =
                      (GameObject)GameObject.Instantiate(prefab,tile_pos, Quaternion.identity);
                    // 生成元の下に複製したプレハブをくっつける 
                    instant_object.transform.parent = parentObj.transform;
                    instant_object.name = "" + cont;
                    objList.Add(instant_object);
                }
            }
        }
        //配置後に床データを挿入する
        for(int i = 0; i< hight; i++)
        {
            for(int j = 0; j < width; j++)
            {
                objList[i * hight + j].GetComponent<TileBase>().planePos.x = j;
                objList[i * hight + j].GetComponent<TileBase>().planePos.y = i;
                if (i == 0) FastColumn(j);                  //最初の列
                else if (i == hight - 1) LastColumn(j, i);  //最後の列
                else if (j == 0) FastLine(i);               //最初の行
                else if (j == width - 1) LastLine(j, i);    //最後の行
                else SquareField(j, i);
            }
        }
    }
}
