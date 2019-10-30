using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

using Model;

namespace Ctrl
{
    public class BldgData_Maneger : NetworkBehaviour
    {
        public static BldgData_Maneger Instance;  //instance

        public GameObject[] obj;
        public GameObject[] colliderObj;
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

        //コライダーを作成する
        public void makeColider(int count, Vector3 pos, GameObject obj)
        {
            Cobj = Instantiate(colliderObj[count], pos, colliderObj[count].transform.rotation);
            Cobj.gameObject.GetComponent<Collider_Maneger>()._myCost = obj.GetComponent<BldgData>().myCost;
            Cobj.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        //コライダーを消す
        public void coliderOff()
        {
            Destroy(Cobj.gameObject);
        }
        void FixedUpdate()
        {
            if (!isLocalPlayer) return;     //他プレイヤーには何もしない
            if (Input.GetMouseButtonDown(0))        //クリックされていれば
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Stage"))
                    {
                        SetPosition(new Vector3(hit.transform.position.x, hit.transform.position.y + 0.5f, hit.transform.position.z));
                        if (makePos != null && isBuild)
                        {
                            isBuild = false;
                            // Commandによってサーバーに通知する
                            CmdMakeBildings(makePos, cost);
                            cost++;
                            //当たり判定を消す
                            if (Cobj != null)
                            {
                                coliderOff();
                            }
                            if (cost >= obj.Length)
                            {
                                cost = 0;
                            }
                        }
                    }
                }
            }
        }

        //建物を作成
        [Command]
        public void CmdMakeBildings(Vector3 pos, int cos)
        {
            GameObject building = Instantiate(obj[cos], pos, transform.rotation);
            NetworkServer.Spawn(building);
        }
    }
}
