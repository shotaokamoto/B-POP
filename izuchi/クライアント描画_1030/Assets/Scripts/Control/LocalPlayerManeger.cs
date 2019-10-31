using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Global;
using Model;

/************************************
 * 
 * Playerが操作する処理
 * PlayerPrefabにアタッチしてください
 * 
 * **********************************/

namespace Ctrl
{
    //public class LocalPlayerManeger : NetworkBehaviour
    //{
    //    public static LocalPlayerManeger instance;
    //    private GlobalParamenter global;
    //    // クライアントが送るデータ
    //    public Model.ClientData sendCardData;
    //    //work用
    //    [SyncVar]
    //    public int playerNum;       //自分のプレイヤーナンバー

    //    //変換のためのデータ構造体List
    //    public List<SendClientDataArray> clientDataArrayList = new List<SendClientDataArray>();
    //    public bool isSendData = false;                 //データを送ったかどうかの判定

    //    //受け取るデータ構造体
    //    public SendSeverDataArray recvSeverData;
    //    public RecvCliantData recvCliantData;

    //    private void Awake()
    //    {
    //        instance = this;
    //        // クライアントが送るデータの初期化
    //        sendCardData = new Model.ClientData(null, 0, 0, new Vector2());
    //        recvSeverData.isRecvFinished = false;
    //    }

    //    // Update is called once per frame
    //    void FixedUpdate()
    //    {
    //        if (!isLocalPlayer) return;     //他プレイヤーには何もしない
    //        //データを受け取っていれば
    //        if (recvSeverData.isRecvFinished)
    //        {
    //            DrawManager.instance._IsDrawFaze = true;    // クライアントの描画を開始する
    //            recvSeverData.isRecvFinished = false;
    //            //SendSeverDataArrayをRecvClientDataclassに変換
    //            recvCliantData = RecvArrayConvertClass(recvSeverData);
    //            if (recvCliantData != null) print("受け取ったよ");
    //        }
    //        //描画処理

    //        if (!UIMgr.instance.isClientTurn)   //ターンエンドであれば
    //        {
    //            UIMgr.instance.isClientTurn = true;
    //            //int Length = UIMgr.instance.useCardGroup.Count;
    //            for (int i = 0; i < UIMgr.instance._UseCardData.Count; i++)
    //            {
    //                clientDataArrayList.Add(SendClassConvertArray(UIMgr.instance._UseCardData[i]));  //一度型変換
    //            }
    //            SendClientDataArray[] clientDataArray = clientDataArrayList.ToArray();
    //            CmdSendClientData(clientDataArray, playerNum);                      //データ送信
    //        }
    //        else
    //        {
    //            if (Input.GetMouseButtonDown(0))        //クリックされていれば
    //            {
    //                UIMgr.instance.isNowClick = true;
    //            }
    //            else
    //            {
    //                UIMgr.instance.isNowClick = false;
    //            }

    //            if (UIMgr.instance.isDragEnd)           //ドラッグが終わっていれば
    //            {
    //                UIMgr.instance.isDragEnd = false;
    //                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //                RaycastHit hit;

    //                if (Physics.Raycast(ray, out hit))
    //                {
    //                    if (hit.collider.CompareTag("Stage"))
    //                    {

    //                        UIMgr.instance.useCardData._PlayerNum = playerNum;
    //                        hit.transform.gameObject.GetComponent<TileData_Maneger>().planePos = new Vector2(hit.transform.position.x, hit.transform.position.z);
    //                        UIMgr.instance.useCardData._FieldPos = new Vector2(hit.transform.position.x, hit.transform.position.z);
    //                        // リストに追加  
    //                        UIMgr.instance._UseCardData.Add(new ClientData(UIMgr.instance.useCardData._CardDataProxy,
    //                            UIMgr.instance.useCardData._PlayerNum,
    //                            UIMgr.instance.useCardData._CardPlayTime,
    //                            UIMgr.instance.useCardData._FieldPos));

    //                        //print(UIMgr.instance.useCardData._CardDataProxy.Name);
    //                    }
    //                }

    //            }
    //        }

    //    } // end Updata

    //    ///ClientDataClass<List>を分解し、ClientData構造体に置き換える
    //    public SendClientDataArray SendClassConvertArray(ClientData client)
    //    {
    //        SendClientDataArray sData;

    //        sData.Number = client.CardDataProxy.Number;
    //        sData.Name = client.CardDataProxy.Name;
    //        sData.Text = client.CardDataProxy.Text;
    //        sData.Cost = client.CardDataProxy.Cost;
    //        sData.Score = client.CardDataProxy.Score;
    //        sData.Type = client.CardDataProxy.Type;
    //        sData.Rarity = client.CardDataProxy.Rarity;

    //        sData._PlayerNum = client._PlayerNum;
    //        sData._CardPlayTime = client._CardPlayTime;
    //        sData._FieldPos = client._FieldPos;

    //        return sData;
    //    }

    //    //ClientData構造体を分解し、ClientDataClassに置き換える
    //    public Model.ClientData SendArrayConvertClass(SendClientDataArray severData)
    //    {
    //        ClientData cData  = new Model.ClientData(null, 0, 0, new Vector2());
    //        CardDataProxy setdata = new CardDataProxy(severData.Number,
    //           severData.Name,
    //           severData.Text,
    //           severData.Cost,
    //           severData.Score,
    //           severData.Type,
    //           severData.Rarity
    //           );

    //        cData.CardDataProxy = setdata;
    //        cData._PlayerNum = severData._PlayerNum;
    //        cData._CardPlayTime = severData._CardPlayTime;
    //        cData._FieldPos = severData._FieldPos;

    //        return cData;
    //    }

    //    //SendSeverDataArrayをRecvClientDataclassに変換
    //    public RecvCliantData RecvArrayConvertClass(SendSeverDataArray sever)
    //    {
    //        RecvCliantData rData = new RecvCliantData(new FieldDataProxy[global.FieldSize.y,global.FieldSize.x],null,-1,null,null);
    //        int severCount = 0;

    //        for (int i = 0; i < sever.AllFeildData.Length / global.FieldSize.x; i++)
    //        {
    //            for (int j = 0; j < sever.AllFeildData.Length / global.FieldSize.y; j++)
    //            {
    //                //カードデータをClassに変換する
    //                rData.AllFeildData[i, j]._CardData.Cost = sever.AllFeildData[severCount].SendCardData.Cost;
    //                rData.AllFeildData[i, j]._CardData.Name = sever.AllFeildData[severCount].SendCardData.Name;
    //                rData.AllFeildData[i, j]._CardData.Number = sever.AllFeildData[severCount].SendCardData.Number;
    //                rData.AllFeildData[i, j]._CardData.Rarity = sever.AllFeildData[severCount].SendCardData.Rarity;
    //                rData.AllFeildData[i, j]._CardData.Score = sever.AllFeildData[severCount].SendCardData.Score;
    //                rData.AllFeildData[i, j]._CardData.Text = sever.AllFeildData[severCount].SendCardData.Text;
    //                rData.AllFeildData[i, j]._CardData.Type = sever.AllFeildData[severCount].SendCardData.Type;

    //                //残りも変換
    //                rData.AllFeildData[i, j].IsRankUp = sever.AllFeildData[severCount].IsRankUp;
    //                rData.AllFeildData[i, j].MaptipNumber = sever.AllFeildData[severCount].MaptipNumber;
    //                rData.AllFeildData[i, j].PossessionPlayer = sever.AllFeildData[severCount].PossessionPlayer;
    //                rData.AllFeildData[i, j].RankID = sever.AllFeildData[severCount].RankID;
    //                severCount++;
    //            }
    //        }
    //        //プレイヤーデータを構造体に変換
    //        for (int i = (int)PlayerType.Player1; i < (int)PlayerType.EndPlayer; i++)
    //        {
    //            rData.AllPlayerData[i].PlayerNumber = sever.AllPlayerData[i].PlayerNumber;
    //            rData.AllPlayerData[i].Ranking = sever.AllPlayerData[i].Ranking;
    //            rData.AllPlayerData[i].ScorePerTurn = sever.AllPlayerData[i].ScorePerTurn;
    //            rData.AllPlayerData[i].TotalScore = sever.AllPlayerData[i].TotalScore;
    //        }
    //        //描画メソッドを構造体に変換
    //        for (int i = 0; i < sever.DrawDataList.Length; i++)
    //        {
    //            for (int j = 0; j < sever.DrawDataList[i].vs.Length; j++)
    //            {
    //                rData.DrawDataList[i][j] = sever.DrawDataList[i].vs[j];
    //            }
    //        }
    //        //引くカードのデータを構造体に変換
    //        for (int i = 0; i < sever.DrawCardList.Length; i++)
    //        {
    //            rData.DrawCardList[i].Name = sever.DrawCardList[i].Name;
    //            rData.DrawCardList[i].Number = sever.DrawCardList[i].Number;
    //            rData.DrawCardList[i].Rarity = sever.DrawCardList[i].Rarity;
    //            rData.DrawCardList[i].Score = sever.DrawCardList[i].Score;
    //            rData.DrawCardList[i].Text = sever.DrawCardList[i].Text;
    //            rData.DrawCardList[i].Type = sever.DrawCardList[i].Type;
    //        }

    //        return rData;
    //    }

    //    //クライアントからサーバへデータを渡す
    //    [Command]
    //    public void CmdSendClientData(SendClientDataArray[] array, int num)
    //    {
    //        isSendData = true;
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            FromManeger.Instance.SetFromDataToServerMgr(SendArrayConvertClass(array[i]),num);  //データをサーバに送る為にデータをセット
    //        }
    //        //デバッグ用
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            print(SendArrayConvertClass(array[i])._CardDataProxy.Name);
    //            print(SendArrayConvertClass(array[i])._PlayerNum);
    //        }
    //    }
    //}
}