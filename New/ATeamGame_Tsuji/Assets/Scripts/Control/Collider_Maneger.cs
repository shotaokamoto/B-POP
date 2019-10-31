using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Model;

namespace Ctrl {
    public class Collider_Maneger : MonoBehaviour
    {
        public int _myNumber;
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject);
            Debug.Log(collision.gameObject.transform.parent);
            if (!collision.transform.parent.gameObject.GetComponent<TileData_Maneger>().Field.IsRankUp&&
                _myNumber == collision.transform.parent.GetComponent<TileData_Maneger>().Field.Data.Number)
            {
                collision.gameObject.transform.parent.gameObject.GetComponent<TileData_Maneger>().isBuilding = false;
                Destroy(collision.gameObject);
            }
        }
    }
}
