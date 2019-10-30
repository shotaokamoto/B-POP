/***
*
*       Title :"B-POP"
*
*        グローバル層：グローバルパラメータマネージャ
*
*       Description:
*               全スクリーン値転送(名前、ID等)
*               
*        
*       Data : 2019.
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public class GloabalParaMgr
    {
        //next scene's name
        public static ScenesEnum NextScenesName = ScenesEnum.LoadingScenes;
        //player's name
        public static string PlayerName = " ";
    }
}

