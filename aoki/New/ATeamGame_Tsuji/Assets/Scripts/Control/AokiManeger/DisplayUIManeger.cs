using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Global;
using System.Linq;
using UnityEngine.UI;

namespace Ctrl
{
    public class DisplayUIManeger : MonoBehaviour
    {
        //UIの変数一覧
        public Color[] PlayerColor;
        public Sprite[] Rank;        //ランキングの画像配列
        public Image[] DisplayRank;
        public Text[] DisplayScore;
        public Text Turn;
        public Text Time;
        public Text Cost;
        public GameObject UIPanel;
        public GameObject ResultPanel;
        public GameObject UIWaiting;

        // Start is called before the first frame update
        void Start()
        {
            //リザルトを非アクティブにする
            ResultPanel.SetActive(false);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            
        }

        #region テキストの数値をセット

        /// <summary>
        /// 時間をセット
        /// </summary>
        /// <param name="time"></param>
        public void SetClientTime(int time)
        {
            Time.text = time.ToString();
        }

        /// <summary>
        /// ターンをセット
        /// </summary>
        /// <param name="turn"></param>
        public void SetClientTurn(int turn)
        {
            Turn.text = turn.ToString();
        }

        /// <summary>
        /// コストのセット
        /// </summary>
        /// <param name="cost"></param>
        public void SetClientCost(int cost)
        {
            Cost.text = cost.ToString();
        }

        #endregion


        public void ChangeRanking(PlayerDataProxy[] playerData)
        {
            //ランキング順にした変数
            PlayerDataProxy[] RankingSort = new PlayerDataProxy[4];

            //ソート
            for (int i = 0; i < 4; i++)
            {
                RankingSort[playerData[i].Ranking - 1] = playerData[i];
            }

            //表示、変更します
            //1
            DisplayRank[0].sprite = Rank[RankingSort[0].Ranking];

            //2

            //3

            //4
        }

        /// <summary>
        /// プレイヤー待ち処理のパネルを消します
        /// </summary>
        public void HiddenWaitPanel()
        {
            UIWaiting.SetActive(false);
        }

        /// <summary>
        /// リザルトのUIを表示
        /// </summary>
        public void ShowResultPanel()
        {
            ResultPanel.SetActive(true);
        }
    }
}