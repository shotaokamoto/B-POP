using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class PlayerData
    {
        //イベント事件定義：カードのデータ処理（委託事件）
        public static event del_PlayerDataModel eveCardData;                                                              //カードの核心データ

        private int _PlayerNumber;             //プレイヤーナンバー
        private int _TotalScore;         //合計スコア
        private int _ScorePerTurn;       //1ターン当たりの獲得スコア

        /*カード属性データ*/
        public int PlayerNumber
        {
            get
            {
                return _PlayerNumber;
            }

            set
            {
                _PlayerNumber = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("PlayerNumber", PlayerNumber);
                    eveCardData(kv);
                }
            }
        }

        public int TotalScore
        {
            get
            {
                return _TotalScore;
            }

            set
            {
                _TotalScore = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("TotalScore", TotalScore);
                    eveCardData(kv);
                }
            }
        }

        public int ScorePerTurn
        {
            get
            {
                return _ScorePerTurn;
            }

            set
            {
                _ScorePerTurn = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("ScorePerTurn", ScorePerTurn);
                    eveCardData(kv);
                }
            }
        }

        //私有構造関数定義
        private PlayerData() { }

        //共有構造関数
        public PlayerData(int playernumber,int totalscore,int scoreperturn)
        {
            this._PlayerNumber = playernumber;
            this._TotalScore = totalscore;
            this._ScorePerTurn = scoreperturn;
        }//共有構造関数_end

    }//class_end
}
