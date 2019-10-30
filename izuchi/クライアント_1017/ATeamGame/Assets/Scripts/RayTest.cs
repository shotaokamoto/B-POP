using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//--------------------------
//  Rayデバッグ用
//--------------------------
public class RayTest : MonoBehaviour
{

    public LayerMask mask;
    public bool ishit = false;
    public Vector3 mousePointer;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            Debug.Log(hit.collider.gameObject.transform.position);
            ishit = true;
            mousePointer = hit.collider.gameObject.transform.position;
        }
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);
    }
}