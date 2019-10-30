using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minitapioka : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
{
    GetComponent<Renderer>().material.color = new Color32(176, 176, 176, 1);
    GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color32(0, 0, 0, 1));

}

// Update is called once per frame
void Update()
{

    //ミニタピオカ建物を暗色にする
    if (Input.GetKey(KeyCode.Q)) { GetComponent<Renderer>().material.color = new Color32(50, 50, 50, 1); }
    else { GetComponent<Renderer>().material.color = new Color32(176, 176, 176, 1); }


    //ミニタピオカのアウトラインを赤にする
    if (Input.GetKey(KeyCode.W)) { GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color32(255, 0, 0, 1)); }

    //ミニタピオカのアウトラインを青にする
    if (Input.GetKey(KeyCode.E)) { GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color32(0, 0, 255, 1)); }

    //ミニタピオカのアウトラインを青にする
    if (Input.GetKey(KeyCode.R)) { GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color32(0, 255, 0, 1)); }

    //ミニタピオカのアウトラインを黄にする
    if (Input.GetKey(KeyCode.T)) { GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color32(255, 255, 0, 1)); }



}
}