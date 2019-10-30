/***
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
*       Data : 2019.10/23 publicにへんこう
*              構造体を追加
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Global
{
    public class GlobalParamenter:MonoBehaviour
    {
        //イベント事件定義：グローバルのデータ処理（委託事件）
        public static event del_CardDataModel eveCardData;                                                              //カードの核心データ

        public int playerSize;
        public Vector2Int fieldSize;
        private int deckSize;
        public int maxTurn = 10;
        public int fieldsizeX = 0;
        public int fieldsizeY = 0;
        //-----------------------------------
        //サイズをUnityからいじれるようにしたい
        //-----------------------------------
        public GlobalParamenter(int x, int y)
        {
            fieldSize.x = x;
            fieldSize.y = y;
        }

        /*カード属性データ*/
        public int PlayerSize
        {
            get
            {
                return playerSize;
            }

            set
            {
                playerSize = value;
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
                return fieldSize;
            }

            set
            {
                fieldSize = value;
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
                return deckSize;
            }

            set
            {
                deckSize = value;
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
                return maxTurn;
            }

            set
            {
                maxTurn = value;
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

    //enumのシーン
    public enum ScenesEnum
    {
        StartScenes,                                            //スタートシーン
        LoadingScenes,                                      //ローディングシーン
        GameScenes,                                         //ゲームシーン
        LogonScenes,                                         //ログイン画面
    }

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
        DrawObj = 0,
    }

    #endregion
    //---------------------------------
    //　ここ追加してください
    //---------------------------------
    #region プロジェクトの構造体宣言
    public struct SendClientDataArray
    {
        //カードデータ 
        public int Number;
        public string Name;
        public string Text;
        public int Cost;
        public int Score;
        public int Type;
        public int Rarity;

        public int _PlayerNum;     // 使用したプレイヤー
        public float _CardPlayTime;  // 使用したタイム
        public Vector2 _FieldPos;      // 配置したフィールドの座標
    }

    //サーバからクライアントにデータを送る際に必要な変換構造体
    //カードデータ構造体
    public struct SendCardDataArray
    {
        public int Number;
        public string Name;
        public string Text;
        public int Cost;
        public int Score;
        public int Type;
        public int Rarity;
    }
    //fieldデータ構造体
    public struct SendFieldDataArray
    {
        //カードデータ
        public SendCardDataArray SendCardData;
        public int PossessionPlayer;
        public int MaptipNumber;
        public bool IsRankUp;
        public int RankID;
    }
    //playerデータ構造体
    public struct SendPlayerDataArray
    {
        public int PlayerNumber;             //プレイヤーナンバー
        public int TotalScore;         //合計スコア
        public int ScorePerTurn;       //1ターン当たりの獲得スコア
        public int Ranking;           //プレイヤー順位
    }
    //配列構造体
    public struct IntArrayStructure
    {
        public int[] vs;
    }

    public struct SendSeverDataArray
    {
        public SendFieldDataArray[] AllFeildData;    //全フィールドデータ 
        public SendPlayerDataArray[]  AllPlayerData; //全プレイヤーデータ
        public int NowTurn;                          //現在のターン
        public IntArrayStructure[] DrawDataList;     //描画メソッドに必要なデータ
        public SendCardDataArray[] DrawCardList;     //引くカードのデータ
        public bool isRecvFinished;                  //データを受け取ったかどうか
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
