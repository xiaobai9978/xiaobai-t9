
namespace t9skin
{
    partial class Form2
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
            this.colorEditor1 = new Cyotek.Windows.Forms.ColorEditor();
            this.colorGrid1 = new Cyotek.Windows.Forms.ColorGrid();
            this.screenColorPicker1 = new Cyotek.Windows.Forms.ScreenColorPicker();
            this.colorEditorManager1 = new Cyotek.Windows.Forms.ColorEditorManager();
            this.colorGrid2 = new Cyotek.Windows.Forms.ColorGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.colorWheel1 = new Cyotek.Windows.Forms.ColorWheel();
            this.colorEditorManager2 = new Cyotek.Windows.Forms.ColorEditorManager();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // colorEditor1
            // 
            this.colorEditor1.Location = new System.Drawing.Point(268, 3);
            this.colorEditor1.Name = "colorEditor1";
            this.colorEditor1.Size = new System.Drawing.Size(344, 240);
            this.colorEditor1.TabIndex = 0;
            // 
            // colorGrid1
            // 
            this.colorGrid1.CellSize = new System.Drawing.Size(5, 5);
            this.colorGrid1.Columns = 17;
            this.colorGrid1.Location = new System.Drawing.Point(268, 259);
            this.colorGrid1.Name = "colorGrid1";
            this.colorGrid1.ShowCustomColors = false;
            this.colorGrid1.Size = new System.Drawing.Size(347, 190);
            this.colorGrid1.TabIndex = 1;
            // 
            // screenColorPicker1
            // 
            this.screenColorPicker1.Color = System.Drawing.Color.Empty;
            this.screenColorPicker1.Location = new System.Drawing.Point(12, 259);
            this.screenColorPicker1.Name = "screenColorPicker1";
            this.screenColorPicker1.Size = new System.Drawing.Size(111, 103);
            this.screenColorPicker1.Text = "取色笔";
            // 
            // colorEditorManager1
            // 
            this.colorEditorManager1.Color = System.Drawing.Color.Empty;
            this.colorEditorManager1.ColorEditor = this.colorEditor1;
            this.colorEditorManager1.ColorGrid = this.colorGrid2;
            this.colorEditorManager1.ColorWheel = this.colorWheel1;
            this.colorEditorManager1.ScreenColorPicker = this.screenColorPicker1;
            // 
            // colorGrid2
            // 
            this.colorGrid2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.colorGrid2.CellBorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.colorGrid2.CellBorderStyle = Cyotek.Windows.Forms.ColorCellBorderStyle.None;
            this.colorGrid2.CellSize = new System.Drawing.Size(0, 0);
            this.colorGrid2.Columns = 1;
            this.colorGrid2.Location = new System.Drawing.Point(141, 259);
            this.colorGrid2.Name = "colorGrid2";
            this.colorGrid2.Size = new System.Drawing.Size(121, 104);
            this.colorGrid2.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 394);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 36);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // colorWheel1
            // 
            this.colorWheel1.Alpha = 1D;
            this.colorWheel1.BackColor = System.Drawing.SystemColors.Control;
            this.colorWheel1.DisplayLightness = true;
            this.colorWheel1.Location = new System.Drawing.Point(12, 3);
            this.colorWheel1.Name = "colorWheel1";
            this.colorWheel1.Size = new System.Drawing.Size(232, 240);
            this.colorWheel1.TabIndex = 2;
            // 
            // colorEditorManager2
            // 
            this.colorEditorManager2.Color = System.Drawing.Color.Empty;
            this.colorEditorManager2.ColorGrid = this.colorGrid1;
            this.colorEditorManager2.ColorWheel = this.colorWheel1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(141, 394);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 36);
            this.button2.TabIndex = 5;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 461);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.colorGrid2);
            this.Controls.Add(this.screenColorPicker1);
            this.Controls.Add(this.colorWheel1);
            this.Controls.Add(this.colorGrid1);
            this.Controls.Add(this.colorEditor1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form2";
            this.Text = "颜色选择器";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Cyotek.Windows.Forms.ColorEditor colorEditor1;
        private Cyotek.Windows.Forms.ColorGrid colorGrid1;
        private Cyotek.Windows.Forms.ColorWheel colorWheel1;
        private Cyotek.Windows.Forms.ScreenColorPicker screenColorPicker1;
        private Cyotek.Windows.Forms.ColorEditorManager colorEditorManager1;
        private Cyotek.Windows.Forms.ColorGrid colorGrid2;
        private System.Windows.Forms.Button button1;
        private Cyotek.Windows.Forms.ColorEditorManager colorEditorManager2;
        private System.Windows.Forms.Button button2;
    }
}