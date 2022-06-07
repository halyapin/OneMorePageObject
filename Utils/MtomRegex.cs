using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace OneMorePageObject
{
    public class MtomRegex
    {
        /* Использование
         * var messageId = MtomRegex.GetValueFromTag("MessageID", responseFromGate);
         * var requestType = MtomRegex.GetValueFromTag("MessageType", responseFromGate);
         */

        public static string GetValueFromTag(string tagName, string text)
        {
            
            //Регулярное выражение - получение значение из двойного тега
            Regex regex = new Regex($@"<ns2:{tagName}>\s*(.+?)\s*</ns2:{tagName}>");
            Match match = regex.Match(text);
            return match.Groups[1].Value;
        }
       
    }
}
