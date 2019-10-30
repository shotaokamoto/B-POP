/***
*
*       Title :"B-POP"
*
*        コントローラー：全てのCPPスクリプト親クラス
*
*       Description:
*                   機能：
*                   　CPPの共有の部分をこのクラスで継承
*
*       Data : 2019.10.09
*
*/
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class BaseControl : MonoBehaviour
{
    /// <summary>
    /// goto next scene
    /// </summary>
    /// <param name="scenesEnumName">Scenes of enum name</param>
    protected void EnterNextScenes(ScenesEnum scenesEnumName)
    {
        //go next scene
        //GlobalParaMgr.NextScenesName = scenesEnumName;    　　　　　　　　　　　　　　　　　　　　　　　　 //Loadingscenesに入る
        //Application.LoadLevel(ConvertEnumToStr.GetInstance().GetStrByEnumScenes(ScenesEnum.GameScenes));
    }
}
