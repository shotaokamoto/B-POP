﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class FromData : MonoBehaviour
    {
        static private List<ClientDataProxy> _From;  //各プレイヤーへ返す変数

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