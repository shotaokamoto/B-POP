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
        private GameObject[] _FromDataObj;

        // Start is called before the first frame update
        void Start()
        {
            _ServerMgrObj = GameObject.FindGameObjectWithTag("Server");
            _FromDataObj = GameObject.FindGameObjectsWithTag("FromData");
        }

        public GameObject Test(int player)
        {
            return _FromDataObj[player];
        }

        public void SetFromDataToServerMgr()
        {

        }
    }
}