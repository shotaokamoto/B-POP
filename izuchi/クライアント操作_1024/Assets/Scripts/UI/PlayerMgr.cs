using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : MonoBehaviour
{
    public static PlayerMgr instance;
    public List<Vector2> CheckUsePosList = new List<Vector2>();    // 使用されている座標リスト

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

   // 使用されている座標をリストに追加
   public void CheckUsePos()
    {
        // 64マスをチェック
        for(int i = -4; i < 4; i++)  // Y
        {
            for(int j = -4; j < 4; j++)  // X
            {
                // そのマスが使われているか
                // サーバからフィールド(全体)データを受け取る
                //if(Model.RecvCliantDataProxy._Instance.AllFeildData[0][0]._PossessionPlayer != -1)
                {
                    Vector2 vUsePos; // 使用されている座標
                    vUsePos.x = j;
                    vUsePos.y = i;
                    CheckUsePosList.Add(vUsePos);   // リストに追加
                    //Debug.Log(CheckUsePosList);
                }
            }
        }
    }


}
