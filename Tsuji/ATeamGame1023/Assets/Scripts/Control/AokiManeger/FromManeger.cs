using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;

/********************
 *
 *大幅に変更しました   
 *    
 ********************/

namespace Ctrl
{
    public class FromManeger : MonoBehaviour
    {
        public static FromManeger Instance;     //インスタンス化

        private GameObject _ServerMgrObj;
        static private GameObject[] _FromDataObj;

        private void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            _ServerMgrObj = GameObject.FindGameObjectWithTag("Server");
            _FromDataObj = GameObject.FindGameObjectsWithTag("FromData");
            //---------------------------------------------------------------------
            //ここで_FromDataObjをnullにしていたせいでエラーが出たので消しました
            //---------------------------------------------------------------------
            //for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            //{
            //    _FromDataObj[player] = null;
            //}
        }

        public GameObject Test(int player)
        {
            return _FromDataObj[player];
        }

        public void SetFromDataToServerMgr(ClientData sendData,int num)
        {
            _FromDataObj[num].GetComponent<FromData>().isSendData = true;       //データを送っているかどうかのフラグ(カードデータがなくてもサーバ側で処理をするためのフラグ)
            //サーバのクライアントリスト配列にデータを入れる
            _FromDataObj[num].GetComponent<FromData>()._FromData.Add(
                               new ClientData(sendData._CardDataProxy,
                                              sendData._PlayerNum,
                                              sendData._CardPlayTime,
                                              sendData._FieldPos
                                              ));
        }
        //FromDataObjのゲッター、セッター
        public GameObject[] FromDataObj
        {
            get
            {
                return _FromDataObj;
            }
            set
            {
                _FromDataObj = value;
            }
        }
    }
}