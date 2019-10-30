/*
 * 
 *                Unetでネットワーク通信方法のcoding 
 * 　　　　　　例）
 * 　　　　　　　　カード出すの例一つ出し
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkCoding : NetworkBehaviour
{
    public Vector3Int CardPosition;                                               //カードの位置
    public Transform CardRotion;                                                  //方向（姿勢）
    public GameObject CardPrefab;                                             //カード

   

    
    private void FixedUpdate()
    {

        
        if (!isLocalPlayer) return;                                                 //自分クライアントのオブジェクトだけを操作する

        //Input関す　todo...例)
        if(Input.GetButton("W"))
        {
           
            //todo
        }
        //OnClieck関数もできる
        
    }

    [Command]
    public void CmdPush()
    {
        //カード操作todo.....例）ポジション、正反面、など...
        CardPosition = new Vector3Int(1, 1, 1);


        //カードをクリエイト、クライアントを同期処理
        GameObject Card = Instantiate(CardPrefab, CardPosition, CardRotion.rotation);
        NetworkServer.Spawn(CardPos, CardPos.GetComponent<NetworkIdentity>().assetId);

        //クライアントのエフェクト表す
        CardEffect();

    }

    [ClientRpc]
    protected void CardEffect()
    {
        //todo...

        UnityHelp.GetInstance.OnA();

    }



}


