using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteo_animation : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.A)) {
            // アニメーターで動かす
            GetComponent<Animator>().SetBool("meteo",true);
			
			
    }
	if (Input.GetKeyUp (KeyCode.A)){
	GetComponent<Animator>().SetBool("meteo",false);
	}
	}
}
