using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class RecvCliantDataProxy : RecvCliantData
    {
        public static RecvCliantDataProxy _Instance = null;

        /// <summary>
        /// 構造関数
        /// </summary>
        /// <param name="AllFieldData"></param>
        /// <param name="AllPlayerData"></param>
        /// <param name="NowTurn"></param>
        /// <param name="DrawDataList"></param>
        public RecvCliantDataProxy(FieldDataProxy[,] AllFieldData, PlayerDataProxy[] AllPlayerData, int NowTurn, List<List<int>> DrawDataList, List<CardDataProxy> DrawCardList)
            : base(AllFieldData, AllPlayerData, NowTurn, DrawDataList, DrawCardList)
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
            else
            {
                Debug.LogError(GetType() + "/RecvCliantDataProxy()/構造関数重複使用は不可です、チェックしてください！");
            }
        }///RecvCliantDataProxy_End

        /// <summary>
        /// このクラスの実例ゲット
        /// </summary>
        /// <returns></returns>
        public static RecvCliantDataProxy GetInstance()
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

        public void DisplayALLOriginalValues()
        {
            base.AllFeildData = base.AllFeildData;
            base.AllPlayerData = base.AllPlayerData;
            base.NowTurn = base.NowTurn;
            base.DrawDataList = base.DrawDataList;
            base.DrawCardList = base.DrawCardList;
        }
    }
}
