using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class ClientData : MonoBehaviour
    {
        public static event del_CliantDataModel eveClientData;  //クライアントの核心データ

        private CardDataProxy   _CardDataProxy; // 使ったカードのデータ
        private int             _PlayerNum;     // 使用したプレイヤー
        private float           _CardPlayTime;  // 使用したタイム
        private Vector2         _FieldPos;      // 配置したフィールドの座標

        
        public CardDataProxy CardDataProxy
        {
            get
            {
                return _CardDataProxy;
            }

            set
            {
                _CardDataProxy = value;
                //イベント関数使う
                if (eveClientData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("CardDataProxy", CardDataProxy);
                    eveClientData(kv);
                }
            }
        }


        public int PlayerNum
        {
            get
            {
                return _PlayerNum;
            }

            set
            {
                _PlayerNum = value;
                //イベント関数使う
                if (eveClientData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("PlayerNum", PlayerNum);
                    eveClientData(kv);
                }
            }
        }

        public float CardPlayTime
        {
            get
            {
                return _CardPlayTime;
            }

            set
            {
                _CardPlayTime = value;
                //イベント関数使う
                if (eveClientData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("CardPlayTime", CardPlayTime);
                    eveClientData(kv);
                }
            }
        }

        public Vector2 FieldPos
        {
            get
            {
                return _FieldPos;
            }

            set
            {
                _FieldPos = value;
                //イベント関数使う
                if (eveClientData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("FieldPos", FieldPos);
                    eveClientData(kv);
                }
            }
        }


        //私有構造関数定義
        private ClientData() { }

        //共有構造関数
        public ClientData(CardDataProxy cardDataProxy,
                                int playerNum,
                                float cardPlayTime,
                                Vector2 fieldPos)
        {
            this._CardDataProxy = cardDataProxy;
            this._PlayerNum = playerNum;
            this._CardPlayTime = cardPlayTime;
            this._FieldPos = fieldPos; ;
        }//構造関数_end

    }//class_end
}
