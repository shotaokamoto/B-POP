/***
*
*       Title :"B-POP"
*
*        グローバル層：グローバルパラメータ定義
*
*       Description:
*               ①グローバルenum定義
*               ②グローバルデリゲート定義
*               ③グローバルパラメータ定義
*         　  ④システムの中の全てのTAG定義
*         　  
*       Data : 2019.10.09
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    /*システムパラメータ定義*/
    public class GloabalParamenter
    {

    }

    #region プロジェクトのenum定義

    public enum ScenesEnum
    {
        StartScenes,                                            //スタートシーン
        LoadingScenes,                                      //ローディングシーン
        GameScenes,                                         //ゲームシーン
        LogonScenes,                                         //ろ
    }
    #endregion

    #region プロジェクトのデリゲート定義
    /// <summary>
    /// キャラクターコントローラーデリゲートstring
    /// </summary>
    /// <param name="controlType">コントロールタイプ</param>
    public delegate void del_PlayerCtrlWithStr(string controlType);

    /// <summary>
    /// キャラクターの核心データ
    /// </summary>
    /// <param name="kv"></param>
    public delegate void del_PlayerKernalModel(KeyValueUpdate kv);

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

