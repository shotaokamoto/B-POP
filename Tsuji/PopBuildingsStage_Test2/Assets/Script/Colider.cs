using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colider : MonoBehaviour
{
    public int _myCost;
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(_myCost);
        if (collision.gameObject.GetComponent<CubeData>().myRank < 2 &&
            _myCost == collision.gameObject.GetComponent<CubeData>().myCost)
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<TileBase>().isBuilding = false;
            //collision.gameObject.transform.parent.gameObject.GetComponent<TileBase>().isBuilding = false;
            Destroy(collision.gameObject);
        }
    }
}
