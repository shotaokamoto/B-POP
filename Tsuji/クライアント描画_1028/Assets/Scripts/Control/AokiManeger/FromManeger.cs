using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;

namespace Ctrl
{
    public class FromManeger : MonoBehaviour
    {
        private GameObject _ServerMgrObj;
        static private GameObject[] _FromDataObj;

        // Start is called before the first frame update
        void Start()
        {
            _ServerMgrObj = GameObject.FindGameObjectWithTag("Server");
            _FromDataObj = GameObject.FindGameObjectsWithTag("FromData");

            for (int player = (int)PlayerType.Player1; player < (int)PlayerType.EndPlayer; player++)
            {
                _FromDataObj[player] = null;
            }
        }

        public GameObject Test(int player)
        {
            return _FromDataObj[player];
        }

        public void SetFromDataToServerMgr()
        {

        }

        //FromDataObjのゲッター、セッター
        public GameObject[] FromDataObj
        {
            get
            {
                return _FromDataObj;
            }
            set
            {
                _FromDataObj = value;
            }
        }
    }
}