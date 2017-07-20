namespace LogMonitor.Core.CheckUser
{
	partial class FormCheckUser
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
			this.textBoxLogin = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonCheck = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxExistInUserList = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxUserListGroup = new System.Windows.Forms.TextBox();
			this.listBoxADGroups = new System.Windows.Forms.ListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxUserListGroupAD = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxADGroup = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
			this.textBoxFIO = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBoxLogin
			// 
			this.textBoxLogin.Location = new System.Drawing.Point(51, 15);
			this.textBoxLogin.Name = "textBoxLogin";
			this.textBoxLogin.Size = new System.Drawing.Size(101, 20);
			this.textBoxLogin.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Login";
			// 
			// buttonCheck
			// 
			this.buttonCheck.Location = new System.Drawing.Point(158, 13);
			this.buttonCheck.Name = "buttonCheck";
			this.buttonCheck.Size = new System.Drawing.Size(75, 23);
			this.buttonCheck.TabIndex = 2;
			this.buttonCheck.Text = "Check";
			this.buttonCheck.UseVisualStyleBackColor = true;
			this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.groupBox4);
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Location = new System.Drawing.Point(15, 42);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(575, 598);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Результаты проверки";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(27, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(50, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Наличие";
			// 
			// textBoxExistInUserList
			// 
			this.textBoxExistInUserList.Location = new System.Drawing.Point(13, 41);
			this.textBoxExistInUserList.Name = "textBoxExistInUserList";
			this.textBoxExistInUserList.ReadOnly = true;
			this.textBoxExistInUserList.Size = new System.Drawing.Size(75, 20);
			this.textBoxExistInUserList.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(134, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Группа";
			// 
			// textBoxUserListGroup
			// 
			this.textBoxUserListGroup.Location = new System.Drawing.Point(94, 41);
			this.textBoxUserListGroup.Name = "textBoxUserListGroup";
			this.textBoxUserListGroup.ReadOnly = true;
			this.textBoxUserListGroup.Size = new System.Drawing.Size(134, 20);
			this.textBoxUserListGroup.TabIndex = 1;
			// 
			// listBoxADGroups
			// 
			this.listBoxADGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxADGroups.FormattingEnabled = true;
			this.listBoxADGroups.Location = new System.Drawing.Point(278, 32);
			this.listBoxADGroups.Name = "listBoxADGroups";
			this.listBoxADGroups.Size = new System.Drawing.Size(279, 199);
			this.listBoxADGroups.TabIndex = 2;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.textBoxUserListGroupAD);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.textBoxUserListGroup);
			this.groupBox2.Controls.Add(this.textBoxExistInUserList);
			this.groupBox2.Location = new System.Drawing.Point(6, 19);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(563, 71);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "UserList";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.listBoxADGroups);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.textBoxADGroup);
			this.groupBox3.Location = new System.Drawing.Point(6, 96);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(563, 239);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "AD";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(275, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Группы";
			// 
			// textBoxUserListGroupAD
			// 
			this.textBoxUserListGroupAD.Location = new System.Drawing.Point(238, 41);
			this.textBoxUserListGroupAD.Name = "textBoxUserListGroupAD";
			this.textBoxUserListGroupAD.ReadOnly = true;
			this.textBoxUserListGroupAD.Size = new System.Drawing.Size(221, 20);
			this.textBoxUserListGroupAD.TabIndex = 1;
			this.textBoxUserListGroupAD.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(315, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(57, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "ГруппаAD";
			// 
			// textBoxADGroup
			// 
			this.textBoxADGroup.Location = new System.Drawing.Point(54, 29);
			this.textBoxADGroup.Name = "textBoxADGroup";
			this.textBoxADGroup.ReadOnly = true;
			this.textBoxADGroup.Size = new System.Drawing.Size(215, 20);
			this.textBoxADGroup.TabIndex = 1;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(42, 13);
			this.label6.TabIndex = 0;
			this.label6.Text = "Группа";
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.richTextBoxLog);
			this.groupBox4.Location = new System.Drawing.Point(6, 341);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(563, 251);
			this.groupBox4.TabIndex = 5;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Log";
			// 
			// richTextBoxLog
			// 
			this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBoxLog.Location = new System.Drawing.Point(6, 19);
			this.richTextBoxLog.Name = "richTextBoxLog";
			this.richTextBoxLog.ReadOnly = true;
			this.richTextBoxLog.Size = new System.Drawing.Size(551, 226);
			this.richTextBoxLog.TabIndex = 0;
			this.richTextBoxLog.Text = "";
			// 
			// textBoxFIO
			// 
			this.textBoxFIO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxFIO.Location = new System.Drawing.Point(239, 16);
			this.textBoxFIO.Name = "textBoxFIO";
			this.textBoxFIO.ReadOnly = true;
			this.textBoxFIO.Size = new System.Drawing.Size(339, 20);
			this.textBoxFIO.TabIndex = 1;
			// 
			// FormCheckUser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(602, 652);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonCheck);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxLogin);
			this.Controls.Add(this.textBoxFIO);
			this.Name = "FormCheckUser";
			this.Text = "FormCheckUser";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxLogin;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonCheck;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ListBox listBoxADGroups;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxUserListGroupAD;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxUserListGroup;
		private System.Windows.Forms.TextBox textBoxExistInUserList;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBoxADGroup;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RichTextBox richTextBoxLog;
		private System.Windows.Forms.TextBox textBoxFIO;
	}
}