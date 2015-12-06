namespace LogMonitor.Core.AllUsers
{
   partial class FormMonitorAcadUsers
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
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.buttonSave = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // richTextBox1
         // 
         this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.richTextBox1.Location = new System.Drawing.Point(0, 0);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.ReadOnly = true;
         this.richTextBox1.Size = new System.Drawing.Size(804, 579);
         this.richTextBox1.TabIndex = 0;
         this.richTextBox1.Text = "";
         // 
         // buttonSave
         // 
         this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.buttonSave.Location = new System.Drawing.Point(737, 544);
         this.buttonSave.Name = "buttonSave";
         this.buttonSave.Size = new System.Drawing.Size(55, 23);
         this.buttonSave.TabIndex = 1;
         this.buttonSave.Text = "Save";
         this.buttonSave.UseVisualStyleBackColor = true;
         this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
         // 
         // FormMonitorAcadUsers
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(804, 579);
         this.Controls.Add(this.buttonSave);
         this.Controls.Add(this.richTextBox1);
         this.Name = "FormMonitorAcadUsers";
         this.Text = "FormMonitorAcadUsers";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.RichTextBox richTextBox1;
      private System.Windows.Forms.Button buttonSave;
   }
}