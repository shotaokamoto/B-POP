using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Model;

namespace Ctrl {
    public class Collider_Maneger : MonoBehaviour
    {
        public int _myCost;
        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log(_myCost);
            if (collision.gameObject.GetComponent<BldgData>().myRank < 2 &&
                _myCost == collision.gameObject.GetComponent<BldgData>().myCost)
            {
                collision.gameObject.transform.parent.gameObject.GetComponent<TileData_Maneger>().isBuilding = false;
                //collision.gameObject.transform.parent.gameObject.GetComponent<TileBase>().isBuilding = false;
                Destroy(collision.gameObject);
            }
        }
    }
}
