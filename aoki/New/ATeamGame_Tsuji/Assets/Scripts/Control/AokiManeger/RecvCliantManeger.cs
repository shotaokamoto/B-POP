using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;

namespace Ctrl
{
    public class RecvCliantManeger : MonoBehaviour
    {
        public static RecvCliantManeger instance;               //インスタンス化
        private GlobalParamenter _prm = new GlobalParamenter(6,6);      //グローバルパラメータ

        //private RecvCliantDataProxy[] _Recv = null;  //返す箱
        private GameObject[] _FieldObj;        //フィールドオブジェ
        private GameObject[] _PlayerObj;       //プレイヤーオブジェ
        private GameObject _ServerObj;         //サーバオブジェ
                                               //   private GameObject _DeckObj;           //デッキオブジェ
        private GameObject _RecvObj;          //レシーブデータ

        public RecvCliantData[] recvData = new RecvCliantData[(int)PlayerType.EndPlayer];               //クライアントに返す為のclassの配列
        public SendSeverDataArray[] sendDataArray = new SendSeverDataArray[(int)PlayerType.EndPlayer];  //クライアントに返す為の構造体の配列

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            _FieldObj = GameObject.FindGameObjectsWithTag("Stage");
            _PlayerObj = GameObject.FindGameObjectsWithTag("PlayerData");
            _ServerObj = GameObject.FindGameObjectWithTag("Server");
            //  _DeckObj = GameObject.FindGameObjectWithTag("Deck");
            _RecvObj = GameObject.FindGameObjectWithTag("RecvData");
        }

        //情報をまとめて送り返す処理
        public void CreateRecvObject(FieldDataProxy[][] Field,PlayerDataProxy[] Player,int Turn,List<List<int>> DrawData,List<CardDataProxy>[] DrawCard)
        {
            _PlayerObj = GameObject.FindGameObjectsWithTag("Player");

            //プレイヤーごとの渡すデータを作成
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                //初期化
                CardDataProxy InitCard = new CardDataProxy(-1, "", "", -1, -1, -1, -1);
                FieldDataProxy[,] AllFieldObj;
                AllFieldObj = new FieldDataProxy[_prm.FieldSize.y, _prm.FieldSize.x];
                for (int i = 0; i < _prm.FieldSize.y; i++) 
                {
                    for (int j = 0; j < _prm.FieldSize.x; j++)
                    {
                        AllFieldObj[i, j] = Field[i][j];
                    }
                }

                recvData[player] = new RecvCliantData(AllFieldObj, Player, Turn, DrawData, DrawCard[player]);

                //ここから処理

                //フィールドデータを詰める
                for (int i = 0; i < _prm.FieldSize.y; i++)
                {
                    for (int j = 0; j < _prm.FieldSize.x; j++)
                    {
                        if (Field[i][j].Data != null)
                        {
                            Debug.Log(recvData[player].AllFeildData[i, j]._CardData = _FieldObj[i * _prm.FieldSize.y + j].GetComponent<TileData_Maneger>().Field._CardData);

                            //各プレイヤーのrecvDataに対して、フィールドデータを入れている
                            recvData[player].AllFeildData[i, j]._CardData = Field[i][j]._CardData;
                            recvData[player].AllFeildData[i, j]._IsRankUp = Field[i][j]._IsRankUp;
                            recvData[player].AllFeildData[i, j]._MaptipNumber = Field[i][j]._MaptipNumber;
                            recvData[player].AllFeildData[i, j]._PossessionPlayer = Field[i][j]._PossessionPlayer;
                            recvData[player].AllFeildData[i, j]._RankID = Field[i][j]._RankID;
                        }
                        else
                        {
                            print("ないよ");
                        }
                    }
                }

                //プレイヤーデータを詰める
                for (int i = 0; i < _PlayerObj.Length; i++)
                {
                    recvData[player].AllPlayerData[i] = Player[i];
                }

                //現在のターン数を詰める
                recvData[player].NowTurn = Turn;

                //使ったカードの情報を詰める
                recvData[player].DrawDataList = DrawData;

                //ドローカード情報を詰める
                recvData[player].DrawCardList = DrawCard[player];
            }

            //全プレイヤーの情報を詰める
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                //クラスを構造体に変換し、構造体配列に代入
                sendDataArray[player] = ClassConvertArray(recvData[player]);
            }
            //_RecvObj.GetComponent<RecvData>().Recv = recvData;
        }

        //RecvCliantDataクラスをSendSeverDataArray構造体に変換

        public SendSeverDataArray ClassConvertArray(RecvCliantData sever)
        {
            //work用
            SendSeverDataArray sData;
            //初期化
            sData.AllFeildData = new SendFieldDataArray[_prm.FieldSize.y * _prm.FieldSize.x];
            sData.AllPlayerData = new SendPlayerDataArray[(int)PlayerType.EndPlayer];
            sData.NowTurn = 0;
            sData.DrawCardList = new SendCardDataArray[sever.DrawCardList.Count];
            sData.DrawDataList = new IntArrayStructure[sever.DrawDataList.Count];
            int sDataCount = 0;

            for (int i = 0; i < sever.DrawDataList.Count; i++)
            {
                sData.DrawDataList[i].vs = new int[sever.DrawDataList[i].Count];
            }

            //フィールドデータを構造体に変換
            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                for (int j = 0; j < _prm.FieldSize.x; j++)
                {
                    if (sever.AllFeildData[i, j]._CardData != null)
                    {
                        //カードデータを構造体に変換する
                        sData.AllFeildData[sDataCount].SendCardData.Cost = sever.AllFeildData[i, j]._CardData.Cost;
                        sData.AllFeildData[sDataCount].SendCardData.Name = sever.AllFeildData[i, j]._CardData.Name;
                        sData.AllFeildData[sDataCount].SendCardData.Number = sever.AllFeildData[i, j]._CardData.Number;
                        sData.AllFeildData[sDataCount].SendCardData.Rarity = sever.AllFeildData[i, j]._CardData.Rarity;
                        sData.AllFeildData[sDataCount].SendCardData.Score = sever.AllFeildData[i, j]._CardData.Score;
                        sData.AllFeildData[sDataCount].SendCardData.Text = sever.AllFeildData[i, j]._CardData.Text;
                        sData.AllFeildData[sDataCount].SendCardData.Type = sever.AllFeildData[i, j]._CardData.Type;
                    }

                    //残りのデータも変換 
                    sData.AllFeildData[sDataCount].IsRankUp = sever.AllFeildData[i, j].IsRankUp;
                    sData.AllFeildData[sDataCount].MaptipNumber = sever.AllFeildData[i, j].MaptipNumber;
                    sData.AllFeildData[sDataCount].PossessionPlayer = sever.AllFeildData[i, j].PossessionPlayer;
                    sData.AllFeildData[sDataCount].RankID = sever.AllFeildData[i, j].RankID;
                    sDataCount++;
                }
            }
            //プレイヤーデータを構造体に変換
            for (int i = (int)PlayerType.Player1; i < (int)PlayerType.EndPlayer; i++)
            {
                sData.AllPlayerData[i].PlayerNumber = sever.AllPlayerData[i].PlayerNumber;
                sData.AllPlayerData[i].Ranking = sever.AllPlayerData[i].Ranking;
                sData.AllPlayerData[i].ScorePerTurn = sever.AllPlayerData[i].ScorePerTurn;
                sData.AllPlayerData[i].TotalScore = sever.AllPlayerData[i].TotalScore;
            }
            //描画メソッドを構造体に変換
            for (int i = 0; i < sever.DrawDataList.Count; i++)
            {
                for (int j = 0; j < sever.DrawDataList[i].Count; j++)
                {
                    sData.DrawDataList[i].vs[j] = sever.DrawDataList[i][j];
                }
            }
            //引くカードのデータを構造体に変換
            for (int i = 0; i < sever._DrawCardList.Count; i++)
            {
                sData.DrawCardList[i].Name = sever.DrawCardList[i].Name;
                sData.DrawCardList[i].Number = sever.DrawCardList[i].Number;
                sData.DrawCardList[i].Rarity = sever.DrawCardList[i].Rarity;
                sData.DrawCardList[i].Score = sever.DrawCardList[i].Score;
                sData.DrawCardList[i].Text = sever.DrawCardList[i].Text;
                sData.DrawCardList[i].Type = sever.DrawCardList[i].Type;
            }
            //
            sData.isRecvFinished = true;
            return sData;
        }
    }
}