
namespace MSFS2020Ctrls
{
  partial class FrmPrint
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrint));
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btClose = new System.Windows.Forms.Button();
      this.RTB = new System.Windows.Forms.RichTextBox();
      this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ctxCopy = new System.Windows.Forms.ToolStripMenuItem();
      this.ctxSelectAll = new System.Windows.Forms.ToolStripMenuItem();
      this.ctxSaveAs = new System.Windows.Forms.ToolStripMenuItem();
      this.SFD = new System.Windows.Forms.SaveFileDialog();
      this.label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.ctxMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.RTB, 0, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(659, 523);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.label1);
      this.panel1.Controls.Add(this.btClose);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(653, 34);
      this.panel1.TabIndex = 0;
      // 
      // btClose
      // 
      this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btClose.Location = new System.Drawing.Point(3, 5);
      this.btClose.Name = "btClose";
      this.btClose.Size = new System.Drawing.Size(89, 26);
      this.btClose.TabIndex = 0;
      this.btClose.Text = "Close";
      this.btClose.UseVisualStyleBackColor = true;
      // 
      // RTB
      // 
      this.RTB.ContextMenuStrip = this.ctxMenu;
      this.RTB.Dock = System.Windows.Forms.DockStyle.Fill;
      this.RTB.Location = new System.Drawing.Point(3, 43);
      this.RTB.Name = "RTB";
      this.RTB.Size = new System.Drawing.Size(653, 477);
      this.RTB.TabIndex = 1;
      this.RTB.Text = "";
      // 
      // ctxMenu
      // 
      this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxCopy,
            this.ctxSelectAll,
            this.ctxSaveAs});
      this.ctxMenu.Name = "ctxMenu";
      this.ctxMenu.Size = new System.Drawing.Size(124, 70);
      // 
      // ctxCopy
      // 
      this.ctxCopy.Name = "ctxCopy";
      this.ctxCopy.Size = new System.Drawing.Size(123, 22);
      this.ctxCopy.Text = "Copy";
      this.ctxCopy.Click += new System.EventHandler(this.ctxCopy_Click);
      // 
      // ctxSelectAll
      // 
      this.ctxSelectAll.Name = "ctxSelectAll";
      this.ctxSelectAll.Size = new System.Drawing.Size(123, 22);
      this.ctxSelectAll.Text = "Select All";
      this.ctxSelectAll.Click += new System.EventHandler(this.ctxSelectAll_Click);
      // 
      // ctxSaveAs
      // 
      this.ctxSaveAs.Name = "ctxSaveAs";
      this.ctxSaveAs.Size = new System.Drawing.Size(123, 22);
      this.ctxSaveAs.Text = "Save As ..";
      this.ctxSaveAs.Click += new System.EventHandler(this.ctxSaveAs_Click);
      // 
      // SFD
      // 
      this.SFD.DefaultExt = "rtf";
      this.SFD.SupportMultiDottedExtensions = true;
      this.SFD.Title = "Save as RTF File";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(157, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(165, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Right Click for a Context Menu";
      // 
      // FrmPrint
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btClose;
      this.ClientSize = new System.Drawing.Size(659, 523);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FrmPrint";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "FrmPrint";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ctxMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btClose;
    private System.Windows.Forms.RichTextBox RTB;
    private System.Windows.Forms.ContextMenuStrip ctxMenu;
    private System.Windows.Forms.ToolStripMenuItem ctxCopy;
    private System.Windows.Forms.ToolStripMenuItem ctxSelectAll;
    private System.Windows.Forms.ToolStripMenuItem ctxSaveAs;
    private System.Windows.Forms.SaveFileDialog SFD;
    private System.Windows.Forms.Label label1;
  }
}