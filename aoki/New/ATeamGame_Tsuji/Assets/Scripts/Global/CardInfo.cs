using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public enum CardName
    {
        DrawEnd = -1,       //描画終了コマンド

        Restaurants = 0,
        TapiocaShop,
        Supermarket,
        AmusementPark,

        Mine,
        WildCard,
        RecessionOrBoom,
        Typhoon,
        Shuffle,
        Meteo,

        CardEnd,

        TouristSpot = 20,
        WildObj,
        OjamaRock,

        DrawObjBatting = 99,    //オブジェクトバッティングコマンド
        DrawObjAlready = 100,   //オブジェクトが既にあった時のコマンド
        DrawActionEnd = 110,    //アクションカード描画エンド
        DrawObjEnd = 111,       //オブジェクト描画終了コマンド
        DrawBattingEnd = 200,   //バッティング描画終了コマンド
        MineBoom = 201,     //地雷爆発コマンド
        DrawMineBoomEnd = 202,     //地雷爆発描画終了コマンド
        DrawRankUpEnd = 210,    //ランクアップ描画終了コマンド
        DrawClosedEnd = 211,    //廃業描画終了コマンド
        DrawWildCheckEnd = 212,     //ワイルドカードのチェック終了コマンド
        DrawDelOjamaRockEnd = 213,  //おジャマ岩削除コマンド
    }

    #region カード排出率（順位別）

    public enum FirstEmissionRate
    {
        Restaurants = 14,
        TapiocaShop = 14,
        Supermarket = 20,
        AmusementPark = 20,

        Mine = 3,
        WildCard = 12,
        Typhoon= 2,
        Shuffle = 14,
        Meteo = 1,
    }

    public enum SecondEmissionRate
    {
        Restaurants = 10,
        TapiocaShop = 10,
        Supermarket = 20,
        AmusementPark = 20,

        Mine = 5,
        WildCard = 18,
        Typhoon = 3,
        Shuffle = 12,
        Meteo = 2,
    }

    public enum TherdEmissionRate
    {
        Restaurants = 5,
        TapiocaShop = 5,
        Supermarket = 32,
        AmusementPark = 22,

        Mine = 10,
        WildCard = 14,
        Typhoon = 7,
        Shuffle = 1,
        Meteo = 4,
    }

    public enum FourthEmissionRate
    {
        Restaurants = 1,
        TapiocaShop = 1,
        Supermarket = 24,
        AmusementPark = 20,

        Mine = 16,
        WildCard = 12,
        Typhoon = 14,
        Shuffle = 2,
        Meteo = 10,
    }


    #endregion

    public struct _CardInfo
    {
        public int number;
        public string name;
        public string text;
        public int cost;
        public int score;
        public int type;
        public int rarity;
        public int Maximum;
    }

    public class CardInfo : MonoBehaviour
    {
        public List<_CardInfo> AllCard = new List<_CardInfo>();      //カード全種類リスト

        public CardEntity[] Card;

        public _CardInfo Restaurants = new _CardInfo();
        public _CardInfo TapiocaShop = new _CardInfo();
        public _CardInfo Supermarket = new _CardInfo();
        public _CardInfo AmusementPark = new _CardInfo();

        public _CardInfo Mine = new _CardInfo();
        public _CardInfo WildCard = new _CardInfo();
        public _CardInfo RecessionOrBoom = new _CardInfo();
        public _CardInfo Typhoon = new _CardInfo();
        public _CardInfo Shuffle = new _CardInfo();
        public _CardInfo Meteo = new _CardInfo();

        public _CardInfo TouristSpot = new _CardInfo();
        public _CardInfo WildObj = new _CardInfo();

        private void Start()
        {
            //建物カード作成
            InitRestaurants();
            InitTapiocaShop();
            InitSupermarket();
            InitAmusementPark();

            //アクションカード作成
            InitMine();
            InitWildCard();
            InitRecessionOrBoom();
            InitTyphoon();
            InitShuffle();
            InitMeteo();

            //デッキには入らない特殊カード
            InitTouristSpot();
            InitWildObj();
        }

        private void InitRestaurants()
        {
            Restaurants.number =  Card[0].CardNumberEntity;
            Restaurants.name =    Card[0].CardNameEntity;
            Restaurants.text =    Card[0].CardTextEntity;
            Restaurants.cost =    Card[0].CardCostEntity;
            Restaurants.score =   Card[0].CardScoreEntity;
            Restaurants.type =    Card[0].CardTypeEntity;
            Restaurants.rarity =  Card[0].CardRarityEntity;
            Restaurants.Maximum = Card[0].CardMaximumEntity;

            AllCard.Add(Restaurants);
        }

        private void InitTapiocaShop()
        {
            TapiocaShop.number =  Card[1].CardNumberEntity;
            TapiocaShop.name =    Card[1].CardNameEntity;
            TapiocaShop.text =    Card[1].CardTextEntity;
            TapiocaShop.cost =    Card[1].CardCostEntity;
            TapiocaShop.score =   Card[1].CardScoreEntity;
            TapiocaShop.type =    Card[1].CardTypeEntity;
            TapiocaShop.rarity =  Card[1].CardRarityEntity;
            TapiocaShop.Maximum = Card[1].CardMaximumEntity;

            AllCard.Add(TapiocaShop);
        }

        private void InitSupermarket()
        {
            Supermarket.number =  Card[2].CardNumberEntity;
            Supermarket.name =    Card[2].CardNameEntity;
            Supermarket.text =    Card[2].CardTextEntity;
            Supermarket.cost =    Card[2].CardCostEntity;
            Supermarket.score =   Card[2].CardScoreEntity;
            Supermarket.type =    Card[2].CardTypeEntity;
            Supermarket.rarity =  Card[2].CardRarityEntity;
            Supermarket.Maximum = Card[2].CardMaximumEntity;

            AllCard.Add(Supermarket);
        }

        private void InitAmusementPark()
        {
            AmusementPark.number =  Card[3].CardNumberEntity;
            AmusementPark.name =    Card[3].CardNameEntity;
            AmusementPark.text =    Card[3].CardTextEntity;
            AmusementPark.cost =    Card[3].CardCostEntity;
            AmusementPark.score =   Card[3].CardScoreEntity;
            AmusementPark.type =    Card[3].CardTypeEntity;
            AmusementPark.rarity =  Card[3].CardRarityEntity;
            AmusementPark.Maximum = Card[3].CardMaximumEntity;

            AllCard.Add(AmusementPark);
        }

        private void InitMine()
        {
            Mine.number =  Card[4].CardNumberEntity;
            Mine.name =    Card[4].CardNameEntity;
            Mine.text =    Card[4].CardTextEntity;
            Mine.cost =    Card[4].CardCostEntity;
            Mine.score =   Card[4].CardScoreEntity;
            Mine.type =    Card[4].CardTypeEntity;
            Mine.rarity =  Card[4].CardRarityEntity;
            Mine.Maximum = Card[4].CardMaximumEntity;

            AllCard.Add(Mine);
        }

        private void InitWildCard()
        {
            WildCard.number =  Card[5].CardNumberEntity;
            WildCard.name =    Card[5].CardNameEntity;
            WildCard.text =    Card[5].CardTextEntity;
            WildCard.cost =    Card[5].CardCostEntity;
            WildCard.score =   Card[5].CardScoreEntity;
            WildCard.type =    Card[5].CardTypeEntity;
            WildCard.rarity =  Card[5].CardRarityEntity;
            WildCard.Maximum = Card[5].CardMaximumEntity;

            AllCard.Add(WildCard);
        }

        private void InitRecessionOrBoom()
        {
            RecessionOrBoom.number =  Card[6].CardNumberEntity;
            RecessionOrBoom.name =    Card[6].CardNameEntity;
            RecessionOrBoom.text =    Card[6].CardTextEntity;
            RecessionOrBoom.cost =    Card[6].CardCostEntity;
            RecessionOrBoom.score =   Card[6].CardScoreEntity;
            RecessionOrBoom.type =    Card[6].CardTypeEntity;
            RecessionOrBoom.rarity =  Card[6].CardRarityEntity;
            RecessionOrBoom.Maximum = Card[6].CardMaximumEntity;

            AllCard.Add(RecessionOrBoom);
        }

        private void InitTyphoon()
        {
            Typhoon.number =  Card[7].CardNumberEntity;
            Typhoon.name =    Card[7].CardNameEntity;
            Typhoon.text =    Card[7].CardTextEntity;
            Typhoon.cost =    Card[7].CardCostEntity;
            Typhoon.score =   Card[7].CardScoreEntity;
            Typhoon.type =    Card[7].CardTypeEntity;
            Typhoon.rarity =  Card[7].CardRarityEntity;
            Typhoon.Maximum = Card[7].CardMaximumEntity;

            AllCard.Add(Typhoon);
        }

        private void InitShuffle()
        {
            Shuffle.number =  Card[8].CardNumberEntity;
            Shuffle.name =    Card[8].CardNameEntity;
            Shuffle.text =    Card[8].CardTextEntity;
            Shuffle.cost =    Card[8].CardCostEntity;
            Shuffle.score =   Card[8].CardScoreEntity;
            Shuffle.type =    Card[8].CardTypeEntity;
            Shuffle.rarity =  Card[8].CardRarityEntity;
            Shuffle.Maximum = Card[8].CardMaximumEntity;

            AllCard.Add(Shuffle);
        }

        private void InitMeteo()
        {
            Meteo.number =  Card[9].CardNumberEntity;
            Meteo.name =    Card[9].CardNameEntity;
            Meteo.text =    Card[9].CardTextEntity;
            Meteo.cost =    Card[9].CardCostEntity;
            Meteo.score =   Card[9].CardScoreEntity;
            Meteo.type =    Card[9].CardTypeEntity;
            Meteo.rarity =  Card[9].CardRarityEntity;
            Meteo.Maximum = Card[9].CardMaximumEntity;

            AllCard.Add(Meteo);
        }

        private void InitTouristSpot()
        {
            TouristSpot.number = Card[10].CardNumberEntity;
            TouristSpot.name = Card[10].CardNameEntity;
            TouristSpot.text = Card[10].CardTextEntity;
            TouristSpot.cost = Card[10].CardCostEntity;
            TouristSpot.score = Card[10].CardScoreEntity;
            TouristSpot.type = Card[10].CardTypeEntity;
            TouristSpot.rarity = Card[10].CardRarityEntity;
            TouristSpot.Maximum = Card[10].CardMaximumEntity;

            AllCard.Add(TouristSpot);
        }

        private void InitWildObj()
        {
            WildObj.number = Card[11].CardNumberEntity;
            WildObj.name = Card[11].CardNameEntity;
            WildObj.text = Card[11].CardTextEntity;
            WildObj.cost = Card[11].CardCostEntity;
            WildObj.score = Card[11].CardScoreEntity;
            WildObj.type = Card[11].CardTypeEntity;
            WildObj.rarity = Card[11].CardRarityEntity;
            WildObj.Maximum = Card[11].CardMaximumEntity;

            AllCard.Add(WildObj);
        }
    }
}
