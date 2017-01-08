namespace BizUnitDesigner
{
    partial class TestHarness
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          this.components = new System.ComponentModel.Container();
          this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
          this.txtResult = new System.Windows.Forms.TextBox();
          this.groupBox1 = new System.Windows.Forms.GroupBox();
          this.label1 = new System.Windows.Forms.Label();
          this.btnFile = new System.Windows.Forms.Button();
          this.btnRun = new System.Windows.Forms.Button();
          this.label2 = new System.Windows.Forms.Label();
          this.txtTC = new System.Windows.Forms.TextBox();
          this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
          this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
          this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
          this.timer1 = new System.Windows.Forms.Timer(this.components);
          this.tableLayoutPanel1.SuspendLayout();
          this.groupBox1.SuspendLayout();
          this.SuspendLayout();
          // 
          // tableLayoutPanel1
          // 
          this.tableLayoutPanel1.ColumnCount = 1;
          this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
          this.tableLayoutPanel1.Controls.Add(this.txtResult, 0, 1);
          this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
          this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
          this.tableLayoutPanel1.Name = "tableLayoutPanel1";
          this.tableLayoutPanel1.RowCount = 2;
          this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
          this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
          this.tableLayoutPanel1.Size = new System.Drawing.Size(626, 433);
          this.tableLayoutPanel1.TabIndex = 0;
          // 
          // txtResult
          // 
          this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
          this.txtResult.Location = new System.Drawing.Point(3, 103);
          this.txtResult.Multiline = true;
          this.txtResult.Name = "txtResult";
          this.txtResult.ReadOnly = true;
          this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
          this.txtResult.Size = new System.Drawing.Size(620, 327);
          this.txtResult.TabIndex = 1;
          this.txtResult.WordWrap = false;
          // 
          // groupBox1
          // 
          this.groupBox1.Controls.Add(this.label1);
          this.groupBox1.Controls.Add(this.btnFile);
          this.groupBox1.Controls.Add(this.btnRun);
          this.groupBox1.Controls.Add(this.label2);
          this.groupBox1.Controls.Add(this.txtTC);
          this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.groupBox1.Location = new System.Drawing.Point(3, 3);
          this.groupBox1.Name = "groupBox1";
          this.groupBox1.Size = new System.Drawing.Size(620, 94);
          this.groupBox1.TabIndex = 2;
          this.groupBox1.TabStop = false;
          this.groupBox1.Text = "Test harness";
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(6, 55);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(307, 26);
          this.label1.TabIndex = 13;
          this.label1.Text = "Note:Please add the bizunit.dll and other assemblies containing\r\n the test steps " +
              "into the folder where BizUnit Designer is installed.";
          // 
          // btnFile
          // 
          this.btnFile.Location = new System.Drawing.Point(559, 22);
          this.btnFile.Name = "btnFile";
          this.btnFile.Size = new System.Drawing.Size(34, 23);
          this.btnFile.TabIndex = 12;
          this.btnFile.Text = "...";
          this.btnFile.UseVisualStyleBackColor = true;
          this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
          // 
          // btnRun
          // 
          this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.btnRun.Location = new System.Drawing.Point(478, 59);
          this.btnRun.Name = "btnRun";
          this.btnRun.Size = new System.Drawing.Size(115, 23);
          this.btnRun.TabIndex = 10;
          this.btnRun.Text = "Run BizUnit Tests";
          this.btnRun.UseVisualStyleBackColor = true;
          this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(7, 26);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(78, 13);
          this.label2.TabIndex = 9;
          this.label2.Text = "Test case files:";
          // 
          // txtTC
          // 
          this.txtTC.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
          this.txtTC.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
          this.txtTC.Location = new System.Drawing.Point(131, 24);
          this.txtTC.Name = "txtTC";
          this.txtTC.Size = new System.Drawing.Size(421, 20);
          this.txtTC.TabIndex = 5;
          // 
          // openFileDialog1
          // 
          this.openFileDialog1.Filter = "Xml files|*.xml";
          this.openFileDialog1.Multiselect = true;
          // 
          // backgroundWorker1
          // 
          this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
          this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
          // 
          // timer1
          // 
          this.timer1.Interval = 1000;
          this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
          // 
          // TestHarness
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(626, 433);
          this.Controls.Add(this.tableLayoutPanel1);
          this.Name = "TestHarness";
          this.Text = "TestHarness";
          this.tableLayoutPanel1.ResumeLayout(false);
          this.tableLayoutPanel1.PerformLayout();
          this.groupBox1.ResumeLayout(false);
          this.groupBox1.PerformLayout();
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTC;
        private System.Windows.Forms.Button btnRun;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
      private System.Windows.Forms.Button btnFile;
      private System.Windows.Forms.Label label1;
    }
}