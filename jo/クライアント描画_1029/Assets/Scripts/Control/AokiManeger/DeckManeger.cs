using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;
using System;
using System.Linq;

namespace Ctrl
{
    public class DeckManeger : MonoBehaviour
    {
        private List<DeckDataProxy> _Deck = new List<DeckDataProxy>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        //デッキデータのゲッター、セッター
        public List<DeckDataProxy> Deck
        {
            get
            {
                return _Deck;
            }
            set
            {
                _Deck = value;
            }
        }

        /// <summary>
        /// 引くカードの確定
        /// </summary>
        public CardDataProxy DrawCardConfirm(int Maximam)
        {
            bool confirm = false;
            CardDataProxy DrawCard;
            System.Random r = new System.Random(DateTime.Now.Second);   //秒数によってシード値を確定
            int num = r.Next(Maximam - 1);
            while(confirm)
            {
                if (!_Deck[num].IsUse)
                {
                    DrawCard = _Deck[num].CardData;
                    _Deck[num].IsUse = true;
                    confirm = true;

                    return DrawCard;
                }
            }
            return null;
        }

        /// <summary>
        /// デッキにカードを戻す
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool ReturnCardToDeck(int number)
        {
            for (int i = 0; i < _Deck.Count(); i++)
            {
                //デッキから抜けているカードを発見
                if(_Deck[i].CardData.Number == number && _Deck[i].IsUse)
                {
                    _Deck[i].IsUse = false;
                    return true;
                }
            }

            //データが見つからなかった
            return false;
        }
    }
}