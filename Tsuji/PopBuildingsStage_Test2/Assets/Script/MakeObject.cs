using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObject : MonoBehaviour
{
    public static MakeObject Instance;  //instance

    public GameObject[] obj;
    public GameObject[] colider;
    public GameObject Cobj;
    public Vector3 makePos;
    public bool isBuild = false;
    public int  Objcount = 0;
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
            Instantiate(obj[Objcount], makePos, transform.rotation);
            Objcount++;
            if (Objcount >= obj.Length)
            {
                Objcount = 0;
            }
        }
    }
    //コライダーを作成する
    public void makeColider(int count,Vector3 pos,GameObject obj)
    {
        Cobj = Instantiate(colider[count], pos, colider[count].transform.rotation);
        Cobj.gameObject.GetComponent<Colider>()._myCost = obj.GetComponent<CubeData>().myCost;
        Cobj.gameObject.GetComponent<BoxCollider>().enabled = true;
        //Debug.Log(colider[count].gameObject.GetComponent<Colision>().myCost);
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
