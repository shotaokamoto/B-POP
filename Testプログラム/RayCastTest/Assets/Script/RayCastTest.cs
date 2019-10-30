using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    public static RayCastTest Instance;

    GameObject Obj;
    public bool isDragEnd = false;
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        // Click確認
        if (isDragEnd)
        {
            isDragEnd = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Obj = hit.collider.gameObject;
                Debug.DrawRay(ray.origin, ray.direction * 3, Color.green, 5, false);
                Debug.Log(Obj);
                
            }
        }
    }
}