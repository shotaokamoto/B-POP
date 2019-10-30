using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Cpp : MonoBehaviour
{
    public static A_Cpp Instance;     //このクラスの実体

    private void Awake()
    {
        Instance = this;
    }



    public void StartCor()
    {
        //todo
    }
}
