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
        private GameObject _AllCard;

        public string[] DeckName = new string[] { "レストラン", "タピオカ屋", "スーパーマーケット", "遊園地", "地雷", "ワイルドカード", "好況/不況", "台風", "シャッフル", "メテオ" };
        public int[] FirstEmissionRate = new int[] { 14, 14, 20, 20, 3, 12, 0, 2, 14, 1 };
        public int[] SecondEmissionRate = new int[] { 10, 10, 20, 20, 5, 18, 0, 3, 12, 2 };
        public int[] TherdEmissionRate = new int[] { 5, 5, 32, 22, 10, 14, 0, 7, 1, 4 };
        public int[] FourthEmissionRate = new int[] { 1, 1, 24, 20, 16, 12, 0, 14, 2, 10 };

        // Start is called before the first frame update
        void Start()
        {
            _AllCard = GameObject.FindGameObjectWithTag("AllCard");
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
            System.Random r = new System.Random((int)DateTime.Now.Ticks);   //秒数によってシード値を確定
            int num = r.Next(Maximam - 1);
            while(!confirm)
            {
                num = r.Next(Maximam - 1);
                if (_Deck[num].IsUse == false)
                {
                    DrawCard = _Deck[num].CardData;
                    _Deck[num].IsUse = true;
                    confirm = true;

                    return DrawCard;
                }
            }
            return null;
        }


        public CardDataProxy DrawCardRateConfirm(int Ranking)
        {
            bool confirm = false;
            CardDataProxy DrawCard;
            int Maximam = _Deck.Count();

            System.Random r = new System.Random((int)DateTime.Now.Ticks);   //秒数によってシード値を確定
            int num = r.Next(100);

            //順位によって排出率を変更
            switch(Ranking)
            {
                case 1:
                    //好況カードを除いたのでー１にしています
                    for (int i = 0; i < DeckName.Length; i++)
                    {
                        _AllCard.GetComponent<CardInfo>().Card[i].CardRarityEntity = FirstEmissionRate[i];
                    }
                    break;

                case 2:
                    //好況カードを除いたのでー１にしています
                    for (int i = 0; i < DeckName.Length; i++)
                    {
                        _AllCard.GetComponent<CardInfo>().Card[i].CardRarityEntity = SecondEmissionRate[i];
                    }

                    break;

                case 3:
                    //好況カードを除いたのでー１にしています
                    for (int i = 0; i < DeckName.Length; i++)
                    {
                        _AllCard.GetComponent<CardInfo>().Card[i].CardRarityEntity = TherdEmissionRate[i];
                    }

                    break;

                case 4:
                    //好況カードを除いたのでー１にしています
                    for (int i = 0; i < DeckName.Length; i++)
                    {
                        _AllCard.GetComponent<CardInfo>().Card[i].CardRarityEntity = FourthEmissionRate[i];
                    }

                    break;

                default:
                    Debug.Log("ドローカードを決定する際に" + Ranking + "位の人がいました");
                    break;
            }

            //ここでカードの確定
            while (!confirm)
            {
                //確率用変数
                int Rate = 0;

                //チェック範囲
                int CheckRange = 0;

                //ランダム数値
                num = r.Next(100);

                //カード検索
                for (int i = 0; i < DeckName.Length; i++)
                {
                    //排出率をプラス
                    Rate += _AllCard.GetComponent<CardInfo>().Card[i].CardRarityEntity;
                    CheckRange += _AllCard.GetComponent<CardInfo>().Card[i].CardMaximumEntity;

                    if (Rate > num)
                    {
                        for (int check = 0; check < _AllCard.GetComponent<CardInfo>().Card[i].CardMaximumEntity; check++)
                        {
                            if (_Deck[CheckRange + check].IsUse == false) 
                            {
                                DrawCard = _Deck[CheckRange + check].CardData;
                                _Deck[CheckRange + check].IsUse = true;
                                confirm = true;

                                CardDataProxy ReturnCard = new CardDataProxy(_AllCard.GetComponent<CardInfo>().Card[i].CardNumberEntity, _AllCard.GetComponent<CardInfo>().Card[i].CardNameEntity, _AllCard.GetComponent<CardInfo>().Card[i].CardTextEntity,
                                                                                _AllCard.GetComponent<CardInfo>().Card[i].CardCostEntity, _AllCard.GetComponent<CardInfo>().Card[i].CardScoreEntity, _AllCard.GetComponent<CardInfo>().Card[i].CardTypeEntity, _AllCard.GetComponent<CardInfo>().Card[i].CardRarityEntity);
                                return ReturnCard;
                            }
                        }
                    }
                }
                return null;
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