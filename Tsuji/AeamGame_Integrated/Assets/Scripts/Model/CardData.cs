/***
*   カードの内部データ
*   メソッドはCardDataProxyにあります
*   Update  2019/10/4
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class CardData
    {
        //イベント事件定義：カードのデータ処理（委託事件）
        public static event del_CardDataModel eveCardData;                                                              //カードの核心データ


        private int _CardNumber;    //カード番号
        private string _CardName;     //カードの名前
        private string _CardText;     //カードの説明、効果文
        private int _CardCost;      //カードのコスト
        private int _CardScore;     //カードのスコア
        private int _CardType;      //カードのタイプ

        /*カード属性データ*/
        public int Number
        {
            get
            {
                return _CardNumber;
            }

            set
            {
                _CardNumber = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Number", Number);
                    eveCardData(kv);
                }
            }
        }

        public string Name
        {
            get
            {
                return _CardName;
            }

            set
            {
                _CardName = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Name", Name);
                    eveCardData(kv);
                }
            }
        }

        public string Text
        {
            get
            {
                return _CardText;
            }

            set
            {
                _CardText = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Text", Text);
                    eveCardData(kv);
                }
            }
        }

        public int Cost
        {
            get
            {
                return _CardCost;
            }

            set
            {
                _CardCost = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Cost", Cost);
                    eveCardData(kv);
                }
            }
        }

        public int Score
        {
            get
            {
                return _CardScore;
            }

            set
            {
                _CardScore = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Score", Score);
                    eveCardData(kv);
                }
            }
        }

        public int Type
        {
            get
            {
                return _CardType;
            }

            set
            {
                _CardType = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("Type", Type);
                    eveCardData(kv);
                }
            }
        }

        //私有構造関数定義
        private CardData() { }

        //共有構造関数
        public CardData(int number, string name, string text, int cost,int score,int type)
        {
            this._CardNumber = number;
            this._CardName = name;
            this._CardCost = cost;
            this._CardScore = score;
            this._CardType = type;
            this._CardText = text;

        }//共有構造関数_end

    }//class_end
}
