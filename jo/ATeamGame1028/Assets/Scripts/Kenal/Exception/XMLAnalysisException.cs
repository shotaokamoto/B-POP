/*
 *
 *  Title:  "B-POP"
 *  
 *          核心層 :　自定義ミスティック（XML解析ミスティック） 
 *          
 *         機能：XML解析エラーを追従する。（もし今エラー出たら、XMLの定義格式が間違う）
 *         
 *         Date:  2019
 */
using System.Collections;
using System.Collections.Generic;
using System;

namespace kernal
{
    public class XMLAnalysisException : Exception
    {
        public XMLAnalysisException() : base() { }

        public XMLAnalysisException(string exceptionMessage) : base(exceptionMessage) { }
    }
}
