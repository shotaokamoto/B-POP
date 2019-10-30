using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class PlayerDataProxy : PlayerData
    {
        public static PlayerDataProxy _Instance = null;

        /// <summary>
        /// 構造関数
        /// </summary>
        /// <param name="PlayerNumber"></param>
        /// <param name="TotalScore"></param>
        /// <param name="ScorePerTurn"></param>
        public PlayerDataProxy(int PlayerNumber,int TotalScore,int ScorePerTurn)
            :base(PlayerNumber,TotalScore,ScorePerTurn)
        {
            if(_Instance == null)
            {
                _Instance = this;
            }
            else
            {
                Debug.LogError(GetType() + "/PlayerDataProxy()/構造関数重複使用は不可です、チェックしてください！");
            }
        }//PlayerDataProxy_End

        /// <summary>
        /// このクラスの実数ゲット
        /// </summary>
        /// <returns></returns>
        public static PlayerDataProxy GetInstance()
        {
            if(_Instance != null)
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
            base.PlayerNumber = base.PlayerNumber;
            base.TotalScore = base.TotalScore;
            base.ScorePerTurn = base.ScorePerTurn;
        }
    }
}
