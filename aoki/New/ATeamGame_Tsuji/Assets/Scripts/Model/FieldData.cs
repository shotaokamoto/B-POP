/***
*   フィールドの内部データ
*   メソッドはFieldDataProxyにあります
*   Update  2019/10/7
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class FieldData
    {
        //イベント事件定義：カードのデータ処理（委託事件）
        public static event del_FieldDataModel eveCardData;                                                              //カードの核心データ

        public CardDataProxy _CardData;
        public int _PossessionPlayer;
        public int _MaptipNumber;
        public bool _IsRankUp;
        public int _RankID;

        /*カード属性データ*/
        public CardDataProxy Data
        {
            get
            {
                return _CardData;
            }

            set
            {
                _CardData = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Data", Data);
                    eveCardData(kv);
                }
            }
        }

        public int PossessionPlayer
        {
            get
            {
                return _PossessionPlayer;
            }

            set
            {
                _PossessionPlayer = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("PossessionPlayer", PossessionPlayer);
                    eveCardData(kv);
                }
            }
        }

        public int MaptipNumber
        {
            get
            {
                return _MaptipNumber;
            }

            set
            {
                _MaptipNumber = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("MaptipNumber", MaptipNumber);
                    eveCardData(kv);
                }
            }
        }

        public bool IsRankUp
        {
            get
            {
                return _IsRankUp;
            }

            set
            {
                _IsRankUp = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("IsRankUp", IsRankUp);
                    eveCardData(kv);
                }
            }
        }

        public int RankID
        {
            get
            {
                return _RankID;
            }

            set
            {
                _RankID = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("RankID", RankID);
                    eveCardData(kv);
                }
            }
        }

        //私有構造関数定義
        private FieldData() { }

        //共有構造関数
        public FieldData(CardDataProxy cardData, int possessionPlayer, int maptipNumber, bool isRankUp, int rankID)
        {
            this._CardData = cardData;
            this._PossessionPlayer = possessionPlayer;
            this._MaptipNumber = maptipNumber;
            this._IsRankUp = isRankUp;
            this._RankID = rankID;
        }
        //共有構造関数_end
    }//class_end
}
