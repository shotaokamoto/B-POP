using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyphoonEffect : MonoBehaviour
{
    public static TyphoonEffect Instance;

    public GameObject Typhoon;
    public GameObject BackGround;
    
    private void Awake()
    {
        Instance = this;
        Typhoon.SetActive(true);
        BackGround.SetActive(true);
     
    }



    public void GetTyphoonEffect()
    {
        Typhoon.transform.position += Vector3.right * 0.5F;
        if (Typhoon.transform.localPosition.x>= 490.0f)
        {
            Typhoon.SetActive(false);
            BackGround.SetActive(false);            
            if(Typhoon.transform.localPosition.x >= 500.0f&& Typhoon.transform.localPosition.x >= 800.0f)
            {
                Typhoon.SetActive(true);
                BackGround.SetActive(true);
            }
        }
    }

}
