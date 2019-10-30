using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Global;

using Model;

namespace Ctrl
{
    public class BldgData_Maneger : NetworkBehaviour
    {

        public static BldgData_Maneger Instance;        //instance
        public Ctrl.RecvCliantManeger sendCliantData;   //受け取る処理
        public Model.ClientData useCardData;            //送るデータ


        public GameObject[] obj;
        public GameObject[] colliderObj;
        public GameObject Cobj;
        public Vector3 makePos;
        public bool isBuild = false;

        SendClientDataArray clientDataArray;
        //デバッグ用
        public int cost = 0;

        private void Awake()
        {
            Instance = this;
            // クライアントが送るデータの初期化
            useCardData = new Model.ClientData(null, 0, 0, new Vector2());
        }

        void Update()
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
                Debug.Log("aaaa");
                UIMgr.instance.isDragEnd = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Stage"))
                    {
                        SetPosition(new Vector3(hit.transform.position.x, hit.transform.position.y + 0.5f, hit.transform.position.z));
                        if (makePos != null && isBuild)
                        {
                            isBuild = false;
                            ////当たり判定を削除
                            //if (Cobj != null)
                            //{
                            //    coliderOff();
                            //}
           
                            hit.transform.gameObject.GetComponent<TileData_Maneger>().planePos = new Vector2(hit.transform.position.x, hit.transform.position.z);
                            // Commandによってサーバーに通知する
                            
                           // clientDataArray = ClassConvertArray(FromManeger.Instance._FromData[useCardData.PlayerNum]); //一度型変換
                          //  CmdSendClientData(clientDataArray, useCardData.PlayerNum);
                            cost++;
                            if (cost >= obj.Length)
                            {
                                cost = 0;
                            }
                        }
                    }
                }

            }
        }
        //ポジションのセット
        public void SetPosition(Vector3 pos)
        {
            makePos = pos;
            isBuild = true;
        }
        //建物を作成
        public void MakeBildings(int number, GameObject parent)
        {
            if (makePos != null && isBuild)
            {
                isBuild = false;
                GameObject c = Instantiate(obj[number - 1], makePos, transform.rotation, parent.transform);  //number - 1にしないと配列がずれます
                parent.GetComponent<TileData_Maneger>().childObj = c;            //親のタイルデータに自分を入れます

                cost++;
                if (cost >= obj.Length)
                {
                    cost = 0;
                }
            }
        }
        //コライダーを作成する
        public void makeColider(int count, Vector3 pos, GameObject obj)
        {
            Cobj = Instantiate(colliderObj[count], pos, colliderObj[count].transform.rotation);
            Cobj.gameObject.GetComponent<Collider_Maneger>()._myNumber = obj.transform.parent.GetComponent<TileData_Maneger>().Field.Data.Number;
            Cobj.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        //コライダーを消す
        //public void coliderOff()
        //{
        //    Destroy(Cobj.gameObject);
        //}
        ///ClientDataClassを分解し、ClientData配列に置き換える
        public SendClientDataArray ClassConvertArray(ClientData client)
        {
            SendClientDataArray sData;

            sData.Number  = client.CardDataProxy.Number;
            sData.Name    = client.CardDataProxy.Name;
            sData.Text    = client.CardDataProxy.Text;
            sData.Cost    = client.CardDataProxy.Cost;
            sData.Score   = client.CardDataProxy.Score;
            sData.Type    = client.CardDataProxy.Type;
            sData.Rarity  = client.CardDataProxy.Rarity;

            sData._PlayerNum = client._PlayerNum;
            sData._CardPlayTime = client._CardPlayTime;
            sData._FieldPos = client._FieldPos;

            return sData;
        }

        //ClientData配列を分解し、ClientDataClassに置き換える
        public Model.ClientData ArrayConvertClass(SendClientDataArray saverData)
        { 
            Model.ClientData cData = new ClientData(null,0,0,new Vector2());

            cData.CardDataProxy.Number  = saverData.Number;
            cData.CardDataProxy.Name    = saverData.Name;
            cData.CardDataProxy.Text    = saverData.Text;
            cData.CardDataProxy.Cost    = saverData.Cost;
            cData.CardDataProxy.Score   = saverData.Score;
            cData.CardDataProxy.Type    = saverData.Type;
            cData.CardDataProxy.Rarity  = saverData.Rarity;

            cData._PlayerNum     = saverData._PlayerNum;
            cData._CardPlayTime  = saverData._CardPlayTime;
            cData._FieldPos      = saverData._FieldPos;

            return cData;
        }


        //クライアントからサーバへデータを渡す&受け取る
        [Command]
        public void CmdSendClientData( SendClientDataArray array,　int num)
        {
            //リストにクライアントデータを加える
            //UIMgr.instance.useCardData.CardDataProxy = clientData.CardDataProxy;    //カードデータ :
            //UIMgr.instance.useCardData.PlayerNum     = clientData.PlayerNum;        //自分の番号   :　今はわからない
            //UIMgr.instance.useCardData.FieldPos      = clientData.FieldPos;         //置いた場所
            //UIMgr.instance.useCardData.CardPlayTime  = clientData.CardPlayTime;     //カードを置いたときの時間
            //UIMgr.instance._UseCardData.Add(clientData);                            //リストに追加

            FromManeger.Instance.SetFromDataToServerMgr(ArrayConvertClass(array), num);  //データをサーバに送る為にデータをセット
            RpcRecvClientData();            
        }

        [ClientRpc]
        public void RpcRecvClientData()
        {
            sendCliantData.CreateRecvObject();                                      //サーバが受け取る処理
        }
    }
}