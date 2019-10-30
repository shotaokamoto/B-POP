using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class ServerDataProxy : ServerData
    {
        public static ServerDataProxy _Instance = null;

        /// <summary>
        /// 構造関数
        /// </summary>
        /// <param name="Turn"></param>
        /// <param name="PlayerDatas"></param>
        /// <param name="FieldDatas"></param>
        /// <param name="Decks"></param>
        public ServerDataProxy(int Turn,PlayerDataProxy[] PlayerDatas ,FieldDataProxy[][] FieldDatas, CardDataProxy[] Decks)
            :base(Turn, PlayerDatas, FieldDatas, Decks)
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
            //else
            //{
            //    Debug.LogError(GetType() + "/ServerDataProxy()/構造関数重複使用は不可です、チェックしてください！");
            //}
        }

        /// <summary>
        /// このクラスの実数ゲット
        /// </summary>
        /// <returns></returns>
        public static ServerDataProxy GetInstance()
        {
            if (_Instance != null)
            {
                return _Instance;
            }
            else
            {
                Debug.LogWarning("/GetInstance()/先に構造関数使ってください");
                return null;
            }
        }//GetInstance_end

        #region 操作系
        #endregion

        /// <summary>
        /// 全ての初期値を返す
        /// </summary>
        public void DisplayALLOriginalValues()
        {
            base.Turn = base.Turn;
            base.PlayerDatas = base.PlayerDatas;
            base.FieldDatas = base.FieldDatas;
            base.Decks = base.Decks;
        }
    }
}