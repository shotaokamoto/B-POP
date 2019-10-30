/*
 *
 *  Title:  "Hal大阪のファイター"
 *  
 *          Interface : 配置マネージャ
 *          
 *         機能：LogSystem （XML）読み込む
 *         
 *  Date:  2019
 *
 */
using System.Collections;
using System.Collections.Generic;

namespace kernal
{
    public interface MyConfigManger
    {
        /// <summary>
        /// 属性：アプリ設置
        /// </summary>
        Dictionary<string, string> AppSetting { get; }

        /// <summary>
        /// AppSetting の最大数を取得
        /// </summary>
        int GetAppSettingMaxNumber();
    }
}

