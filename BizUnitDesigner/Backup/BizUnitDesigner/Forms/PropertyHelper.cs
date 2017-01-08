using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BizUnitDesigner
{
  public partial class PropertyHelper : Form
  {
    Designer c = null;
    public PropertyHelper(Designer c, bool url)
    {
      InitializeComponent();
      this.c = c;
      textBox1.Text = c.strDetails;
      if (url)
      {
        textBox1.Multiline = false;
        textBox1.AcceptsReturn = false;
        textBox1.KeyDown+=new KeyEventHandler(textBox1_KeyDown);
        textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        textBox1.AutoCompleteSource = AutoCompleteSource.AllSystemSources;

        this.Height = textBox1.Height + 40;
        this.Width = textBox1.Width + 10;
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      }
    }

    private void PropertyHelper_FormClosing(object sender, FormClosingEventArgs e)
    {
      c.strDetails = textBox1.Text;
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
        this.Close();
    }
  }
}