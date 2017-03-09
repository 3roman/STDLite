namespace STDLite
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.lstStandard = new System.Windows.Forms.ListView();
            this.colNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colReplacement = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDownloadURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.munCopyNumber = new System.Windows.Forms.ToolStripMenuItem();
            this.munCopyName = new System.Windows.Forms.ToolStripMenuItem();
            this.munCopyStandard = new System.Windows.Forms.ToolStripMenuItem();
            this.munSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuTopMost = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ssDownloadPercent = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssProcessBar = new System.Windows.Forms.ToolStripProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(524, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.TabStop = false;
            this.btnSearch.Text = "检  索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtKeyword.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtKeyword.Location = new System.Drawing.Point(92, 6);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(425, 21);
            this.txtKeyword.TabIndex = 1;
            this.txtKeyword.TabStop = false;
            this.txtKeyword.Text = "示例：【SHT 3405】【发电厂设计规范】【化工 泵】";
            this.txtKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyDown);
            // 
            // lstStandard
            // 
            this.lstStandard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNumber,
            this.colName,
            this.colState,
            this.colReplacement,
            this.colDownloadURL});
            this.lstStandard.ContextMenuStrip = this.contextMenuStrip1;
            this.lstStandard.FullRowSelect = true;
            this.lstStandard.GridLines = true;
            this.lstStandard.Location = new System.Drawing.Point(0, 33);
            this.lstStandard.MultiSelect = false;
            this.lstStandard.Name = "lstStandard";
            this.lstStandard.Size = new System.Drawing.Size(602, 362);
            this.lstStandard.TabIndex = 3;
            this.lstStandard.TabStop = false;
            this.lstStandard.UseCompatibleStateImageBehavior = false;
            this.lstStandard.View = System.Windows.Forms.View.Details;
            this.lstStandard.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstMain_MouseDoubleClick);
            // 
            // colNumber
            // 
            this.colNumber.Text = "标准号";
            this.colNumber.Width = 132;
            // 
            // colName
            // 
            this.colName.Text = "标准名";
            this.colName.Width = 242;
            // 
            // colState
            // 
            this.colState.Text = "标准状态";
            this.colState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colState.Width = 70;
            // 
            // colReplacement
            // 
            this.colReplacement.Text = "替代说明";
            this.colReplacement.Width = 147;
            // 
            // colDownloadURL
            // 
            this.colDownloadURL.Text = "下载地址";
            this.colDownloadURL.Width = 4;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.munCopyNumber,
            this.munCopyName,
            this.munCopyStandard,
            this.munSaveAs,
            this.toolStripSeparator2,
            this.mnuTopMost,
            this.mnuAbout});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 142);
            // 
            // munCopyNumber
            // 
            this.munCopyNumber.Name = "munCopyNumber";
            this.munCopyNumber.Size = new System.Drawing.Size(148, 22);
            this.munCopyNumber.Text = "复制标准号";
            this.munCopyNumber.Click += new System.EventHandler(this.munCopyNumber_Click);
            // 
            // munCopyName
            // 
            this.munCopyName.Name = "munCopyName";
            this.munCopyName.Size = new System.Drawing.Size(148, 22);
            this.munCopyName.Text = "复制标准名";
            this.munCopyName.Click += new System.EventHandler(this.munCopyName_Click);
            // 
            // munCopyStandard
            // 
            this.munCopyStandard.Name = "munCopyStandard";
            this.munCopyStandard.Size = new System.Drawing.Size(148, 22);
            this.munCopyStandard.Text = "复制标准全称";
            this.munCopyStandard.Click += new System.EventHandler(this.munCopyStandard_Click);
            // 
            // munSaveAs
            // 
            this.munSaveAs.Name = "munSaveAs";
            this.munSaveAs.Size = new System.Drawing.Size(148, 22);
            this.munSaveAs.Text = "标准另存为";
            this.munSaveAs.Click += new System.EventHandler(this.munSaveAs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // mnuTopMost
            // 
            this.mnuTopMost.CheckOnClick = true;
            this.mnuTopMost.Name = "mnuTopMost";
            this.mnuTopMost.ShowShortcutKeys = false;
            this.mnuTopMost.Size = new System.Drawing.Size(148, 22);
            this.mnuTopMost.Text = "窗口置顶";
            this.mnuTopMost.Click += new System.EventHandler(this.mnuTopMost_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(148, 22);
            this.mnuAbout.Text = "关于本软件";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssDownloadPercent,
            this.ssProcessBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 373);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(602, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ssDownloadPercent
            // 
            this.ssDownloadPercent.Name = "ssDownloadPercent";
            this.ssDownloadPercent.Size = new System.Drawing.Size(56, 17);
            this.ssDownloadPercent.Text = "下载进度";
            // 
            // ssProcessBar
            // 
            this.ssProcessBar.MarqueeAnimationSpeed = 250;
            this.ssProcessBar.Name = "ssProcessBar";
            this.ssProcessBar.Size = new System.Drawing.Size(532, 16);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "标准号/标准名";
            // 
            // FrmMain
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 395);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lstStandard);
            this.Controls.Add(this.txtKeyword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.ListView lstStandard;
        private System.Windows.Forms.ColumnHeader colNumber;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colDownloadURL;
        private System.Windows.Forms.ColumnHeader colReplacement;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem munCopyName;
        private System.Windows.Forms.ToolStripMenuItem munCopyNumber;
        private System.Windows.Forms.ToolStripMenuItem munCopyStandard;
        private System.Windows.Forms.ToolStripMenuItem mnuTopMost;
        private System.Windows.Forms.ToolStripMenuItem munSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ColumnHeader colState;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ssDownloadPercent;
        private System.Windows.Forms.ToolStripProgressBar ssProcessBar;
        private System.Windows.Forms.Label label1;
    }
}

