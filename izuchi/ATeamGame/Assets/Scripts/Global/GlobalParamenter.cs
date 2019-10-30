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

    } 


    #region enum定義

    /// <summary>
    /// player of enum type
    /// </summary>
    public enum PlayerType
    {
        Player1,
        Player2,
        Player3,
        Player4
    }
    #endregion


    #region プロジェクトのデリゲート定義

    /// <summary>
    /// カードの核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_CardDataModel(KeyValueUpdate kv);

    /// <summary>
    /// クライアントの核心データ
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

