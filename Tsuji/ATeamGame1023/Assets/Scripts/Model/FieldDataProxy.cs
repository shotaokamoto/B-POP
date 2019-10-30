using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class FieldDataProxy : FieldData
    {
        public static FieldDataProxy _Instance = null;

        /// <summary>
        /// 構造関数
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="PossessionPlayer"></param>
        /// <param name="MaptipNumber"></param>
        /// <param name="IsRankUp"></param>
        public FieldDataProxy(CardDataProxy Data,int PossessionPlayer,int MaptipNumber,bool IsRankUp,int RankID)
            :base(Data,PossessionPlayer,MaptipNumber,IsRankUp,RankID)
        {
            if(_Instance == null)
            {
                _Instance = this;
            }
            else
            {
             //   Debug.LogError(GetType() + "/FieldDataProxy()/構造関数重複使用は不可です、チェックしてください！");
            }
        }//FieldDataProxy_End

        /// <summary>
        /// このクラスの実数ゲット
        /// </summary>
        /// <returns></returns>
        public static FieldDataProxy GetInstance()
        {
            if(_Instance != null)
            {
                return _Instance;
            }
            else
            {
               /// Debug.LogWarning("/GetInstance()/先に構造関数使ってください");
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
            base.Data = base.Data;
            base.PossessionPlayer = base.PossessionPlayer;
            base.MaptipNumber = base.MaptipNumber;
            base.IsRankUp = base.IsRankUp;
            base.RankID = base.RankID;
        }
    }
}
