/***
*
*       Title :"B-POP"
*
*        ビュー：scenes Loading 
*
*       Description:
*
*
*       Data : 2019..
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;

public class View_LoadingScenes : MonoBehaviour
{
    public Slider SliLoadingProgress;                                         //進捗チェックコンポーネント
    private float _FloProgressNumber;                                        //進捗値
    private AsyncOperation _Asyoper;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1F);

        //ゲームのシーンに入る
        //GlobalParaMgr.NextScenesName = ScenesEnum.GameScenes;
        NetworkMangerCustom.NetworkGame();
        StartCoroutine("LoadingScenesProgress");
    }

    /// <summary>
    /// 非同期処理 
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingScenesProgress()
    {
        _Asyoper = Application.LoadLevelAsync(ConvertEnumToStr.GetInstance().GetStrByEnumScenes(GlobalParaMgr.NextScenesName));
        _FloProgressNumber = _Asyoper.progress;
        yield return _Asyoper;
    }

    /// <summary>
    /// 進捗バー表す
    /// </summary>
    void Update()
    {
        if (_FloProgressNumber <= 1)
        {
            _FloProgressNumber += 0.01F;
        }
        SliLoadingProgress.value = _FloProgressNumber;
    }
}
