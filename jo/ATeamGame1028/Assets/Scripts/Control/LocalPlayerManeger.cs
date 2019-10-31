using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using kernal;
using Global;
using Model;

namespace Ctrl
{
    public class LocalPlayerManeger : NetworkBehaviour
    {
        public static LocalPlayerManeger instance;
        public RecvCliantManeger sendCliantData;                                                                   //受け取る処理

        // クライアントが送るデータ
        public Model.ClientData sendCardData;
        //work用
        public ClientData cData;

        [SyncVar]
        public int playerNum;

        private SendClientDataArray clientDataArray;


        private void Awake()
        {
            instance = this;
            // クライアントが送るデータの初期化
            cData = new Model.ClientData(null, 0, 0, new Vector2());
            sendCardData = new Model.ClientData(null, 0, 0, new Vector2());
        }

        public override void OnStartClient()
        {
            cData = new Model.ClientData(null, 0, 0, new Vector2());
            sendCardData = new Model.ClientData(null, 0, 0, new Vector2());
        }





        // Update is called once per frame
        void FixedUpdate()
        {
            if (!isLocalPlayer) return;     //他プレイヤーには何もしない
            if (Input.GetMouseButtonDown(0))        //クリックされていれば
            {
                UIMgr.instance.isNowClick = true;
            }
            else
            {
                UIMgr.instance.isNowClick = false;
            }

            if (UIMgr.instance.isDragEnd)
            {
                UIMgr.instance.isDragEnd = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Stage"))
                    {

                        UIMgr.instance.useCardData._PlayerNum = playerNum;
                        hit.transform.gameObject.GetComponent<TileData_Maneger>().planePos = new Vector2(hit.transform.position.x, hit.transform.position.z);
                        UIMgr.instance.useCardData._FieldPos = new Vector2(hit.transform.position.x, hit.transform.position.z);

                        //print(UIMgr.instance.useCardData._CardDataProxy.Name);
                        
                        clientDataArray = ClassConvertArray(UIMgr.instance.useCardData); //一度型変換
                        CmdSendClientData(clientDataArray, playerNum);
                    }
                }

            }
        } // end Updata
          ///ClientDataClassを分解し、ClientData配列に置き換える
        public SendClientDataArray ClassConvertArray(ClientData client)
        {
            SendClientDataArray sData;

            sData.Number = client.CardDataProxy.Number;
            sData.Name = client.CardDataProxy.Name;
            sData.Text = client.CardDataProxy.Text;
            sData.Cost = client.CardDataProxy.Cost;
            sData.Score = client.CardDataProxy.Score;
            sData.Type = client.CardDataProxy.Type;
            sData.Rarity = client.CardDataProxy.Rarity;

            sData._PlayerNum = client._PlayerNum;
            sData._CardPlayTime = client._CardPlayTime;
            sData._FieldPos = client._FieldPos;

            return sData;
        }

        //ClientData配列を分解し、ClientDataClassに置き換える
        public Model.ClientData ArrayConvertClass(SendClientDataArray saverData)
        {
            CardDataProxy setdata = new CardDataProxy(saverData.Number,
                saverData.Name, 
                saverData.Text, 
                saverData.Cost,
                saverData.Score, 
                saverData.Type,
                saverData.Rarity
                );

            cData.CardDataProxy = setdata;
            cData._PlayerNum = saverData._PlayerNum;
            cData._CardPlayTime = saverData._CardPlayTime;
            cData._FieldPos = saverData._FieldPos;

            return cData;
        }


        //クライアントからサーバへデータを渡す&受け取る
        [Command]
        public void CmdSendClientData(SendClientDataArray array, int num)
        {
            FromManeger.Instance.SetFromDataToServerMgr(ArrayConvertClass(array), num);  //データをサーバに送る為にデータをセット
            print(ArrayConvertClass(array)._CardDataProxy.Name);
            print(ArrayConvertClass(array)._FieldPos);
            print(ArrayConvertClass(array)._PlayerNum);
            print(ArrayConvertClass(array)._CardPlayTime);

            //RpcRecvClientData();            
        }

        [ClientRpc]
        public void RpcRecvClientData()
        {            
            sendCliantData.CreateRecvObject();                                      //サーバが受け取る処理
            Log.Write("ClientRpcを実行したよ");
        }
    }
}
