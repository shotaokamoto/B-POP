/*
 *
 *  Title:  "B-POP"
 *  
 *          核心層 :　設置マネージャ 
 *           
 *         機能：LogSystem （XML）読み込む
 *         
 *  　   Date:  2019
 *
 */
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;  //XDocument のnamespace
using System.IO;

namespace kernal
{
    public class ConfigManger : MyConfigManger
    {
        private static Dictionary<string, string> _AppSetting;                           //アプリ設置集合

        /// <summary>
        /// 配置マネージャ構造関数
        /// </summary>
        /// <param name="logPath">ログのパス</param>
        /// <param name="xmlRootName">XMLの根節点の名前</param>
        public ConfigManger(string logPath, string xmlRootName)
        {
            _AppSetting = new Dictionary<string, string>();
            //XMLを初期化と解析 TOAppSetting集合に
            InitAndAnalysisXML(logPath, xmlRootName);
        }

        /// <summary>
        /// XMLを初期化と解析　TO　AppSetting集合に
        /// </summary>
        /// <param name="logPath">ログのパス</param>
        /// <param name="xmlRootName">XMLの根節点の名前</param>
        private void InitAndAnalysisXML(string logPath, string xmlRootName)
        {
            //引数チェック
            if (string.IsNullOrEmpty(logPath) || string.IsNullOrEmpty(xmlRootName))
            {
                return;
            }
            XDocument xmlDoc;                                                                                                             //XML配置ファイル
            XmlReader xmlReader;                                                                                                        //XMLReader機

            try
            {
                xmlDoc = XDocument.Load(logPath);                                                                                //LogSystemのパス
                xmlReader = XmlReader.Create(new StringReader(xmlDoc.ToString()));                     //XMLReaderを作る
            }
            catch
            {
                throw new XMLAnalysisException(GetType() + "/InitAndAnalysisXML()XML Analysis Exception!!! check again please");
            }

            //無限ループでXMLを解析
            while (xmlReader.Read())
            {
                //XMLReaderが指定した根節点から読み取り
                if (xmlReader.IsStartElement() && xmlReader.LocalName == xmlRootName)
                {
                    using (XmlReader xmlReaderItem = xmlReader.ReadSubtree())
                    {
                        while (xmlReaderItem.Read())
                        {
                            //もし”節点元素”の場合、
                            if (xmlReaderItem.NodeType == XmlNodeType.Element)
                            {
                                //節点元素
                                string strNode = xmlReaderItem.Name;
                                //当行のXMLの内容を読み取り
                                xmlReaderItem.Read();
                                //もし節点の内容
                                if (xmlReaderItem.NodeType == XmlNodeType.Text)
                                {
                                    _AppSetting[strNode] = xmlReaderItem.Value;
                                }
                            }
                        }
                    }//using_end
                }
            }
        }//InitAndAnalysisXML_end

        /// <summary>
        /// 属性：アプリ設置
        /// </summary>
        public Dictionary<string, string> AppSetting
        {
            get { return _AppSetting; }
        }

        /// <summary>
        /// AppSetting の最大数を取得
        /// </summary>
        public int GetAppSettingMaxNumber()
        {
            if (_AppSetting != null && _AppSetting.Count >= 1)
            {
                return _AppSetting.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}
