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

        DrawObjBatting = 99,    //オブジェクトバッティングコマンド
        DrawObjAlready = 100,   //オブジェクトが既にあった時のコマンド
        DrawActionEnd = 110,    //アクションカード描画エンド
        DrawObjEnd = 111,       //オブジェクト描画終了コマンド
        DrawBattingEnd = 200,   //バッティング描画終了コマンド
        DrawRankUpEnd = 210,    //ランクアップ描画終了コマンド
        DrawClosedEnd = 211,    //廃業描画終了コマンド
    }

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

        private void Awake()
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
        }

        private void InitRestaurants()
        {
            Restaurants.number = (int)CardName.Restaurants;
            Restaurants.name = "レストラン";
            Restaurants.text = "rank1 10/rank2 20";
            Restaurants.cost = 1;
            Restaurants.score = 10;
            Restaurants.type = (int)CardType.Object;
            Restaurants.rarity = 0;
            Restaurants.Maximum = 15;

            AllCard.Add(Restaurants);
        }

        private void InitTapiocaShop()
        {
            TapiocaShop.number = (int)CardName.TapiocaShop;
            TapiocaShop.name = "タピオカ屋";
            TapiocaShop.text = "rank1 10/rank2 20";
            TapiocaShop.cost = 1;
            TapiocaShop.score = 10;
            TapiocaShop.type = (int)CardType.Object;
            TapiocaShop.rarity = 0;
            TapiocaShop.Maximum = 10;

            AllCard.Add(TapiocaShop);
        }

        private void InitSupermarket()
        {
            Supermarket.number = (int)CardName.Supermarket;
            Supermarket.name = "スーパー";
            Supermarket.text = "rank1 20/rank2 50";
            Supermarket.cost = 2;
            Supermarket.score = 20;
            Supermarket.type = (int)CardType.Object;
            Supermarket.rarity = 0;
            Supermarket.Maximum = 7;

            AllCard.Add(Supermarket);
        }

        private void InitAmusementPark()
        {
            AmusementPark.number = (int)CardName.AmusementPark;
            AmusementPark.name = "遊園地";
            AmusementPark.text = "rank1 60/rank2 120";
            AmusementPark.cost = 3;
            AmusementPark.score = 60;
            AmusementPark.type = (int)CardType.Object;
            AmusementPark.rarity = 0;
            AmusementPark.Maximum = 5;

            AllCard.Add(AmusementPark);
        }

        private void InitMine()
        {
            Mine.number = (int)CardName.Mine;
            Mine.name = "地雷";
            Mine.text = "誰かが地雷を埋めやがった💦触れると全部ドカンだっ！";
            Mine.cost = 1;
            Mine.score = -1;
            Mine.type = (int)CardType.Action;
            Mine.rarity = 0;
            Mine.Maximum = 3;

            AllCard.Add(Mine);
        }

        private void InitWildCard()
        {
            WildCard.number = (int)CardName.WildCard;
            WildCard.name = "ワイルドカード";
            WildCard.text = "お望みの建物をランクアップしてあげるわ！";
            WildCard.cost = 1;
            WildCard.score = -1;
            WildCard.type = (int)CardType.Action;
            WildCard.rarity = 0;
            WildCard.Maximum = 4;

            AllCard.Add(WildCard);
        }

        private void InitRecessionOrBoom()
        {
            RecessionOrBoom.number = (int)CardName.RecessionOrBoom;
            RecessionOrBoom.name = "不況 / 好況";
            RecessionOrBoom.text = "結果は神のみぞ知る";
            RecessionOrBoom.cost = 1;
            RecessionOrBoom.score = -1;
            RecessionOrBoom.type = (int)CardType.Action;
            RecessionOrBoom.rarity = 0;
            RecessionOrBoom.Maximum = 2;

            AllCard.Add(RecessionOrBoom);
        }

        private void InitTyphoon()
        {
            Typhoon.number = (int)CardName.Typhoon;
            Typhoon.name = "台風";
            Typhoon.text = "お宅の物件大丈夫？";
            Typhoon.cost = 2;
            Typhoon.score = -1;
            Typhoon.type = (int)CardType.Action;
            Typhoon.rarity = 0;
            Typhoon.Maximum = 3;

            AllCard.Add(Typhoon);
        }

        private void InitShuffle()
        {
            Shuffle.number = (int)CardName.Shuffle;
            Shuffle.name = "シャッフル";
            Shuffle.text = "はーい、今から席替えを始めま～す。";
            Shuffle.cost = 2;
            Shuffle.score = -1;
            Shuffle.type = (int)CardType.Action;
            Shuffle.rarity = 0;
            Shuffle.Maximum = 2;

            AllCard.Add(Shuffle);
        }

        private void InitMeteo()
        {
            Meteo.number = (int)CardName.Meteo;
            Meteo.name = "レストラン";
            Meteo.text = "落ちてきますグッドラック！！";
            Meteo.cost = 3;
            Meteo.score = -1;
            Meteo.type = (int)CardType.Action;
            Meteo.rarity = 0;
            Meteo.Maximum = 2;

            AllCard.Add(Meteo);
        }
    }
}
