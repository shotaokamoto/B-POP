using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotion : MonoBehaviour
{
    public Transform RotionObject;

    private void FixedUpdate()
    {
        RotionObject.Rotate(new Vector3(0, 0, 0.5F));
    }
}
