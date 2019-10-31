using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class FromData : MonoBehaviour
    {
        public List<ClientDataProxy> _From;  //各プレイヤーへ返す変数(publicに変換)
        public List<ClientData> _FromData = new List<ClientData>();  //サーバからの情報を受け取るためのリスト
        public bool isSendData = false;

        private void Awake()
        {
            _From = new List<ClientDataProxy>() { null };
        }

        //デッキデータのゲッター、セッター
        public List<ClientDataProxy> From
        {
            get
            {
                return _From;
            }
            set
            {
                _From = value;
            }
        }
    }
}