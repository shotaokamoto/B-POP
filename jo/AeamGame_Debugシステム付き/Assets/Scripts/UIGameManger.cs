using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using kernal;

public class UIGameManger : NetworkBehaviour
{
    public Slider[] PlayerSize;
    
    public void OnPlayerSizeChanged(SyncListInt.Operation op, int index)
    {
        Log.Write("dasdas dsaasdasd");
        PlayerSize[index].value = GameManger.GetInstance().size[index];
    }
}
