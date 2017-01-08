using System;
using System.Collections.Generic;
using System.Text;

namespace BizUnitDesigner
{
  static class XmlFormatter
  {
    static int indentLevel;
    public static string MakeReadable(string xmlDoc)
    {
      xmlDoc = xmlDoc.Replace("><", ">\r\n<");
      xmlDoc = xmlDoc.Replace("\r\n\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Trim();
      string[] strLines = xmlDoc.Split(new string[] { "\r\n" }, StringSplitOptions.None);
      xmlDoc = "";
      indentLevel=0;
      foreach (string str in strLines)
      {
        if (str.StartsWith("</") && indentLevel > 3)
          indentLevel = indentLevel - 4;
        xmlDoc = xmlDoc + new string(' ', indentLevel) + str + "\r\n";
        if (!str.Contains("</") && !str.Contains("<!--") && !str.Contains("/>") && str.StartsWith("<"))
            indentLevel = indentLevel + 4;
      }
      return xmlDoc.Trim();
    }
  }
}
