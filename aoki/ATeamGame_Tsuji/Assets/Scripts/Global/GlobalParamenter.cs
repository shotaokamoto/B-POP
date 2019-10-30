﻿/***
*
*       Title :"ATeam　α版"
*
*        グローバル層：グローバルパラメータ
*
*       Description:
*               ①グローバルenum定義
*               ②グローバルデリゲート定義
*               ③グローバルパラメータ定義
*         　  ④システムの中の全てのTAG定義
*       Data : 2019.
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Global
{
    public class GlobalParamenter
    {
        //イベント事件定義：グローバルのデータ処理（委託事件）
        public static event del_CardDataModel eveCardData;                                                              //カードの核心データ

        private int _PlayerSize;
        private Vector2Int _FieldSize;
        private int _DeckSize;
        private int _MaxTurn;

        public GlobalParamenter()
        {
            _FieldSize.x = 8;
            _FieldSize.y = 8;
        }

        /*カード属性データ*/
        public int PlayerSize
        {
            get
            {
                return _PlayerSize;
            }

            set
            {
                _PlayerSize = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("PlayerSize", PlayerSize);
                    eveCardData(kv);
                }
            }
        }

        public Vector2Int FieldSize
        {
            get
            {
                return _FieldSize;
            }

            set
            {
                _FieldSize = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("FieldSize", FieldSize);
                    eveCardData(kv);
                }
            }
        }

        public int DeckSize
        {
            get
            {
                return _DeckSize;
            }

            set
            {
                _DeckSize = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("DeckSize", DeckSize);
                    eveCardData(kv);
                }
            }
        }

        public int MaxTurn
        {
            get
            {
                return _MaxTurn;
            }

            set
            {
                _MaxTurn = value;
                //イベント関数使う
                if (eveCardData != null)
                {
                    KeyValueUpdate kv = new KeyValueUpdate("MaxTurn", MaxTurn);
                    eveCardData(kv);
                }
            }
        }

    }

    #region enum定義

    /// <summary>
    /// player of enum type
    /// </summary>
    public enum PlayerType
    {
        Player1 = 0,
        Player2,
        Player3,
        Player4,
        EndPlayer,
    }


    public enum CardType
    {
        Object = 0,
        Action,
        CardTypeEnd,
    }

    public enum DrawFaze
    {
        DrawEnd = -1,
        DrawAct = 0,
        DrawBatting,
        DrawObj,
        DrawRankUp,
        DrawClosed,
    }

    #endregion


    #region プロジェクトのデリゲート定義

    /// <summary>
    /// グローバルの核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_GlobalDataModel(KeyValueUpdate kv);

    /// <summary>
    /// カードの核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_CardDataModel(KeyValueUpdate kv);

    /// <summary>
    /// フィールド(1マス)の核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_FieldDataModel(KeyValueUpdate kv);

    /// <summary>
    /// プレイヤーの核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_PlayerDataModel(KeyValueUpdate kv);

    /// <summary>
    /// サーバーの核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_ServerDataModel(KeyValueUpdate kv);

    /// <summary>
    /// デッキの核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_DeckDataModel(KeyValueUpdate kv);

    /// <summary>
    /// クライアントに返す核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_RecvCliantDataModel(KeyValueUpdate kv);

    /// <summary>
    /// クライアントからもらう核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_CliantDataModel(KeyValueUpdate kv);

    /// <summary>
    /// key and value 対応更新
    /// </summary>
    public class KeyValueUpdate
    {
        private string _Key;              //key
        private object _Values;        //value

        /*const属性*/
        public string Key
        {
            get
            {
                return _Key;
            }
        }
        public object Values
        {
            get
            {
                return _Values;
            }
        }

        public KeyValueUpdate(string key, object value)
        {
            _Key = key;
            _Values = value;
        }
    }

    #endregion

}

