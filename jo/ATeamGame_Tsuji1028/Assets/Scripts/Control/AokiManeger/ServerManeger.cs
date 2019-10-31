using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;
using System;
using System.Linq;

using UnityEngine.Networking;           //ネットワーク処理を行うため

//描画リストについて
/***
 * アクション
 * 　⇓
 * バッティング
 * 　⇓
 * オブジェクト
 * 　⇓
 * ランクアップ
 * 　⇓
 * 廃業
 * 　⇓
 * 　
 * というような描画リストを作成しています。
 * 各矢印に終了コマンドを設けていて、描画しやすいように設計しています。
 ***/

namespace Ctrl
{
    public class ServerManeger : NetworkBehaviour
    {
        private ServerDataProxy _Server = ServerDataProxy._Instance;
        private GlobalParamenter _prm = new GlobalParamenter(6,6);

        private GameObject _AllCardDataObj;                 //全カードデータが入ったオブジェ

        private List<CardDataProxy>[] _DrawCard = new List<CardDataProxy>[4];     //各プレーヤが引くカードリスト
        private List<List<int>> _DrawData = new List<List<int>>();          //描画リスト
        private List<ClientData> _UseCard = new List<ClientData>();     //使用カード

        private List<List<int>> _RankUpList = new List<List<int>>();               //ランクアップしたリスト
        private List<List<int>> _ClosedList = new List<List<int>>();               //廃業したリスト

        public GameObject[][] _AllFieldObj;

        private GameObject _RecvMgrObj;                //レシーブマネージャのオブジェ
        private GameObject _CliantMgrObj;              //クライアントから持ってきたデータのオブジェ
        private GameObject _DeckMgrObj;                //デッキマネージャオブジェ
        private GameObject _PlayerMgrObj;              //プレイヤーマネージャオブジェ

        public List<Vector2Int> _MinePos = new List<Vector2Int>();             //地雷の座標リスト
        private bool _IsBoom = false;                                          //地雷爆発フラグ

        public int RankUpID = 0;

        //おジャマ岩を四分割したときの配置個数
        public int RockCount = 0;

        //おジャマ岩を消す条件（建物数）
        public int DelOjamaRock = 0;

        //ネットワーク関係で必要な変数
        private bool _AllClientDataCollection = false;  //クライアントからデータが全員分送られてきたらtrueにします。

        private GameObject[] _PlayerObj;              //プレイヤーのゲームオブジェクト ＊追加しました

        int count = 0;      //データを送っていないプレイヤーをカウントしてます。0ならsever処理開始
        private bool IsFarst = true;

        [ServerCallback]
        // Start is called before the first frame update
        void Start()
        {
            //ClientDataProxy init = new ClientDataProxy(new CardDataProxy(1, "7,7 obj", "ww", 3, 10, 0, 0), 1, 3.0f, new Vector2(7, 7));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 2, 3.0f, new Vector2(4, 4));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 3, 3.0f, new Vector2(5, 4));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 4, 3.0f, new Vector2(6, 4));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 1, 3.0f, new Vector2(4, 5));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 1, 3.0f, new Vector2(5, 5));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 2, 3.0f, new Vector2(6, 5));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 3, 3.0f, new Vector2(4, 6));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 4, 3.0f, new Vector2(5, 6));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa2", "2ww", 1, 10, 0, 0), 1, 3.0f, new Vector2(6, 6));
            //_UseCard.Add(init);


            //init = new ClientDataProxy(new CardDataProxy(3, "aaa3", "3ww", 2, 100, 0, 0), 2, 3.0f, new Vector2(2, 4));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(1, "aaa1", "1ww", 1, 10, 0, 0), 4, 3.0f, new Vector2(7, 7));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(1, "aaa1", "1ww", 1, 10, 0, 0), 4, 3.0f, new Vector2(6, 3));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa4", "4ww", 2, 100, 0, 0), 1, 3.0f, new Vector2(3, 1));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(1, "aaa1", "1ww", 1, 10, 0, 0), 3, 3.0f, new Vector2(1, 1));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(1, "aaa1", "1ww", 1, 10, 0, 0), 3, 3.0f, new Vector2(1, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa1", "1ww", 1, 10, 0, 0), 3, 3.0f, new Vector2(4, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa1", "1ww", 1, 10, 0, 0), 1, 3.0f, new Vector2(4, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(3, "aaa1", "1ww", 2, 100, 0, 0), 1, 3.0f, new Vector2(7, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(4, "aaa1", "1ww", 3, 150, 0, 0), 1, 3.0f, new Vector2(3, 2));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(3, "aaa1", "1ww", 2, 100, 0, 0), 1, 3.0f, new Vector2(6, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(2, "aaa1", "1ww", 1, 10, 0, 0), 3, 3.0f, new Vector2(6, 0));
            //_UseCard.Add(init);

            //ClientDataProxy init = new ClientDataProxy(new CardDataProxy(9, "メテオ3.0", "ww", 1, 10, 0, 0), 1, 3.0f, new Vector2(1, 1));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(5, "メテオ3.0", "ww", 1, 10, 0, 0), 1, 3.0f, new Vector2(1, 2));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(5, "メテオ3.0", "ww", 1, 10, 0, 0), 1, 3.0f, new Vector2(1, 3));
            //_UseCard.Add(init);

            //init = new ClientDataProxy(new CardDataProxy(4, "地雷5.0", "ww", 1, 10, 1, 0), 1, 5.0f, new Vector2(0, 0));
            //_UseCard.Add(init);

            //init = new ClientDataProxy(new CardDataProxy(9, "メテオ5.0", "ww", 1, 10, 1, 0), 1, 5.0f, new Vector2(0, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(7, "台風2.0", "ww", 1, 10, 1, 0), 1, 2.0f, new Vector2(0, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(7, "台風12.0", "ww", 1, 10, 1, 0), 1, 12.0f, new Vector2(0, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(7, "台風10.0", "ww", 1, 10, 1, 0), 1, 10.0f, new Vector2(0, 0));
            //_UseCard.Add(init);

            //init = new ClientDataProxy(new CardDataProxy(9, "メテオ2.0", "ww", 1, 10, 1, 0), 1, 2.0f, new Vector2(0, 0));
            //_UseCard.Add(init);
            //init = new ClientDataProxy(new CardDataProxy(9, "メテオ1.5", "ww", 1, 10, 1, 0), 1, 1.5f, new Vector2(0, 0));
            //_UseCard.Add(init);
            if (!isServer) IsFarst = false;          //サーバかどうかの判定(これがないと全クライアントでサーバ処理が走る)

            if (IsFarst)
            {

                //各オブジェの取得
                _AllCardDataObj = GameObject.FindGameObjectWithTag("AllCard");
                _RecvMgrObj = GameObject.FindGameObjectWithTag("RecvMgr");
                _CliantMgrObj = GameObject.FindGameObjectWithTag("FromMgr");
                _DeckMgrObj = GameObject.FindGameObjectWithTag("DeckMgr");
                _PlayerMgrObj = GameObject.FindGameObjectWithTag("PlayerData");

                //フィールドデータを整地して作成する
                CreateFieldData();

                //サーバの初期化
                ServerInitialize();
                //StartCoroutine("ServerInitialize");            //待ち処理のために変更しました。

                //おジャマ岩配置
                //CreateOjamaRock();

                //フィールドのテスト
                FieldTest();

                IsFarst = false;
            }
        }

        [ServerCallback]
        private void FixedUpdate()
        {

            if (!isServer) return;          //サーバかどうかの判定(これがないと全クライアントでサーバ処理が走る)

            print("serverStart2");          //Debug
            //---------------------------------------------------------------------------------------------
            //処理を変更しました

            ////クライアントからデータ集まった?
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                //まだデータを送っていないプレイヤーがいないかどうか
                if (!_CliantMgrObj.GetComponent<FromManeger>().FromDataObj[player].GetComponent<FromData>().isSendData)
                {
                    count++;
                }
            }
            if (count != 0 && !(_AllClientDataCollection))
            {
                _AllClientDataCollection = false;
                count = 0;
            }
            else
            {
                print("serverStart");
                //全プレイヤーのデータが集まった
                _AllClientDataCollection = true;

                _PlayerObj = GameObject.FindGameObjectsWithTag("Player");       //プレイヤーのオブジェクトを格納

            }
            //クライアントからデータがすべて集まったらサーバ処理開始
            if (_AllClientDataCollection)   //ToDo::サーバにデータが集まったらっていう条件式
            {
                ServerMain();

                //クライアントのデータを消す
                DeleteClientData();

                //クライアントからデータをもらったフラグをへしおるよん
                _AllClientDataCollection = false;
                count = 0;
            }
        }

        #region テスト処理

            /// <summary>
            /// テスト処理なので最後に消してください
            /// </summary>
            private void FieldTest()
            {
            List<ClientDataProxy> UseTestCard = new List<ClientDataProxy>();
            ClientDataProxy init;
            int num = 0;

            //テストデータ挿入(建物)
            for (int i = 0; i < 0; i++) 
            {
                for (int j = 0; j < 0; j++)
                {
                    init = new ClientDataProxy(new CardDataProxy(num, "aaa", "ww", 1, 10, 0, 0), num + 1, 3.0f, new Vector2(j, i));
                    UseTestCard.Add(init);

                    num++;

                    if (num > 2)
                    {
                        num = 1;
                    }
                }
            }

            //フィールドに反映
            for (int i = 0; i < UseTestCard.Count(); i++)
            {
                SetFieldData(UseTestCard[i]);
            }

            //ランクアップテスト
            CheckRankUpObj();

            //描画データを一応削除
            _DrawData.Clear();

            //地雷テスト
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        Vector2Int test = new Vector2Int(i, j);
            //        _MinePos.Add(test);
            //    }
            //}
        }

        #endregion

        #region 初期処理

        /// <summary>
        /// サーバの初期化を行います
        /// </summary>
        public void ServerInitialize()
        {
            //サーバデータ作成
            _Server = new ServerDataProxy(0, null, null, null);

            //現在のターン数を1にする
            _Server.Turn = 1;

            //デッキ初期化
            DeckInitialize();

            //プレイヤー初期化
            PlayerInitialize();

            //フィールド初期化
            FieldInitialize();
        }

        /// <summary>
        /// フィールドデータを整地して作成する
        /// </summary>
        private void CreateFieldData()
        {
            //リスト新規作成
            _AllFieldObj = new GameObject[_prm.FieldSize.y][];
            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                _AllFieldObj[i] = new GameObject[(int)_prm.FieldSize.x];
            }

            //タイルをソート
            GameObject[] allFieldData = GameObject.FindGameObjectsWithTag("Stage");

            for (int i = 0; i < allFieldData.Length; i++)
            {
                _AllFieldObj[(int)allFieldData[i].GetComponent<TileData_Maneger>().planePos.y]
                            [(int)allFieldData[i].GetComponent<TileData_Maneger>().planePos.x] = allFieldData[i];
            }
        }

        /// <summary>
        /// デッキ初期化
        /// </summary>
        private void DeckInitialize()
        {
            for (int i = (int)CardName.Restaurants; i < (int)CardName.CardEnd; i++)//種類分回す
            {
                //カード種類参照
                CardDataProxy addCardData = new CardDataProxy(_AllCardDataObj.GetComponent<CardInfo>().AllCard[i].number, _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].name,
                                            _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].text, _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].cost,
                                            _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].score, _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].type, _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].rarity);

                DeckDataProxy addData = new DeckDataProxy(addCardData, false);

                //枚数分追加
                for (int j = 0; j < _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].Maximum; j++)
                {
                    //カードデータ作成
                    _DeckMgrObj.GetComponent<DeckManeger>().Deck.Add(addData);
                }
            }

            //サーバにセット
            CardDataProxy[] serverdata = new CardDataProxy[_DeckMgrObj.GetComponent<DeckManeger>().Deck.Count];

            for (int i = 0; i < _DeckMgrObj.GetComponent<DeckManeger>().Deck.Count; i++)
            {
                serverdata[i] = _DeckMgrObj.GetComponent<DeckManeger>().Deck[i].CardData;
            }

            _Server.Decks = serverdata;
        }

        /// <summary>
        /// プレイヤー初期化
        /// </summary>
        private void PlayerInitialize()
        {
            PlayerDataProxy InitPlayer;
            PlayerDataProxy[] serverdata = new PlayerDataProxy[4];

            for (int i = (int)PlayerType.Player1; i < (int)PlayerType.EndPlayer; i++)
            {
                //yield return new WaitForSeconds(1F);  //待ち処理

                InitPlayer = new PlayerDataProxy(i + 1, 0, 0, 1);
                _PlayerMgrObj.GetComponent<PlayerManeger>().Player[i] = new PlayerDataProxy(i + 1, 0, 0, 1);
                serverdata[i] = _PlayerMgrObj.GetComponent<PlayerManeger>().Player[i];
            }

            _Server.PlayerDatas = serverdata;
            _PlayerMgrObj.GetComponent<PlayerManeger>().Player = serverdata;
        }

        /// <summary>
        /// フィールド初期化
        /// </summary>
        private void FieldInitialize()
        {
            FieldDataProxy InitField;
            FieldDataProxy[][] serverdata = new FieldDataProxy[_prm.FieldSize.y][];
            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                serverdata[i] = new FieldDataProxy[_prm.FieldSize.x];
            }

            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                for (int j = 0; j < _prm.FieldSize.x; j++)
                {
                    InitField = new FieldDataProxy(null, -1, -1, false, -1);
                    _AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field = InitField;

                    serverdata[i][j] = _AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field;
                }
            }

            _Server.FieldDatas = serverdata;
        }

        /// <summary>
        /// 初期手札確定
        /// </summary>
        private void DrawCardInitialize()
        {
            //全プレイヤーループ　：　条件式が反転してたので修正
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                //カードを引く枚数の確定
                int DrawNum = 5;

                //カード選出処理
                for (int i = 0; i < DrawNum; i++)
                {
                    CardDataProxy ConfirmCard;

                    //ドローカード確定
                    ConfirmCard = _DeckMgrObj.GetComponent<DeckManeger>().DrawCardConfirm(_DeckMgrObj.GetComponent<DeckManeger>().Deck.Count());

                    //ドローカードリストに追加
                    _DrawCard[player].Add(ConfirmCard);
                }
            }
        }

        #endregion

        #region サーバメイン処理

        private void ServerMain()
        {
            //カードソート
            UseCardSort();

            //カード使用
            UseCard();

            //スコアの確定
            ConfirmScore();

            //ターン終了後に引くカードの確定
            DrawCardConfirm();

            //クライアントにデータを返すorサーバ終了
            GoNextTurn();
        }


        private void ConfirmScore()
        {
            //各プレイヤーのスコアを0に戻す
            for (int player = 0; player < 4; player++)
            {
                _PlayerMgrObj.GetComponent<PlayerManeger>().Player[player].TotalScore = 0;
            }

            //フィールド探索
            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                for (int j = 0; j < _prm.FieldSize.x; j++)
                {
                    if(_AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.PossessionPlayer != -1)
                        {
                            _PlayerMgrObj.GetComponent<PlayerManeger>().Player[_AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.PossessionPlayer - 1].TotalScore
                                += _AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.Data.Score;
                        }
                    }
                }
            }

            //ランキング変動

            //ソート準備
            List<PlayerDataProxy> SortPlayer = new List<PlayerDataProxy>();
            for (int player = 0; player < 4; player++)
            {
                SortPlayer.Add(_PlayerMgrObj.GetComponent<PlayerManeger>().Player[player]);
            }

            //スコアによるソート
            SortPlayer.Sort(CompareByIDRanking);

            //順位確定
            for (int player = 0; player < 4; player++)
            {
                SortPlayer[player].Ranking = player + 1;

                //マネージャに変更をかける
                _PlayerMgrObj.GetComponent<PlayerManeger>().Player[SortPlayer[player].PlayerNumber] = SortPlayer[player];
            }
        }

        #endregion

        #region クライアントに送る処理

        [ClientRpc]         //ネットワーク処理
        public void RpcRecvProssess(SendSeverDataArray[] sendData)
        {
            for (int i = (int)PlayerType.Player1; i < (int)PlayerType.EndPlayer; i++)
            {
                if (_PlayerObj[i] != null)
                {
                    _PlayerObj[i].GetComponent<LocalPlayerManeger>().recvSeverData = sendData[i];       //各プレイヤーの受け取るデータ構造体にデータを挿入
                    _PlayerObj[i].GetComponent<LocalPlayerManeger>().recvSeverData.isRecvFinished = true;
                }
            }
            print("返したよ"); //Debug
        }

        #endregion

        #region　デッキ系処理

        /// <summary>
        /// 各プレイヤーのドローカード確定
        /// ※カードを使う前に確定
        /// </summary>
        public void DrawCardConfirm()
        {
            //全プレイヤーループ
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                //初期化
                //_DrawCard[player] = new List<CardDataProxy>();

                ////カードを引く枚数の確定
                //int DrawNum = _CliantMgrObj.GetComponent<FromManeger>().Test(player).GetComponent<FromData>().From.Count();

                ////カード選出処理
                //for (int i = 0; i < DrawNum; i++)
                //{
                //    CardDataProxy ConfirmCard = new CardDataProxy(-1, null, null, -1, -1, -1, -1); //初期化を追加

                //    //ドローカード確定
                //    ConfirmCard = _DeckMgrObj.GetComponent<DeckManeger>().DrawCardConfirm(_DeckMgrObj.GetComponent<DeckManeger>().Deck.Count());

                //    //ドローカードリストに追加
                //    _DrawCard[player].Add(ConfirmCard);
                //}

                //ここから新システム(排出率)ですよ
                //初期化
                _DrawCard[player] = new List<CardDataProxy>();

                //カードを引く枚数の確定
                int DrawNum = _CliantMgrObj.GetComponent<FromManeger>().Test(player).GetComponent<FromData>().From.Count();

                //カード選出処理
                for (int i = 0; i < DrawNum; i++)
                {
                    CardDataProxy ConfirmCard = new CardDataProxy(-1, null, null, -1, -1, -1, -1); //初期化を追加

                    //ドローカード確定
                    ConfirmCard = _DeckMgrObj.GetComponent<DeckManeger>().DrawCardRateConfirm(_PlayerMgrObj.GetComponent<PlayerManeger>().Player[player].Ranking);

                    //ドローカードリストに追加
                    _DrawCard[player].Add(ConfirmCard);
                }

            }
        }

        #endregion

        #region カード系処理

        /// <summary>
        /// カード使用メソッドを司ります
        /// </summary>
        public void UseCard()
        {
            //アクションカード使用
            UseActionCard();

            //オブジェクトカード使用
            UseObjectCard();

            //各描画リストのソート&追加
            DrawListSort();

            //おジャマ岩チェック
            //DeleteOjamaRock();

            //描画データ終了宣言
            //描画終了番号(-1)を当てる
            List<int> DrawEnd = new List<int>();
            DrawEnd.Add((int)CardName.DrawEnd);
            _DrawData.Add(DrawEnd);

            //カード使用チェック
            if (!CheckUseAllCard())
            {
                Debug.Log("以上のカードが使用されませんでした。内部データを確認してください。");
            }
        }


        /// <summary>
        /// 各プレイヤーの使用カードをまとめ、使用順にソートする
        /// この時にタイプで選別はしない
        /// </summary>
        public void UseCardSort()
        {
            //カード追加
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                for (int i = 0; i < _CliantMgrObj.GetComponent<FromManeger>().Test(player).GetComponent<FromData>()._FromData.Count(); i++)
                {
                    _UseCard.Add(_CliantMgrObj.GetComponent<FromManeger>().Test(player).GetComponent<FromData>()._FromData[i]);
                }
            }

            //ソート
            _UseCard.Sort(CompareByID);
        }

        /// <summary>
        /// アクションカード使用処理
        /// カードは使用順に実行されていきます
        /// 使用されたカードは削除します
        /// </summary>
        public void UseActionCard()
        {
            //全カード探索
            for (int i = 0; i != _UseCard.Count();i++)
            {
                //アクションカードのみ処理を行う
                if (_UseCard[i].CardDataProxy.Type == (int)CardType.Action)
                {
                    //描画リストに追加するための変数を用意
                    List<int> CardDrawData = new List<int>();

                    //カードのメソッド処理(リストに追加、フィールドに反映)
                    //ToDo:: 戻り値：CardDrawData、引数：_UseCard[i]でメソッド作成
                    switch(_UseCard[i].CardDataProxy.Number)
                    {
                        case (int)CardName.Mine:            //地雷
                            CardDrawData = UseMine();
                            Debug.Log(_UseCard[i].CardDataProxy.Name + "が使用されました");
                            break;

                        //case (int)CardName.WildCard:        //ワイルドカード
                        //    CardDrawData = UseWildCard(_UseCard[i]);
                        //    Debug.Log(_UseCard[i].CardDataProxy.Name + "が使用されました");
                        //    break;

                        //case (int)CardName.RecessionOrBoom: //※廃止されました※
                        //    Debug.Log(_UseCard[i].CardDataProxy.Name + "が使用されました");
                        //    break;

                        case (int)CardName.Typhoon:         //台風
                            CardDrawData = UseTyphoon(_UseCard[i]);
                            Debug.Log(_UseCard[i].CardDataProxy.Name + "が使用されました");
                            break;

                        case (int)CardName.Shuffle:         //シャッフル
                            CardDrawData = UseShuffle();
                            Debug.Log(_UseCard[i].CardDataProxy.Name + "が使用されました");
                            break;

                        case (int)CardName.Meteo:           //メテオ
                            CardDrawData = UseMeteo(_UseCard[i]);
                            Debug.Log(_UseCard[i].CardDataProxy.Name + "が使用されました");
                            break;

                        default:
                            Debug.Log("知らないアクションカードが使われたぜ！番号は" + _UseCard[i].CardDataProxy.Number + "だぜ！！！！！！");
                            break;
                    }

                    //描画リストに追加
                    _DrawData.Add(CardDrawData);
                }
            }

            //削除カード探索
            int val = _UseCard.Count();
            for (int i = 0; i != _UseCard.Count(); i++)
            {
                //アクションカードのみ削除
                if (_UseCard[i].CardDataProxy.Type == (int)CardType.Action)
                {
                    //削除
                    _UseCard.RemoveAt(i);

                    i--;
                }
            }

            //アクション描画リスト終了(111)を当てる
            List<int> DrawActEnd = new List<int>();
            DrawActEnd.Add((int)CardName.DrawActionEnd);
            _DrawData.Add(DrawActEnd);
        }

        /// <summary>
        /// 地雷設置のアクションメソッド
        /// </summary>
        /// <returns></returns>
        public List<int> UseMine()
        {
            List<int> ReturnList = new List<int>();

            //カードナンバーをリストに追加
            ReturnList.Add((int)CardName.Mine);

            //地雷セット
            SetMinePosition();

            return ReturnList;
        }

        /// <summary>
        /// 地雷検索
        /// </summary>
        /// <param name="useCard"></param>
        public void CheckMine(ClientData useCard)
        {
            //地雷がなければそのままスルー
            if (_MinePos.Count != 0)
            {
                //爆発まだしていないときに検索スル
                if (!_IsBoom)
                {
                    //地雷探索
                    for (int check = 0; check < _MinePos.Count; check++)
                    {
                        //地雷の場所に置いてるよ～
                        if (_MinePos[check] == useCard.FieldPos)
                        {
                            //爆発フラグを建てます
                            _IsBoom = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ワイルドカードのアクションメソッド
        /// </summary>
        /// <returns></returns>
        public List<int> UseWildCard(ClientData useCard)
        {
            List<int> ReturnList = new List<int>();

            //フィールドにセットする内容(仮)
            FieldDataProxy CreateObj = new FieldDataProxy(null, useCard.PlayerNum, 0, false, -1);
            CardDataProxy CreateCard = new CardDataProxy(-1, "", "", -1, -1, -1, -1);

            //作るカードの探索
            CreateCard = CreateWildObj(useCard);
            CreateObj.Data = CreateCard;

            //描画リスト生成
            ReturnList.AddRange(CreateDrawListWildObj(useCard));

            //フィールドにセット
            if(CreateCard.Number == (int)CardName.WildObj)
            {
                //はてなをセット
                _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field = CreateObj;
            }
            else
            {
                //オブジェをセット
                CreateObj.RankID = RankUpID;
                _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field = CreateObj;

                RankUpID++;
            }

            //疑似フィールド表示データにオブジェクトを生成
            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().CreateNewObj(CreateObj);

            //建物建ったフラグをオン
            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().isBuilding = true;

            return ReturnList;
        }

        /// <summary>
        /// ワイルドカードでの作るカードの情報を作成
        /// </summary>
        /// <param name="useCard"></param>
        /// <returns></returns>
        public CardDataProxy CreateWildObj(ClientData useCard)
        {
            CardDataProxy CreateCard = new CardDataProxy((int)CardName.WildObj, "WildObj", "えらー！", 0, 0, 0, 0);

            //生成する座標
            Vector2Int CreatePos = new Vector2Int((int)useCard.FieldPos.x, (int)useCard.FieldPos.y);

            ///探索方法
            ///左→右→上→下の順番ですべて探索します    
            ///
            ///探索時の注意点
            ///全フィールドの範囲からはみ出ないようにしよう
            //観光地の探索
            {
                //左
                if ((int)useCard.FieldPos.x > 0)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.TouristSpot &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //右
                if ((int)useCard.FieldPos.x < _prm.FieldSize.x)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.TouristSpot &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //上
                if ((int)useCard.FieldPos.y > 0)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.TouristSpot &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //下
                if ((int)useCard.FieldPos.y < _prm.FieldSize.y)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.TouristSpot &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }
            }

            //コスト3の探索
            {
                //左
                if ((int)useCard.FieldPos.x > 0)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.AmusementPark &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //右
                if ((int)useCard.FieldPos.x < _prm.FieldSize.x)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.AmusementPark &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //上
                if ((int)useCard.FieldPos.y > 0)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.AmusementPark &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //下
                if ((int)useCard.FieldPos.y < _prm.FieldSize.y)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.AmusementPark &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().AmusementPark.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }
            }

            //コスト2の探索
            {
                //左
                if ((int)useCard.FieldPos.x > 0)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.Supermarket &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //右
                if ((int)useCard.FieldPos.x < _prm.FieldSize.x)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.Supermarket &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //上
                if ((int)useCard.FieldPos.y > 0)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.Supermarket &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //下
                if ((int)useCard.FieldPos.y < _prm.FieldSize.y)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.Supermarket &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.number;
                            CreateCard.Name = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.name;
                            CreateCard.Text = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.text;
                            CreateCard.Cost = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.cost;
                            CreateCard.Score = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.score;
                            CreateCard.Type = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.type;
                            CreateCard.Rarity = _AllCardDataObj.GetComponent<CardInfo>().Supermarket.rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }
            }

            //コスト1の探索
            {
                //左
                if ((int)useCard.FieldPos.x > 0)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Cost == 1 &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Number;
                            CreateCard.Name = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Name;
                            CreateCard.Text = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Text;
                            CreateCard.Cost = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Cost;
                            CreateCard.Score = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Score;
                            CreateCard.Type = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Type;
                            CreateCard.Rarity = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.Data.Rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x - 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //右
                if ((int)useCard.FieldPos.x < _prm.FieldSize.x)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Cost == 1 &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Number;
                            CreateCard.Name = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Name;
                            CreateCard.Text = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Text;
                            CreateCard.Cost = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Cost;
                            CreateCard.Score = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Score;
                            CreateCard.Type = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Type;
                            CreateCard.Rarity = _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.Data.Rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x + 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //上
                if ((int)useCard.FieldPos.y > 0)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Cost == 1 &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Number;
                            CreateCard.Name = _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Name;
                            CreateCard.Text = _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Text;
                            CreateCard.Cost = _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Cost;
                            CreateCard.Score = _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Score;
                            CreateCard.Type = _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Type;
                            CreateCard.Rarity = _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }

                //下
                if ((int)useCard.FieldPos.y < _prm.FieldSize.y)
                {
                    if (_AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Cost == 1 &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == useCard.PlayerNum &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID == -1)
                        {
                            CreateCard.Number = _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Number;
                            CreateCard.Name = _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Name;
                            CreateCard.Text = _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Text;
                            CreateCard.Cost = _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Cost;
                            CreateCard.Score = _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Score;
                            CreateCard.Type = _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Type;
                            CreateCard.Rarity = _AllFieldObj[(int)useCard.FieldPos.y + 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.Data.Rarity;

                            _AllFieldObj[(int)useCard.FieldPos.y - 1][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                            return CreateCard;
                        }
                    }
                }
            }

            //見つからなかった時
            return CreateCard;
        }

        /// <summary>
        /// 台風のアクションメソッド
        /// </summary>
        /// <returns></returns>
        public List<int> UseTyphoon(ClientData useCard)
        {
            //返すリスト
            List<int> ReturnList = new List<int>();

            //壊れる座標
            List<Vector2Int> BreakPos = new List<Vector2Int>();

            //メテオの落ちる座標をランダムに確定
            System.Random r = new System.Random((int)DateTime.Now.Ticks);   //シード値を確定
            int x;
            int y;

            //建物の数を探索
            int Findobj = 0;
            bool SearchEnd = false;
            List<int> CheckId = new List<int>();

            for (int i = 0; i < _prm.FieldSize.y && SearchEnd == false; i++)
            {
                for (int j = 0; j < _prm.FieldSize.x && SearchEnd == false; j++)
                {
                    if (_AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.PossessionPlayer != -1 &&
                        _AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.PossessionPlayer != useCard.PlayerNum)
                    {
                        if(_AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.IsRankUp)
                        {

                        }
                        Findobj++;

                        //立ってるオブジェクトが台風の規定数以上見つけたらフラグを建てる
                        if(Findobj >= 4)
                        {
                            SearchEnd = true;
                        }
                    }
                }

            }

            //壊す建物の探索
            while (Findobj != 0)
            {
                x = r.Next(_prm.FieldSize.x);
                y = r.Next(_prm.FieldSize.y);

                if (_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer != -1 &&
                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer != useCard.PlayerNum)
                {
                    bool Already = false;

                    for (int check = 0; check < BreakPos.Count; check++)
                    {
                        if(x == BreakPos[check].x && y == BreakPos[check].y)
                        {
                            Already = true;
                        }
                    }

                    if (!Already)
                    {
                        BreakPos.Add(new Vector2Int(x, y));
                        Findobj--;
                    }
                }
            }

            //リストの作成、追加
            ReturnList.AddRange(CreateDrawListTyphoon(BreakPos));

            //フィールドに反映
            SetFieldTyphoon(BreakPos);

            return ReturnList;
        }

        /// <summary>
        /// シャッフルのアクションメソッド
        /// </summary>
        /// <returns></returns>
        public List<int> UseShuffle()
        {
            //返すリスト
            List<int> ReturnList = new List<int>();
            //退避させたオブジェクト
            List<ClientDataProxy> EvacuationList = new List<ClientDataProxy>();

            //シャッフルの撤去のリスト作成
            ReturnList.AddRange(CreateDrawListShuffleDel());

            //シャッフルの撤去のフィールド反映
            EvacuationList = SetFieldShuffleDel(ReturnList);

            //シャッフルの入れ替え後のリスト作成
            ReturnList.AddRange(CreateDrawListShuffleChangePos(EvacuationList));

            //シャッフルの入れ替え後のフィールド反映
            //SetFieldShuffleChangePos(EvacuationList,ReturnList);

            return ReturnList;
        }

        /// <summary>
        /// メテオのアクションメソッド
        /// </summary>
        public List<int> UseMeteo(ClientData useCard)
        {
            //返すリスト
            List<int> ReturnList = new List<int>();

            //メテオの落ちる座標をランダムに確定
            System.Random r = new System.Random((int)DateTime.Now.Ticks);   //秒数によってシード値を確定
            int x = r.Next(_prm.FieldSize.x - 2);
            int y = r.Next(_prm.FieldSize.y - 2);
            x++;
            y++;

            //メテオのリスト作成、追加
            ReturnList.AddRange(CreateDrawListMeteo(x, y));

            //フィールドに反映
            SetFieldMeteo(useCard, x, y);

            return ReturnList;
        }

        /// <summary>
        /// オブジェクトカード使用処理
        /// 使用されたカードは削除します
        /// </summary>
        public void UseObjectCard()
        {
            //全カード探索
            for (int i = 0; i != _UseCard.Count();)
            {
                //オブジェクトカードのみ処理を行う
                if (_UseCard[i].CardDataProxy.Type == (int)CardType.Object)
                {
                    bool IsBatting = false;
                    List<int> CardDrawData = new List<int>(0);                    //描画リストに追加するための変数を用意
                    List<int> BattingPlayerList = new List<int>(0);               //バッティングさせたプレイヤーリスト

                    //先にデッキにカードを戻す
                    bool sts = _DeckMgrObj.GetComponent<DeckManeger>().ReturnCardToDeck(_UseCard[i].CardDataProxy.Number);

                    if (!sts)
                    {
                        Debug.Log("デッキに" + _UseCard[i].CardDataProxy.Name + "が戻らなかったぞ！！");
                    }

                    //同座標探索
                    for (int j = i + 1; j < _UseCard.Count(); j++)
                    {
                        if (i != j)
                        {
                            //同座標発見
                            if (_UseCard[i].FieldPos == _UseCard[j].FieldPos)
                            {
                                //デッキにカードを戻す
                                sts = _DeckMgrObj.GetComponent<DeckManeger>().ReturnCardToDeck(_UseCard[j].CardDataProxy.Number);

                                if (!sts)
                                {
                                    Debug.Log("デッキに" + _UseCard[j].CardDataProxy.Name + "が戻らなかったぞ！！");
                                }

                                //バッティングフラグをオンにする
                                IsBatting = true;

                                //バッティングさせたプレイヤーの番号李外に追加
                                BattingPlayerList.Add(_UseCard[j].PlayerNum);

                                //batting(引っかかったものだけ)したデータを削除する
                                _UseCard.RemoveAt(j);

                                j--;
                            }
                        }
                    }

                    //バッティングしたかで描画を分ける
                    if(IsBatting)
                    {
                        //バッティング時の描画リスト作成
                        //バッティングさせたプレイヤーの番号李外に追加
                        BattingPlayerList.Add(_UseCard[i].PlayerNum);

                        //バッティングメソッド処理
                        CardDrawData = CreateDrawListBatting(_UseCard[i],BattingPlayerList);

                        //描画リストに追加
                        _DrawData.Add(CardDrawData);
                    }
                    else
                    {
                        //アクションカードですでにオブジェが入ってしまっている場合
                        if (_AllFieldObj[(int)_UseCard[i].FieldPos.y]
                                        [(int)_UseCard[i].FieldPos.x].GetComponent<TileData_Maneger>().Field.Data != null)
                        {
                            //カードのメソッド処理
                            CardDrawData = CreateDrawListAlreadyObj(_UseCard[i]);

                            //描画リストに追加
                            _DrawData.Add(CardDrawData);
                        }
                        else
                        {
                            //地雷爆発チェック
                            CheckMine(_UseCard[i]);

                            //ワイルド建物の時のみ別処理
                            if (_UseCard[i].CardDataProxy.Number == (int)CardName.WildCard)
                            {
                                //ワイルドカードのメソッド
                                CardDrawData = UseWildCard(_UseCard[i]);

                                //描画リストに追加
                                _DrawData.Add(CardDrawData);
                            }
                            else
                            {
                                //カードのメソッド処理
                                CardDrawData = CreateDrawListObj(_UseCard[i]);

                                //描画リストに追加
                                _DrawData.Add(CardDrawData);

                                //フィールドデータに反映
                                SetFieldData(_UseCard[i]);
                            }
                        }

                    }
                    //探索されたカードはリストから削除
                    _UseCard.RemoveAt(i);
                }
            }

            //削除カード探索
            int val = _UseCard.Count();
            for (int i = 0; i != val; i++)
            {
                //アクションカードのみ削除
                if (_UseCard[i].CardDataProxy.Type == (int)CardType.Object)
                {
                    //削除
                    _UseCard.RemoveAt(i);
                }
            }

            //オブジェクト描画リスト終了番号(-1)を当てる
            List<int> DrawEnd = new List<int>();
            DrawEnd.Add((int)CardName.DrawObjEnd);
            _DrawData.Add(DrawEnd);
        }

        /// <summary>
        ///ソート補助メソッド(ソート順を逆にするならreturnの値を交換してください)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static int CompareByID(ClientData a, ClientData b)
        {
            if (a.CardPlayTime > b.CardPlayTime)
            {
                return 1;
            }

            if (a.CardPlayTime < b.CardPlayTime)
            {
                return -1;
            }

            return 0;
        }


        private static int CompareByIDRanking(PlayerDataProxy a, PlayerDataProxy b)
        {
            if (a.TotalScore > b.TotalScore)
            {
                return 1;
            }

            if (a.TotalScore < b.TotalScore)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// カードがすべて使用完了できたか確認します
        /// </summary>
        /// <returns>すべて使われていたらtrueを返し、使われていないカードをDebug.Logで出力します</returns>
        private bool CheckUseAllCard()
        {
            //使われてないカードがあるぞー！！
            if(_UseCard.Count() != 0)
            {
                for(int i = 0; i < _UseCard.Count();)
                {
                    Debug.Log("プレイヤー" + _UseCard[i].PlayerNum + "の" + _UseCard[i].CardDataProxy.Name + "が使われませんでした。。。" + "(" + "座標" + _UseCard[i].FieldPos.x + "," + _UseCard[i].FieldPos.y + ")");

                    //とりあえず削除
                    _UseCard.RemoveAt(i);
                }

                return false;
            }

            //ダイジョブダッタネ
            return true;
        }

        #endregion

        #region フィールド系処理

        /// <summary>
        /// おジャマ岩の配置
        /// </summary>
        private void CreateOjamaRock()
        {
            //各場所の配置箇所
            int CreateRock = RockCount;

            //おジャマ岩配置座標
            List<Vector2Int> CreatePos = new List<Vector2Int>();

            //設置する座標をランダムに確定
            System.Random r = new System.Random((int)DateTime.Now.Ticks);   //シード値を確定
            bool Confirm = false;

            int rx = r.Next(_prm.FieldSize.x / 2);
            int ry = r.Next(_prm.FieldSize.y / 2);

            //おジャマ岩のカードデータ作成
            CardDataProxy OjamaRockData = new CardDataProxy((int)CardName.OjamaRock, "おジャマ岩", "ジャマだよん", 0, 0, 1, 0);

            //左上
            for (int CreateCount = 0; CreateCount < RockCount; CreateCount++)
            {
                List<int> AddData = new List<int>();

                Confirm = false;

                while (!Confirm)
                {
                    rx = r.Next(_prm.FieldSize.x / 2);
                    ry = r.Next(_prm.FieldSize.y / 2);

                    if (ry < _prm.FieldSize.y / 2 && rx < _prm.FieldSize.x / 2)
                    {
                        bool Create = true;

                        //座標チェック
                        for (int i = 0; i < CreatePos.Count; i++)
                        {
                            if(ry == CreatePos[i].y && rx == CreatePos[i].x)
                            {
                                Create = false;
                                break;
                            }
                            Create = true;
                        }

                        if (Create)
                        {
                            CreatePos.Add(new Vector2Int(rx, ry));
                            SetOjamaRock(OjamaRockData, rx, ry);
                            Confirm = true;

                            //描画データ追加
                            AddData.Add(rx);
                            AddData.Add(ry);
                        }
                    }
                }
                //描画データ追加
                _DrawData.Add(AddData);
            }

            //右上
            for (int CreateCount = 0; CreateCount < RockCount; CreateCount++)
            {
                List<int> AddData = new List<int>();

                Confirm = false;

                while (!Confirm)
                {
                    rx = r.Next(_prm.FieldSize.x / 2) + _prm.FieldSize.x / 2;
                    ry = r.Next(_prm.FieldSize.y / 2);

                    if (ry < _prm.FieldSize.y / 2 && rx < _prm.FieldSize.x)
                    {
                        bool Create = true;

                        //座標チェック
                        for (int i = 0; i < CreatePos.Count; i++)
                        {
                            if (ry == CreatePos[i].y && rx == CreatePos[i].x)
                            {
                                Create = false;
                                break;
                            }
                            Create = true;
                        }

                        if (Create)
                        {
                            CreatePos.Add(new Vector2Int(rx, ry));
                            SetOjamaRock(OjamaRockData, rx, ry);
                            Confirm = true;

                            //描画データ追加
                            AddData.Add(rx);
                            AddData.Add(ry);
                        }
                    }
                }
                //描画データ追加
                _DrawData.Add(AddData);
            }

            //左下
            for (int CreateCount = 0; CreateCount < RockCount; CreateCount++)
            {
                List<int> AddData = new List<int>();

                Confirm = false;

                while (!Confirm)
                {
                    rx = r.Next(_prm.FieldSize.x / 2);
                    ry = r.Next(_prm.FieldSize.y / 2) + _prm.FieldSize.y / 2;

                    if (ry < _prm.FieldSize.y && rx < _prm.FieldSize.x / 2)
                    {
                        bool Create = true;

                        //座標チェック
                        for (int i = 0; i < CreatePos.Count; i++)
                        {
                            if (ry == CreatePos[i].y && rx == CreatePos[i].x)
                            {
                                Create = false;
                                break;
                            }
                            Create = true;
                        }

                        if (Create)
                        {
                            CreatePos.Add(new Vector2Int(rx, ry));
                            SetOjamaRock(OjamaRockData, rx, ry);
                            Confirm = true;

                            //描画データ追加
                            AddData.Add(rx);
                            AddData.Add(ry);
                        }
                    }
                }
                //描画データ追加
                _DrawData.Add(AddData);
            }

            //右下
            for (int CreateCount = 0; CreateCount < RockCount; CreateCount++)
            {
                List<int> AddData = new List<int>();

                Confirm = false;

                while (!Confirm)
                {
                    rx = r.Next(_prm.FieldSize.x / 2) + _prm.FieldSize.x / 2;
                    ry = r.Next(_prm.FieldSize.y / 2) + _prm.FieldSize.y / 2;

                    if (ry < _prm.FieldSize.y && rx < _prm.FieldSize.x)
                    {
                        bool Create = true;

                        //座標チェック
                        for (int i = 0; i < CreatePos.Count; i++)
                        {
                            if (ry == CreatePos[i].y && rx == CreatePos[i].x)
                            {
                                Create = false;
                                break;
                            }
                            Create = true;
                        }

                        if (Create)
                        {
                            CreatePos.Add(new Vector2Int(rx, ry));
                            SetOjamaRock(OjamaRockData, rx, ry);
                            Confirm = true;

                            //描画データ追加
                            AddData.Add(rx);
                            AddData.Add(ry);
                        }
                    }
                }
                //描画データ追加
                _DrawData.Add(AddData);
            }

        }

        /// <summary>
        /// おジャマ岩削除
        /// </summary>
        private void DeleteOjamaRock()
        {
            int NowObj = 0;
            //建物の探索
            for (int check_y = 0; check_y < _prm.FieldSize.y; check_y++)
            {
                for (int check_x = 0; check_x < _prm.FieldSize.x; check_x++)
                {
                    //データがあり、所有プレイヤーが存在する
                    if(_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field.PossessionPlayer != -1)
                        {
                            NowObj++;
                        }
                    }
                }
            }

            //建物が条件を満たしていなければ戻る
            if(NowObj < DelOjamaRock)
            {
                return;
            }

            //おジャマ岩のカードデータ作成
            CardDataProxy OjamaRockData = new CardDataProxy((int)CardName.OjamaRock, "おジャマ岩", "ジャマだよん", 0, 0, 1, 0);
            //リスト作成
            List<int> AddList = new List<int>();
            AddList.Add((int)CardName.DrawDelOjamaRockEnd);

            //おジャマ岩削除
            for (int check_y = 0; check_y < _prm.FieldSize.y; check_y++)
            {
                for (int check_x = 0; check_x < _prm.FieldSize.x; check_x++)
                {
                    //データがあり、所有プレイヤーが存在する
                    if (_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        if (_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field.Data == OjamaRockData)
                        {
                            //リスト削除
                            AddList.Add(check_x);
                            AddList.Add(check_y);

                            //オブジェ削除
                            _AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field = new FieldDataProxy(null, -1, -1, false, -1);
                            _AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().isBuilding = false;
                            _AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().myNumber = -1;
                            Destroy(_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().childObj);
                        }
                    }
                }
            }

            //描画データ追加
            _DrawData.Add(AddList);
        }

        /// <summary>
        /// 地雷の座標をセット
        /// </summary>
        private void SetMinePosition()
        {
            System.Random r = new System.Random((int)DateTime.Now.Ticks);   //秒数によってシード値を確定
            int x = r.Next(_prm.FieldSize.x);
            int y = r.Next(_prm.FieldSize.y);

            //一回の地雷で3個セットする
            for (int i = 0; i < 3; i++)
            {
                //確定フラグ
                bool Confirm = false;

                //地雷がかぶったら配置決めなおし
                while(!Confirm)
                {
                    //地雷がマス総数以上になったらログ表示し、処理を抜ける
                    if(_MinePos.Count() >= _prm.FieldSize.x * _prm.FieldSize.y)
                    {
                        Debug.Log("地雷がフィールドのマス総数に達しました。新しい地雷は配置されません");
                        break;
                    }

                    //ランダム座標にセット
                    x = r.Next(_prm.FieldSize.x);
                    y = r.Next(_prm.FieldSize.y);

                    //各地雷座標と被らないように探索
                    if (_MinePos.Count() != 0)
                    {
                        //地雷がかぶっていないか探索
                        for (int check = 0; check < _MinePos.Count(); check++)
                        {
                            //地雷がかぶっていれば再抽選
                            if (_MinePos[check].x == x && _MinePos[check].y == y)
                            {
                                Confirm = false;
                                break;
                            }
                            else
                            {
                                //地雷がチェックのところになかったら仮確定
                                Confirm = true;
                            }
                        }
                    }
                    else
                    {
                        //初埋め時
                        Confirm = true;
                    }

                    //確定していたら座標追加
                    if(Confirm)
                    {
                        Vector2Int NewPos = new Vector2Int(x, y);
                        _MinePos.Add(NewPos);
                    }
                }
            }
        }

        /// <summary>
        /// 地雷爆発時のフィールド状況
        /// </summary>
        private void SetFieldBoomMine()
        {
            //全てを消し去るやつ
            for (int boom = 0; boom < _MinePos.Count(); boom++)
            {
                //地雷の場所に所有プレイヤーがいる建物を消し飛ばします
                if (_AllFieldObj[_MinePos[boom].y][_MinePos[boom].x].GetComponent<TileData_Maneger>().Field.PossessionPlayer != -1)
                {
                    //ランクアップしているオブジェなら対を探索
                    if (_AllFieldObj[_MinePos[boom].y][_MinePos[boom].x].GetComponent<TileData_Maneger>().Field.IsRankUp)
                    {
                        bool EndSearch = false;

                        //対を探索
                        for (int search_y = 0; search_y < _prm.FieldSize.y && EndSearch == false; search_y++)
                        {
                            for (int search_x = 0; search_x < _prm.FieldSize.x && EndSearch == false; search_x++)
                            {
                                //探索源は探索から排除
                                if (_MinePos[boom].y != search_y || _MinePos[boom].x != search_x)
                                {
                                    //ランクアップしたオブジェクト発見！！
                                    if (_AllFieldObj[_MinePos[boom].y][_MinePos[boom].x].GetComponent<TileData_Maneger>().Field.RankID == _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().Field.RankID)
                                    {
                                        //対になっているオブジェ削除
                                        _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().Field = new FieldDataProxy(null, -1, -1, false, -1);
                                        _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().isBuilding = false;
                                        _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().myNumber = -1;
                                        Destroy(_AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().childObj);

                                        //探索終了
                                        EndSearch = true;
                                    }
                                }
                            }
                        }
                    }
                    //削除
                    _AllFieldObj[_MinePos[boom].y][_MinePos[boom].x].GetComponent<TileData_Maneger>().Field = new FieldDataProxy(null, -1, -1, false, -1);
                    _AllFieldObj[_MinePos[boom].y][_MinePos[boom].x].GetComponent<TileData_Maneger>().isBuilding = false;
                    _AllFieldObj[_MinePos[boom].y][_MinePos[boom].x].GetComponent<TileData_Maneger>().myNumber = -1;
                    Destroy(_AllFieldObj[_MinePos[boom].y][_MinePos[boom].x].GetComponent<TileData_Maneger>().childObj);
                }
            }
        }

        /// <summary>
        /// 台風によるフィールドの変更
        /// </summary>
        /// <param name="pos"></param>
        private void SetFieldTyphoon(List<Vector2Int> pos)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                //ランクアップしているオブジェなら対を探索
                if (_AllFieldObj[pos[i].y][pos[i].x].GetComponent<TileData_Maneger>().Field.IsRankUp)
                {
                    bool EndSearch = false;

                    //対を探索
                    for (int search_y = 0; search_y < _prm.FieldSize.y && EndSearch == false; search_y++)
                    {
                        for (int search_x = 0; search_x < _prm.FieldSize.x && EndSearch == false; search_x++)
                        {
                            //探索源は探索から排除
                            if (pos[i].y != search_y || pos[i].x != search_x)
                            {
                                //ランクアップしたオブジェクト発見！！
                                if (_AllFieldObj[pos[i].y][pos[i].x].GetComponent<TileData_Maneger>().Field.RankID == _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().Field.RankID)
                                {
                                    //対になっているオブジェ削除
                                    _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().Field = new FieldDataProxy(null, -1, -1, false, -1);
                                    _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().isBuilding = false;
                                    _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().myNumber = -1;
                                    Destroy(_AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().childObj);

                                    //探索終了
                                    EndSearch = true;
                                }
                            }
                        }
                    }
                }
                //オブジェ削除
                _AllFieldObj[pos[i].y][pos[i].x].GetComponent<TileData_Maneger>().Field = new FieldDataProxy(null, -1, -1, false, -1);
                _AllFieldObj[pos[i].y][pos[i].x].GetComponent<TileData_Maneger>().isBuilding = false;
                _AllFieldObj[pos[i].y][pos[i].x].GetComponent<TileData_Maneger>().myNumber = -1;
                Destroy(_AllFieldObj[pos[i].y][pos[i].x].GetComponent<TileData_Maneger>().childObj);
            }
        }

        /// <summary>
        /// シャッフルによるオブジェクトの退避
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<ClientDataProxy> SetFieldShuffleDel(List<int> data)
        {
            List<ClientDataProxy> ReturnData = new List<ClientDataProxy>();

            //退避オブジェの数
            int DelValue = (data.Count() - 2) / 2;

            //退避データ作成,オブジェ削除
            for (int i = 0; i < DelValue; i++)
            {
                //退避データ作成
                ClientDataProxy Evacuation = new ClientDataProxy(_AllFieldObj[data[i * 2 + 1 + 2]][data[i * 2 + 2]].GetComponent<TileData_Maneger>().Field.Data,
                                                                 _AllFieldObj[data[i * 2 + 1 + 2]][data[i * 2 + 2]].GetComponent<TileData_Maneger>().Field.PossessionPlayer,
                                                                 0.0f,
                                                                 new Vector2(data[i * 2 + 1 + 2], data[i * 2 + 2]));

                ReturnData.Add(Evacuation);

                //オブジェ削除
                _AllFieldObj[data[i * 2 + 1 + 2]][data[i * 2 + 2]].GetComponent<TileData_Maneger>().Field = new FieldDataProxy(null, -1, -1, false, -1);
                _AllFieldObj[data[i * 2 + 1 + 2]][data[i * 2 + 2]].GetComponent<TileData_Maneger>().isBuilding = false;
                _AllFieldObj[data[i * 2 + 1 + 2]][data[i * 2 + 2]].GetComponent<TileData_Maneger>().myNumber = -1;
                Destroy(_AllFieldObj[data[i * 2 + 1 + 2]][data[i * 2 + 2]].GetComponent<TileData_Maneger>().childObj);
            }

            return ReturnData;
        }

        /// <summary>
        /// シャッフルによる退避オブジェクトを再設置
        /// </summary>
        /// <param name="data"></param>
        private void SetFieldShuffleChangePos(List<ClientDataProxy> EvacuationData,List<int> data)
        {
            //退避オブジェクトの数
            int Evacuation = EvacuationData.Count();

            //再設置のデータがスタートしている添え字
            int ChangePosSubscript = Evacuation * 2 + 2;

            //再配置データの座標
            Vector2Int[] Pos = new Vector2Int[Evacuation];

            for (int i = 0; i < Evacuation; i++)
            {
                Pos[i].x = data[ChangePosSubscript + i + 1];
                Pos[i].y = data[ChangePosSubscript + i];
            }

            //再設置開始
            for (int i = 0; i < Evacuation; i++)
            {
                SetFieldShuffleObj(EvacuationData[i], Pos[i].x, Pos[i].y);
            }
        }

        /// <summary>
        /// メテオによるフィールドの変更
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetFieldMeteo(ClientData useCard, int x, int y)
        {
            //メテオが落ちた座標の周囲マスのデータを消します
            for (int i = -1; i < 2; i++) 
            {
                for (int j = -1; j < 2; j++)
                {
                    //建物があったら探索
                    if (_AllFieldObj[y + i][x + j].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        //ランクアップしたオブジェがあった時
                        if (_AllFieldObj[y + i][x + j].GetComponent<TileData_Maneger>().Field.IsRankUp)
                        {
                            //探索終了フラグ
                            bool EndSearch = false;

                            //ランクアップIDを探索してください
                            for (int search_y = 0; search_y < _prm.FieldSize.y && EndSearch == false; search_y++)
                            {
                                for (int search_x = 0; search_x < _prm.FieldSize.x && EndSearch == false; search_x++)
                                {
                                    //探索源は探索から排除
                                    if (y + i != search_y || x + j != search_x)
                                    {
                                        //ランクアップしたオブジェクト発見！！
                                        if (_AllFieldObj[y + i][x + j].GetComponent<TileData_Maneger>().Field.RankID == _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().Field.RankID)
                                        {
                                            //対になっているオブジェ削除
                                            _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().Field = new FieldDataProxy(null, -1, -1, false, -1);
                                            _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().isBuilding = false;
                                            _AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().myNumber = -1;
                                            Destroy(_AllFieldObj[search_y][search_x].GetComponent<TileData_Maneger>().childObj);

                                            //探索終了
                                            EndSearch = true;
                                        }
                                    }
                                }
                            }
                        }
                        //オブジェクト削除
                        _AllFieldObj[y + i][x + j].GetComponent<TileData_Maneger>().Field = new FieldDataProxy(null, -1, -1, false, -1);
                        _AllFieldObj[y + i][x + j].GetComponent<TileData_Maneger>().isBuilding = false;
                        _AllFieldObj[y + i][x + j].GetComponent<TileData_Maneger>().myNumber = -1;
                        Destroy(_AllFieldObj[y + i][x + j].GetComponent<TileData_Maneger>().childObj);
                    }
                }
            }

            //ToDo::観光地生成
            //観光地のカードを生成
            CardDataProxy TouristSpot = new CardDataProxy(_AllCardDataObj.GetComponent<CardInfo>().TouristSpot.number, _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.name, _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.text,
                                                            _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.cost, _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.score, _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.type, _AllCardDataObj.GetComponent<CardInfo>().TouristSpot.rarity);

            //新しくフィールドの情報を生成、情報を詰める(カード情報　　　　　、使用プレイヤー、マップチップ、ランクアップしてる？、ランクID)
            FieldDataProxy SetData = new FieldDataProxy(TouristSpot, useCard.PlayerNum, 0, false, -1);

            //使用されたカードをフィールド情報に埋め込む
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field = SetData;

            //疑似フィールド表示データにオブジェクトを生成
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().CreateNewObj(SetData);

            //建物建ったフラグをオン
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().isBuilding = true;
        }

        /// <summary>
        /// フィールドに使用データをセットします
        /// </summary>
        public void SetFieldData(ClientData useCard)
        {
            //新しくフィールドの情報を生成、情報を詰める(カード情報　　　　　、使用プレイヤー、マップチップ、ランクアップしてる？、ランクID)
            FieldDataProxy SetData = new FieldDataProxy(useCard.CardDataProxy, useCard.PlayerNum, 0, false, -1);

            //使用されたカードをフィールド情報に埋め込む
            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field = SetData;

            //yield return new WaitForSeconds(2F);        //待ち処理
            //疑似フィールド表示データにオブジェクトを生成
            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().CreateNewObj(new FieldDataProxy(useCard.CardDataProxy, useCard.PlayerNum, useCard.CardDataProxy.Number, false, -1));

            //建物建ったフラグをオン
            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().isBuilding = true;
        }

        /// <summary>
        /// おジャマ岩配置
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetOjamaRock(CardDataProxy Data, int x, int y)
        {
            //新しくフィールドの情報を生成、情報を詰める(カード情報　　　　　、使用プレイヤー、マップチップ、ランクアップしてる？、ランクID)
            FieldDataProxy SetData = new FieldDataProxy(Data, -1, 0, false, -1);

            //使用されたカードをフィールド情報に埋め込む
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field = SetData;

            //疑似フィールド表示データにオブジェクトを生成
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().CreateNewObj(new FieldDataProxy(Data, -1 , (int)CardName.OjamaRock, false, -1));

            //建物建ったフラグをオン
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().isBuilding = true;

            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Player = _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer;
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().RankID = _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.RankID;
        }

        /// <summary>
        /// シャッフルによるオブジェクト配置
        /// </summary>
        /// <param name="useCard"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetFieldShuffleObj(ClientData useCard, int x, int y)
        {
            //新しくフィールドの情報を生成、情報を詰める(カード情報　　　　　、使用プレイヤー、マップチップ、ランクアップしてる？、ランクID)
            FieldDataProxy SetData = new FieldDataProxy(useCard.CardDataProxy, useCard.PlayerNum, 0, false, -1);

            //使用されたカードをフィールド情報に埋め込む
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field = SetData;

            //疑似フィールド表示データにオブジェクトを生成
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().CreateNewObj(new FieldDataProxy(useCard.CardDataProxy, useCard.PlayerNum, useCard.CardDataProxy.Number, false, -1));

            //建物建ったフラグをオン
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().isBuilding = true;

            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Player = _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer;
            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().RankID = _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.RankID;
        }

        /// <summary>
        /// ランクアップできるオブジェクトの探索
        /// </summary>
        private void CheckRankUpObj()
        {
            //全フィールド探索
            for (int y = 0; y < _prm.FieldSize.y; y++)
            {
                for (int x = 0; x < _prm.FieldSize.x; x++)
                {
                    if(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        //ランクアップ可能か探索
                        if (_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.IsRankUp == false &&
                            _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data.Number != (int)CardName.WildObj)
                        {
                            int RankUpObj = _AllFieldObj[y][x].GetComponent<TileData_Maneger>().SearchRankUp();
                            List<int> RankUpData = new List<int>();

                            //リスト生成
                            switch (RankUpObj)
                            {
                                case 1: //←
                                    RankUpData.Add(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data.Number);
                                    RankUpData.Add(x);
                                    RankUpData.Add(y);
                                    RankUpData.Add(x - 1);
                                    RankUpData.Add(y);
                                    RankUpData.Add(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer);

                                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.IsRankUp = true;
                                    _AllFieldObj[y][x - 1].GetComponent<TileData_Maneger>().Field.IsRankUp = true;

                                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;
                                    _AllFieldObj[y][x - 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                                    RankUpID++;

                                    _RankUpList.Add(RankUpData);
                                    break;

                                case 2: //⇒
                                    RankUpData.Add(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data.Number);
                                    RankUpData.Add(x);
                                    RankUpData.Add(y);
                                    RankUpData.Add(x + 1);
                                    RankUpData.Add(y);
                                    RankUpData.Add(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer);

                                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.IsRankUp = true;
                                    _AllFieldObj[y][x + 1].GetComponent<TileData_Maneger>().Field.IsRankUp = true;

                                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;
                                    _AllFieldObj[y][x + 1].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                                    RankUpID++;

                                    _RankUpList.Add(RankUpData);
                                    break;

                                case 3: //↑
                                    RankUpData.Add(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data.Number);
                                    RankUpData.Add(x);
                                    RankUpData.Add(y);
                                    RankUpData.Add(x);
                                    RankUpData.Add(y - 1);
                                    RankUpData.Add(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer);

                                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.IsRankUp = true;
                                    _AllFieldObj[y - 1][x].GetComponent<TileData_Maneger>().Field.IsRankUp = true;

                                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;
                                    _AllFieldObj[y - 1][x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                                    RankUpID++;

                                    _RankUpList.Add(RankUpData);
                                    break;

                                case 4: //⇓
                                    RankUpData.Add(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data.Number);
                                    RankUpData.Add(x);
                                    RankUpData.Add(y);
                                    RankUpData.Add(x);
                                    RankUpData.Add(y + 1);
                                    RankUpData.Add(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer);

                                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.IsRankUp = true;
                                    _AllFieldObj[y + 1][x].GetComponent<TileData_Maneger>().Field.IsRankUp = true;

                                    _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;
                                    _AllFieldObj[y + 1][x].GetComponent<TileData_Maneger>().Field.RankID = RankUpID;

                                    RankUpID++;

                                    _RankUpList.Add(RankUpData);
                                    break;

                                case 0: //ランクアップせず
                                    break;

                                default:
                                    Debug.Log("SearchRankUpから" + RankUpObj + "っていう気持ち悪い値が返ってきたぞ！！かくにんだー！！");
                                    break;
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 廃業探索、リスト追加
        /// </summary>
        private void CheckClosedObj()
        {
            //全フィールド探索
            for (int y = 0; y < _prm.FieldSize.y; y++) 
            {
                for (int x = 0; x < _prm.FieldSize.x; x++)
                {

                    //ランクアップしたフィールドを発見
                    if(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.IsRankUp)
                    {
                        //周囲のマスを探索
                        for (int check_y = y - 1; check_y <= y + 1; check_y++) 
                        {
                            //範囲外は調べない
                            if (check_y >= 0 && check_y < _prm.FieldSize.y)
                            {

                                for (int check_x = x - 1; check_x <= x + 1; check_x++)
                                {
                                    //範囲外は調べない
                                    if (check_x >= 0 && check_x < _prm.FieldSize.x)
                                    {
                                        //建物が建っているとき
                                        if (_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().isBuilding)
                                        {
                                            //ランクアップしていない、同playerじゃない、同コストの建物は排除します
                                            if (!(_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field.IsRankUp) &&
                                                (_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field.PossessionPlayer != _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer) &&
                                                (_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field.Data.Number == _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data.Number))
                                            {
                                                //建物削除リスト追加
                                                List<int> DelPos = new List<int>();
                                                DelPos.Add(check_x);
                                                DelPos.Add(check_y);

                                                _ClosedList.Add(DelPos);

                                                //建物を削除
                                                _AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().isBuilding = false;
                                                _AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().Field = InitializeFieldData();
                                                Destroy(_AllFieldObj[check_y][check_x].GetComponent<TileData_Maneger>().childObj);
                                            }
                                        }
                                    }

                                }
                            }

                        }
                    }

                }
            }
        }

        /// <summary>
        /// フィールドの初期処理
        /// マップチップをランダムに入れたければこの中で変えてデータを返してあげてください
        /// </summary>
        /// <returns></returns>
        private FieldDataProxy InitializeFieldData()
        {
            FieldDataProxy InitData;

            InitData = new FieldDataProxy(null, -1, -1, false, -1);

            return InitData;
        }

        #endregion

        #region 描画リスト作成関数

        /// <summary>
        /// 地雷爆破リスト作成
        /// </summary>
        /// <returns></returns>
        private List<int> CreateDrawListBoomMine()
        {
            List<int> BoomMine = new List<int>();

            //地雷爆破コマンドを最初に入れる
            BoomMine.Add((int)CardName.MineBoom);

            //座標をリストに詰める
            for (int itr = 0; itr < _MinePos.Count; itr++)
            {
                //座標追加
                BoomMine.Add(_MinePos[itr].x);
                BoomMine.Add(_MinePos[itr].y);
            }

            return BoomMine;
        }

        /// <summary>
        /// ワイルド建物の描画リストを生成
        /// </summary>
        /// <returns></returns>
        public List<int> CreateDrawListWildObj(ClientData useCard)
        {
            List<int> WildObj = new List<int>();

            //ワイルド建物ナンバーの追加
            WildObj.Add((int)CardName.WildCard);

            //座標追加
            WildObj.Add((int)useCard.FieldPos.x);
            WildObj.Add((int)useCard.FieldPos.y);

            //プレイヤー追加
            WildObj.Add(useCard.PlayerNum);

            return WildObj;
        }

        /// <summary>
        /// 台風のリストを作成
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<int> CreateDrawListTyphoon(List<Vector2Int> pos)
        {
            List<int> Typhoon = new List<int>();

            //台風ナンバーの追加
            Typhoon.Add((int)CardName.Typhoon);

            //リストの追加
            for (int i = 0; i < pos.Count; i++)
            {
                Typhoon.Add(pos[i].x);
                Typhoon.Add(pos[i].y);
            }

            return Typhoon;
        }

        /// <summary>
        /// シャッフルの撤去リスト作成
        /// </summary>
        /// <returns></returns>
        public List<int> CreateDrawListShuffleDel()
        {
            List<int> ShuffleDel = new List<int>();

            //シャッフルナンバーの追加
            ShuffleDel.Add((int)CardName.Shuffle);

            //撤去の探索
            int DelCount = 0;
            List<int> DelPos = new List<int>();

            for (int y = 0; y < _prm.FieldSize.y; y++)
            {
                for (int x = 0; x < _prm.FieldSize.x; x++)
                {
                    //所有プレイヤーが存在し、ランクアップしていない建物をカウントする
                    if(_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer != -1 &&
                       _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data != null &&
                       _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.IsRankUp == false)
                    {
                        //撤去座標の記憶
                        DelPos.Add(x);
                        DelPos.Add(y);

                        //撤去数追加
                        DelCount++;
                    }
                }
            }

            //撤去数をリストに追加
            ShuffleDel.Add(DelCount);

            //撤去座標をリストに追加
            ShuffleDel.AddRange(DelPos);

            return ShuffleDel;
        }

        /// <summary>
        /// オブジェクトの再設置のリストを作成
        /// </summary>
        /// <param name="EvacuationList"></param>
        /// <returns></returns>
        public List<int> CreateDrawListShuffleChangePos(List<ClientDataProxy> EvacuationList)
        {
            //追加する型作成
            List<int> ChangePos = new List<int>();

            System.Random r = new System.Random((int)DateTime.Now.Ticks);   //秒数によってシード値を確定
            int x = r.Next(_prm.FieldSize.x);
            int y = r.Next(_prm.FieldSize.y);

            Vector2Int[] test = new Vector2Int[EvacuationList.Count()];

            //新規作成
            //ToDo::フィールドに反映させたデータから参照
            for (int i = 0; i < EvacuationList.Count(); i++)
            {
                //決定フラグ
                bool Confirm = false;

                //探索開始
                while(!Confirm)
                {
                    //探索座標確定
                    x = r.Next(_prm.FieldSize.x);
                    y = r.Next(_prm.FieldSize.y);

                    //所有プレイヤーがいないかつ、何も建っていないとき
                    if (_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.PossessionPlayer == -1 &&
                        _AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data == null &&
                        _AllFieldObj[y][x].GetComponent<TileData_Maneger>().isBuilding == false)
                    {
                        //カードの番号をセット
                        ChangePos.Add(EvacuationList[i].CardDataProxy.Number);
                        
                        //セットされた座標をセット
                        ChangePos.Add(x);
                        ChangePos.Add(y);
                        
                        //プレイヤー番号をセット
                        ChangePos.Add(EvacuationList[i].PlayerNum);

                        //配置座標
                        SetFieldShuffleObj(EvacuationList[i], x, y);

                        test[i].x = x;
                        test[i].y = y;

                        Confirm = true;
                    }
                }
            }

            return ChangePos;
        }

        /// <summary>
        /// メテオのリストを作成
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<int> CreateDrawListMeteo(int x, int y)
        {
            List<int> MeteoList = new List<int>();

            //メテオナンバーの追加
            MeteoList.Add((int)CardName.Meteo);

            //リストの追加
            MeteoList.Add(x);
            MeteoList.Add(y);

            return MeteoList;
        }

        /// <summary>
        /// バッディング(99)したときの描画リスト作成
        /// カード番号、X座標、Y座標
        /// </summary>
        /// <param name="useCard"></param>
        public List<int> CreateDrawListBatting(ClientData useCard,List<int> battingPlayer)
        {
            //追加する型作成
            List<int> DrawData = new List<int>();

            //バッディングナンバー(99)をセット
            DrawData.Add((int)CardName.DrawObjBatting);

            //バッディングした座標をセット
            DrawData.Add((int)useCard.FieldPos.x);
            DrawData.Add((int)useCard.FieldPos.y);

            //バッティングしたプレイヤーリストをセット
            DrawData.AddRange(battingPlayer);

            return DrawData;
        }

        /// <summary>
        /// オブジェがあった時(100)の描画リスト作成
        /// カード番号、X座標、Y座標
        /// </summary>
        /// <param name="useCard"></param>
        public List<int> CreateDrawListAlreadyObj(ClientData useCard)
        {
            //追加する型作成
            List<int> DrawData = new List<int>();

            //カードの番号をセット
            DrawData.Add((int)CardName.DrawObjAlready);

            //セットされた座標をセット
            DrawData.Add((int)useCard.FieldPos.x);
            DrawData.Add((int)useCard.FieldPos.y);

            //プレイヤー番号をセット
            DrawData.Add(useCard.PlayerNum);

            return DrawData;
        }


        /// <summary>
        /// オブジェの描画リスト作成
        /// カード番号、X座標、Y座標
        /// </summary>
        /// <param name="useCard"></param>
        public List<int> CreateDrawListObj(ClientData useCard)
        {
            //追加する型作成
            List<int> DrawData = new List<int>();

            //カードの番号をセット
            DrawData.Add(useCard.CardDataProxy.Number);

            //セットされた座標をセット
            DrawData.Add((int)useCard.FieldPos.x);
            DrawData.Add((int)useCard.FieldPos.y);

            //プレイヤー番号をセット
            DrawData.Add(useCard.PlayerNum);

            return DrawData;
        }

        /// <summary>
        /// 描画リストのソート＆追加
        /// バッティング　⇒　オブジェ配置　⇒　ランクアップ　⇒　廃業
        /// </summary>
        public void DrawListSort()
        {
            //バッティングソート
            BattingSort();

            //地雷爆発
            if(_IsBoom)
            {
                //リスト作成
                DrawData.Add(CreateDrawListBoomMine());

                //フィールドにセットする
                SetFieldBoomMine();

                //地雷リストを消す
                _MinePos.Clear();

                //爆発フラグを戻す
                _IsBoom = false;
            }

            //地雷描画終了コマンド追加
            List<int> BoomEnd = new List<int>();
            BoomEnd.Add((int)CardName.DrawMineBoomEnd);
            _DrawData.Add(BoomEnd);

            //ランクアップ描画データ追加
            AddRankUpDrawData();

            //廃業データ追加
            AddClosedDrawData();

            //ワイルドカードチェック
            AddWildCheckDrawData();
        }

        /// <summary>
        /// バッティングソート
        /// </summary>
        public void BattingSort()
        {
            //バッティングカウント
            int BattingCount = 0;

            //アクション終了コマンドを探索
            int DrawActEndSubscript = -1;
            for (int i = 0; i < _DrawData.Count() && DrawActEndSubscript == -1; i++)
            {
                //アクション描画コマンドの添え字探索
                if (_DrawData[i][0] == (int)CardName.DrawActionEnd)
                {
                    DrawActEndSubscript = i;
                }
            }

            //アクション終了コマンドが見つからなかったので抜ける
            if (DrawActEndSubscript == -1)
            {
                Debug.Log("アクション終了コマンドが見つかりませんでした");
                return;
            }

            //オブジェクト描画のみ探索する
            for (int i = DrawActEndSubscript + 1 ; i < _DrawData.Count() - (DrawActEndSubscript + 1); i++)
            {
                //バッティング探索
                if (_DrawData[i][0] == (int)CardName.DrawObjBatting)
                {
                    //バッティングした情報を取得
                    List<int> BattingList = _DrawData[i];

                    //バッティングデータを一時削除
                    _DrawData.RemoveAt(i);

                    //バッティングデータをアクション描画終了後に持ってくる
                    _DrawData.Insert(DrawActEndSubscript + 1, BattingList);

                    //バッティング数をプラスする
                    BattingCount++;
                }
            }

            //バッティング描画リスト終了番号(200)を当てる
            List<int> DrawEnd = new List<int>();
            DrawEnd.Add((int)CardName.DrawBattingEnd);
            _DrawData.Insert(DrawActEndSubscript + BattingCount + 1, DrawEnd);
        }

        /// <summary>
        /// ランクアップ描画リストの追加
        /// </summary>
        public void AddRankUpDrawData()
        {
            //ランクアップ探索を行います
            CheckRankUpObj();

            //ランクアップリストを描画リストに追加
            _DrawData.AddRange(_RankUpList);

            //ランクアップ描画リスト終了番号(210)を当てる
            List<int> DrawEnd = new List<int>();
            DrawEnd.Add((int)CardName.DrawRankUpEnd);
            _DrawData.Add(DrawEnd);
        }

        /// <summary>
        /// 廃業描画リストの追加
        /// </summary>
        public void AddClosedDrawData()
        {
            //廃業探索を行います
            CheckClosedObj();

            //廃業リストを描画リストに追加
            _DrawData.AddRange(_ClosedList);

            //廃業描画リスト終了番号(211)を当てる
            List<int> DrawEnd = new List<int>();
            DrawEnd.Add((int)CardName.DrawClosedEnd);
            _DrawData.Add(DrawEnd);
        }


        public void AddWildCheckDrawData()
        {
            //ワイルドカードの探索
            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                for (int j = 0; j < _prm.FieldSize.x; j++)
                {
                    if (_AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        //ワイルド建物が残っているとき
                        if (_AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field.Data.Number == (int)CardName.WildObj)
                        {
                            List<int> AddData = new List<int>();

                            //リスト作成(x,y座標)
                            AddData.Add(j);
                            AddData.Add(i);

                            //削除!!
                            _AllFieldObj[i][j].GetComponent<TileData_Maneger>().isBuilding = false;
                            _AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field = InitializeFieldData();
                            Destroy(_AllFieldObj[i][j].GetComponent<TileData_Maneger>().childObj);

                            _DrawData.Add(AddData);
                        }
                    }
                }
            }

            //終了コマンド追加
            List<int> DrawEnd = new List<int>();
            DrawEnd.Add((int)CardName.DrawWildCheckEnd);
            _DrawData.Add(DrawEnd);
        }

        #endregion

        #region クライアントに送信する系処理

        /// <summary>
        /// 1ターンのサーバ側処理が終わった時に使う
        /// 用途はターン数を追加したり、データ詰めたりなど
        /// </summary>
        public void GoNextTurn()
        {
            _Server.Turn++;

            //ゲーム終了かどうか
            if (_Server.Turn < _prm.MaxTurn)
            {
                FieldDataProxy[][] AllField = new FieldDataProxy[_prm.FieldSize.y][];
                for (int i = 0; i < _prm.FieldSize.y; i++)
                {
                    AllField[i] = new FieldDataProxy[_prm.FieldSize.x];
                }

                for (int i = 0; i < _prm.FieldSize.y; i++)
                {
                    for (int j = 0; j < _prm.FieldSize.x; j++)
                    {
                        AllField[i][j] = _AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field;
                    }
                }

                //次のターンへ行くためのメソッド
                _RecvMgrObj.GetComponent<RecvCliantManeger>().CreateRecvObject(AllField,_PlayerMgrObj.GetComponent<PlayerManeger>().Player,_Server.Turn,DrawData,DrawCard);           //データを詰めて型変換

                RpcRecvProssess(RecvCliantManeger.instance.sendDataArray);           //データをクライアント全体に転送               
            }
            else
            {
                //ゲーム終了処理
                ServerUnInitialized();
            }
        }

        /// <summary>
        /// クライアントからもらったデータを削除し、フラグを折る
        /// </summary>
        private void DeleteClientData()
        {
            //クライアントからもらったデータを削除する
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                //リセット
                _CliantMgrObj.GetComponent<FromManeger>().FromDataObj[player].GetComponent<FromData>()._FromData = new List<ClientData>();
                _CliantMgrObj.GetComponent<FromManeger>().FromDataObj[player].GetComponent<FromData>().isSendData = false;

                //ドローカードリスト削除
                _DrawCard[player].Clear();
            }

            //描画リスト削除
            _DrawData.Clear();
        }

        #endregion

        #region サーバ終了処理

        /// <summary>
        /// サーバの終了処理
        /// </summary>
        public void ServerUnInitialized()
        {
            
        }

        #endregion

        #region 疑似フィールド系

        private void CreatePseudoField()
        {

        }

        #endregion

        /*カード属性データ*/
        public ServerDataProxy Server
        {
            get
            {
                return _Server;
            }
            set
            {
                _Server = value;
            }
        }

        public List<CardDataProxy>[] DrawCard
        {
            get
            {
                return _DrawCard;
            }
            set
            {
                _DrawCard = value;
            }
        }

        public List<List<int>> DrawData
        {
            get
            {
                return _DrawData;
            }
            set
            {
                _DrawData = value;
            }
        }

    }
}