using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;
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

/**************************************
 * 
 * 大幅に変更しています
 * PlayerPreafabにアタッチしてます
 *
 * *************************************/

namespace Ctrl
{
    public class ServerManeger : NetworkBehaviour               
    {
        private ServerDataProxy _Server = ServerDataProxy._Instance;
        private GlobalParamenter _prm = new GlobalParamenter(8,8);

        private GameObject _AllCardDataObj;                 //全カードデータが入ったオブジェ

        private List<CardDataProxy>[] _DrawCard = null;     //各プレーヤが引くカードリスト
        private List<List<int>> _DrawData = new List<List<int>>(0);          //描画リスト
        private List<ClientDataProxy> _UseCard = new List<ClientDataProxy>(0);     //使用カード

        private List<List<int>> _RankUpList = new List<List<int>>(0);               //ランクアップしたリスト
        private List<List<int>> _ClosedList = new List<List<int>>(0);               //廃業したリスト

        public GameObject[][] _AllFieldObj;

        private GameObject _RecvMgrObj;                //レシーブマネージャのオブジェ
        private GameObject _CliantMgrObj;              //クライアントから持ってきたデータのオブジェ
        private GameObject _DeckMgrObj;                //デッキマネージャオブジェ
        private GameObject _PlayerMgrObj;              //プレイヤーマネージャオブジェ

        private GameObject[] _PlayerObj;              //プレイヤーのゲームオブジェクト ＊追加しました

        private bool _AllClientDataCollection = false;  //クライアントからデータが全員分送られてきたらtrueにします。(trueだったのでfalseに変更)

        int count = 0;      //データを送っていないプレイヤーをカウントしてます。0ならsever処理開始
        // Start is called before the first frame update
        [ServerCallback]
        void Start()
        { 
            //------------------------
            //仮データは削除しました
            //------------------------

            //各オブジェの取得
            _AllCardDataObj = GameObject.FindGameObjectWithTag("AllCard");
            _RecvMgrObj = GameObject.FindGameObjectWithTag("RecvMgr");
            _CliantMgrObj = GameObject.FindGameObjectWithTag("FromMgr");
            _DeckMgrObj = GameObject.FindGameObjectWithTag("DeckMgr");
            _PlayerMgrObj = GameObject.FindGameObjectWithTag("PlayerMgr");
            //フィールドデータを整地して作成する
            CreateFieldData();

            //サーバの初期化
            // ServerInitialize();

            StartCoroutine("ServerInitialize");            //待ち処理のために変更しました。

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

            //-------------------------------------------------------------------------------------------------
            //if (UIMgr.instance.test)
            //{
            //    //サーバ側テスト
            //    Rpchoge();
            //    UIMgr.instance.test = false;
            //}
        }

        //[ClientRpc]
        //public void Rpchoge()
        //{
        //    print("わっはっは");
        //}


        #region 初期処理
        //-------------------
        //変更しました
        //-------------------
        /// <summary>
        /// サーバの初期化を行います
        /// </summary>

        IEnumerator ServerInitialize()  //IEnumeratorはyieldを使う場合セットでいります
        {
            //サーバデータ作成
            _Server = new ServerDataProxy(0, null, null, null);

            //現在のターン数を1にする
            _Server.Turn = 1;

            //デッキ初期化
            DeckInitialize();

            yield return new WaitForSeconds(1F);            //待ち処理(バグの原因か？)
                
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
                _AllFieldObj[i] = new GameObject[(int)_prm.FieldSize.y];
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
                                            _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].score, _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].type, _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].rarity); ;

                DeckDataProxy addData = new DeckDataProxy(addCardData, false);

                //枚数分追加
                for (int j = 0; j < _AllCardDataObj.GetComponent<CardInfo>().AllCard[i].Maximum; j++)
                {
                    //yield return new WaitForSeconds(0.5F);
                    //カードデータ作成
                    _DeckMgrObj.GetComponent<DeckManeger>().Deck.Add(addData);
                }
            }
        }
        //-------------------
        //変更しました
        //-------------------
        /// <summary>
        /// プレイヤー初期化
        /// </summary>
        IEnumerator PlayerInitialize()
        {
            PlayerDataProxy InitPlayer;
            for (int i = (int)PlayerType.Player1; i < (int)PlayerType.EndPlayer; i++)
            {

                yield return new WaitForSeconds(1F);  //待ち処理

                InitPlayer = new PlayerDataProxy(i, 0, 0, 1);
                _PlayerMgrObj.GetComponent<PlayerManeger>().Player[i] = InitPlayer;
            }
        }

        /// <summary>
        /// フィールド初期化
        /// </summary>
        private void FieldInitialize()
        {
            FieldDataProxy InitField;
            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                for (int j = 0; j < _prm.FieldSize.x; j++)
                {
                    InitField = new FieldDataProxy(null, -1, -1, false, -1);
                    _AllFieldObj[i][j].GetComponent<TileData_Maneger>().Field = InitField;
                }
            }
        }
        //-------------------
        //変更しました
        //-------------------
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
            //ターン終了後に引くカードの確定
            DrawCardConfirm();

            //カードソート
            UseCardSort();

            //カード使用
            UseCard();

            //クライアントにデータを返すorサーバ終了
            GoNextTurn();
        }

        #endregion

        //-------------------
        //変更しました
        //-------------------
        #region クライアントに送る処理
        [ClientRpc]         //ネットワーク処理
        public void RpcRecvProssess(SendSeverDataArray[] sendData)
        {
            for(int i = (int)PlayerType.Player1; i < (int)PlayerType.EndPlayer; i++)
            {
                if (_PlayerObj[i] != null)
                {
                    _PlayerObj[i].GetComponent<LocalPlayerManeger>().recvSeverData = sendData[i];       //各プレイヤーの受け取るデータ構造体にデータを挿入
                }
            }
            print("返したよ"); //Debug
        }

        #endregion

        #region　デッキ系処理
        //-------------------
        //変更しました
        //-------------------
        /// <summary>
        /// 各プレイヤーのドローカード確定
        /// ※カードを使う前に確定
        /// </summary>
        public void DrawCardConfirm()
        {
            //全プレイヤーループ
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                //カードを引く枚数の確定
                int DrawNum = _CliantMgrObj.GetComponent<FromManeger>().Test(player).GetComponent<FromData>().From.Count();

                //カード選出処理
                for (int i = 0; i < DrawNum; i++)
                {
                    CardDataProxy ConfirmCard = new CardDataProxy(-1,null,null,-1,-1,-1,-1); //初期化を追加
                    //Debug.Log(_DeckMgrObj.GetComponent<DeckManeger>().Deck.Count());     
                    //ドローカード確定
                    ConfirmCard = _DeckMgrObj.GetComponent<DeckManeger>().DrawCardConfirm(_DeckMgrObj.GetComponent<DeckManeger>().Deck.Count());

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
                for (int i = 0; i < _CliantMgrObj.GetComponent<FromManeger>().Test(player).GetComponent<FromData>().From.Count(); i++)
                {
                    _UseCard.Add(_CliantMgrObj.GetComponent<FromManeger>().Test(player).GetComponent<FromData>().From[i]);
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
            for (int i = 0; i < _UseCard.Count(); i++)
            {
                //アクションカードのみ処理を行う
                if (_UseCard[i].CardDataProxy.Type == (int)CardType.Action)
                {
                    //描画リストに追加するための変数を用意
                    List<int> CardDrawData = new List<int>(0);

                    //カードのメソッド処理
                    //ToDo:: 戻り値：CardDrawData、引数：_UseCard[i]でメソッド作成

                    //ToDo:: フィールドにデータを反映

                    //描画リストに追加
                    _DrawData.Add(CardDrawData);

                    //使用されたカードはリストから削除
                    _UseCard.RemoveAt(i);
                }
            }

            //アクション描画リスト終了(111)を当てる
            List<int> DrawActEnd = new List<int>();
            DrawActEnd.Add((int)CardName.DrawActionEnd);
            _DrawData.Add(DrawActEnd);
        }

        /// <summary>
        /// オブジェクトカード使用処理
        /// 使用されたカードは削除します
        /// </summary>
        public void UseObjectCard()
        {
            //オブジェクト描画開始宣言を描画リストに追記
            int StartDrawObj = (int)DrawFaze.DrawObj;
            List<int> StartDrawObjStruct = new List<int>(0);
            StartDrawObjStruct.Add(StartDrawObj);
            _DrawData.Add(StartDrawObjStruct);          //ここで宣言完了です

            //全カード探索
            for (int i = 0; i != _UseCard.Count();)
            {
                //オブジェクトカードのみ処理を行う
                if (_UseCard[i].CardDataProxy.Type == (int)CardType.Object)
                {
                    bool IsBatting = false;
                    List<int> CardDrawData = new List<int>(0);                    //描画リストに追加するための変数を用意
                    List<int> BattingPlayerList = new List<int>(0);               //バッティングさせたプレイヤーリスト

                    //同座標探索
                    for (int j = i + 1; j < _UseCard.Count(); j++)
                    {
                        //同座標発見
                        if (_UseCard[i].FieldPos == _UseCard[j].FieldPos)
                        {
                            //バッティングフラグをオンにする
                            IsBatting = true;

                            //バッティングさせたプレイヤーの番号李外に追加
                            BattingPlayerList.Add(_UseCard[j].PlayerNum);

                            //batting(引っかかったものだけ)したデータを削除する
                            _UseCard.RemoveAt(j);

                            j--;
                        }
                    }

                    //バッティングしたかで描画を分ける
                    if (IsBatting)
                    {
                        //バッティング時の描画リスト作成
                        //バッティングさせたプレイヤーの番号李外に追加
                        BattingPlayerList.Add(_UseCard[i].PlayerNum);

                        //バッティングメソッド処理
                        CardDrawData = CreateDrawListBatting(_UseCard[i], BattingPlayerList);

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
                            //カードのメソッド処理
                            CardDrawData = CreateDrawListObj(_UseCard[i]);

                            //描画リストに追加
                            _DrawData.Add(CardDrawData);

                            //フィールドデータに反映
                            SetFieldData(_UseCard[i]);
                        }

                    }
                    //探索されたカードはリストから削除
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
        private static int CompareByID(ClientDataProxy a, ClientDataProxy b)
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

        /// <summary>
        /// カードがすべて使用完了できたか確認します
        /// </summary>
        /// <returns>すべて使われていたらtrueを返し、使われていないカードをDebug.Logで出力します</returns>
        private bool CheckUseAllCard()
        {
            //使われてないカードがあるぞー！！
            if (_UseCard.Count() != 0)
            {
                for (int i = 0; i < _UseCard.Count();)
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
        //-------------------
        //変更しました
        //-------------------
        /// <summary>
        /// フィールドに使用データをセットします
        /// </summary>
        IEnumerator SetFieldData(ClientDataProxy useCard)
        {
            //新しくフィールドの情報を生成、情報を詰める(カード情報　　　　　、使用プレイヤー、マップチップ、ランクアップしてる？、ランクID)
            FieldDataProxy SetData = new FieldDataProxy(useCard.CardDataProxy, useCard.PlayerNum, 0, false, -1);

            //使用されたカードをフィールド情報に埋め込む
            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().Field = SetData;

            yield return new WaitForSeconds(2F);        //待ち処理
            //疑似フィールド表示データにオブジェクトを生成
            _AllFieldObj[(int)useCard.FieldPos.y][(int)useCard.FieldPos.x].GetComponent<TileData_Maneger>().CreateNewObj(new FieldDataProxy(useCard.CardDataProxy, useCard.PlayerNum, useCard.CardDataProxy.Number, false, -1));
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
                    if (_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.Data != null)
                    {
                        //ランクアップ可能か探索
                        _AllFieldObj[y][x].GetComponent<TileData_Maneger>().SearchRankUp();
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
                    if (_AllFieldObj[y][x].GetComponent<TileData_Maneger>().Field.IsRankUp)
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
                                                Destroy(_AllFieldObj[check_y][check_x].transform.GetChild(0).gameObject);
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
        /// バッディング(99)したときの描画リスト作成
        /// カード番号、X座標、Y座標
        /// </summary>
        /// <param name="useCard"></param>
        public List<int> CreateDrawListBatting(ClientDataProxy useCard, List<int> battingPlayer)
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
        public List<int> CreateDrawListAlreadyObj(ClientDataProxy useCard)
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
        /// レストランの描画リスト作成
        /// カード番号、X座標、Y座標
        /// </summary>
        /// <param name="useCard"></param>
        public List<int> CreateDrawListObj(ClientDataProxy useCard)
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
            //BattingSort();

            //ランクアップ描画データ追加
            AddRankUpDrawData();

            //廃業データ追加
            AddClosedDrawData();
        }

        /// <summary>
        /// バッティングソート
        /// </summary>
        public void BattingSort()
        {
            int DrawActEndSubscript = _DrawData.IndexOf(new List<int>((int)CardName.DrawActionEnd));    //アクション描画コマンドの添え字取得

            //オブジェクト描画のみ探索する
            for (int i = DrawActEndSubscript + 1; i < _DrawData.Count() - (DrawActEndSubscript + 1); i++)
            {
                //バッティング探索
                if (_DrawData[i][0] == (int)CardName.DrawObjBatting)
                {
                    //バッティングした情報を取得
                    List<int> BattingList = _DrawData[i];

                    //バッティングデータを一時削除
                    _DrawData.RemoveAt(i);

                    //バッティングデータをアクション描画終了後に持ってくる
                    _DrawData.Insert(DrawActEndSubscript, BattingList);
                }
            }

            //バッティング描画リスト終了番号(200)を当てる
            List<int> DrawEnd = new List<int>();
            DrawEnd.Add((int)CardName.DrawBattingEnd);
            _DrawData.Add(DrawEnd);
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
                //次のターンへ行くためのメソッド
                _RecvMgrObj.GetComponent<RecvCliantManeger>().CreateRecvObject();           //データを詰めて型変換
  
                RpcRecvProssess(RecvCliantManeger.instance.sendDataArray);           //データをクライアント全体に転送               
            }
            else
            {
                //ゲーム終了処理
                ServerUnInitialized();
            }
        }
        //-------------------
        //変更しました
        //-------------------
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
            }
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


    

        //
    }
}