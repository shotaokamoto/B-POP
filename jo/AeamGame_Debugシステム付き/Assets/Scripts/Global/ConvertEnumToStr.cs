/***
*
*       Title :"B-POP"
*
*        グローバル層：enum to string
*
*       Description:
*            機能：
*               Instance 応用
*        
*       Data : 2019.
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public class ConvertEnumToStr
    {
        private static ConvertEnumToStr _Instance;

        //eunm型のシーンのコレクション
        private Dictionary<ScenesEnum, string> _DicScenesEnumLib;

        /// <summary>
        /// シーンの構造関数
        /// </summary>
        private ConvertEnumToStr()
        {
            _DicScenesEnumLib = new Dictionary<ScenesEnum, string>();
            _DicScenesEnumLib.Add(ScenesEnum.StartScenes, "StartScenes");
            _DicScenesEnumLib.Add(ScenesEnum.LoadingScenes, "LoadingScenes");
            _DicScenesEnumLib.Add(ScenesEnum.LogonScenes, "LogonScenes");
            _DicScenesEnumLib.Add(ScenesEnum.GameScenes, "FieldDataTest");
        }


        /// <summary>
        /// このクラスの実態を取得
        /// </summary>
        public static ConvertEnumToStr GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ConvertEnumToStr();
            }
            return _Instance;
        }

        /// <summary>
        /// string型のシーンの名前を取得
        /// </summary>
        /// <param name="scenesEnum">enumスクリーン名前</param>
        /// <returns></returns>
        public string GetStrByEnumScenes(ScenesEnum scenesEnum)
        {
            if (_DicScenesEnumLib != null && _DicScenesEnumLib.Count >= 1)
            {
                return _DicScenesEnumLib[scenesEnum];
            }
            else
            {
                return null;
            }
        }
    }

}
