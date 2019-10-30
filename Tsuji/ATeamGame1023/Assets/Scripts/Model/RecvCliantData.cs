using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class RecvCliantData
    {
        //イベント事件定義：カードのデータ処理（委託事件）
        public static event del_RecvCliantDataModel eveCardData;                                                              //カードの核心データ

        public FieldDataProxy[,] _AllFeildData;   //全フィールドデータ
        public PlayerDataProxy[] _AllPlayerData;   //全プレイヤーデータ
        public int _NowTurn;                       //現在のターン
        public List<List<int>> _DrawDataList;      //描画メソッドに必要なデータ
        public List<CardDataProxy> _DrawCardList;  //引くカードのデータ

        /*カード属性データ*/
        public FieldDataProxy[,] AllFeildData
        {
            get
            {
                return _AllFeildData;
            }

            set
            {
                _AllFeildData = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("AllFeildData", AllFeildData);
                    eveCardData(kv);
                }
            }
        }

        public PlayerDataProxy[] AllPlayerData
        {
            get
            {
                return _AllPlayerData;
            }

            set
            {
                _AllPlayerData = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("AllPlayerData", AllPlayerData);
                    eveCardData(kv);
                }
            }
        }

        public int NowTurn
        {
            get
            {
                return _NowTurn;
            }

            set
            {
                _NowTurn = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("NowTurn", NowTurn);
                    eveCardData(kv);
                }
            }
        }

        public List<List<int>> DrawDataList
        {
            get
            {
                return _DrawDataList;
            }

            set
            {
                _DrawDataList = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("DrawDataList", DrawDataList);
                    eveCardData(kv);
                }
            }
        }

        public List<CardDataProxy> DrawCardList
        {
            get
            {
                return _DrawCardList;
            }

            set
            {
                _DrawCardList = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("DrawCardList", DrawCardList);
                    eveCardData(kv);
                }
            }
        }
       
        //私有構造関数定義
        private RecvCliantData() { }

        //共有構造関数
        public RecvCliantData(FieldDataProxy[,] allFieldData,PlayerDataProxy[] allPlayerData,int nowTurm,List<List<int>> drawDataList ,List<CardDataProxy> drawCardList)
        {
            this._AllFeildData = allFieldData;
            this._AllPlayerData = AllPlayerData;
            this._NowTurn = nowTurm;
            this._DrawDataList = drawDataList;
            this._DrawCardList = drawCardList;
        }

    }
}
