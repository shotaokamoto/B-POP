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
        public void MakeBildings(int number,GameObject parent)
        {
            if (makePos != null && isBuild)
            {
                isBuild = false;
                GameObject c = Instantiate(obj[number], makePos, transform.rotation,parent.transform);  //number - 1にしないと配列がずれます
                parent.GetComponent<TileData_Maneger>().childObj = c;            //親のタイルデータに自分を入れます

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
            Cobj.gameObject.GetComponent<Collider_Maneger>()._myNumber = obj.transform.parent.GetComponent<TileData_Maneger>().Field.Data.Number;
            Cobj.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        //コライダーを消す
        public void coliderOff()
        {
            Destroy(Cobj.gameObject);
        }
        void Update()
        {
            //MakeBildings();
        }
    }

}
