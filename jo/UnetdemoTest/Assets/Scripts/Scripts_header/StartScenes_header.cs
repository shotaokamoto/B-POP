﻿/***
*
*       Title :"B-POP"
*
*        ビュー：開始スクリーン
*
*       Description:
*               機能：①開始ボタン
*                           ②閉めるボタン
*                     
*       Data : 2019.10.09
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScenes_header : MonoBehaviour
{

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void ClikNewGame()
    {
        StartScenes_cpp.Instance.ClickNewGame();
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void ClikGameQuit()
    {
        Application.Quit();
    }





}
