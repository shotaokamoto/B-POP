using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIGameMgr : NetworkBehaviour
{
    public Slider[] Playersize;


    public void OnPlayerSizeChanged(SyncListInt.Operation op, int index)
    {
        Playersize[index].value = GameMgr.GetInstance().size[index];
    }



}
