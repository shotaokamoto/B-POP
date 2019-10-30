using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;

namespace Ctrl
{
    public class PlayerManeger : MonoBehaviour
    {
        private PlayerDataProxy[] _Player = new PlayerDataProxy[4];

        private void Awake()
        {
            for (int i = 0; i < 4; i++)
            {
                _Player[i] = new PlayerDataProxy(-1, -1, -1, -1);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //プレイヤーデータのゲッター、セッター
        public PlayerDataProxy[] Player
        {
            get
            {
                return _Player;
            }
            set
            {
                _Player = value;
            }
        }
    }
}