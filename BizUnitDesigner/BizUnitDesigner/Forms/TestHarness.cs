using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BizUnitDesigner
{
    public partial class TestHarness : Form
    {
        System.IO.StringWriter sw = null;
        bool complete = true;

        public TestHarness()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";            
            if (txtTC.Text == "")
            {
                MessageBox.Show("Please select the test cases", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              try
              {
                new BizUnit.BizUnit(txtTC.Text);
                complete = false;
                timer1.Enabled = !complete;
                backgroundWorker1.RunWorkerAsync();
                btnRun.Enabled = false;
              }
              catch (Exception exp)
              {
                MessageBox.Show(exp.Message, "Test harness error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              }
            }
        }

      private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
      {
          sw = new System.IO.StringWriter();
          Console.SetOut(sw);

          string[] strFiles = txtTC.Text.Split(',');
          foreach (string str in strFiles)
          {
            BizUnit.BizUnit bizunit = new BizUnit.BizUnit(str);
            try
            {
              Console.WriteLine("|||||||||||||||||||||||||||||||| Executing test case: " + str + " ||||||||||||||||||||||||||||||||");
              bizunit.RunTest();
              Console.WriteLine("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||\r\n\r\n");
            }
            catch (Exception exp)
            {
              MessageBox.Show(exp.Message, "Test case failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
      }

        private void timer1_Tick(object sender, EventArgs e)
        {
          timer1.Enabled = !complete;
            try
            {
                txtResult.Text = sw.ToString();
                txtResult.SelectionStart = txtResult.Text.Length - 1;
                txtResult.ScrollToCaret();
            }
            catch (Exception)
            { }
        }

      private void btnFile_Click(object sender, EventArgs e)
      {
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
          txtTC.Text = String.Join(",", openFileDialog1.FileNames);
      }

      private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        complete = true;
        btnRun.Enabled = true;
      }
    }
}