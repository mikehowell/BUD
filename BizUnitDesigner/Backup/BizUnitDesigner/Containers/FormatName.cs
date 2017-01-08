using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BizUnitDesigner
{
  class FormatName
  {
    private string _Name;

    public string Name
    {
      get { return _Name; }
      set { _Name = value; }
    }

    public FormatName(string name)
    {
      Name = name;
    }

    public override string ToString()
    {
        return Name.Replace("Microsoft.Services.BizTalkApplicationFramework.BizUnit.", "").Replace("T:", "");
    }

    public override bool Equals(object obj)
    {
      return obj.ToString()==this.ToString();
    }

    public static string GetListBoxItem(object lstBox)
    {
      return ((FormatName)(((ListBox)lstBox).SelectedItem)).Name;
    }
  }
}
