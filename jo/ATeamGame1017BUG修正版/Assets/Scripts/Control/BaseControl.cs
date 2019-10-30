/***
*
*       Title :"ATeam α版"
*
*        コントローラー：親クラスのコントローラー層
*
*       Description:
*                   機能：
*                   　コントローラー層の共有の部分をこのクラスで継承
*
*       Data : 2019..
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Ctrl
{
    public class BaseControl : MonoBehaviour
    {
        /// <summary>
        /// goto next scene
        /// </summary>
        /// <param name="scenesEnumName">Scenes of enum name</param>
        protected void EnterNextScenes(ScenesEnum scenesEnumName)
        {
            //go next scene
            GlobalParaManger.NextScenesName = scenesEnumName;                             //Loadingscenesに入る
            Application.LoadLevel(ConvertEnumToStr.GetInstance().GetStrByEnumScenes(ScenesEnum.GameScenes));
        }
    }
}

