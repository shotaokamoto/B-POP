using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;
using kernal;

namespace Ctrl
{
    public class RecvCliantManeger : MonoBehaviour
    {
        private GlobalParamenter _prm = new GlobalParamenter();      //グローバルパラメータ
        private RecvCliantDataProxy[] _Recv = null;  //返す箱
        private GameObject[] _FieldObj;        //フィールドオブジェ
        private GameObject[] _PlayerObj;       //プレイヤーオブジェ
        private GameObject _ServerObj;         //サーバオブジェ
   //     private GameObject _DeckObj;           //デッキオブジェ
        private GameObject _RecvObj;          //レシーブデータ

        private void Start()
        {
            _FieldObj = GameObject.FindGameObjectsWithTag("Stage");
            _PlayerObj = GameObject.FindGameObjectsWithTag("Player");
            _ServerObj = GameObject.FindGameObjectWithTag("Server");
          //  _DeckObj = GameObject.FindGameObjectWithTag("Deck");
            _RecvObj = GameObject.FindGameObjectWithTag("RecvData");
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public void CreateRecvObject()
        {
            //プレイヤーごとの渡すデータを作成
            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                //フィールドデータを詰める
                StartCoroutine("FieldDataABC");
                //for (int i = 0; i < _prm.FieldSize.y; i++)
                //{
                //    for (int j = 0; j < _prm.FieldSize.x; j++)
                //    {
                //        //yield return new WaitForSeconds(1.5F);
                //        _Recv[player].AllFeildData[i][j] = _FieldObj[i * _prm.FieldSize.y + j].GetComponent<FieldManeger>().Field;
                //    }
                //}

                //プレイヤーデータを詰める
                for (int i = 0; i < _PlayerObj.Length; i++)
                {
                    _Recv[player].AllPlayerData[i] = _PlayerObj[i].GetComponent<PlayerManeger>().Player[i];
                }

                //現在のターン数を詰める
                _Recv[player].NowTurn = _ServerObj.GetComponent<ServerManeger>().Server.Turn;

                //使ったカードの情報を詰める
                _Recv[player].DrawDataList = _ServerObj.GetComponent<ServerManeger>().DrawData;

                //ドローカード情報を詰める
                _Recv[player].DrawCardList = _ServerObj.GetComponent<ServerManeger>().DrawCard[player];

            }

            _RecvObj.GetComponent<RecvData>().Recv = _Recv;
        }

        /// <summary>
        /// フィールドデータを詰める
        /// </summary>
        /// <returns></returns>
        IEnumerator FieldDataABC()
        {
            
            int player = (int)PlayerType.Player1;
            
            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                for (int j = 0; j < _prm.FieldSize.x; j++)
                {
                    yield return new WaitForSeconds(1.5F);
                    Log.Write("フィールドデータを詰めましたよ！！！");
                    _Recv[player].AllFeildData[i][j] = _FieldObj[i * _prm.FieldSize.y + j].GetComponent<FieldManeger>().Field;
                }
            }
        }



    }
}