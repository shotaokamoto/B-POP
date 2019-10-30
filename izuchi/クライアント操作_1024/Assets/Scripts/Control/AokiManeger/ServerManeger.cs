using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;
using System.Linq;

namespace Ctrl
{
    public class ServerManeger : MonoBehaviour
    {
        private ServerDataProxy _Server = null;
        private GlobalParamenter _prm = new GlobalParamenter();

        private DeckManeger _DeckManeger;

        private List<CardDataProxy>[] _DrawCard = null;     //各プレーヤが引くカードリスト
        private List<List<int>> _DrawData = null;          //描画リスト
        private List<ClientDataProxy> _UseCard = null;     //使用カード

        public GameObject[][] _AllFieldObj;

        private GameObject _RecvMgrObj;                //レシーブマネージャのオブジェ
        private GameObject _CliantMgrObj;              //クライアントから持ってきたデータのオブジェ

        private bool _AllClientDataCollection = false;  //クライアントからデータが全員分送られてきたらtrueにします。

        // Start is called before the first frame update
        void Start()
        {
            //各オブジェの取得
            _RecvMgrObj = GameObject.FindGameObjectWithTag("RecvMgr");
            _CliantMgrObj = GameObject.FindGameObjectWithTag("FromMgr");

            //フィールドデータを整地して作成する
            CreateFieldData();
                
            //サーバの初期化
            ServerInitialize();
        }

        private void FixedUpdate()
        {
            //クライアントからデータ集まった?


            //クライアントからデータがすべて集まったらサーバ処理開始
            if(_AllClientDataCollection)   //ToDo::サーバにデータが集まったらっていう条件式
            {
                ServerMain();
            }
        }

        #region 初期処理

        /// <summary>
        /// サーバの初期化を行います
        /// </summary>
        public void ServerInitialize()
        {
            //サーバデータ作成
            _Server = new ServerDataProxy(0, null, null, null);

            //現在のターン数を0にする
            _Server.Turn = 0;

            //ToDo::プレイヤー、フィールド、デッキの初期化は各マネージャから呼び出します。

            //フィールドデータの新規作成

        }

        /// <summary>
        /// フィールドデータを整地して作成する
        /// </summary>
        private void CreateFieldData()
        {
            _AllFieldObj = new GameObject[_prm.FieldSize.y][];
            for(int i = 0;i<_prm.FieldSize.y;i++)
            {
                _AllFieldObj[i] = new GameObject[(int)_prm.FieldSize.y];
            }

            GameObject[] allFieldData = GameObject.FindGameObjectsWithTag("Stage");

            for (int i = 0; i < allFieldData.Length; i++)
            {
                _AllFieldObj[(int)allFieldData[i].GetComponent<TileData_Maneger>().planePos.y]
                            [(int)allFieldData[i].GetComponent<TileData_Maneger>().planePos.x] = allFieldData[i];
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

        #region クライアントに送る処理

        public void RecvProssess()
        {
            _RecvMgrObj.GetComponent<RecvCliantManeger>().CreateRecvObject();
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
            for (int player = (int)PlayerType.Player1; player > (int)PlayerType.EndPlayer; player++)
            {
                //カードを引く枚数の確定
                int DrawNum = _CliantMgrObj.GetComponent<FromManeger>().Test(player).GetComponent<FromData>().From.Count();

                //カード選出処理
                for (int i = 0; i < DrawNum; i++)
                {
                    CardDataProxy ConfirmCard = null;

                    //ToDo:: ConfirmCard = DeckManegerからのカード引く処理

                    //ドローカードリストに追加
                    //_DrawCard[player].Add(ConfirmCard);
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
            
            //カード使用チェック
            if(!CheckUseAllCard())
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
            for (int player = (int)PlayerType.Player1; player > (int)PlayerType.EndPlayer; player++)
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
                if(_UseCard[i].CardDataProxy.Type == (int)CardType.Action)
                {
                    //描画リストに追加するための変数を用意
                    List<int> CardDrawData = new List<int>(0);

                    //カードのメソッド処理
                    //ToDo:: 戻り値：CardDrawData、引数：_UseCard[i]でメソッド作成

                    //描画リストに追加
                    _DrawData.Add(CardDrawData);

                    //使用されたカードはリストから削除
                    _UseCard.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// オブジェクトカード使用処理
        /// 使用されたカードは削除します
        /// </summary>
        public void UseObjectCard()
        {
            //オブジェクト描画開始宣言を描画リストに追記
            int StartDrawObj = (int)DrawFaze.DrawObj;
            List<int> StartDrawObjStruct = null;
            StartDrawObjStruct.Add(StartDrawObj);
            _DrawData.Add(StartDrawObjStruct);          //ここで宣言完了です

            //全カード探索
            for (int i = 0; _UseCard[i] != _UseCard[_UseCard.Count()];)
            {
                //オブジェクトカードのみ処理を行う
                if (_UseCard[i].CardDataProxy.Type == (int)CardType.Object)
                {
                    bool IsBatting = false;
                    List<int> CardDrawData = new List<int>(0);                    //描画リストに追加するための変数を用意

                    //同座標探索
                    for (int j = 0; j < _UseCard.Count() - (i + 1); j++)
                    {
                        //同座標発見
                        if(_UseCard[i].FieldPos == _UseCard[j].FieldPos)
                        {
                            //バッティングフラグをオンにする
                            IsBatting = true;

                            //batting(引っかかったものだけ)したデータを削除する
                            _UseCard.RemoveAt(j);

                            j--;
                        }
                    }

                    //バッティングしたかで描画を分ける
                    if(IsBatting)
                    {
                        //バッティング時の描画リスト作成
                        //カードのメソッド処理
                        //ToDo:: 戻り値：CardDrawData、引数：_UseCard[i]でメソッド作成

                        //描画リストに追加
                        _DrawData.Add(CardDrawData);
                    }
                    else
                    {
                        //アクションカードですでにオブジェが入ってしまっている場合
                        if (_AllFieldObj[(int)_UseCard[i].FieldPos.x]
                                        [(int)_UseCard[i].FieldPos.y].GetComponent<FieldManeger>().Field.Data != null)
                        {
                            //カードのメソッド処理
                            //ToDo:: 戻り値：CardDrawData、引数：_UseCard[i]でメソッド作成

                            //描画リストに追加
                            _DrawData.Add(CardDrawData);
                        }
                        else
                        {
                            //カードのメソッド処理
                            //ToDo:: 戻り値：CardDrawData、引数：_UseCard[i]でメソッド作成

                            //描画リストに追加
                            _DrawData.Add(CardDrawData);

                            //フィールドデータに反映
                        }

                    }
                    //探索されたカードはリストから削除
                    _UseCard.RemoveAt(i);
                }
            }
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

        #region クライアントに送信する系処理
        
        /// <summary>
        /// 1ターンのサーバ側処理が終わった時に使う
        /// 用途はターン数を追加したり、データ詰めたりなど
        /// </summary>
        public void GoNextTurn()
        {
            Server.Turn++;

            //ゲーム終了かどうか
            if (Server.Turn < _prm.MaxTurn)
            {
                //次のターンへ行くためのメソッド
                _RecvMgrObj.GetComponent<RecvCliantManeger>().CreateRecvObject();
            }
            else
            {
                //ゲーム終了処理
                ServerUnInitialized();
            }

            //クライアントからデータをもらったフラグをへしおるよん
            _AllClientDataCollection = false;
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