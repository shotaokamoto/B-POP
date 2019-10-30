using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManger : NetworkBehaviour
{
    private static GameManger _Instance;

    public static GameManger GetInstance()
    {
        return _Instance;
    }

    public Player[] players;

    public SyncListInt size = new SyncListInt();

    public UIGameManger UI;

    private void Awake()
    {
        _Instance = this;
    }

    //サーバ繋がる時
    public override void OnStartServer()
    {
        if(size.Count!=players.Length)
        {
            for (int i = 0; i < players.Length; i++)
            {
                size.Add(0);
            }
        }
    }

    //クライアント起動時
    public override void OnStartClient()
    {
        size.Callback = UI.OnPlayerSizeChanged;
        for (int i = 0; i < players.Length; i++)
        {
            UI.OnPlayerSizeChanged(SyncListInt.Operation.OP_DIRTY, i);
        }
    }

    public int GetPlayerFill()
    {
        int PlayerNo = 0;
        int min = size[0];

        for (int i = 0; i < players.Length; i++)
        {
            if(size[i]<min)
            {
                min = size[i];
                PlayerNo = i;
            }
        }
        return PlayerNo;
    }












}

/// <summary>
/// プレイヤーの構造体
/// </summary>
[System.Serializable]
public class Player
{
    public string Playername;                                                   //プレイヤーの名前
}