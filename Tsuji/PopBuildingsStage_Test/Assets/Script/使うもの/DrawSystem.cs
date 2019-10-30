using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSystem : MonoBehaviour
{
    public GameObject[] Field;
    public List<List<int>> DrawData;

    public GameObject Tile;
    public GameObject RedObj;
    public GameObject BlueObj;
    public GameObject GreenObj;

    private void Start()
    {
        Field = GameObject.FindGameObjectsWithTag("Stage");

        /*ここからはサーバーの処理なので無視してください*/
        DrawData = new List<List<int>>()
        {
            new List<int>(){3,1,1},
            new List<int>(){3,5,3},
            new List<int>(){2,1,4,3,5,6,7},
            new List<int>(){3,6,6},
            new List<int>(){-1},
        };
    }

    private void Update()
    {
        DrawSys();
    }

    //メイン描画処理
    private void DrawSys()
    {
        // DrawData[0][0] == -1は終了フラグです
        // DrawData.RemoveAt(0)はリストの一番最初を削除しています
        if (DrawData[0][0] != -1)
        {
            switch (DrawData[0][0])
            {
                case 1:
                    CrateObj(DrawData[0][1], DrawData[0][2]);
                    break;

                case 2:
                    CrateColorfulObj(DrawData[0][1], DrawData[0][2], DrawData[0][3], DrawData[0][4], DrawData[0][5], DrawData[0][6]);
                    break;

                case 3:
                    CrateBlueObj(DrawData[0][1], DrawData[0][2]);
                    break;
            }
            DrawData.RemoveAt(0);
        }
    }

    //オブジェクト追加のメソッド(case1)
    private void CrateObj(int x, int y)
    {
        //変換
        Vector3 CreatePos = new Vector3(0,0,0);                //オブジェクトを生成する際に使う座標
        Quaternion CreateQt = new Quaternion(0, 0, 0, 0);      //オブジェクトを生成する際に使うクォータニオン
        CreatePos = ExchangeArrayToVec3(CreatePos, x, y);                  //配列からVector3に変換

        //描画系
        Instantiate(Tile, CreatePos, CreateQt);                //オブジェクト作成
    }

    //オブジェクト追加のメソッド(case2)
    private void CrateColorfulObj(int x1, int y1, int x2, int y2, int x3, int y3)
    {
        //変換
        Vector3[] CreatePos = new Vector3[3];                //オブジェクトを生成する際に使う座標
        Quaternion[] CreateQt = new Quaternion[3];           //オブジェクトを生成する際に使うクォータニオン
        CreatePos[0] = ExchangeArrayToVec3(CreatePos[0], x1, y1);           //配列からVector3に変換
        CreatePos[1] = ExchangeArrayToVec3(CreatePos[1], x2, y2);           //配列からVector3に変換
        CreatePos[2] = ExchangeArrayToVec3(CreatePos[2], x3, y3);           //配列からVector3に変換

        //描画系
        InstantiateRedObj(CreatePos[0], CreateQt[0]);
        InstantiateBlueObj(CreatePos[1], CreateQt[1]);
        InstantiateGreenObj(CreatePos[2], CreateQt[2]);
    }

    //オブジェクト追加のメソッド(case3)
    private void CrateBlueObj(int x, int y)
    {
        //変換
        Vector3 CreatePos = new Vector3(0, 0, 0);                //オブジェクトを生成する際に使う座標
        Quaternion CreateQt = new Quaternion(0, 0, 0, 0);      //オブジェクトを生成する際に使うクォータニオン
        CreatePos = ExchangeArrayToVec3(CreatePos, x, y);                  //配列からVector3に変換

        //描画系
        InstantiateBlueObj(CreatePos, CreateQt);
    }


    void InstantiateRedObj(Vector3 pos,Quaternion qt)
    {
        Instantiate(RedObj, pos, qt);
    }

    void InstantiateBlueObj(Vector3 pos, Quaternion qt)
    {
        Instantiate(BlueObj, pos, qt);
    }

    void InstantiateGreenObj(Vector3 pos, Quaternion qt)
    {
        Instantiate(GreenObj, pos, qt);
    }



    /// <summary>
    /// 変換関数
    /// </summary>
    /// <param name="outVec">出力</param>
    /// <param name="x">配列x</param>
    /// <param name="y">配列y</param>
    /// <returns></returns>
    private Vector3 ExchangeArrayToVec3(Vector3 outVec,int x,int y)
    {
        for(int i = 0;i< Field.Length;i++)
        {
           if(Field[i].GetComponent<TileBase>().planePos.x == x && Field[i].GetComponent<TileBase>().planePos.y == y)
            {
                outVec = Field[i].GetComponent<Transform>().position;
                return outVec;
            }
        }
        Debug.Log("Vector3に変換するための配列が見つかりませんでした");
        return outVec;
    }
}
