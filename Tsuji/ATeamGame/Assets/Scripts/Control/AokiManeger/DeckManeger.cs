using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;

namespace Ctrl
{
    public class DeckManeger : MonoBehaviour
    {
        private DeckDataProxy _Deck;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //デッキデータのゲッター、セッター
        public DeckDataProxy Deck
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
    }
}