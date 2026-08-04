namespace NumKeyboardTray
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBoxMapping = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelForm = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxBrowserHome = new System.Windows.Forms.ComboBox();
            this.comboBoxTab = new System.Windows.Forms.ComboBox();
            this.comboBoxMail = new System.Windows.Forms.ComboBox();
            this.comboBoxApp2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxCtrl = new System.Windows.Forms.ComboBox();
            this.comboBox0 = new System.Windows.Forms.ComboBox();
            this.comboBoxDot = new System.Windows.Forms.ComboBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.groupBoxMapping.SuspendLayout();
            this.tableLayoutPanelForm.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxMapping, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.panelBottom, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(640, 329);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(3, 3);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(634, 54);
            this.panelHeader.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(20, 18);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(188, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "小白T9无线键盘驱动";
            // 
            // groupBoxMapping
            // 
            this.groupBoxMapping.Controls.Add(this.tableLayoutPanelForm);
            this.groupBoxMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMapping.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxMapping.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.groupBoxMapping.Location = new System.Drawing.Point(3, 63);
            this.groupBoxMapping.Name = "groupBoxMapping";
            this.groupBoxMapping.Padding = new System.Windows.Forms.Padding(15, 35, 15, 15);
            this.groupBoxMapping.Size = new System.Drawing.Size(634, 213);
            this.groupBoxMapping.TabIndex = 2;
            this.groupBoxMapping.TabStop = false;
            this.groupBoxMapping.Text = "按键功能映射配置";
            // 
            // tableLayoutPanelForm
            // 
            this.tableLayoutPanelForm.ColumnCount = 4;
            this.tableLayoutPanelForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelForm.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanelForm.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanelForm.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanelForm.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanelForm.Controls.Add(this.comboBoxBrowserHome, 0, 1);
            this.tableLayoutPanelForm.Controls.Add(this.comboBoxTab, 1, 1);
            this.tableLayoutPanelForm.Controls.Add(this.comboBoxMail, 2, 1);
            this.tableLayoutPanelForm.Controls.Add(this.comboBoxApp2, 3, 1);
            this.tableLayoutPanelForm.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanelForm.Controls.Add(this.label8, 1, 2);
            this.tableLayoutPanelForm.Controls.Add(this.label10, 2, 2);
            this.tableLayoutPanelForm.Controls.Add(this.comboBoxCtrl, 0, 3);
            this.tableLayoutPanelForm.Controls.Add(this.comboBox0, 1, 3);
            this.tableLayoutPanelForm.Controls.Add(this.comboBoxDot, 2, 3);
            this.tableLayoutPanelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelForm.Location = new System.Drawing.Point(15, 51);
            this.tableLayoutPanelForm.Name = "tableLayoutPanelForm";
            this.tableLayoutPanelForm.RowCount = 4;
            this.tableLayoutPanelForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelForm.Size = new System.Drawing.Size(604, 147);
            this.tableLayoutPanelForm.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label4.Location = new System.Drawing.Point(5, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "按键1功能";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label3.Location = new System.Drawing.Point(156, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "按键2功能";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.Location = new System.Drawing.Point(307, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "按键3功能";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label2.Location = new System.Drawing.Point(458, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "按键4功能";
            // 
            // comboBoxBrowserHome
            // 
            this.comboBoxBrowserHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxBrowserHome.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBrowserHome.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxBrowserHome.Location = new System.Drawing.Point(5, 32);
            this.comboBoxBrowserHome.Margin = new System.Windows.Forms.Padding(5, 0, 15, 10);
            this.comboBoxBrowserHome.Name = "comboBoxBrowserHome";
            this.comboBoxBrowserHome.Size = new System.Drawing.Size(131, 25);
            this.comboBoxBrowserHome.TabIndex = 4;
            // 
            // comboBoxTab
            // 
            this.comboBoxTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTab.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxTab.Location = new System.Drawing.Point(156, 32);
            this.comboBoxTab.Margin = new System.Windows.Forms.Padding(5, 0, 15, 10);
            this.comboBoxTab.Name = "comboBoxTab";
            this.comboBoxTab.Size = new System.Drawing.Size(131, 25);
            this.comboBoxTab.TabIndex = 5;
            // 
            // comboBoxMail
            // 
            this.comboBoxMail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxMail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMail.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxMail.Location = new System.Drawing.Point(307, 32);
            this.comboBoxMail.Margin = new System.Windows.Forms.Padding(5, 0, 15, 10);
            this.comboBoxMail.Name = "comboBoxMail";
            this.comboBoxMail.Size = new System.Drawing.Size(131, 25);
            this.comboBoxMail.TabIndex = 6;
            // 
            // comboBoxApp2
            // 
            this.comboBoxApp2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxApp2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApp2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxApp2.Location = new System.Drawing.Point(458, 32);
            this.comboBoxApp2.Margin = new System.Windows.Forms.Padding(5, 0, 15, 10);
            this.comboBoxApp2.Name = "comboBoxApp2";
            this.comboBoxApp2.Size = new System.Drawing.Size(131, 25);
            this.comboBoxApp2.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label9.Location = new System.Drawing.Point(5, 82);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 15, 5, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 17);
            this.label9.TabIndex = 8;
            this.label9.Text = "Ctrl 键";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label8.Location = new System.Drawing.Point(156, 82);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 15, 5, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 17);
            this.label8.TabIndex = 9;
            this.label8.Text = "0 键";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label10.Location = new System.Drawing.Point(307, 82);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 15, 5, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 17);
            this.label10.TabIndex = 10;
            this.label10.Text = "点 . 键";
            // 
            // comboBoxCtrl
            // 
            this.comboBoxCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCtrl.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxCtrl.Location = new System.Drawing.Point(5, 104);
            this.comboBoxCtrl.Margin = new System.Windows.Forms.Padding(5, 0, 15, 10);
            this.comboBoxCtrl.Name = "comboBoxCtrl";
            this.comboBoxCtrl.Size = new System.Drawing.Size(131, 25);
            this.comboBoxCtrl.TabIndex = 11;
            // 
            // comboBox0
            // 
            this.comboBox0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox0.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBox0.Location = new System.Drawing.Point(156, 104);
            this.comboBox0.Margin = new System.Windows.Forms.Padding(5, 0, 15, 10);
            this.comboBox0.Name = "comboBox0";
            this.comboBox0.Size = new System.Drawing.Size(131, 25);
            this.comboBox0.TabIndex = 12;
            // 
            // comboBoxDot
            // 
            this.comboBoxDot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDot.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxDot.Location = new System.Drawing.Point(307, 104);
            this.comboBoxDot.Margin = new System.Windows.Forms.Padding(5, 0, 15, 10);
            this.comboBoxDot.Name = "comboBoxDot";
            this.comboBoxDot.Size = new System.Drawing.Size(131, 25);
            this.comboBoxDot.TabIndex = 13;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.checkBox1);
            this.panelBottom.Controls.Add(this.button1);
            this.panelBottom.Controls.Add(this.label5);
            this.panelBottom.Controls.Add(this.label6);
            this.panelBottom.Controls.Add(this.label7);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(3, 282);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.panelBottom.Size = new System.Drawing.Size(634, 44);
            this.panelBottom.TabIndex = 3;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.checkBox1.Location = new System.Drawing.Point(20, 15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(123, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "开机自动启动驱动";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Underline);
            this.button1.ForeColor = System.Drawing.Color.Gray;
            this.button1.Location = new System.Drawing.Point(520, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "临时强制开启";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(180, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "支持设备:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label6.Location = new System.Drawing.Point(240, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 16);
            this.label6.TabIndex = 3;
            this.label6.Text = "小白T9无线键盘2.0";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(370, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "| 版本：V2.2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 329);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "小白T9无线键盘驱动";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.groupBoxMapping.ResumeLayout(false);
            this.tableLayoutPanelForm.ResumeLayout(false);
            this.tableLayoutPanelForm.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // === 原有控件声明 (Name绝对不变) ===
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBoxMail;
        private System.Windows.Forms.ComboBox comboBoxApp2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxTab;
        private System.Windows.Forms.ComboBox comboBoxBrowserHome;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox0;
        private System.Windows.Forms.ComboBox comboBoxCtrl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxDot;

        // === 新增布局控件声明 ===
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.GroupBox groupBoxMapping;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelForm;
    }
}