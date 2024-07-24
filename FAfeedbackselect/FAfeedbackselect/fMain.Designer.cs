namespace FAfeedbackselect
{
    partial class fMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btclear = new System.Windows.Forms.Button();
            this.btexport = new System.Windows.Forms.Button();
            this.checkSN = new System.Windows.Forms.CheckBox();
            this.lbmessage1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dt_end = new System.Windows.Forms.DateTimePicker();
            this.dt_start = new System.Windows.Forms.DateTimePicker();
            this.txtsn = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btselect = new System.Windows.Forms.Button();
            this.m_tGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_tGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox1.Controls.Add(this.btclear);
            this.groupBox1.Controls.Add(this.btexport);
            this.groupBox1.Controls.Add(this.checkSN);
            this.groupBox1.Controls.Add(this.lbmessage1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dt_end);
            this.groupBox1.Controls.Add(this.dt_start);
            this.groupBox1.Controls.Add(this.txtsn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btselect);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(699, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询界面";
            // 
            // btclear
            // 
            this.btclear.Location = new System.Drawing.Point(532, 49);
            this.btclear.Name = "btclear";
            this.btclear.Size = new System.Drawing.Size(75, 23);
            this.btclear.TabIndex = 18;
            this.btclear.Text = "清除";
            this.btclear.UseVisualStyleBackColor = true;
            this.btclear.Click += new System.EventHandler(this.btclear_Click);
            // 
            // btexport
            // 
            this.btexport.Location = new System.Drawing.Point(532, 81);
            this.btexport.Name = "btexport";
            this.btexport.Size = new System.Drawing.Size(75, 23);
            this.btexport.TabIndex = 17;
            this.btexport.Text = "导出";
            this.btexport.UseVisualStyleBackColor = true;
            this.btexport.Click += new System.EventHandler(this.btexport_Click);
            // 
            // checkSN
            // 
            this.checkSN.AutoSize = true;
            this.checkSN.Location = new System.Drawing.Point(245, 31);
            this.checkSN.Name = "checkSN";
            this.checkSN.Size = new System.Drawing.Size(84, 16);
            this.checkSN.TabIndex = 16;
            this.checkSN.Text = "请用SN查询";
            this.checkSN.UseVisualStyleBackColor = true;
            // 
            // lbmessage1
            // 
            this.lbmessage1.AutoSize = true;
            this.lbmessage1.ForeColor = System.Drawing.Color.Red;
            this.lbmessage1.Location = new System.Drawing.Point(186, 111);
            this.lbmessage1.Name = "lbmessage1";
            this.lbmessage1.Size = new System.Drawing.Size(47, 12);
            this.lbmessage1.TabIndex = 15;
            this.lbmessage1.Text = "Message";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(234, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "结束:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "开始:";
            // 
            // dt_end
            // 
            this.dt_end.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dt_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_end.Location = new System.Drawing.Point(275, 77);
            this.dt_end.Name = "dt_end";
            this.dt_end.Size = new System.Drawing.Size(159, 21);
            this.dt_end.TabIndex = 12;
            // 
            // dt_start
            // 
            this.dt_start.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dt_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_start.Location = new System.Drawing.Point(57, 77);
            this.dt_start.Name = "dt_start";
            this.dt_start.Size = new System.Drawing.Size(159, 21);
            this.dt_start.TabIndex = 11;
            // 
            // txtsn
            // 
            this.txtsn.Location = new System.Drawing.Point(68, 26);
            this.txtsn.Name = "txtsn";
            this.txtsn.Size = new System.Drawing.Size(148, 21);
            this.txtsn.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "SN:";
            // 
            // btselect
            // 
            this.btselect.Location = new System.Drawing.Point(532, 20);
            this.btselect.Name = "btselect";
            this.btselect.Size = new System.Drawing.Size(75, 23);
            this.btselect.TabIndex = 0;
            this.btselect.Text = "查询";
            this.btselect.UseVisualStyleBackColor = true;
            this.btselect.Click += new System.EventHandler(this.btselect_Click);
            // 
            // m_tGridView
            // 
            this.m_tGridView.AllowUserToAddRows = false;
            this.m_tGridView.AllowUserToDeleteRows = false;
            this.m_tGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.m_tGridView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.m_tGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_tGridView.Location = new System.Drawing.Point(3, 143);
            this.m_tGridView.Name = "m_tGridView";
            this.m_tGridView.ReadOnly = true;
            this.m_tGridView.RowTemplate.Height = 23;
            this.m_tGridView.Size = new System.Drawing.Size(699, 309);
            this.m_tGridView.TabIndex = 1;
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 453);
            this.Controls.Add(this.m_tGridView);
            this.Controls.Add(this.groupBox1);
            this.Name = "fMain";
            this.Text = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_tGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtsn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btselect;
        private System.Windows.Forms.DataGridView m_tGridView;
        private System.Windows.Forms.Label lbmessage1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dt_end;
        private System.Windows.Forms.DateTimePicker dt_start;
        private System.Windows.Forms.Button btexport;
        private System.Windows.Forms.CheckBox checkSN;
        private System.Windows.Forms.Button btclear;
    }
}