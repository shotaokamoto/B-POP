using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class ServerData
    {
        //イベント事件定義：カードのデータ処理（委託事件）
        public static event del_ServerDataModel eveCardData;                                                              //カードの核心データ

        private int _Turn;      //現在のターン
        private PlayerDataProxy[] _PlayerDatas;     //プレイヤーデータ配列
        private FieldDataProxy[][] _FieldDatas;     //フィールドデータ配列
        private CardDataProxy[] _Decks;      //デッキ配列

        /*カード属性データ*/

        public int Turn
        {
            get
            {
                return _Turn;
            }

            set
            {
                _Turn = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Turn", Turn);
                    eveCardData(kv);
                }
            }
        }

        public PlayerDataProxy PlayerDatas
        {
            get
            {
                return _PlayerDatas[4];
            }

            set
            {
                _PlayerDatas[4] = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("PlayerDatas", PlayerDatas);
                    eveCardData(kv);
                }
            }
        }

        public FieldDataProxy FieldDatas
        {
            get
            {
                return _FieldDatas[8][8];
            }

            set
            {
                _FieldDatas[8][8] = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("FieldDatas", FieldDatas);
                    eveCardData(kv);
                }
            }
        }

        public CardDataProxy Decks
        {
            get
            {
                return _Decks[40];
            }

            set
            {
                _Decks[40] = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Decks", Decks);
                    eveCardData(kv);
                }
            }
        }

        //私有構造関数定義
        private ServerData() { }

        //共有構造関数
        public ServerData(int turn, PlayerDataProxy[] playerDataProxy, FieldDataProxy[][] fieldDataProxy, CardDataProxy[] decks)
        {
            this._Turn = turn;
            this._PlayerDatas = playerDataProxy;
            this._FieldDatas = fieldDataProxy;
            this._Decks = decks;
        }

    }
}
