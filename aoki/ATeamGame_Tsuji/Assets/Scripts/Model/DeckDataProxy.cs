using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Model
{
    public class DeckDataProxy : DeckData
    {
        public static DeckDataProxy _Instance = null;

        /// <summary>
        /// 構造関数
        /// </summary>
        /// <param name="CardData"></param>
        /// <param name="IsUse"></param>
        public DeckDataProxy(CardDataProxy CardData, bool IsUse)
            : base(CardData, IsUse)
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
            else
            {
                Debug.LogError(GetType() + "/DeckDataProxy()/構造関数重複使用は不可です、チェックしてください！");
            }
        }//DeckDataProxy_End

        /// <summary>
        /// このクラスの実例ゲット
        /// </summary>
        /// <returns></returns>
        public static DeckDataProxy GetInstance()
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
            base.CardData = base.CardData;
            base.IsUse = base.IsUse;
        }
    }
}
