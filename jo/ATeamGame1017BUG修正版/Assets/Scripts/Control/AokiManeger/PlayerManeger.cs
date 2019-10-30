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