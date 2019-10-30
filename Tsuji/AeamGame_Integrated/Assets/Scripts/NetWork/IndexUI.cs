using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexUI : MonoBehaviour
{
    public void StartButtonClick()
    {
        NetworkMangerCustom.NetworkGame();
    }

    public void QuitButtonClick()
    {
        Application.Quit();
    }

}
