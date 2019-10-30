/*
 *
 *  Title:  "B-POP"
 *  
 *          核心層 :　核心層のパラメータ
 *                  
 *          Date:  2019
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KernalParameter
{
    /// <summary>
    /// systemconfig information_ログパス取得
    /// </summary>
    /// <returns></returns>
    public static string GetLogPath()
    {
        string logPath = null;

        //Android or Iphone
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            logPath = Application.streamingAssetsPath + "/SystemConfigInfo.xml";
        }
        //Windows
        else
        {
            logPath = "file://" + Application.streamingAssetsPath + "/SystemConfigInfo.xml";
        }

        return logPath;
    }

    /// <summary>
    /// systemconfig information_ログ根節点名前取得
    /// </summary>
    /// <returns></returns>
    public static string GetLogRootNodeName()
    {
        string strReturnXMLRootNodeName = null;

        strReturnXMLRootNodeName = "SystemConfigInfo";

        return strReturnXMLRootNodeName;

    }
}
