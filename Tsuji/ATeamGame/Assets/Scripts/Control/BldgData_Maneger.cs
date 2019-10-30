using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Model;

namespace Ctrl
{
    //public class BldgData_Maneger : NetworkBehaviour
    //{
    //    public static BldgData_Maneger Instance;  //instance

    //    public GameObject[] obj;
    //    public GameObject[] colliderObj;
    //    public GameObject Cobj;
    //    public Vector3 makePos;
    //    public bool isBuild = false;
    //    //デバッグ用
    //    public int cost = 0;
    //    private void Awake()
    //    {
    //        Instance = this;
    //    }
    //    //ポジションのセット
    //    public void SetPosition(Vector3 pos)
    //    {
    //        makePos = pos;
    //        isBuild = true;
    //    }

    //    //コライダーを作成する
    //    public void makeColider(int count, Vector3 pos, GameObject obj)
    //    {
    //        Cobj = Instantiate(colliderObj[count], pos, colliderObj[count].transform.rotation);
    //        Cobj.gameObject.GetComponent<Collider_Maneger>()._myCost = obj.GetComponent<BldgData>().myCost;
    //        Cobj.gameObject.GetComponent<BoxCollider>().enabled = true;
    //    }
    //    //コライダーを消す
    //    public void coliderOff()
    //    {
    //        Destroy(Cobj.gameObject);
    //    }
    //    void Update()
    //    {
    //        if (!isLocalPlayer) return;     //他プレイヤーには何もしない
    //        if (Input.GetMouseButtonDown(0))        //クリックされていれば
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;
    //            if (Physics.Raycast(ray, out hit))
    //            {
    //                if (hit.collider.CompareTag("Stage"))
    //                {
    //                    SetPosition(new Vector3(hit.transform.position.x, hit.transform.position.y + 0.5f, hit.transform.position.z));
    //                    if (makePos != null && isBuild)
    //                    {
    //                        isBuild = false;
    //                        // Commandによってサーバーに通知する
    //                        CmdMakeBildings(makePos, cost);
    //                        cost++;
    //                        if (Cobj != null)
    //                        {
    //                            coliderOff();
    //                        }
    //                        if (cost >= obj.Length)
    //                        {
    //                            cost = 0;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    //建物を作成
    //    [Command]
    //    public void CmdMakeBildings(Vector3 pos, int cos)
    //    {
    //        GameObject building = Instantiate(obj[cos], pos, transform.rotation);
    //        NetworkServer.Spawn(building);
    //    }
    //}

    public class BldgData_Maneger : NetworkBehaviour
    {
        public static BldgData_Maneger Instance;  //instance
        public Ctrl.RecvCliantManeger sendCliantData;

        public GameObject[] obj;
        public GameObject[] colliderObj;
        public GameObject Cobj;
        public Vector3 makePos;
        public bool isBuild = false;


        //デバッグ用
        public int cost = 0;
        public bool isMouseUp = false;
        private void Awake()
        {
            Instance = this;
        }
        void Update()
        {
            if (!isLocalPlayer) return;     //他プレイヤーには何もしない
            if (Input.GetMouseButtonDown(0))        //クリックされていれば
            {

                UIMgr.instance.isNowClick = true;
                isMouseUp = false;
            }
            else
            {
                UIMgr.instance.isNowClick = false;
            }

            //if (Input.GetMouseButtonUp(0))
            //{
            //    isMouseUp = true;
            //}

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
                            //当たり判定を削除
                            if (Cobj != null)
                            {
                                coliderOff();
                            }
           
                            hit.transform.gameObject.GetComponent<TileData_Maneger>().planePos = new Vector2(hit.transform.position.x, hit.transform.position.z);
                            CmdSendClientData();
                            //Debug.Log(UIMgr.instance._UseCardData);

                            // Commandによってサーバーに通知する
                            //CmdMakeBildings(makePos, cost);
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

        //コライダーを作成する
        public void makeColider(int count, Vector3 pos, GameObject obj)
        {
            Cobj = Instantiate(colliderObj[count], pos, colliderObj[count].transform.rotation);
            Cobj.gameObject.GetComponent<Collider_Maneger>()._myCost = obj.GetComponent<BldgData>().myCost;
            Cobj.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        //コライダーを消す
        public void coliderOff()
        {
            Destroy(Cobj.gameObject);
        }

        //建物を作成
        [Command]
        public void CmdMakeBildings(Vector3 pos, int cos)
        {
            GameObject building = Instantiate(obj[cos], pos, transform.rotation);
            NetworkServer.Spawn(building);
        }

        ////クライアントからサーバへデータを渡す&受け取る
        //[Command]
        //public void CmdSendClientData(/*ClientData clientData*/)
        //{
        //    ClientData sendPlayerData;
        //    sendPlayerData = UIMgr.instance.useCardData;

        //    //リストにクライアントデータを加える
        //    //UIMgr.instance.useCardData.CardDataProxy = clientData.CardDataProxy;    //カードデータ :
        //    //UIMgr.instance.useCardData.PlayerNum     = clientData.PlayerNum;        //自分の番号   :　今はわからない
        //    //UIMgr.instance.useCardData.FieldPos      = clientData.FieldPos;         //置いた場所
        //    //UIMgr.instance.useCardData.CardPlayTime  = clientData.CardPlayTime;     //カードを置いたときの時間
        //    UIMgr.instance._UseCardData.Add(clientData);                            //リストに追加
        //    sendCliantData.CreateRecvObject();                                      //サーバが受け取る処理
        //}

        [ClientRpc]
        public void RpcRecvClientData()
        {

        }
    }
}