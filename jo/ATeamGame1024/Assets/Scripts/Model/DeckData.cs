using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class DeckData
    {
        //イベント事件定義：カードのデータ処理（委託事件）
        public static event del_DeckDataModel eveCardData;                                                              //カードの核心データ

        private CardDataProxy _CardData;    //カードのデータ
        private bool _IsUse;                //現在使用中か

        /*カード属性データ*/
        public CardDataProxy CardData
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
                    KeyValueUpdate kv = new KeyValueUpdate("CardData", CardData);
                    eveCardData(kv);
                }
            }
        }

        public bool IsUse
        {
            get
            {
                return _IsUse;
            }

            set
            {
                _IsUse = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("IsUse", IsUse);
                    eveCardData(kv);
                }
            }
        }

        //私有構造関数定義
        private DeckData() { }

        //共有構造関数
        public DeckData(CardDataProxy cardData,bool isUse)
        {
            this._CardData = cardData;
            this._IsUse = isUse;
        }
    }
}