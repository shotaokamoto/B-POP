/***
*
*       Title :"B-POP"
*
*        コントローラー：開始スクリーン
*
*       Description:
*
*
*       Data : 2019.10.9
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;


public class StartScenes_cpp : BaseControl
{
    public static StartScenes_cpp Instance;                             //クラスの実体

    private void Awake()
    {
        Instance = this;
    }

    internal void ClickNewGame()
    {
        NetworkMangerCustom.NetworkGame();

        //次のスクリーンに入る
        StartCoroutine("EnterNextScenes");
    }

    /// <summary>
    /// 次のシーンに入る
    /// </summary>
    /// <returns></returns>
    IEnumerator EnterNextScenes()
    {
        //スクリーンフェードアウト
        //FadeInAndOut.Instance.SetScenesToBlack();
        yield return new WaitForSeconds(1.5F);
        //次のシーンに入る
        base.EnterNextScenes(Global.ScenesEnum.GameScenes);
    }




}
