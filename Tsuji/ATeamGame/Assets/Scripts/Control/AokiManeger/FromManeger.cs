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
        private ClientData[] _FromData;

        // Start is called before the first frame update
        void Start()
        {
            _ServerMgrObj = GameObject.FindGameObjectWithTag("Server");
            //_FromDataObj = ClientData.FindGameObjectsWithTag("FromData");
        }

        public ClientData Test(int player)
        {
            return _FromData[player];
        }

        public void SetFromDataToServerMgr()
        {

        }
    }
}