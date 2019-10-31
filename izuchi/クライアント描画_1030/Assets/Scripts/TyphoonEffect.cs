using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyphoonEffect : MonoBehaviour
{
    public static TyphoonEffect Instance;

    public GameObject Typhoon;
    public GameObject BackGround;
    float TyphoonSpeed;
    private void Awake()
    {
        Instance = this;
        Typhoon.SetActive(true);
        BackGround.SetActive(true);
        TyphoonSpeed = 0.5f;
    }

    public void GetTyphoonEffect()
    {
        Typhoon.transform.position += Vector3.right * TyphoonSpeed;
        if (Typhoon.transform.localPosition.x >= 490.0f)
        {
            Typhoon.SetActive(false);
            BackGround.SetActive(false);
            if (Typhoon.transform.localPosition.x >= 600.0f)
            {
                TyphoonSpeed = 0.0f;
                Typhoon.SetActive(true);
                BackGround.SetActive(true);

                //Typhoon.transform.localPosition.x = -483.0f;
                Typhoon.GetComponent<RectTransform>().localPosition = new Vector3(-483.0f, 0.0f, 0.0f);
            }
        }
    }

}
