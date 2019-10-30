using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class FromData : MonoBehaviour
    {
        private List<ClientDataProxy> _From;  //各プレイヤーへ返す変数

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