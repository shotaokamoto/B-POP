using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIGameManger : NetworkBehaviour
{
    public static UIGameManger Instance;

    public Slider[] PlayerSize;
    public GameObject UIPanel;
    public GameObject UIWaiting;
    public GameObject _UIMgr;

    [SyncVar]
    public bool IsStart = false;

    private void Awake()
    {
        Instance = this;      
    }

    //クライアント起動時
    public override void OnStartClient()
    {
        UIPanel.SetActive(false);
        UIWaiting.SetActive(true);
        _UIMgr.SetActive(false);
    }

    [ClientRpc]
    public void RpcWaitting()
    {
        UIPanel.SetActive(false);
        UIWaiting.SetActive(true);
        _UIMgr.SetActive(false);
    }

    [ClientRpc]
    public void RpcStarting()
    {
        UIPanel.SetActive(true);
        UIWaiting.SetActive(false);
        _UIMgr.SetActive(true);
    }

    public void OnPlayerSizeChanged(SyncListInt.Operation op, int index)
    {
        PlayerSize[index].value = GameManger.GetInstance().size[index];
    }



}
