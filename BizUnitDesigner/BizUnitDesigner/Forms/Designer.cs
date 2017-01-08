using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BizUnitDesigner
{
  public partial class Designer : Form
  {
      bool isDirty = false;
    public TestDetails td = new TestDetails();
    public TestCase tc;
    public string strDetails = null;

    public Designer()
    {
      InitializeComponent();
    }

    private void LoadListBoxes()
    {
      Dictionary<string, Step>.Enumerator e = td.steps.GetEnumerator();
      while (e.MoveNext())
      {
        if (e.Current.Value.Cat == StepCategory.TestStep)
          lstTest.Items.Add(new FormatName(e.Current.Key));
        if (e.Current.Value.Cat == StepCategory.ValidationStep)
          lstValid.Items.Add(new FormatName(e.Current.Key));
        if (e.Current.Value.Cat == StepCategory.ContextLoader)
          lstContext.Items.Add(new FormatName(e.Current.Key));
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      td.Load();
      LoadListBoxes();
    }

    private void lstTestSteps_Click(object sender, EventArgs e)
    {
      if (((ListBox)sender).SelectedItem != null)
      {
        LblDesc.Text = td.steps["" + FormatName.GetListBoxItem(sender)].Desc;
        txtSample.Text = XmlFormatter.MakeReadable(td.steps["" + FormatName.GetListBoxItem(sender)].Sample);
        toolTip1.SetToolTip((Control)sender, LblDesc.Text);
        toolTip1.SetToolTip(LblDesc, LblDesc.Text);
      }
    }

    private void lstTestCase_DragDrop(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Copy;
      string strTemp = new Random().NextDouble().ToString();
      string strData = e.Data.GetData(e.Data.GetFormats()[0]).ToString();
      Step s = td.steps["" + strData];
      if (s.Cat != StepCategory.TestStep)
        strData = "> " + strData;
      ((ListBox)sender).Items.Add(new FormatName(strData + "\t\t\t\t\t\t\t\t" + strTemp));
      tc.testSteps.Add("" + strData + "\t\t\t\t\t\t\t\t" + strTemp, Step.Copy(s));
      isDirty = true;
    }

    private void lstTestSteps_MouseDown(object sender, MouseEventArgs e)
    {
      DoDragDrop(FormatName.GetListBoxItem(sender), DragDropEffects.Copy);
      lstTestSteps_Click(sender, null);
    }

    private void lstTestCase_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Copy;
    }

    private void lstTestCase_Click(object sender, EventArgs e)
    {
      stepAttributeBindingSource.CurrentItemChanged-= new EventHandler(stepAttributeBindingSource_CurrentChanged);
      stepAttributeBindingSource.Clear();
      if (((ListBox)sender).SelectedItem != null)
      {
        Dictionary<string, StepAttribute>.Enumerator enumerator = tc.testSteps[FormatName.GetListBoxItem(sender)].StepAttributes.GetEnumerator();
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.Key == "ValidationStep" || enumerator.Current.Key == "ContextLoaderStep")
            continue;
          stepAttributeBindingSource.Add(enumerator.Current.Value);
        }
        LblDesc.Text = tc.testSteps["" + FormatName.GetListBoxItem(sender)].Desc;
        txtSample.Text = XmlFormatter.MakeReadable(tc.testSteps["" + FormatName.GetListBoxItem(sender)].Sample);
        toolTip1.SetToolTip((Control)sender, LblDesc.Text);
        toolTip1.SetToolTip(LblDesc, LblDesc.Text);
      }
      stepAttributeBindingSource.CurrentItemChanged += new EventHandler(stepAttributeBindingSource_CurrentChanged);
    }

    private void dgv_Click(object sender, EventArgs e)
    {
      if (dgvAttributes.SelectedCells.Count > 0)
      {
        LblDesc.Text = dgvAttributes.SelectedCells[0].OwningRow.Cells["Desc"].Value.ToString();
        toolTip1.SetToolTip(dgvAttributes, LblDesc.Text);
      }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = !ResetTestCase();
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!ResetTestCase())
        return;
      grpTestCase.Enabled = true;
      tc = new TestCase(this);
    }

    private bool ResetTestCase()
    {
      if (isDirty && grpTestCase.Enabled && MessageBox.Show("BizUnit Designer has a test case loaded in memory. Any unsaved changes will be discarded. Do you wish to continue?", "Reminder", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
        return false;
      lstSetup.Items.Clear();
      lstExecute.Items.Clear();
      lstCleanup.Items.Clear();
      stepAttributeBindingSource.Clear();
      grpTestCase.Enabled = false;
      tc = null;
      isDirty = false;
      this.Text = "BizUnit Designer 2.0";
      return true;
    }

    private void sampleComboBox1_Click(object sender, EventArgs e)
    {
      if (!ResetTestCase())
        return;
      grpTestCase.Enabled = true;
      tc = new TestCase(this);
      tc.xmlFile = Application.StartupPath + "\\templates\\" + sampleComboBox1.SelectedItem + ".xml";
      tc.Load();
      tc.xmlFile = "";
      this.Text = "BizUnit Designer 2.0 - " + tc.xmlFile;
      fileToolStripMenuItem.DropDown.Close();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!ResetTestCase())
        return;
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        grpTestCase.Enabled = true;
        tc = new TestCase(this);
        tc.xmlFile = openFileDialog1.FileName;
        this.Text = "BizUnit Designer 2.0 - " + tc.xmlFile;
        tc.Load();
      }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (tc == null)
      {
        MessageBox.Show("No test case loaded","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        return;
      }
      if (tc.xmlFile == "")
        saveAsToolStripMenuItem_Click(sender, e);
      else
        tc.Save();
      isDirty = false;
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (tc == null)
      {
        MessageBox.Show("No test case loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      if(saveFileDialog1.ShowDialog()== DialogResult.OK)
      {
        tc.xmlFile = saveFileDialog1.FileName;
        this.Text = "BizUnit Designer 2.0 - " + tc.xmlFile;
        tc.Save();
        isDirty = false;
      }
    }

    private void removeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (((ListBox)ctxStrip.SourceControl).SelectedItem != null)
      {
        tc.testSteps.Remove("" + FormatName.GetListBoxItem(ctxStrip.SourceControl));
        ((ListBox)ctxStrip.SourceControl).Items.Remove(((ListBox)ctxStrip.SourceControl).SelectedItem);
        isDirty = true;
      }
    }

    private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FormatName strItemName= (FormatName)((ListBox)ctxStrip.SourceControl).SelectedItem;
      if (strItemName!= null)
      {
        int index = ((ListBox)ctxStrip.SourceControl).Items.IndexOf(strItemName);
        if (index < ((ListBox)ctxStrip.SourceControl).Items.Count - 1)
        {
          ((ListBox)ctxStrip.SourceControl).Items.Remove(strItemName);
          ((ListBox)ctxStrip.SourceControl).Items.Insert(index + 1, strItemName);
          ((ListBox)ctxStrip.SourceControl).SelectedItem = strItemName;
          isDirty = true;
        }
      }
    }

    private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FormatName strItemName = (FormatName)((ListBox)ctxStrip.SourceControl).SelectedItem;
      if (strItemName != null)
      {
        int index = ((ListBox)ctxStrip.SourceControl).Items.IndexOf(strItemName);
        if (index > 0)
        {
          ((ListBox)ctxStrip.SourceControl).Items.Remove(strItemName);
          ((ListBox)ctxStrip.SourceControl).Items.Insert(index - 1, strItemName);
          ((ListBox)ctxStrip.SourceControl).SelectedItem = strItemName;
          isDirty = true;
        }
      }
    }

    private void btnFile_Click(object sender, EventArgs e)
    {
      if (openFileDialog2.ShowDialog() == DialogResult.OK)
        dgvAttributes.SelectedCells[0].OwningRow.Cells[2].Value = openFileDialog2.FileName;
    }

    private void btnFolder_Click(object sender, EventArgs e)
    {
      if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
        dgvAttributes.SelectedCells[0].OwningRow.Cells[2].Value = folderBrowserDialog1.SelectedPath;
    }

    private void dgvAttributes_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex == 4)
        ctxDgvStrip.Show(MousePosition.X, MousePosition.Y);
    }

    private void btnGeneric_Click(object sender, EventArgs e)
    {
      strDetails = ""+dgvAttributes.SelectedCells[0].OwningRow.Cells[2].Value;
      new PropertyHelper(this,false).ShowDialog();
      dgvAttributes.SelectedCells[0].OwningRow.Cells[2].Value=strDetails;
    }

    private void genericUrlToolStripMenuItem_Click(object sender, EventArgs e)
    {
      strDetails = "" + dgvAttributes.SelectedCells[0].OwningRow.Cells[2].Value;
      new PropertyHelper(this,true).ShowDialog();
      dgvAttributes.SelectedCells[0].OwningRow.Cells[2].Value = strDetails;
    }

      private void stepAttributeBindingSource_CurrentChanged(object sender, EventArgs e)
      {
          isDirty = true;
      }

    private void closeStripMenuItem1_Click(object sender, EventArgs e)
    {
      ResetTestCase();
    }

    private void menuStrip1_Click(object sender, EventArgs e)
    {
      try
      {
        dgvAttributes.CurrentCell = dgvAttributes.Rows[0].Cells[0];
      }
      catch (Exception)
      { }
    }

    private void newFromSampleToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
      string[] files=System.IO.Directory.GetFiles(Application.StartupPath+"\\Templates");
      sampleComboBox1.Items.Clear();
      foreach (string strSamplePath in files)
        sampleComboBox1.Items.Add(strSamplePath.Replace(Application.StartupPath + "\\Templates\\", "").Replace(".xml",""));
    }

      private void runTestCaseToolStripMenuItem_Click(object sender, EventArgs e)
      {
        if(!isDirty || MessageBox.Show("The currently open test case has not been saved. Do wish to run the test harness with the data from this unsaved test case","BizUnit Designer",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
          new TestHarness().ShowDialog();
      }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Copyright(c) 2008 Praneeth Reddy\npraneeth03@msn.com\nhttp://www.codeplex.com/bud\n\nBizUnit is Copyright (c) 2004-2007, Kevin B. Smith", "BizUnit Designer 2.0", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void readmeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\readme.pdf");
        }
        catch (Exception exp)
        {
            MessageBox.Show(exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
  }
}