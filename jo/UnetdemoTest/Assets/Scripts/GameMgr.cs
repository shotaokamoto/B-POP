using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMgr : NetworkBehaviour
{
    private static GameMgr _Instance;

    public static GameMgr GetInstance()
    {
        return _Instance;
    }

    public Team[] teams;

    public SyncListInt size = new SyncListInt();

    public UIGameMgr UI;

    private void Awake()
    {
        _Instance = this;
    }

    public override void OnStartServer()
    {
        if(size.Count!=teams.Length)
        {
            for (int i = 0; i < teams.Length; i++)
            {
                size.Add(0);
            }
        }
    }

    public override void OnStartClient()
    {
        size.Callback = UI.OnPlayerSizeChanged;

        for (int i = 0; i < teams.Length; i++)
        {
            UI.OnPlayerSizeChanged(SyncListInt.Operation.OP_DIRTY, i);
        }

    }

    public int GetTeamFill()
    {
        int teamNo = 0;
        int min = size[0];
        for (int i = 0; i < teams.Length; i++)
        {
            if(size[i]<min)
            {
                min = size[i];
                teamNo = i;
            }
        }
        return teamNo;
    }

    public Vector3 GetSpawnPosition(int teamIndex)
    {
        Vector3 pos = teams[teamIndex].spawn.position;
        BoxCollider col = teams[teamIndex].spawn.GetComponent<BoxCollider>();
        if (col != null) ;
        {
            pos.y = col.transform.position.y;
            int counter = 10;
            do
            {
                pos.x = Random.Range(col.bounds.min.x, col.bounds.max.x);
                pos.z = Random.Range(col.bounds.min.z, col.bounds.max.z);
                counter--;
            }
            while (!col.bounds.Contains(pos) || counter > 0);
        }

        return teams[teamIndex].spawn.position;
    }

}

[System.Serializable]
public class Team
{
    public string playname;                                              //playername
    //public Material material;
    public Transform spawn;                                           //キャラクタ生まれポジション
    public Color color;                                                      //キャラクタ色
}