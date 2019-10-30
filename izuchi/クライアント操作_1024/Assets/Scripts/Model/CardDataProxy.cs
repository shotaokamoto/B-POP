using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Model
{
    public class CardDataProxy : CardData
    {
        public static CardDataProxy _Instance = null;

        /// <summary>
        /// 構造関数
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Name"></param>
        /// <param name="Text"></param>
        /// <param name="Cost"></param>
        /// <param name="Score"></param>
        /// <param name="Type"></param>
        public CardDataProxy(int Number, string Name, string Text, int Cost, int Score, int Type,int Rarity)
            : base(Number, Name, Text, Cost, Score, Type , Rarity)
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
            else
            {
                //Debug.LogError(GetType() + "/CardDataProxy()/構造関数重複使用は不可です、チェックしてください！");
            }
        }//CardDataProxy_End

        /// <summary>
        /// このクラスの実例ゲット
        /// </summary>
        /// <returns></returns>
        public static CardDataProxy GetInstance()
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

        #region テスト操作

        /// <summary>
        /// ナンバー値調整(本番ではナンバーは値を変えない)
        /// </summary>
        /// <param name="number">増加値</param>
        public void AddNumber(int number)
        {
            base.Number += number;
        }

        /// <summary>
        /// ナンバー値調整(本番ではナンバーは値を変えない)
        /// </summary>
        /// <param name="name">変更する名前</param>
        public void ChangeName(string name)
        {
            base.Name = name;
        }

        /// <summary>
        /// ナンバー値調整(本番ではナンバーは値を変えない)
        /// </summary>
        /// <param name="name">変更するテキスト</param>
        public void ChangeText(string text)
        {
            base.Text = text;
        }

        /// <summary>
        /// ナンバー値調整(本番ではナンバーは値を変えない)
        /// </summary>
        /// <param name="number">増加値</param>
        public void AddCost(int cost)
        {
            base.Cost += cost;
        }

        /// <summary>
        /// ナンバー値調整(本番ではナンバーは値を変えない)
        /// </summary>
        /// <param name="number">増加値</param>
        public void AddScore(int score)
        {
            base.Score += score;
        }

        /// <summary>
        /// ナンバー値調整(本番ではナンバーは値を変えない)
        /// </summary>
        /// <param name="number">増加値</param>
        public void ChangeType(int type)
        {
            base.Type = type;
        }

        #endregion

        #region 操作系

        #endregion

        /// <summary>
        /// 全ての初期値を返す
        /// </summary>
        public void DisplayALLOriginalValues()
        {
            base.Number = base.Number;
            base.Name = base.Name;
            base.Text = base.Text;
            base.Cost = base.Cost;
            base.Score = base.Score;
            base.Type = base.Type;
            base.Rarity = base.Rarity;
        }

        public static implicit operator int(CardDataProxy v)
        {
            throw new NotImplementedException();
        }
    }
}
