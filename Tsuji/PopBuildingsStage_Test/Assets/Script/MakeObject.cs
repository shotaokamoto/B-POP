using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObject : MonoBehaviour
{
    public static MakeObject Instance;

    public GameObject[] obj;
    public GameObject[] colider;
    public Vector3 makePos;
    public bool isBuild = false;
    public int  Objcount = 0;
    private void Awake()
    {
        Instance = this;
    }
    public void makeObject(Vector3 pos)
    {
        makePos = pos;
        isBuild = true;
    }
    public void makeColider(int count,Vector3 pos,GameObject obj)
    {
        Instantiate(colider[count], pos, colider[count].transform.rotation);
        colider[count].gameObject.GetComponent<Colision>().myCost = obj.GetComponent<CubeData>().myCost;
        Debug.Log(colider[count].gameObject.GetComponent<Colision>().myCost);
    }
    // Update is called once per frame
    void Update()
    {
        if(makePos != null && isBuild)
        {
            isBuild = false;
            Instantiate(obj[Objcount], makePos, transform.rotation);
            Objcount++;
            if(Objcount >= obj.Length)
            {
                Objcount = 0;
            }
        }
    }
}
