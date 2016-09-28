namespace STDLite
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.lstMain = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.munCopySTDId = new System.Windows.Forms.ToolStripMenuItem();
            this.munCopySTDName = new System.Windows.Forms.ToolStripMenuItem();
            this.munCopySTD = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.munSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.munOpenSavedDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCheckUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(529, 5);
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
            this.txtKeyword.Location = new System.Drawing.Point(95, 6);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(429, 21);
            this.txtKeyword.TabIndex = 1;
            this.txtKeyword.TabStop = false;
            this.txtKeyword.Text = "关键字示例：【SHT 3405】【发电厂设计规范】【化工 泵】";
            this.txtKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyDown);
            // 
            // lstMain
            // 
            this.lstMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5,
            this.columnHeader4});
            this.lstMain.ContextMenuStrip = this.contextMenuStrip1;
            this.lstMain.FullRowSelect = true;
            this.lstMain.GridLines = true;
            this.lstMain.Location = new System.Drawing.Point(6, 32);
            this.lstMain.MultiSelect = false;
            this.lstMain.Name = "lstMain";
            this.lstMain.Size = new System.Drawing.Size(600, 362);
            this.lstMain.TabIndex = 3;
            this.lstMain.TabStop = false;
            this.lstMain.UseCompatibleStateImageBehavior = false;
            this.lstMain.View = System.Windows.Forms.View.Details;
            this.lstMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstMain_MouseDoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "标准号";
            this.columnHeader2.Width = 140;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "标准名";
            this.columnHeader3.Width = 370;
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 3;
            this.columnHeader5.Text = "状态";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 2;
            this.columnHeader4.Text = "下载地址";
            this.columnHeader4.Width = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.munCopySTDId,
            this.munCopySTDName,
            this.munCopySTD,
            this.toolStripSeparator1,
            this.munSaveAs,
            this.munOpenSavedDirectory,
            this.toolStripSeparator2,
            this.mnuOnTop,
            this.mnuCheckUpdate,
            this.mnuAbout});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 214);
            // 
            // munCopySTDId
            // 
            this.munCopySTDId.Name = "munCopySTDId";
            this.munCopySTDId.Size = new System.Drawing.Size(152, 22);
            this.munCopySTDId.Text = "复制标准号";
            this.munCopySTDId.Click += new System.EventHandler(this.munCopySTDId_Click);
            // 
            // munCopySTDName
            // 
            this.munCopySTDName.Name = "munCopySTDName";
            this.munCopySTDName.Size = new System.Drawing.Size(152, 22);
            this.munCopySTDName.Text = "复制标准名";
            this.munCopySTDName.Click += new System.EventHandler(this.munCopySTDName_Click);
            // 
            // munCopySTD
            // 
            this.munCopySTD.Name = "munCopySTD";
            this.munCopySTD.Size = new System.Drawing.Size(152, 22);
            this.munCopySTD.Text = "复制标准全称";
            this.munCopySTD.Click += new System.EventHandler(this.munCopySTD_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // munSaveAs
            // 
            this.munSaveAs.Name = "munSaveAs";
            this.munSaveAs.Size = new System.Drawing.Size(152, 22);
            this.munSaveAs.Text = "另存为";
            this.munSaveAs.Click += new System.EventHandler(this.munSaveAs_Click);
            // 
            // munOpenSavedDirectory
            // 
            this.munOpenSavedDirectory.Name = "munOpenSavedDirectory";
            this.munOpenSavedDirectory.Size = new System.Drawing.Size(152, 22);
            this.munOpenSavedDirectory.Text = "打开存放目录";
            this.munOpenSavedDirectory.Click += new System.EventHandler(this.munOpenSavedDirectory_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // mnuOnTop
            // 
            this.mnuOnTop.CheckOnClick = true;
            this.mnuOnTop.Name = "mnuOnTop";
            this.mnuOnTop.Size = new System.Drawing.Size(152, 22);
            this.mnuOnTop.Text = "窗口置顶";
            this.mnuOnTop.Click += new System.EventHandler(this.mnuOnTop_Click);
            // 
            // mnuCheckUpdate
            // 
            this.mnuCheckUpdate.Name = "mnuCheckUpdate";
            this.mnuCheckUpdate.Size = new System.Drawing.Size(152, 22);
            this.mnuCheckUpdate.Text = "检查更新";
            this.mnuCheckUpdate.Click += new System.EventHandler(this.mnuCheckUpdate_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(152, 22);
            this.mnuAbout.Text = "关于本软件";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "标准号/标准名";
            // 
            // FormMain
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 396);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstMain);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.btnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.ListView lstMain;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem munCopySTDName;
        private System.Windows.Forms.ToolStripMenuItem munCopySTDId;
        private System.Windows.Forms.ToolStripMenuItem munCopySTD;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuOnTop;
        private System.Windows.Forms.ToolStripMenuItem munSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem munOpenSavedDirectory;
        private System.Windows.Forms.ToolStripMenuItem mnuCheckUpdate;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
    }
}

