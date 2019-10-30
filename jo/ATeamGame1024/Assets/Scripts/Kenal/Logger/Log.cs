/***
*
*       Title :"B-POP"
*
*        核心層：デバックログシステム 
*
*　　　　機能：ゲームをデバックしやすくなる
*                システム基本流れ：
*                ①プログラマがデバッグしたい内容をログの”キャッシュ”に書き込み
*                ②キャッシュの数＞最大書き込み数の場合、キャッシュの内容一括にテキストに書き込み
* 
*       Data : 2019
*
*/
using System.Collections;
using System.Collections.Generic;

using System;                                      //C#核心
using System.IO;                                 //ファイルの読み取り
using System.Threading;                   //スレッド

namespace kernal
{
    public static class Log
    {
        /*核心定義*/
        private static List<string> _LiLogArray;                                          //Log キャッシュデータ
        private static string _LogPath;                                                         //Log ファイルパス
        private static State _LogState;                                                        //Log　状態（配置モード）
        private static int _LogMaxCapacity;                                                //Log 最大容量
        private static int _LogBufferMaxNumer;                                          //Log バッファの最大容量
        /*ログファイルパラメータ定義*/
        //XML　ファイル　”TAG”　パラメータ定義
        private const string XML_CONFIG_LOG_PATH = "LogPath";
        private const string XML_CONFIG_LOG_STATE = "LogState";
        private const string XML_CONFIG_LOG_MAXCAPACITY = "LogMaxCapacity";
        private const string XML_CONFIG_LOG_BUFFERNUMBER = "LogBufferNumber";

        //ログ状態配置モードのパラメータ定義
        private const string XML_CONFIG_LOG_STATE_DEVELOP = "Develop";
        private const string XML_CONFIG_LOG_STATE_SPEACIAL = "Speacial";
        private const string XML_CONFIG_LOG_STATE_DEPLOY = "Deploy";
        private const string XML_CONFIG_LOG_STATE_STOP = "Stop";

        //ログパス
        private const string XML_CONFIG_LOG_DEFAULT_PATH = "B-POPLog.txt";
        //ログの最大容量
        private const int LOG_DEFAULT_MAX_CACITY_NUMBER = 2000;
        //ログバッファの最大容量
        private const int LOG_DEFAULT_MAX_BUFFER_NUMBER = 1;

        /// <summary>
        /// 静的構造関数
        /// </summary>
        static Log()
        {
            //Logデータ
            _LiLogArray = new List<string>();

#if UNITY_STANDALONE_WIN
            //Log ファイルパス
            MyConfigManger configMgr = new ConfigManger(KernalParameter.GetLogPath(), KernalParameter.GetLogRootNodeName());
            _LogPath = configMgr.AppSetting[XML_CONFIG_LOG_PATH];
            //Log　状態（配置モード）
            string strLogState = configMgr.AppSetting[XML_CONFIG_LOG_STATE];
            //Log   最大容量
            string strLogMaxCapacity = configMgr.AppSetting[XML_CONFIG_LOG_MAXCAPACITY];
            //Log  バッファの最大容量
            string strLogBufferNumber = configMgr.AppSetting[XML_CONFIG_LOG_BUFFERNUMBER];
#endif

            //Log ファイルパス
            if (string.IsNullOrEmpty(_LogPath))
            {
                _LogPath = UnityEngine.Application.persistentDataPath + "\\" + XML_CONFIG_LOG_DEFAULT_PATH;
            }

            //Log　状態（配置モード）

            if (!string.IsNullOrEmpty(strLogState))
            {
                switch (strLogState)
                {
                    case XML_CONFIG_LOG_STATE_DEVELOP:
                        _LogState = State.Develop;
                        break;
                    case XML_CONFIG_LOG_STATE_SPEACIAL:
                        _LogState = State.Speacial;
                        break;
                    case XML_CONFIG_LOG_STATE_DEPLOY:
                        _LogState = State.Deploy;
                        break;
                    case XML_CONFIG_LOG_STATE_STOP:
                        _LogState = State.Stop;
                        break;
                    default:
                        _LogState = State.Stop;
                        break;
                }
            }
            else
            {
                _LogState = State.Stop;
            }

            //Log   最大容量

            if (!string.IsNullOrEmpty(strLogMaxCapacity))
            {
                _LogMaxCapacity = Convert.ToInt32(strLogMaxCapacity);
            }
            else
            {
                _LogMaxCapacity = LOG_DEFAULT_MAX_CACITY_NUMBER;
            }

            //Log  バッファの最大容量

            if (!string.IsNullOrEmpty(strLogBufferNumber))
            {
                _LogBufferMaxNumer = Convert.ToInt32(strLogBufferNumber);
            }
            else
            {
                _LogBufferMaxNumer = LOG_DEFAULT_MAX_BUFFER_NUMBER;
            }

            //ファイルクリエイト
            if (!File.Exists(_LogPath))
            {
                File.Create(_LogPath);
                //スレッド閉める
                Thread.CurrentThread.Abort();
            }

            //LogのデータをLog”キャッシュ”の中に書き込み
            SyncFileDataToLogArray();

        }//Log_end(構造関数)

        //LogのデータをLog”キャッシュ”の中に書き込み
        private static void SyncFileDataToLogArray()
        {
            if (!string.IsNullOrEmpty(_LogPath))
            {
                StreamReader sr = new StreamReader(_LogPath);
                while (sr.Peek() >= 0)
                {
                    _LiLogArray.Add(sr.ReadLine());
                }
                sr.Close();
            }
        }

        /// <summary>
        /// データをファイルに輸入
        /// </summary>
        /// <param name="writeFileDate">入力の調整情報</param>
        /// <param name="level">情報の大切さ</param>
        public static void Write(string writeFileDate, Level level)
        {
            //引数チェック
            if (_LogState == State.Stop)
            {
                return;
            }

            //もしログの”キャッシュ”の数＞指定の容量、クリア
            if (_LiLogArray.Count >= _LogMaxCapacity)
            {
                _LiLogArray.Clear();                                   //データをクリア
            }

            if (!string.IsNullOrEmpty(writeFileDate))
            {
                //日と時間を増加
                writeFileDate = "Log  State: " + _LogState.ToString() + "/" + DateTime.Now.ToString() + "/" + writeFileDate;

                //"ログ状態"によって、特定状況でファイルに入れる
                if (level == Level.High)
                {
                    writeFileDate = "||||||  Error or Warning or Imporntent !!!  ||||||" + writeFileDate;
                }

                switch (_LogState)
                {
                    case State.Develop:                                                   //開発モード
                        //追加データ
                        AppendDateToFile(writeFileDate);
                        break;
                    case State.Speacial:                                                  //”指定”モード
                        if (level == Level.High || level == Level.Special)
                        {
                            AppendDateToFile(writeFileDate);
                        }
                        break;
                    case State.Deploy:                                                     //配置モード
                        if (level == Level.High)
                        {
                            AppendDateToFile(writeFileDate);
                        }
                        break;
                    case State.Stop:                                                         //停止モード
                        break;
                    default:
                        break;
                }
            }
        }//Write_end

        //↑の関数オーバーロード
        public static void Write(string writeFileDate)
        {
            Write(writeFileDate, Level.Low);
        }

        /// <summary>
        /// データ追加 in ファイル
        /// </summary>
        /// <param name="writeFileDate"></param>
        private static void AppendDateToFile(string writeFileDate)
        {
            if (!string.IsNullOrEmpty(writeFileDate))
            {
                //データ追加 in バッファ集合
                _LiLogArray.Add(writeFileDate);
            }

            //バッファ集合の数＞指定の数（最大容量）、ファイルの中に同期うつす
            if (_LiLogArray.Count % _LogBufferMaxNumer == 0)
            {
                //同期バッファのデータ to ファイル
                SyncLogArrayToFile();
            }
        }

        #region ログマネージャ

        /// <summary>
        /// ログバッファの全てのデータ調べる
        /// </summary>
        /// <returns></returns>
        public static List<string> QueryAllDateFromLogBuffer()
        {
            if (_LiLogArray != null)
            {
                return _LiLogArray;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// ファイルとファイル中のデータをクリア
        /// </summary>
        public static void ClearLogFileAndBufferAllDate()
        {
            if (_LiLogArray != null)
            {
                //データクリア
                _LiLogArray.Clear();
                //同期バッファのデータ to ファイル
                SyncLogArrayToFile();
            }
        }

        /// <summary>
        /// 同期バッファのデータ to ファイル
        /// </summary>
        public static void SyncLogArrayToFile()
        {
            if (!string.IsNullOrEmpty(_LogPath))
            {
                StreamWriter sw = new StreamWriter(_LogPath);
                foreach (string item in _LiLogArray)
                {
                    sw.WriteLine(item);
                }
                sw.Close();
            }
        }
        #endregion

        #region 今クラスのenum
        /// <summary>
        /// log状態（配置モード） 
        /// </summary>
        public enum State
        {
            Develop,         //開発モード（何も出る）
            Speacial,        //指定outputモード（指定の内容出る）
            Deploy,           //配置モード（核心データだけ出る）
            Stop                //output停止モード(何も出ない)
        };

        /// <summary>
        /// 情報の重要性
        /// </summary>
        public enum Level
        {
            High,
            Special,
            Low
        }
        #endregion
    }
}
