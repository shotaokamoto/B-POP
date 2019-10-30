﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public class CloudRotion : MonoBehaviour
    {
        public Transform RotionObject;

        private void FixedUpdate()
        {
            RotionObject.Rotate(new Vector3(0, 0, -0.2F));
        }
    }
}
