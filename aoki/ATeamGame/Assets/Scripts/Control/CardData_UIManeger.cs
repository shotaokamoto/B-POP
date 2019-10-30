using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using UnityEngine.UI;

using Model;

namespace Ctrl
{
    public class CardData_UIManeger : MonoBehaviour
    {
        public int AddNumValue;
        public string Name;
        public string Text;
        public int AddCostValue;
        public int AddScoreValue;
        public int Type;
        public int Rarity;

        public int InitNumber;
        public string InitName;
        public string InitText;
        public int InitCost;
        public int InitScore;
        public int InitType;
        public int InitRarity;

        public Text TxtNumber;
        public Text TxtName;
        public Text TxtText;
        public Text TxtCost;
        public Text TxtScore;
        public Text TxtType;

        private void Awake()
        {
            CardData.eveCardData += DisplayNumber;
            CardData.eveCardData += DisplayName;
            CardData.eveCardData += DisplayText;
            CardData.eveCardData += DisplayCost;
            CardData.eveCardData += DisplayScore;
            CardData.eveCardData += DisplayType;
        }

        private void Start()
        {
            CardDataProxy cardObj = new CardDataProxy(InitNumber, InitName, InitText, InitCost, InitScore, InitType, InitRarity);

            //初期表示
            CardDataProxy.GetInstance().DisplayALLOriginalValues();
        }

        #region イベントクリック

        public void AddNumber()
        {
            CardDataProxy.GetInstance().AddNumber(AddNumValue);
        }

        public void ChangeName()
        {
            CardDataProxy.GetInstance().ChangeName(Name);
        }

        public void ChangeText()
        {
            CardDataProxy.GetInstance().ChangeText(Text);
        }

        public void AddCost()
        {
            CardDataProxy.GetInstance().AddCost(AddCostValue);
        }

        public void AddScore()
        {
            CardDataProxy.GetInstance().AddScore(AddScoreValue);
        }

        public void ChangeType()
        {
            CardDataProxy.GetInstance().ChangeType(Type);
        }

        #endregion

        #region イベントログオン

        private void DisplayNumber(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Number"))
            {
                TxtNumber.text = kv.Values.ToString();
            }
        }
        private void DisplayName(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Name"))
            {
                TxtName.text = kv.Values.ToString();
            }
        }
        private void DisplayText(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Text"))
            {
                TxtText.text = kv.Values.ToString();
            }
        }
        private void DisplayCost(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Cost"))
            {
                TxtCost.text = kv.Values.ToString();
            }
        }
        private void DisplayScore(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Score"))
            {
                TxtScore.text = kv.Values.ToString();
            }
        }
        private void DisplayType(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Type"))
            {
                TxtType.text = kv.Values.ToString();
            }
        }


        #endregion
    }
}
