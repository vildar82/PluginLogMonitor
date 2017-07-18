namespace LogMonitor.Core.AddNewUsers
{
	partial class FormAddNewUsers
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
			this.dgUsers = new System.Windows.Forms.DataGridView();
			this.buttonOK = new System.Windows.Forms.Button();
			this.Login = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Group = new System.Windows.Forms.DataGridViewComboBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgUsers)).BeginInit();
			this.SuspendLayout();
			// 
			// dgUsers
			// 
			this.dgUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Login,
            this.Group});
			this.dgUsers.Location = new System.Drawing.Point(12, 12);
			this.dgUsers.Name = "dgUsers";
			this.dgUsers.Size = new System.Drawing.Size(459, 292);
			this.dgUsers.TabIndex = 0;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.Location = new System.Drawing.Point(396, 310);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// Login
			// 
			this.Login.HeaderText = "Login";
			this.Login.Name = "Login";
			// 
			// Group
			// 
			this.Group.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
			this.Group.HeaderText = "Group";
			this.Group.Items.AddRange(new object[] {
            "АР",
            "ВК",
            "ГП",
            "ГТО",
            "ДО",
            "ЖБК-ТО",
            "КР-МН",
            "КР-СБ",
            "КР-СБ-ГК",
            "НС",
            "ОВ",
            "СС",
            "ТО",
            "ЭО"});
			this.Group.Name = "Group";
			this.Group.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Group.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// FormAddNewUsers
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(483, 345);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.dgUsers);
			this.Name = "FormAddNewUsers";
			this.Text = "FormAddNewUsers";
			this.Load += new System.EventHandler(this.FormAddNewUsers_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgUsers)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgUsers;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.DataGridViewTextBoxColumn Login;
		private System.Windows.Forms.DataGridViewComboBoxColumn Group;
	}
}