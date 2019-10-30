using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Global;

namespace View
{
    public class TestModelLayer : MonoBehaviour
    {
        public Text TxtHp;
        public Text TxtMp;
        public Text TxtAtk;
        public Text TxtDef;
        public Text TxtDex;

        public Text TxtMaxtHp;
        public Text TxtMaxMp;
        public Text TxtMaxAtk;
        public Text TxtMaxDef;
        public Text TxtMaxDex;




        private void Awake()
        {
            PlayerKernalData.evePlayerKernalData += DisplayHp;
            PlayerKernalData.evePlayerKernalData += DisplayMp;
            PlayerKernalData.evePlayerKernalData += DisplayAtk;
            PlayerKernalData.evePlayerKernalData += DisplayDef;
            PlayerKernalData.evePlayerKernalData += DisplayDex;

            PlayerKernalData.evePlayerKernalData += DisplayMaxHp;
            PlayerKernalData.evePlayerKernalData += DisplayMaxMp;
            PlayerKernalData.evePlayerKernalData += DisplayMaxAtk;
            PlayerKernalData.evePlayerKernalData += DisplayMaxDef;
            PlayerKernalData.evePlayerKernalData += DisplayMaxDex;


        }


        private void Start()
        {
            PlayerKernalDataProxy playerkernalDataObj = new PlayerKernalDataProxy(100,100,10,5,45,
                                                                                                                                               100,100,10,5,50,
                                                                                                                                               0,0,0);

            //初期値表せる
            PlayerKernalDataProxy.GetInstance().DisplayALLOriginalValues();
        }


        #region    イベントクリック

        public void HpUp()
        {
            PlayerKernalDataProxy.GetInstance().IncreaseHpValue(10);
        }

        public void MpUp()
        {
            PlayerKernalDataProxy.GetInstance().IncreaseMpValue(10);
        }

        public void HpDown()
        {
            PlayerKernalDataProxy.GetInstance().DecreaseHpValue(10);
        }

        public void MpDown()
        {
            PlayerKernalDataProxy.GetInstance().DecreaseMpValue(10);
        }
        #endregion

        #region イベントログオン


        private void DisplayHp(KeyValueUpdate kv)
        {
            if(kv.Key.Equals("Health"))
            {
                TxtHp.text = kv.Values.ToString();
            }
        }

        private void DisplayMp(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Magic"))
            {
                TxtMp.text = kv.Values.ToString();
            }
        }

        private void DisplayAtk(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Attack"))
            {
                TxtAtk.text = kv.Values.ToString();
            }
        }

        private void DisplayDef(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Defence"))
            {
                TxtDef.text = kv.Values.ToString();
            }
        }

        private void DisplayDex(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("Dexterity"))
            {
                TxtDex.text = kv.Values.ToString();
            }
        }





        private void DisplayMaxHp(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("MaxHealth"))
            {
                TxtMaxtHp.text = kv.Values.ToString();
            }
        }

        private void DisplayMaxMp(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("MaxMagic"))
            {
                TxtMaxMp.text = kv.Values.ToString();
            }
        }

        private void DisplayMaxAtk(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("MaxAttack"))
            {
                TxtMaxAtk.text = kv.Values.ToString();
            }
        }

        private void DisplayMaxDef(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("MaxDefence"))
            {
                TxtMaxDef.text = kv.Values.ToString();
            }
        }

        private void DisplayMaxDex(KeyValueUpdate kv)
        {
            if (kv.Key.Equals("MaxDexterity"))
            {
                TxtMaxDex.text = kv.Values.ToString();
            }
        }



        #endregion

    }
}

