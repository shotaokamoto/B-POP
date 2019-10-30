using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class ClientDataProxy : ClientData
    {
        public static ClientDataProxy _Instance = null;


        /// <summary>
        /// 構造関数
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="playerNum"></param>
        /// <param name="cardPlayTime"></param>
        /// <param name="fieldCoordinates"></param>
        public ClientDataProxy(CardDataProxy cardDataProxy,
                                int playerNum,
                                float cardPlayTime,
                                Vector2 fieldPos
                                ):base(cardDataProxy, playerNum, cardPlayTime, fieldPos)
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
            else
            {
                Debug.LogError(GetType() + "/ClientDataProxy()/構造関数重複使用は不可です、チェックしてください！");
            }
        }// ClientDataProxy_end


        /// <summary>
        /// このクラスの実例ゲット
        /// </summary>
        /// <returns></returns>
        public static ClientDataProxy GetInstance()
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


        #region カードデータの操作
        /// <summary>
        /// 使ったカードのデータ
        /// </summary>
        public void GetCardDataProxy()
        {
            CardDataProxy._Instance.Number = CardModel.instance._CardNumber;
            CardDataProxy._Instance.Name = CardModel.instance._CardName;
            CardDataProxy._Instance.Text = CardModel.instance._CardText;
            CardDataProxy._Instance.Cost = CardModel.instance._CardCost;
            CardDataProxy._Instance.Score = CardModel.instance._CardScore;
            CardDataProxy._Instance.Type = CardModel.instance._CardType;

        }
        #endregion


        #region プレイヤーの操作
        /// <summary>
        /// 使用したプレイヤー
        /// </summary>
        public void GetPlayerNum()
        {
            
        }
        #endregion


        #region カード時間の操作
        /// <summary>
        /// カードを使用した時間
        /// </summary>
        public void GetCardPlayTime()
        {
            
        }
        #endregion


        #region カード座標の操作
        /// <summary>
        /// カードを配置したフィールドの座標
        /// </summary>
        public void GetFieldPos()
        {
            
        }
        #endregion


        /// <summary>
        /// 全ての初期値を表す
        /// </summary>
        public void DisplayALLOriginalValues()
        {
            base.CardDataProxy = base.CardDataProxy;
            base.PlayerNum = base.PlayerNum;
            base.CardPlayTime = base.CardPlayTime;
            base.FieldPos = base.FieldPos;
        }

    }

}
