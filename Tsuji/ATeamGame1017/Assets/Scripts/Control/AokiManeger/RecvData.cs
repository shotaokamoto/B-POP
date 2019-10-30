using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class RecvData : MonoBehaviour
    {
        private RecvCliantDataProxy[] _Recv;  //各プレイヤーへ返す変数

        //デッキデータのゲッター、セッター
        public RecvCliantDataProxy[] Recv
        {
            get
            {
                return _Recv;
            }
            set
            {
                _Recv = value;
            }
        }
    }
}