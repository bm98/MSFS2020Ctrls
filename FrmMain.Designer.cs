
namespace MSFS2020Ctrls
{
  partial class FrmMain
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) ) {
        components.Dispose( );
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
      this.cbxLang = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.RTB = new System.Windows.Forms.RichTextBox();
      this.clbDevices = new System.Windows.Forms.CheckedListBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.btPrint = new System.Windows.Forms.Button();
      this.btLoad = new System.Windows.Forms.Button();
      this.btShowForm = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.lblLang = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // cbxLang
      // 
      this.cbxLang.FormattingEnabled = true;
      this.cbxLang.Location = new System.Drawing.Point(482, 28);
      this.cbxLang.Name = "cbxLang";
      this.cbxLang.Size = new System.Drawing.Size(87, 21);
      this.cbxLang.Sorted = true;
      this.cbxLang.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(457, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(109, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Use Language Pack:";
      // 
      // RTB
      // 
      this.RTB.Location = new System.Drawing.Point(15, 359);
      this.RTB.Name = "RTB";
      this.RTB.Size = new System.Drawing.Size(554, 228);
      this.RTB.TabIndex = 8;
      this.RTB.Text = "";
      // 
      // clbDevices
      // 
      this.clbDevices.CheckOnClick = true;
      this.clbDevices.FormattingEnabled = true;
      this.clbDevices.Location = new System.Drawing.Point(15, 105);
      this.clbDevices.Name = "clbDevices";
      this.clbDevices.Size = new System.Drawing.Size(554, 208);
      this.clbDevices.TabIndex = 9;
      this.clbDevices.ThreeDCheckBoxes = true;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 343);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "Collection Report:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(21, 319);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(308, 13);
      this.label3.TabIndex = 11;
      this.label3.Text = "Only the first 4 checked profiles will be used for the layout";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 85);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(83, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Profiles found:";
      // 
      // btPrint
      // 
      this.btPrint.BackgroundImage = global::MSFS2020Ctrls.Properties.Resources.paper_content_pencil_48;
      this.btPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.btPrint.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btPrint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
      this.btPrint.Location = new System.Drawing.Point(223, 12);
      this.btPrint.Name = "btPrint";
      this.btPrint.Size = new System.Drawing.Size(106, 70);
      this.btPrint.TabIndex = 12;
      this.btPrint.Text = "Report";
      this.btPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.btPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
      this.btPrint.UseVisualStyleBackColor = true;
      this.btPrint.Click += new System.EventHandler(this.btPrint_Click);
      // 
      // btLoad
      // 
      this.btLoad.BackgroundImage = global::MSFS2020Ctrls.Properties.Resources.box_download_48;
      this.btLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.btLoad.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btLoad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
      this.btLoad.Location = new System.Drawing.Point(12, 12);
      this.btLoad.Name = "btLoad";
      this.btLoad.Size = new System.Drawing.Size(109, 70);
      this.btLoad.TabIndex = 5;
      this.btLoad.Text = "Load Profiles";
      this.btLoad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.btLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
      this.btLoad.UseVisualStyleBackColor = true;
      this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
      // 
      // btShowForm
      // 
      this.btShowForm.BackgroundImage = global::MSFS2020Ctrls.Properties.Resources.paper_content_chart_48;
      this.btShowForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.btShowForm.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btShowForm.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
      this.btShowForm.Location = new System.Drawing.Point(342, 12);
      this.btShowForm.Name = "btShowForm";
      this.btShowForm.Size = new System.Drawing.Size(109, 70);
      this.btShowForm.TabIndex = 4;
      this.btShowForm.Text = "Layout";
      this.btShowForm.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.btShowForm.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
      this.btShowForm.UseVisualStyleBackColor = true;
      this.btShowForm.Click += new System.EventHandler(this.btShowForm_Click);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(457, 65);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(117, 13);
      this.label5.TabIndex = 14;
      this.label5.Text = "Use Keyboard Layout:";
      // 
      // lblLang
      // 
      this.lblLang.AutoSize = true;
      this.lblLang.Location = new System.Drawing.Point(479, 85);
      this.lblLang.Name = "lblLang";
      this.lblLang.Size = new System.Drawing.Size(41, 13);
      this.lblLang.TabIndex = 14;
      this.lblLang.Text = "Use Ke";
      // 
      // FrmMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.ClientSize = new System.Drawing.Size(585, 602);
      this.Controls.Add(this.lblLang);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.btPrint);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.clbDevices);
      this.Controls.Add(this.RTB);
      this.Controls.Add(this.btLoad);
      this.Controls.Add(this.btShowForm);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cbxLang);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FrmMain";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MSFS2020 Control Layouts";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FmrMain_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.ComboBox cbxLang;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btShowForm;
    private System.Windows.Forms.Button btLoad;
    private System.Windows.Forms.RichTextBox RTB;
    private System.Windows.Forms.CheckedListBox clbDevices;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btPrint;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label lblLang;
  }
}

