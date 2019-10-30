using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour
{
    public int myCost;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(myCost);
        if (collision.gameObject.GetComponent<CubeData>().myRank < 2 && 
            myCost == collision.gameObject.GetComponent<CubeData>().myCost)
        {
            Destroy(collision.gameObject);
        }
    }
}
