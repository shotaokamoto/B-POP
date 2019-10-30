using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;

namespace Ctrl
{
    public class ServerManeger : MonoBehaviour
    {
        private ServerDataProxy _Server;

        private DeckManeger _DeckManeger;

        private List<CardDataProxy>[] _DrawCard;    //各プレーヤが引くカードリスト
        private List<List<int>> _DrawData;          //描画リスト

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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