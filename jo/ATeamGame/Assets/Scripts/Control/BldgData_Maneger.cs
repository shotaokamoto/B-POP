using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Model;

namespace Ctrl
{
    public class BldgData_Maneger : MonoBehaviour
    {
        public static BldgData_Maneger Instance;  //instance

        public GameObject[] obj;
        public GameObject[] collider;
        public GameObject Cobj;
        public Vector3 makePos;
        public bool isBuild = false;
        //デバッグ用
        public int cost = 0;
        private void Awake()
        {
            Instance = this;
        }
        //ポジションのセット
        public void SetPosition(Vector3 pos)
        {
            makePos = pos;
            isBuild = true;
        }
        //建物を作成
        public void MakeBildings()
        {
            if (makePos != null && isBuild)
            {
                isBuild = false;
                //CardController card = GetComponent<CardController>();
                //Debug.Log(card.model._CardCost);
                Instantiate(obj[cost], makePos, transform.rotation);
                cost++;
                if (cost >= obj.Length)
                {
                    cost = 0;
                }
            }
        }
        //コライダーを作成する
        public void makeColider(int count, Vector3 pos, GameObject obj)
        {
            Cobj = Instantiate(collider[count], pos, collider[count].transform.rotation);
            Cobj.gameObject.GetComponent<Collider_Maneger>()._myCost = obj.GetComponent<BldgData>().myCost;
            Cobj.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        //コライダーを消す
        public void coliderOff()
        {
            Destroy(Cobj.gameObject);
        }
        void Update()
        {
            MakeBildings();
        }
    }

}
