using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIGameManger : NetworkBehaviour
{
    public static UIGameManger instance;
    public Slider[] PlayerSize;

    public void OnPlayerSizeChanged(SyncListInt.Operation op, int index)
    {
        PlayerSize[index].value = GameManger.GetInstance().size[index];
    }
}
