namespace t9s2t
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // UI controls
        private System.Windows.Forms.Button btnDownloadModel;
        private System.Windows.Forms.Button btnDeleteModel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblEngine;
        private System.Windows.Forms.PictureBox appIcon;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.Timer hoverTimer;

        // Ad panel controls
        private System.Windows.Forms.Panel adPanel;
        private System.Windows.Forms.Button btnToggleAd;
        private System.Windows.Forms.PictureBox qrWechat;
        private System.Windows.Forms.PictureBox qrAlipay;
        private System.Windows.Forms.PictureBox qrTaobao;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusPanel = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.hoverTimer = new System.Windows.Forms.Timer(this.components);
            this.btnDownloadModel = new System.Windows.Forms.Button();
            this.btnDeleteModel = new System.Windows.Forms.Button();
            this.lblEngine = new System.Windows.Forms.Label();
            this.adPanel = new System.Windows.Forms.Panel();
            this.qrTaobao = new System.Windows.Forms.PictureBox();
            this.qrAlipay = new System.Windows.Forms.PictureBox();
            this.qrWechat = new System.Windows.Forms.PictureBox();
            this.btnToggleAd = new System.Windows.Forms.Button();
            this.appIcon = new System.Windows.Forms.PictureBox();
            this.statusPanel.SuspendLayout();
            this.adPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qrTaobao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qrAlipay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qrWechat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.Color.White;
            this.statusPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusPanel.Controls.Add(this.lblStatus);
            this.statusPanel.Location = new System.Drawing.Point(29, 40);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Padding = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.statusPanel.Size = new System.Drawing.Size(300, 46);
            this.statusPanel.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(147)))));
            this.lblStatus.Location = new System.Drawing.Point(15, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(268, 26);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "准备就绪 | 按 Ctrl+Alt+D 开始语音输入";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDownloadModel
            // 
            this.btnDownloadModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(116)))), ((int)(((byte)(181)))));
            this.btnDownloadModel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownloadModel.FlatAppearance.BorderSize = 0;
            this.btnDownloadModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadModel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDownloadModel.ForeColor = System.Drawing.Color.White;
            this.btnDownloadModel.Location = new System.Drawing.Point(29, 114);
            this.btnDownloadModel.Name = "btnDownloadModel";
            this.btnDownloadModel.Size = new System.Drawing.Size(140, 42);
            this.btnDownloadModel.TabIndex = 3;
            this.btnDownloadModel.Text = "下载模型 (42MB)";
            this.btnDownloadModel.UseVisualStyleBackColor = false;
            this.btnDownloadModel.Click += new System.EventHandler(this.btnDownloadModel_Click);
            // 
            // btnDeleteModel
            // 
            this.btnDeleteModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.btnDeleteModel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteModel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnDeleteModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteModel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDeleteModel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnDeleteModel.Location = new System.Drawing.Point(189, 114);
            this.btnDeleteModel.Name = "btnDeleteModel";
            this.btnDeleteModel.Size = new System.Drawing.Size(140, 42);
            this.btnDeleteModel.TabIndex = 4;
            this.btnDeleteModel.Text = "删除模型";
            this.btnDeleteModel.UseVisualStyleBackColor = false;
            this.btnDeleteModel.Click += new System.EventHandler(this.btnDeleteModel_Click);
            // 
            // lblEngine
            // 
            this.lblEngine.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblEngine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.lblEngine.Location = new System.Drawing.Point(29, 20);
            this.lblEngine.Name = "lblEngine";
            this.lblEngine.Size = new System.Drawing.Size(300, 17);
            this.lblEngine.TabIndex = 1;
            this.lblEngine.Text = "引擎: 未加载";
            this.lblEngine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // adPanel
            // 
            this.adPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.adPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.adPanel.Controls.Add(this.qrTaobao);
            this.adPanel.Controls.Add(this.qrAlipay);
            this.adPanel.Controls.Add(this.qrWechat);
            this.adPanel.Location = new System.Drawing.Point(362, 12);
            this.adPanel.Name = "adPanel";
            this.adPanel.Size = new System.Drawing.Size(530, 250);
            this.adPanel.TabIndex = 5;
            this.adPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.adPanel_Paint);
            // 
            // qrTaobao
            // 
            this.qrTaobao.Image = global::t9s2t.Properties.Resources.taobao_qrcode;
            this.qrTaobao.Location = new System.Drawing.Point(375, 45);
            this.qrTaobao.Name = "qrTaobao";
            this.qrTaobao.Size = new System.Drawing.Size(110, 110);
            this.qrTaobao.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrTaobao.TabIndex = 2;
            this.qrTaobao.TabStop = false;
            // 
            // qrAlipay
            // 
            this.qrAlipay.Image = global::t9s2t.Properties.Resources._0beffad9_8150_4b9c_849a_7960866759b1;
            this.qrAlipay.Location = new System.Drawing.Point(200, 45);
            this.qrAlipay.Name = "qrAlipay";
            this.qrAlipay.Size = new System.Drawing.Size(110, 110);
            this.qrAlipay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrAlipay.TabIndex = 1;
            this.qrAlipay.TabStop = false;
            // 
            // qrWechat
            // 
            this.qrWechat.Image = global::t9s2t.Properties.Resources.dc09882c_1165_40a0_b42b_913b121e6299;
            this.qrWechat.Location = new System.Drawing.Point(25, 45);
            this.qrWechat.Name = "qrWechat";
            this.qrWechat.Size = new System.Drawing.Size(110, 110);
            this.qrWechat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrWechat.TabIndex = 0;
            this.qrWechat.TabStop = false;
            // 
            // btnToggleAd
            // 
            this.btnToggleAd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.btnToggleAd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(180)))), ((int)(((byte)(160)))));
            this.btnToggleAd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleAd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnToggleAd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(100)))), ((int)(((byte)(60)))));
            this.btnToggleAd.Location = new System.Drawing.Point(340, 110);
            this.btnToggleAd.Name = "btnToggleAd";
            this.btnToggleAd.Size = new System.Drawing.Size(22, 50);
            this.btnToggleAd.TabIndex = 6;
            this.btnToggleAd.Text = "<";
            this.btnToggleAd.UseVisualStyleBackColor = false;
            this.btnToggleAd.Click += new System.EventHandler(this.btnToggleAd_Click);
            // 
            // appIcon
            // 
            this.appIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.appIcon.Location = new System.Drawing.Point(399, 49);
            this.appIcon.Name = "appIcon";
            this.appIcon.Size = new System.Drawing.Size(128, 118);
            this.appIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.appIcon.TabIndex = 0;
            this.appIcon.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(900, 232);
            this.Controls.Add(this.btnToggleAd);
            this.Controls.Add(this.adPanel);
            this.Controls.Add(this.appIcon);
            this.Controls.Add(this.lblEngine);
            this.Controls.Add(this.statusPanel);
            this.Controls.Add(this.btnDownloadModel);
            this.Controls.Add(this.btnDeleteModel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "小白T9语音输入 - 按住 Ctrl+Alt+D 开始语音输入";
            this.statusPanel.ResumeLayout(false);
            this.adPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.qrTaobao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qrAlipay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qrWechat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appIcon)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
